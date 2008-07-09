//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using ComTypes=System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Threading;
using OrganismBase;
using Terrarium.Configuration;
using Terrarium.Game;
using Terrarium.Tools;
using Terrarium.Renderer;

namespace Terrarium.Hosting 
{
    // This class does all of the work to give the creatures timeslices and ensure that they
    // get killed if they take too much time.
    internal class GameScheduler : MarshalByRefObject, IGameScheduler
    {
        // The collection of animals that are currently hosted.
        GameObjectCollection _organisms;

        // Enumerator to enumerate over _organisms.  This tracks the current animal we are
        // running so we always know where to start.
        IEnumerator _orgEnum;

        // How many times will Tick() be called in a single game turn?  We divide the animals
        // into this many buckets and go through one bucket in each call to Tick()
        int _ticksPerSec = 5;

        // Default amount of time (in microseconds) to allow an animal to run
        int _quantum = 5000;  

        // Amount of time (in microseconds) that we allow an organism to go over their quantum
        // before we start penalize them by skipping their turns
        Int64 _maxOverage = EngineSettings.OrganismSchedulingMaximumOvertime;

        // Amount of time (in microseconds) that we allow an organism to go over their quantum
        // before we destroy them.
        Int64 _maxAllowance = EngineSettings.OrganismSchedulingBlacklistOvertime;

        // This is how long we set our thread timer to fire.  When the thread timer fires, we try
        // to ThreadAbort the animal thread to get control back. 
        const int animalTimeoutMSec = 1000;

        // If an animal actually gets this much kernel time in a single timeslice, we permanently blacklist them 
        // from the game and restart the game.  This is set to a large time because permanently blacklisting
        // them is a big deal and because some of the kernel time their thread truly gets will be time used by
        // the CLR doing GCs, Jitting, etc. and they shouldn't be penalized for it.
        const int animalDeadlock100NSec = 50000000; // 5 seconds

        // How often the deadlock detection should check to see if an animal has returned the thread
        const int animalDeadlockCheckMSec = 7500;   // check for deadlock after 7.5 seconds

        // How many times we retry our check in deadlock detection to see if the thread returned. If we check
        // 3 times (with 7.5 second intervals) and the animal still hasn't gotten 5 seconds
        // of kernel time, we still restart the game, we just don't permanently blacklist them.
        const int animalDeadlockRetries = 3;     

        // In certain cases (like when a debugger is attached) we don't
        // want to set in play all the logic that detects rogue animals.
        Boolean penalizeForTime = true;

        // In certain cases (like when a laptop returns from standby) there
        // can be lots of system activity that could unfairly cause us to 
        // destroy an animal.  This allows us to suspend blacklisting until
        // the system settles down.
        int ticksToSuspendBlacklisting = 0;
        bool suspendBlacklisting = false;

        // This is the timer that we use as a first line of defense for animals
        // that take too long.
        System.Threading.Timer threadTimer = null;
        DateTime timerStoppedTime = DateTime.MinValue;
        bool safeToAbort = false;

        // Alternate thread that animals get executed on
        Thread activationThread;
        IntPtr threadHandle;
        bool threadHandleValid = false;
        bool exitAnimalThread = false;

        // These events synchronize the activation thread and the main UI thread
        ManualResetEvent handleRetrieved = new ManualResetEvent(false);
        ManualResetEvent animalReady = new ManualResetEvent(false);
        ManualResetEvent animalDone = new ManualResetEvent(false);

        // The current animal we are giving a timeslice to on the activation thread
        OrganismWrapper bug = null;
    
        static WorldState currentState;
        PrivateAssemblyCache pac;
        IntPtr processHandle;
        GameEngine currentEngine;
        Int64 _totalActivations = 0;
        TimeMonitor _monitor = new TimeMonitor();
        Int64 _startTicks;
//        Int64 _timersActivated = 0;
        Int64 _lastReport = 0;
        int   _reportInterval = 150;
        int _organismsActivated = 0;
        int tickCount = 0;

        public GameScheduler()
        {
            _startTicks = DateTime.Now.Ticks;
            _organisms = new GameObjectCollection();
            _orgEnum = _organisms.GetEnumerator();

            Debug.Assert( _maxOverage > _quantum );

            AppMgr.CurrentScheduler = this;

            // This handle is a psuedo handle that doesn't need to be closed
            processHandle = GetCurrentProcess();

            // Create a thread other than the UI thread to run the animals on
            activationThread = new Thread(new ThreadStart(ActivateBug));
            activationThread.Priority = ThreadPriority.AboveNormal;
            activationThread.Start();

            // Wait for the ActivateBug routine to get far enough to 
            // retreive its threadhandle which is needed to get accurate
            // timings on how much time the animal actually used on its thread.
            bool success = handleRetrieved.WaitOne();
            Debug.Assert(success);
        }

        public void Close()
        {
            if (threadHandleValid)
            {
                CloseHandle(threadHandle);
            }

            // Tell the ActivateBug routine to go ahead and exit the thread.
            exitAnimalThread = true;
            animalReady.Set();

            threadHandleValid = false;
        }

        // Called by the game engine when it is time to give a set of animals slices of time
        // to execute their logic.  This is what sets everything in this class in motion.
        public void Tick()
        {
            int activated = 0;

            // See comment on ticksToSuspendBlacklisting for why this is useful.
            if (ticksToSuspendBlacklisting > 0)
            {
                Debug.WriteLine("Suspending blacklisting for this tick.");
            }

            if (_organismsActivated < _organisms.Count)
            {
                // Loop through a set of animals
                while (activated < OrganismsPerTick)
                {
                    if (_orgEnum.MoveNext())
                    {
                        activated++;

                        // Give the animal a timeslice, but monitor for cases where it tries to
                        // hang the machine and deal with them gracefully
                        RunAnimalWithDeadlockDetection((OrganismWrapper)_orgEnum.Current);
                    }
                    else
                    {
                        break;
                    }
                } // end animal loop
            }

            _organismsActivated += activated;
            tickCount++;

            if (_organismsActivated == _organisms.Count && tickCount == _ticksPerSec)
            {
                // Start over at the first animal once we've gone through them all 
                // and we've gone through all the ticks required for a turn.
                _orgEnum.Reset();
                _organismsActivated = 0;
                tickCount = 0;
                if (ticksToSuspendBlacklisting > 0)
                {
                    ticksToSuspendBlacklisting--;
                }
            }
        }

        // This is our last line of defense to deal with animals that try to hang the game (deadlock). 
        // (the first line of defense is ThreadAborting the thread with our timer, see description in ActivateBug()).
        // Thus, it must have robust code that can always fail in some graceful way, and should blacklist
        // any animal that hangs. Because blacklisting an animal is very drastic, we go through great pains
        // to do it fairly, which means that we want to ensure that the animal is getting actual time to run
        // in the OS kernel, and the elapsed time isn't simply because the system is starving its thread
        // or something.  If the animal got plenty of kernel time and didn't come back, we restart the game
        // and blacklist them permanently.  If they aren't getting kernel time but it still taking way too long
        // we simply restart the game.
        void RunAnimalWithDeadlockDetection(OrganismWrapper currentAnimal)
        {
            Int64 kernelStart, userStart, kernelStop, userStop;
            bool validTime = false;
            int tries = 0;
            bool blacklist = false;
            bool shutdownWithoutBlacklist = false;
                    
            // Hand the activationThread an animal and kick off processing
            bug = currentAnimal;
            validTime = GetAnimalThreadTime(out kernelStart, out userStart);
            animalDone.Reset();
            animalReady.Set();

            // Now we spin in a loop and periodically check to see if the animal returned the thread to us.
            while (true)
            {
                // If the animal returns, animalDone will get set on the activation thread and we'll continue.
                // if not, this will timeout.
                bool executionDone = animalDone.WaitOne(animalDeadlockCheckMSec, false);
            
                if (!executionDone)
                {
                    Trace.WriteLine("Animal thread not stopped after " + animalDeadlockCheckMSec + " mSec, checking kernel time.");

                    if (DetectDeadlock)
                    {
                        // If we were unable to retreive the time from GetAnimalThreadTime() above, just wait 
                        // for the full number of tries
                        if (validTime)
                        {
                            // Only permanently blacklist the animal if we are first trying to threadabort them 
                            // (PenalizeForTime == true) Otherwise, animals that really won't hang the machine will
                            // get permanently blacklisted even though they could have been stopped with a threadabort.
                            // PenalizeForTime is only false when the tracewindow is up since it affects animal timings
                            if (PenalizeForTime)
                            {
                                validTime = GetAnimalThreadTime(out kernelStop, out userStop);
                                if (validTime)
                                {
                                    Int64 totalTime = (kernelStop - kernelStart) + (userStop - userStart);

                                    // Give the animal a bunch of time since lots of things can happen on their thread
                                    // that is actually reflected as time their thread actually got in the kernel like
                                    // GCs, Jitting, etc.
                                    if (totalTime > animalDeadlock100NSec)
                                    {
                                        Trace.WriteLine("Thread overtime: " + ((double) (totalTime) / (double) 10000000).ToString() + " seconds, Blacklist and exit");
                                        blacklist = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    Debug.WriteLine("Invalid Time From GetThreadTimes()");
                                }
                            }
                            else
                            {
                                Debug.WriteLine("Not penalizing for time -- don't permanently blacklist.");
                            }
                        }
                        else
                        {
                            Debug.WriteLine("Invalid Time From GetThreadTimes()");
                        }

                        tries++;

                        if (tries >= animalDeadlockRetries)
                        {
                            // If we've tried this many times and got to this point, either the animal never got
                            // a lot of actual kernel time, or we're not penalizing for time (the user has the debugger attached, 
                            // or trace window open, for example). Don't blacklist them, but go ahead and restart the game
                            Trace.WriteLine("Tried accessing animal thread " + tries + " times, but not blacklisted yet -- restart.");
                            shutdownWithoutBlacklist = true;
                            break;
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Deadlock detection off");
                    }
                }
                else
                {
                    break;
                }
            }  // end timing while loop
                
                
            // Restart and Blacklist
            if (DetectDeadlock && blacklist)
            {
                Debug.WriteLine("Permanently blacklisting: " + ((Species) currentAnimal.Organism.State.Species).AssemblyInfo.FullName);
                
                // Mark the animal in a magic file on disk so when we restart we will blacklist them.  We can't do it now
                // because the assembly is locked.
                currentEngine.Pac.LastRun = ((Species) currentAnimal.Organism.State.Species).AssemblyInfo.FullName;
                throw new MaliciousOrganismException();
            }

            // Restart, but don't blacklist anyone
            // also check for blacklist here in case DetectDeadlock changed values after blacklist was set
            if (shutdownWithoutBlacklist || blacklist)
            {
                throw new MaliciousOrganismException();
            }
        }

        // This routine is run on the activationThread.  It just waits until a new animal is ready to
        // be run and then runs it and waits again.  There are two ways we attempt to mitigate the 
        // fact that an animal may not want to return execution and hang the machine:
        // 
        // 1. The threadTimer timer is started before we start executing animal code, and 
        //      if the animal takes too long, the threadTimer will fire and run a routine
        //      that calls ThreadAbort on this thread. There are ways for animals to hog the
        //      thread such that this won't ever get a chance to abort them.  See #2 for 
        //      mitigation of this.
        // 2. The RunAnimalWithDeadlockDetection() is what actually feeds animals into the
        //      ActivateBug() routine.  RunAnimalWithDeadlockDetection() provides the next
        //      circle of protection by monitoring the thread with a much longer duration.
        //      if the thread never comes back (which means that ThreadAbort must be failing)
        //      then RunAnimalWithDeadlockDetection() will restart the game and permanently
        //      blacklist the animal.
        internal void ActivateBug()
        {
            // First retreive the thread handle for this thread so we can get accurate reads on how 
            // much actual time an animal spent executing.  
            // This is a pseudo handle that can't be used on other threads until it is duplicated
            IntPtr pseudoThreadHandle = GetCurrentThread();
            threadHandleValid = DuplicateHandle(
                processHandle,                  // handle to source process
                pseudoThreadHandle,             // handle to duplicate
                processHandle,                  // handle to target process
                ref threadHandle,               // duplicate handle
                0,                              // requested access
                false,                          // handle inheritance option
                2                               // optional actions
                );

            // Notify the constructor of this class that we have retreived the handle.
            bool setSuccess = handleRetrieved.Set();
            Debug.Assert(setSuccess);

            // If we get an exception that somehow happens outside the try blocks below, it will end this thread,
            // be caught by our global exception handler and our deadlock logic will restart the game automatically because
            // it will think the thread never came back and is deadlocked.
            while (true)
            {
                // Wait for the first animal
                animalReady.WaitOne();
                if (exitAnimalThread)
                {
                    return;
                }

                bool success = false;
                Int64 duration = 0;
                PopulationChangeReason deathReason = PopulationChangeReason.NotDead;
                string exceptionInfo = "";
                bool skippedTurn = false;

                // short circut inactive bugs
                if (bug.Active)
                {
                    OrganismState state = currentState.GetOrganismState(bug.Organism.ID);
                    if (!state.IsAlive)
                    {
                        bug.Active = false;
                    }
                    else
                    {
                        try
                        {
                            try
                            {
                                // See if the animal has consistently taken too much time and has finally
                                // gone over the maxAllowance.
                                if (bug.Overage > _maxAllowance)
                                {
                                    Trace.WriteLine("Organism blacklisted: bug.Overage > _maxAllowance");
                                    deathReason = PopulationChangeReason.Timeout;
                                    bug.Active = false;
                                    success = false;
                                }
                                else if (bug.Overage > _maxOverage)
                                {
                                    // if the bug is overtime, don't schedule it
                                    // but deduce one quantum from its overage
                                    // until it's back under the allowable limit
                                    bug.Overage -= _quantum;
                                    if (bug.Overage < 0)
                                    {
                                        bug.Overage = 0;
                                    }
                                
                                    // We still need to call InternalMain with "true" so that it can clear
                                    // out state for event handlers.  This won't run organism code
                                    bug.Organism.InternalMain(true);

                                    bug.Organism.WriteTrace("Animal's turn was skipped because they took longer than " + _quantum + " microseconds for their turn too many times.");
                                
                                    // We didn't fail, we're just skipping them.  Don't remove them from the world.
                                    success = true;
                                    skippedTurn = true;
                                }
                                else
                                {
                                    // Start the thread that will ensure we ThreadAbort if the elapsed
                                    // time the animal takes is too long.
                                    StartTimer();

                                    // Time the animal
                                    _monitor.Start();

                                    // Tell the timer thread it is OK to abort in this section
                                    // If a ThreadAbort exception gets thrown from after this lock statement on down to 
                                    // safeToAbort = false, we won't shut off safeToAbort and we could abort again in the 
                                    // catch handlers which is why we are wrapped with another try  / catch block
                                    lock (_monitor)
                                    {
                                        safeToAbort = true;
                                    }
                                    
                                    // Hook up the tracing event handler if the animal is selected
                                    if (GameConfig.LoggingMode != "None")
                                    {
                                        if (GameConfig.LoggingMode != "Full")
                                        {
                                            Object renderInfo = bug.Organism.State.RenderInfo;
                                            if (renderInfo != null && ((TerrariumSprite) renderInfo).Selected)
                                            {
                                                bug.Organism.Trace += new TraceEventHandler(TraceEventHandler);
                                            }
                                        }
                                        else
                                        {
                                            bug.Organism.Trace += new TraceEventHandler(TraceEventHandler);
                                        }
                                    }

                                    // Actually allow the animal to run their code now.
                                    bug.Organism.InternalMain(false);
                        
                                    // Tell the timer thread it is not OK to abort anymore
                                    // If we get aborted before this can complete, the aborter will 
                                    // set this to false as well
                                    lock (_monitor)
                                    {
                                        safeToAbort = false;
                                    }

                                    CancelTimer();

                                    // Determine how much time the animal spent
                                    duration = _monitor.EndGetMicroseconds();

                                    // This means we weren't aborted or didn't throw an exception
                                    success = true;
                                }
                            }
                            catch (ThreadAbortException)
                            {
                                // If we get aborted, the aborter will set safeToAbort to false so no need 
                                // to do it here
                                deathReason = PopulationChangeReason.Timeout;
                                Trace.WriteLine("Inner Try ThreadAborted");
                                Thread.ResetAbort();
                                Trace.WriteLine("Cancelling Timer");
                                CancelTimer();
                            }
                            catch (SecurityException e)
                            {
                                // Tell the timer thread it is not OK to abort anymore
                                // If we get aborted before this can complete, the aborter will set this
                                // to false as well
                                lock (_monitor)
                                {
                                    safeToAbort = false;
                                }

                                CancelTimer();

                                // Organism needs to be removed from the game because it had an exception
                                deathReason = PopulationChangeReason.SecurityViolation;
                                exceptionInfo = e.ToString();
                            }
                            catch (OrganismBlacklistedException)
                            {
                                // This exception gets thrown when we try to give a time slice to an animal
                                // that has been replaced by the special TerrariumOrganism, whose whole purpose
                                // is to just replace a blacklisted animal until it is safe to remove them
                                // from the game, which is now.

                                // Tell the timer thread it is not OK to abort anymore
                                // If we get aborted before this can complete, the aborter will set this
                                // to false as well
                                lock (_monitor)
                                {
                                    safeToAbort = false;
                                }
                            
                                CancelTimer();
                        
                                deathReason = PopulationChangeReason.OrganismBlacklisted;
                            }
                            catch (Exception e)
                            {
                                // Tell the timer thread it is not OK to abort anymore
                                // If we get aborted before this can complete, the aborter will set this
                                // to false as well
                                lock (_monitor)
                                {
                                    safeToAbort = false;
                                }

                                CancelTimer();

                                // Organism needs to be removed from the game because it had an exception
                                deathReason = PopulationChangeReason.Error;
                                exceptionInfo = e.ToString();
                            }
                        }
                        catch (ThreadAbortException)
                        {
                            // if we catch the abort here in the outer of the two try blocks, there was an exception, 
                            // but we lost it since the threadabort happened in the catch handler
                            if (deathReason == PopulationChangeReason.NotDead)
                            {
                                deathReason = PopulationChangeReason.Error;
                                exceptionInfo = "Exception was lost because organism timed out before we could grab it.";
                            }
                            Thread.ResetAbort();
                            Trace.WriteLine("Outer Try ThreadAborted");
                        }
                        finally
                        {
                            bug.Organism.Trace -= new TraceEventHandler(TraceEventHandler);
                            if (!success)
                            {
                                // Organism needs to be removed from the game because it had an exception
                                if (deathReason == PopulationChangeReason.Timeout)
                                {
                                    currentEngine.RemoveOrganismQueued(new KilledOrganism(bug.Organism.ID, deathReason));
                                }
                                else
                                {
                                    // The organism threw an exception
                                    currentEngine.RemoveOrganismQueued(new KilledOrganism(bug.Organism.ID, deathReason, exceptionInfo));
                                    Trace.WriteLine("Exception in Animal: \r\n" + exceptionInfo);
                                }
                            }
                            else
                            {
                                _totalActivations++;
                                if (PenalizeForTime)
                                {
                                    bug.TotalTime += duration;
                                    bug.LastTime = duration;
                                    if (duration > _quantum)
                                    {
                                        bug.Overage += (duration - _quantum);
                                    }
                                    else if (bug.Overage > 0 && !skippedTurn)
                                    {
                                        // If the animal ran under time, subtract this off of
                                        // their overage
                                        bug.Overage -= (_quantum - duration);
                                        if (bug.Overage < 0)
                                        {
                                            bug.Overage = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    bug.LastTime = duration;
                                    bug.Overage = 0;
                                }

                                bug.TotalActivations++;
                            }
                        }

                        if (_totalActivations > _lastReport + _reportInterval)
                        {
                            _lastReport = _totalActivations;
                        }
                    } 
                } // end if(bug.Active)
            
                // Tell the UI thread we are done with this animal
                animalReady.Reset();
                animalDone.Set();
            } // end while
        }

        void StartTimer()
        {
			if ( threadTimer != null )
				throw new InvalidOperationException( "Thread Timer already exists" );

			threadTimer = new System.Threading.Timer( new TimerCallback(AbortThread), null, 1000, System.Threading.Timeout.Infinite );
		}

        void CancelTimer()
        {
			if ( threadTimer != null )
			{
                timerStoppedTime = DateTime.Now;
				threadTimer.Dispose();
				threadTimer = null;
			}
		}

        // This routine gets called by the threadTimer timer when a certain
        // amount of time has elapsed.  If it fires, it means an animal has taken
        // way too long to execute and we should ThreadAbort the thread the 
        // animals run on to try to regain control of the game.
        public void AbortThread(object arg )
        {
            // Wait until the main thread is in a safe place to allow the abort
            lock (_monitor)
            {
                // See note on CancelTimer for why we need to check e.SignalTime > timerStoppedTime
                if (PenalizeForTime && DateTime.Now > timerStoppedTime && safeToAbort)
                { 
                    // If any other timers fire, they should not abort again until the activationThread says it's safe
                    // again.  We only want one abort per "safe time".  Set safeToAbort to false to ensure this.
                    safeToAbort = false;
                    activationThread.Abort();
                }
            }
        }

        // Determines how much actual kernel and user time this thread has actually gotten
        bool GetAnimalThreadTime(out Int64 kernel, out Int64 user)
        {
            if (threadHandleValid)
            {
                ComTypes.FILETIME c = new ComTypes.FILETIME();
                ComTypes.FILETIME e = new ComTypes.FILETIME();
                ComTypes.FILETIME kernelFileTime = new ComTypes.FILETIME();
                ComTypes.FILETIME userFileTime = new ComTypes.FILETIME();

                bool success = (GetThreadTimes(threadHandle, ref c, ref e, ref kernelFileTime, ref userFileTime) > 0);
                if (success)
                {
                    kernel = (((Int64) kernelFileTime.dwHighDateTime) << 32) + kernelFileTime.dwLowDateTime;
                    user = (((Int64) userFileTime.dwHighDateTime) << 32) + userFileTime.dwLowDateTime;
                    return true;
                }
                else
                {
                    kernel = 0;
                    user = 0;
                    return false;
                }
            }
            else
            {
                kernel = 0;
                user = 0;
                return false;
            }
        }

        // Add an organism to get timesliced.
        public void Add(Organism org, string id)
        {
            // We should only be adding organisms at the beginning
            Debug.Assert(_organismsActivated == 0);

            // Reconstruct its world boundary
            OrganismWorldBoundary.SetWorldBoundary(org, id);

            if (_organisms.ContainsKey(id))
            {
                throw new OrganismAlreadyExistsException();
            }

            _organisms.Add(new OrganismWrapper(org));
        }

        public int OrganismsPerTick
        {
            get
            {
                return(_organisms.Count / _ticksPerSec) + 1;
            }        
        }

        public int TicksPerSec
        {
            get
            {
                return _ticksPerSec;
            }
        
            set
            {
                _ticksPerSec = value;
            }
        }

        public int Quantum
        {
            get
            {
                return _quantum;
            }
        
            set
            {
                _quantum = value;
            }
        }

        public Int64 MaxOverage
        {
            get
            {
                return _maxOverage;
            }
        
            set
            {
                _maxOverage = value;
            }
        }

        // in microseconds, time before component is removed
        // default is 10,000,000 (10 secs)
        public Int64 MaxAllowance
        {
            get
            {
                return _maxAllowance;
            }
        
            set
            {
                _maxAllowance = value;
            }
        }

        // Going into power saving mode can cause threads to suck a bunch of time
        // This method gets called so animals don't get unnecessarily blacklisted in this case
        // This method needs to be threadsafe because it will be called from the systemevents thread
        public void TemporarilySuspendBlacklisting()
        {
            ticksToSuspendBlacklisting = 2;
        }

        public bool SuspendBlacklisting
        {
            get
            {
                return suspendBlacklisting;
            }

            set
            {
                suspendBlacklisting = value;
            }
        }

        // Always detect Deadlock
        public Boolean DetectDeadlock
        {
            get
            {
                // Don't kill the bug if a debugger is attached
                if (Debugger.IsAttached)
                {
                    return false;
                }
                else if (ticksToSuspendBlacklisting > 0)
                {
                    return false;
                }
                else if (suspendBlacklisting)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        // This returns true if we should shutoff the simple Timeouts that don't hang the machine
        public Boolean PenalizeForTime
        {
            get
            {
                // If we're not detecting deadlock, don't bother detecting simple timeouts since they are
                // the least of our worries
                if (!DetectDeadlock)
                {
                    return false;
                }
                else
                {
                    return penalizeForTime;
                }
            }

            set
            {
                penalizeForTime = value;
            }
        }

        public Organism GetOrganism(string id)
        {
            return(Organism)_organisms[id];
        }

        public ICollection Organisms
        {
            get
            {
                ArrayList l = new ArrayList();
                foreach (OrganismWrapper w in _organisms)
                {
                    l.Add(w.Organism);
                }
                return l;
            }
        }

        public void SerializeOrganisms(Stream stream)
        {
            try
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(stream, _organisms);
            } 
            catch (Exception e)
            {
                ErrorLog.LogHandledException(e);
            }
        }

        public void DeserializeOrganisms(Stream stream)
        {
            BinaryFormatter b = new BinaryFormatter();
            try
            {
                _organisms = (GameObjectCollection) b.Deserialize(stream);
                _orgEnum = _organisms.GetEnumerator();
            }
            catch
            {
                _organisms = new GameObjectCollection();
                _orgEnum = _organisms.GetEnumerator();

                throw;
            }
        }

        public void CompleteOrganismDeserialization()
        {
            try
            {
                _organisms.CompleteOrganismDeserialization();
            }
            catch
            {
                _organisms = new GameObjectCollection();
                _orgEnum = _organisms.GetEnumerator();

                throw;
            }
        }

        public void Remove(string organismID)
        {
            _organisms.Remove(organismID);
        }

        public void Create(Type species, string id)
        {
            Organism newOrganism = (Organism)Activator.CreateInstance(species);
            if (null == newOrganism)
            {
                throw new Exception("Failed to create instance of: " + species.ToString());
            }

            Add(newOrganism, id);
        }

        public TickActions GatherTickActions()
        {
            TickActions act = new TickActions();
            act.GatherActionsFromOrganisms(this);
            return act;
        }

        public WorldState CurrentState
        {
            get 
            {
                return currentState;
            }
        
            set
            {
                currentState = value;
            }
        }

        public AppDomain OrganismAppDomain
        {
            get
            {
                return AppDomain.CurrentDomain;
            }
        }
    
        private void TraceEventHandler(object sender, params object[] items)
        {
            if (items.Length == 1)
            {
                string item = items[0].ToString();
                if (item.StartsWith("#"))
                {
                    return;
                }
            }
    
            for (int i = 0; i < items.Length; i++)
            {
                Trace.WriteLine(items[i]);
            }
        }

        public PrivateAssemblyCache PrivateAssemblyCache
        {
            set
            {
                if (pac != null)
                {
                    pac.Close();
                }

                // Hook this pac up to our assemblyresolve event
                pac = value;
                pac.HookAssemblyResolve();
            }
        }

        public GameEngine CurrentGameEngine
        {
            get
            {
                return currentEngine;
            }

            set
            {
                currentEngine = value;
                PrivateAssemblyCache = currentEngine.Pac;
            }
        }

        /* Left in for debugging purposes
        void Report()
        {
            Int64 nowTicks = DateTime.Now.Ticks;
            TimeSpan delta = TimeSpan.FromTicks(nowTicks - _startTicks);
            Trace.WriteLine("Total Activations: " + _totalActivations);
            if (_totalActivations > 0 && delta.TotalSeconds > 0)
            {
                double perSec = _totalActivations / delta.TotalSeconds;
                Trace.WriteLine(perSec.ToString() + " activations per second");
                Trace.WriteLine(_timersActivated.ToString() + " timers expired");
            }

            IEnumerator e = _organisms.GetEnumerator();
            Int64 highMark = 0;
            Int64 lowMark = 0;
            while (e.MoveNext())
            {
                OrganismWrapper w = (OrganismWrapper)e.Current;
                if (w.TotalTime > highMark)
                {
                    highMark = w.TotalTime;
                }
            
                if (lowMark == 0 || w.TotalTime < lowMark)
                {
                    lowMark = w.TotalTime;
                }
            }

            double secs = (double)highMark / (double)1000000; // convert micro secs to secs
            Trace.WriteLine("Most time is " + highMark.ToString()
                + " microsecs or " + secs.ToString() + " seconds");
            secs = (double)lowMark / (double)1000000;
            Trace.WriteLine("Least time is " + lowMark.ToString()
                + " microsecs or " + secs.ToString() + " seconds");

        }
*/

        public string GetOrganismTimingReport(string organismID)
        {
            OrganismWrapper w = _organisms.GetWrapperForOrganism(organismID);
            if (w == null)
            {
                return "[organism doesn't exist]";
            }
            else
            {
                // Only report timings if we are penalizing for slow times because this means
                // we've shut off all elements that slow down the animals like tracing
                if (PenalizeForTime)
                {
                    string gcHappened = "";

                    if (w.LastTime > _quantum)
                    {
                        return "Warning: Time to execute last turn: " + w.LastTime.ToString() + " microseconds is over maxiumum allowed time of " + _quantum + " microseconds.  Animal may be penalized by skipping a turn.";
                    }
                    else
                    {
                        return gcHappened + "Time to execute last turn: " + w.LastTime.ToString() + " microseconds.  [less than maximum allowed time of " + _quantum + " microseconds.]";
                    }
                }
                else
                {
                    return "Inaccurate time due to debugging :" + w.LastTime.ToString() + " microseconds";
                }
            }
        }

        /// <summary>
        ///  Used to get thread times for the current thread.  This enables
        ///  the Hosting code to time out creatures.
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute()]
        [DllImport("kernel32", CharSet=CharSet.Auto)]
        static extern int GetThreadTimes(
            IntPtr hThread,
            ref ComTypes.FILETIME lpCreationTime,
            ref ComTypes.FILETIME lpExitTime,
            ref ComTypes.FILETIME lpKernelTime,
            ref ComTypes.FILETIME lpUserTime
            );

        /// <summary>
        ///  Used to get the current thread handle for use with GetThreadTimes
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute()]
        [DllImport("kernel32", CharSet=CharSet.Auto)]
        static extern IntPtr GetCurrentThread();

        /// <summary>
        ///  Used to get the current process handle for use with DuplicateHandle
        /// </summary>
        [DllImport( "kernel32" )]
        static extern IntPtr GetCurrentProcess();

        /// <summary>
        ///  Used to duplicate the thread handle returned from GetCurrentThread
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute()]
        [DllImport( "kernel32" )]
        static extern bool DuplicateHandle(
            IntPtr hSourceProcessHandle,    // handle to source process
            IntPtr hSourceHandle,           // handle to duplicate
            IntPtr hTargetProcessHandle,    // handle to target process
            ref IntPtr lpTargetHandle,      // duplicate handle
            int dwDesiredAccess,            // requested access
            bool bInheritHandle,            // handle inheritance option
            int dwOptions                   // optional actions
            );

        /// <summary>
        ///  Used to close all handles allocated by GetCurrentThread, GetCurrentProcess,
        ///  and DuplicateHandle
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute()]
        [DllImport( "kernel32" )]
        static extern bool CloseHandle(
            IntPtr hObject   // handle to object
            );
    }
}