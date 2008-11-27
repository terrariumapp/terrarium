//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Threading;
using OrganismBase;
using Terrarium.Configuration;
using Terrarium.Game;
using Terrarium.Renderer;
using Terrarium.Tools;
using FILETIME=System.Runtime.InteropServices.ComTypes.FILETIME;

namespace Terrarium.Hosting
{
    /// <summary>
    /// This class does all of the work to give the creatures timeslices and ensure 
    /// that they get killed if they take too much time.
    /// </summary>
    internal class GameScheduler : MarshalByRefObject, IGameScheduler
    {
        // If an animal actually gets this much kernel time in a single timeslice, we permanently blacklist them 
        // from the game and restart the game.  This is set to a large time because permanently blacklisting
        // them is a big deal and because some of the kernel time their thread truly gets will be time used by
        // the CLR doing GCs, Jitting, etc. and they shouldn't be penalized for it.
        private const int AnimalDeadlock100NSec = 50000000; // 5 seconds

        // How often the deadlock detection should check to see if an animal has returned the thread
        private const int AnimalDeadlockCheckMSec = 7500; // check for deadlock after 7.5 seconds

        // How many times we retry our check in deadlock detection to see if the thread returned. If we check
        // 3 times (with 7.5 second intervals) and the animal still hasn't gotten 5 seconds
        // of kernel time, we still restart the game, we just don't permanently blacklist them.
        private const int AnimalDeadlockRetries = 3;

        private const int ReportInterval = 150;
        private readonly Thread _activationThread;
        private readonly ManualResetEvent _animalDone = new ManualResetEvent(false);
        private readonly ManualResetEvent _animalReady = new ManualResetEvent(false);
        private readonly ManualResetEvent _handleRetrieved = new ManualResetEvent(false);
        private readonly TimeMonitor _monitor = new TimeMonitor();
        private readonly IntPtr _processHandle;
        private OrganismWrapper _bug;
        private GameEngine _currentEngine;
        private bool _exitAnimalThread;
        private Int64 _lastReport;
        private GameObjectCollection _organisms;
        private int _organismsActivated;
        private IEnumerator _orgEnum;
        private PrivateAssemblyCache _pac;
        private Boolean _penalizeForTime = true;
        private bool _safeToAbort;
        private IntPtr _threadHandle;
        private bool _threadHandleValid;
        private Timer _threadTimer;
        private int _tickCount;
        private int _ticksToSuspendBlacklisting;
        private DateTime _timerStoppedTime = DateTime.MinValue;
        private Int64 _totalActivations;

        public GameScheduler()
        {
            MaxAllowance = EngineSettings.OrganismSchedulingBlacklistOvertime;
            TicksPerSec = 5;
            Quantum = 5000;
            MaxOverage = EngineSettings.OrganismSchedulingMaximumOvertime;
            _organisms = new GameObjectCollection();
            _orgEnum = _organisms.GetEnumerator();

            Debug.Assert(MaxOverage > Quantum);

            AppMgr.CurrentScheduler = this;

            // This handle is a psuedo handle that doesn't need to be closed
            _processHandle = GetCurrentProcess();

            // Create a thread other than the UI thread to run the animals on
            _activationThread = new Thread(ActivateBug) {Priority = ThreadPriority.AboveNormal};
            _activationThread.Start();

            // Wait for the ActivateBug routine to get far enough to 
            // retreive its threadhandle which is needed to get accurate
            // timings on how much time the animal actually used on its thread.
            var success = _handleRetrieved.WaitOne();
            Debug.Assert(success);
        }

        public Boolean DetectDeadlock
        {
            get
            {
                // Don't kill the bug if a debugger is attached
                if (Debugger.IsAttached)
                {
                    return false;
                }
                if (_ticksToSuspendBlacklisting > 0)
                {
                    return false;
                }
                return !SuspendBlacklisting;
            }
        }

        public PrivateAssemblyCache PrivateAssemblyCache
        {
            set
            {
                if (_pac != null)
                {
                    _pac.Close();
                }

                // Hook this pac up to our assemblyresolve event
                _pac = value;
                _pac.HookAssemblyResolve();
            }
        }

        #region IGameScheduler Members

        public void Close()
        {
            if (_threadHandleValid)
            {
                CloseHandle(_threadHandle);
            }

            // Tell the ActivateBug routine to go ahead and exit the thread.
            _exitAnimalThread = true;
            _animalReady.Set();

            _threadHandleValid = false;
        }

        // Called by the game engine when it is time to give a set of animals slices of time
        // to execute their logic.  This is what sets everything in this class in motion.
        public void Tick()
        {
            var activated = 0;

            // See comment on ticksToSuspendBlacklisting for why this is useful.
            if (_ticksToSuspendBlacklisting > 0)
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
                        RunAnimalWithDeadlockDetection((OrganismWrapper) _orgEnum.Current);
                    }
                    else
                    {
                        break;
                    }
                } // end animal loop
            }

            _organismsActivated += activated;
            _tickCount++;

            if (_organismsActivated == _organisms.Count && _tickCount == TicksPerSec)
            {
                // Start over at the first animal once we've gone through them all 
                // and we've gone through all the ticks required for a turn.
                _orgEnum.Reset();
                _organismsActivated = 0;
                _tickCount = 0;
                if (_ticksToSuspendBlacklisting > 0)
                {
                    _ticksToSuspendBlacklisting--;
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
            get { return (_organisms.Count/TicksPerSec) + 1; }
        }

        public int TicksPerSec { get; set; }

        public int Quantum { get; set; }

        public long MaxOverage { get; set; }

        /// <summary>
        /// in microseconds, time before component is removed
        /// default is 10,000,000 (10 secs)
        /// </summary>
        public long MaxAllowance { get; set; }

        // Going into power saving mode can cause threads to suck a bunch of time
        // This method gets called so animals don't get unnecessarily blacklisted in this case
        // This method needs to be threadsafe because it will be called from the systemevents thread
        public void TemporarilySuspendBlacklisting()
        {
            _ticksToSuspendBlacklisting = 2;
        }

        public bool SuspendBlacklisting { get; set; }

        // This returns true if we should shutoff the simple Timeouts that don't hang the machine
        public Boolean PenalizeForTime
        {
            get
            {
                // If we're not detecting deadlock, don't bother detecting simple timeouts since they are
                // the least of our worries
                return DetectDeadlock && _penalizeForTime;
            }

            set { _penalizeForTime = value; }
        }

        public Organism GetOrganism(string id)
        {
            return (Organism) _organisms[id];
        }

        public ICollection Organisms
        {
            get
            {
                var list = new List<Organism>(_organisms.Count);
                foreach (OrganismWrapper wrapper in _organisms)
                {
                    list.Add(wrapper.Organism);   
                }
                return list;
            }
        }

        public void SerializeOrganisms(Stream stream)
        {
            try
            {
                var b = new BinaryFormatter();
                b.Serialize(stream, _organisms);
            }
            catch (Exception e)
            {
                ErrorLog.LogHandledException(e);
            }
        }

        public void DeserializeOrganisms(Stream stream)
        {
            var b = new BinaryFormatter();
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
            var newOrganism = (Organism) Activator.CreateInstance(species);
            if (null == newOrganism)
            {
                throw new Exception("Failed to create instance of: " + species);
            }

            Add(newOrganism, id);
        }

        public TickActions GatherTickActions()
        {
            var act = new TickActions();
            act.GatherActionsFromOrganisms(this);
            return act;
        }

        public WorldState CurrentState { get; set; }

        public AppDomain OrganismAppDomain
        {
            get { return AppDomain.CurrentDomain; }
        }

        public GameEngine CurrentGameEngine
        {
            get { return _currentEngine; }

            set
            {
                _currentEngine = value;
                PrivateAssemblyCache = _currentEngine.Pac;
            }
        }

        public string GetOrganismTimingReport(string organismID)
        {
            var w = _organisms.GetWrapperForOrganism(organismID);
            if (w == null)
            {
                return "[organism doesn't exist]";
            }

            // Only report timings if we are penalizing for slow times because this means
            // we've shut off all elements that slow down the animals like tracing
            if (!PenalizeForTime)
            {
                return "Inaccurate time due to debugging :" + w.LastTime + " microseconds";
            }

            return w.LastTime > Quantum
                       ?
                           string.Format(
                               "Warning: Time to execute last turn: {0} microseconds is over maxiumum allowed time of {1} microseconds.  Animal may be penalized by skipping a turn.",
                               w.LastTime, Quantum)
                       :
                           string.Format(
                               "Time to execute last turn: {0} microseconds.  [less than maximum allowed time of {1} microseconds.]",
                               w.LastTime, Quantum);
        }

        #endregion

        private void RunAnimalWithDeadlockDetection(OrganismWrapper currentAnimal)
        {
            Int64 kernelStart;
            Int64 userStart;
            var tries = 0;
            var blacklist = false;
            var shutdownWithoutBlacklist = false;

            // Hand the activationThread an animal and kick off processing
            _bug = currentAnimal;
            var validTime = getAnimalThreadTime(out kernelStart, out userStart);
            _animalDone.Reset();
            _animalReady.Set();

            // Now we spin in a loop and periodically check to see if the animal returned the thread to us.
            while (true)
            {
                // If the animal returns, _animalDone will get set on the activation thread and we'll continue.
                // if not, this will timeout.
                var executionDone = _animalDone.WaitOne(AnimalDeadlockCheckMSec, false);

                if (!executionDone)
                {
                    Trace.WriteLine(string.Format("Animal thread not stopped after {0} mSec, checking kernel time.",
                                                  AnimalDeadlockCheckMSec));

                    if (DetectDeadlock)
                    {
                        // If we were unable to retreive the time from getAnimalThreadTime() above, just wait 
                        // for the full number of tries
                        if (validTime)
                        {
                            // Only permanently blacklist the animal if we are first trying to threadabort them 
                            // (PenalizeForTime == true) Otherwise, animals that really won't hang the machine will
                            // get permanently blacklisted even though they could have been stopped with a threadabort.
                            // PenalizeForTime is only false when the tracewindow is up since it affects animal timings
                            if (PenalizeForTime)
                            {
                                Int64 kernelStop;
                                Int64 userStop;
                                validTime = getAnimalThreadTime(out kernelStop, out userStop);
                                if (validTime)
                                {
                                    var totalTime = (kernelStop - kernelStart) + (userStop - userStart);

                                    // Give the animal a bunch of time since lots of things can happen on their thread
                                    // that is actually reflected as time their thread actually got in the kernel like
                                    // GCs, Jitting, etc.
                                    if (totalTime > AnimalDeadlock100NSec)
                                    {
                                        Trace.WriteLine(string.Format(
                                                            "Thread overtime: {0} seconds, Blacklist and exit",
                                                            ((totalTime)/(double) 10000000)));
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

                        if (tries >= AnimalDeadlockRetries)
                        {
                            // If we've tried this many times and got to this point, either the animal never got
                            // a lot of actual kernel time, or we're not penalizing for time (the user has the debugger attached, 
                            // or trace window open, for example). Don't blacklist them, but go ahead and restart the game
                            Trace.WriteLine(
                                string.Format(
                                    "Tried accessing animal thread {0} times, but not blacklisted yet -- restart.",
                                    tries));
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
            }

            handleRestartsAndBlacklist(currentAnimal, blacklist);
            handleRestartWithoutBlacklist(blacklist, shutdownWithoutBlacklist);
        }

        private static void handleRestartWithoutBlacklist(bool blacklist, bool shutdownWithoutBlacklist)
        {
            if (shutdownWithoutBlacklist || blacklist)
            {
                throw new MaliciousOrganismException();
            }
        }

        private void handleRestartsAndBlacklist(OrganismWrapper currentAnimal, bool blacklist)
        {
            if (DetectDeadlock && blacklist)
            {
                Debug.WriteLine(string.Format("Permanently blacklisting: {0}",
                                              ((Species) currentAnimal.Organism.State.Species).AssemblyInfo.FullName));

                // Mark the animal in a magic file on disk so when we restart we will blacklist them.  We can't do it now
                // because the assembly is locked.
                _currentEngine.Pac.LastRun = ((Species) currentAnimal.Organism.State.Species).AssemblyInfo.FullName;

                throw new MaliciousOrganismException();
            }
        }

        // This routine is run on the activationThread.  It just waits until a new animal is ready to
        // be run and then runs it and waits again.  There are two ways we attempt to mitigate the 
        // fact that an animal may not want to return execution and hang the machine:
        // 
        // 1. The threadTimer timer is started before we start executing animal code, and 
        //      if the animal takes too long, the _threadTimer will fire and run a routine
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
            var pseudoThreadHandle = GetCurrentThread();
            _threadHandleValid = DuplicateHandle(
                _processHandle, // handle to source process
                pseudoThreadHandle, // handle to duplicate
                _processHandle, // handle to target process
                ref _threadHandle, // duplicate handle
                0, // requested access
                false, // handle inheritance option
                2 // optional actions
                );

            // Notify the constructor of this class that we have retreived the handle.
            var setSuccess = _handleRetrieved.Set();
            Debug.Assert(setSuccess);

            // If we get an exception that somehow happens outside the try blocks below, it will end this thread,
            // be caught by our global exception handler and our deadlock logic will restart the game automatically because
            // it will think the thread never came back and is deadlocked.
            while (true)
            {
                // Wait for the first animal
                _animalReady.WaitOne();
                if (_exitAnimalThread)
                {
                    return;
                }

                var success = false;
                Int64 duration = 0;
                var deathReason = PopulationChangeReason.NotDead;
                var exceptionInfo = "";
                var skippedTurn = false;

                // short circut inactive bugs
                if (_bug.Active)
                {
                    var state = CurrentState.GetOrganismState(_bug.Organism.ID);
                    if (!state.IsAlive)
                    {
                        _bug.Active = false;
                    }
                    else
                    {
                        try
                        {
                            try
                            {
                                // See if the animal has consistently taken too much time and has finally
                                // gone over the maxAllowance.
                                if (_bug.Overage > MaxAllowance)
                                {
                                    Trace.WriteLine("Organism blacklisted: bug.Overage > _maxAllowance");
                                    deathReason = PopulationChangeReason.Timeout;
                                    _bug.Active = false;
                                }
                                else if (_bug.Overage > MaxOverage)
                                {
                                    // if the bug is overtime, don't schedule it
                                    // but deduce one quantum from its overage
                                    // until it's back under the allowable limit
                                    _bug.Overage -= Quantum;
                                    if (_bug.Overage < 0)
                                    {
                                        _bug.Overage = 0;
                                    }

                                    // We still need to call InternalMain with "true" so that it can clear
                                    // out state for event handlers.  This won't run organism code
                                    _bug.Organism.InternalMain(true);

                                    _bug.Organism.WriteTrace(
                                        "Animal's turn was skipped because they took longer than " +
                                        Quantum + " microseconds for their turn too many times.");

                                    // We didn't fail, we're just skipping them.  Don't remove them from the world.
                                    success = true;
                                    skippedTurn = true;
                                }
                                else
                                {
                                    // Start the thread that will ensure we ThreadAbort if the elapsed
                                    // time the animal takes is too long.
                                    startTimer();

                                    // Time the animal
                                    _monitor.Start();

                                    // Tell the timer thread it is OK to abort in this section
                                    // If a ThreadAbort exception gets thrown from after this lock statement on down to 
                                    // safeToAbort = false, we won't shut off safeToAbort and we could abort again in the 
                                    // catch handlers which is why we are wrapped with another try  / catch block
                                    lock (_monitor)
                                    {
                                        _safeToAbort = true;
                                    }

                                    // Hook up the tracing event handler if the animal is selected
                                    if (GameConfig.LoggingMode != "None")
                                    {
                                        if (GameConfig.LoggingMode != "Full")
                                        {
                                            var renderInfo = _bug.Organism.State.RenderInfo;
                                            if (renderInfo != null && ((TerrariumSprite) renderInfo).Selected)
                                            {
                                                _bug.Organism.Trace += traceEventHandler;
                                            }
                                        }
                                        else
                                        {
                                            _bug.Organism.Trace += traceEventHandler;
                                        }
                                    }

                                    // Actually allow the animal to run their code now.
                                    _bug.Organism.InternalMain(false);

                                    // Tell the timer thread it is not OK to abort anymore
                                    // If we get aborted before this can complete, the aborter will 
                                    // set this to false as well
                                    lock (_monitor)
                                    {
                                        _safeToAbort = false;
                                    }

                                    cancelTimer();

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
                                cancelTimer();
                            }
                            catch (SecurityException e)
                            {
                                // Tell the timer thread it is not OK to abort anymore
                                // If we get aborted before this can complete, the aborter will set this
                                // to false as well
                                lock (_monitor)
                                {
                                    _safeToAbort = false;
                                }

                                cancelTimer();

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
                                    _safeToAbort = false;
                                }

                                cancelTimer();

                                deathReason = PopulationChangeReason.OrganismBlacklisted;
                            }
                            catch (Exception e)
                            {
                                // Tell the timer thread it is not OK to abort anymore
                                // If we get aborted before this can complete, the aborter will set this
                                // to false as well
                                lock (_monitor)
                                {
                                    _safeToAbort = false;
                                }

                                cancelTimer();

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
                            _bug.Organism.Trace -= traceEventHandler;
                            if (!success)
                            {
                                // Organism needs to be removed from the game because it had an exception
                                if (deathReason == PopulationChangeReason.Timeout)
                                {
                                    _currentEngine.RemoveOrganismQueued(new KilledOrganism(_bug.Organism.ID, deathReason));
                                }
                                else
                                {
                                    // The organism threw an exception
                                    _currentEngine.RemoveOrganismQueued(new KilledOrganism(_bug.Organism.ID, deathReason,
                                                                                           exceptionInfo));
                                    Trace.WriteLine("Exception in Animal: \r\n" + exceptionInfo);
                                }
                            }
                            else
                            {
                                _totalActivations++;
                                if (PenalizeForTime)
                                {
                                    _bug.TotalTime += duration;
                                    _bug.LastTime = duration;
                                    if (duration > Quantum)
                                    {
                                        _bug.Overage += (duration - Quantum);
                                    }
                                    else if (_bug.Overage > 0 && !skippedTurn)
                                    {
                                        // If the animal ran under time, subtract this off of
                                        // their overage
                                        _bug.Overage -= (Quantum - duration);
                                        if (_bug.Overage < 0)
                                        {
                                            _bug.Overage = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    _bug.LastTime = duration;
                                    _bug.Overage = 0;
                                }

                                _bug.TotalActivations++;
                            }
                        }

                        if (_totalActivations > _lastReport + ReportInterval)
                        {
                            _lastReport = _totalActivations;
                        }
                    }
                }

                // Tell the UI thread we are done with this animal
                _animalReady.Reset();
                _animalDone.Set();
            }
        }

        private void startTimer()
        {
            if (_threadTimer != null)
                throw new InvalidOperationException("Thread Timer already exists");

            _threadTimer = new Timer(AbortThread, null, 1000, Timeout.Infinite);
        }

        private void cancelTimer()
        {
            if (_threadTimer == null) return;
            _timerStoppedTime = DateTime.Now;
            _threadTimer.Dispose();
            _threadTimer = null;
        }

        // This routine gets called by the _threadTimer timer when a certain
        // amount of time has elapsed.  If it fires, it means an animal has taken
        // way too long to execute and we should ThreadAbort the thread the 
        // animals run on to try to regain control of the game.
        public void AbortThread(object arg)
        {
            // Wait until the main thread is in a safe place to allow the abort
            lock (_monitor)
            {
                // See note on cancelTimer for why we need to check e.SignalTime > timerStoppedTime
                if (!PenalizeForTime || DateTime.Now <= _timerStoppedTime || !_safeToAbort) return;
                // If any other timers fire, they should not abort again until the activationThread says it's safe
                // again.  We only want one abort per "safe time".  Set safeToAbort to false to ensure this.
                _safeToAbort = false;
                _activationThread.Abort();
            }
        }

        /// <summary>
        /// Determines how much actual kernel and user time this thread has actually gotten
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private bool getAnimalThreadTime(out Int64 kernel, out Int64 user)
        {
            if (_threadHandleValid)
            {
                var c = new FILETIME();
                var e = new FILETIME();
                var kernelFileTime = new FILETIME();
                var userFileTime = new FILETIME();

                var success = (GetThreadTimes(_threadHandle, ref c, ref e, ref kernelFileTime, ref userFileTime) > 0);
                if (success)
                {
                    kernel = (((Int64) kernelFileTime.dwHighDateTime) << 32) + kernelFileTime.dwLowDateTime;
                    user = (((Int64) userFileTime.dwHighDateTime) << 32) + userFileTime.dwLowDateTime;
                    return true;
                }
                kernel = 0;
                user = 0;
                return false;
            }
            kernel = 0;
            user = 0;
            return false;
        }

        private static void traceEventHandler(object sender, params object[] items)
        {
            if (items.Length == 1)
            {
                var item = items[0].ToString();
                if (item.StartsWith("#"))
                {
                    return;
                }
            }

            for (var i = 0; i < items.Length; i++)
            {
                Trace.WriteLine(items[i]);
            }
        }

        /// <summary>
        ///  Used to get thread times for the current thread.  This enables
        ///  the Hosting code to time out creatures.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern int GetThreadTimes(
            IntPtr hThread,
            ref FILETIME lpCreationTime,
            ref FILETIME lpExitTime,
            ref FILETIME lpKernelTime,
            ref FILETIME lpUserTime
            );

        /// <summary>
        ///  Used to get the current thread handle for use with GetThreadTimes
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern IntPtr GetCurrentThread();

        /// <summary>
        ///  Used to get the current process handle for use with DuplicateHandle
        /// </summary>
        [DllImport("kernel32")]
        private static extern IntPtr GetCurrentProcess();

        /// <summary>
        ///  Used to duplicate the thread handle returned from GetCurrentThread
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        private static extern bool DuplicateHandle(
            IntPtr hSourceProcessHandle, // handle to source process
            IntPtr hSourceHandle, // handle to duplicate
            IntPtr hTargetProcessHandle, // handle to target process
            ref IntPtr lpTargetHandle, // duplicate handle
            int dwDesiredAccess, // requested access
            bool bInheritHandle, // handle inheritance option
            int dwOptions // optional actions
            );

        /// <summary>
        ///  Used to close all handles allocated by GetCurrentThread, GetCurrentProcess,
        ///  and DuplicateHandle
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        private static extern bool CloseHandle(IntPtr hObject);
    }
}