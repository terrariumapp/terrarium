using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using OrganismBase;
using Terrarium.Configuration;
using Terrarium.Forms;
using Terrarium.Hosting;
using Terrarium.PeerToPeer;
using Terrarium.Services.Species;
using Terrarium.Tools;

namespace Terrarium.Game
{
    /// <summary>
    ///  This class encapsulates the majority of the Terrarium gaming engine
    ///  that controls the creatures, updates of the world data, manipulation
    ///  of events, and other common engine features.
    ///  To see the basic game logic, look at the ProcessTurn() function.  This 
    ///  is what controls the flow of the game.
    /// </summary>
    public class GameEngine
    {
        private static bool _invalidPeerError;
        private static int _maxAnimals;
        private static int _maxPlants;
        private static int _organismQuanta;
        private static bool _reloadSettings = true;
        private static bool _shutdownError;
        private static int _worldHeight;
        private static int _worldWidth;

        /// <summary>
        ///  Provides static access to the current gaming engine.  This is
        ///  useful for quickly getting a handle to the game engine, but
        ///  makes it impossible to host multiple game engines in the same
        ///  process.
        /// </summary>
        private static GameEngine _engine;

        /// <summary>
        ///  Queue for managing the insertion of new creatures into the
        ///  game.  They are queue'd up and added to the world at a point that is "safe".
        /// </summary>
        private readonly Queue _newOrganismQueue = Queue.Synchronized(new Queue());

        private readonly PrivateAssemblyCache _pac;

        private readonly Random _random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        ///  Queue for managing the removal of creatures from the game.  Again, they are
        ///  queued and removed at a safe point.
        /// </summary>
        private readonly Queue _removeOrganismQueue = new Queue();

        /// <summary>
        ///  The game scheduler is responsible for managing creature time
        ///  slices.
        /// </summary>
        private readonly IGameScheduler _scheduler;

        private int _animalCount;
        private string _currentStateFileName;

        /// <summary>
        ///  This WorldVector object contains the current state of the world, as well as
        ///  the actions animals would like to perform to it to create the next state.
        /// </summary>
        private WorldVector _currentVector;

        private Boolean _ecosystemMode;

        /// <summary>
        ///  Array referencing each of the LED indicators within the
        ///  UI portion of the Terrarium.
        /// </summary>
        private TerrariumLed[] _ledIndicators;

        private Boolean _logState;

        private NetworkEngine _networkEngine;

        /// <summary>
        /// This is the state object that gets built up that eventually represents the
        /// next state the world will be in.
        /// </summary>
        private WorldState _newWorldState;

        /// <summary>
        ///  Listing of all organism IDs in the currentVector, used for enhancing enumeration
        ///  structures.
        /// </summary>
        private string[] _organismIDList;

        /// <summary>
        ///  Identifies the current count of Plant class creatures in the
        ///  game engine.
        /// </summary>
        private int _plantCount;

        private PopulationData _populationData;

        /// <summary>
        /// The processing of each actual turn of the game is broken down into steps so that we can paint the screen
        /// in between them.  turnPhase represents what step we are currently doing.
        /// </summary>
        private int _turnPhase;

        /// <summary>
        ///  Indicates whether or not the Network engine is going
        ///  to be used to enable a connected Terrarium.
        /// </summary>
        private bool _usingNetwork = true;

        /// <summary>
        ///  Constructs a new game engine.
        /// </summary>
        /// <param name="dataPath">The path to save game directory.</param>
        /// <param name="useNetwork">Controls the usage of the network engine.</param>
        /// <param name="deserializeState">Controls if the state is deserialized or not.</param>
        /// <param name="fileName">The path to the state file.</param>
        /// <param name="reportData">Determines if data should be reported.</param>
        /// <param name="leds">Provides a listing of game leds that can be used.</param>
        /// <param name="trackLastRun">Controls whether the PAC keeps track of the last run creature for blacklisting.</param>
        private GameEngine(string dataPath, bool useNetwork, bool deserializeState, string fileName, bool reportData,
                           TerrariumLed[] leds, bool trackLastRun)
        {
            _ledIndicators = leds;
            _currentStateFileName = fileName;

            // test to make sure we're not violating any constraints by current
            // physics settings in the engine.
            EngineSettings.EngineSettingsAsserts();

            // Calculate quanta and worldsize if we haven't done so yet
            if (_reloadSettings)
                CalculateWorldSize();

            _pac = new PrivateAssemblyCache(dataPath, fileName, true, trackLastRun);

            // Reset the appdomain policy since we changed the location of the organism dlls
            // This must be done before any animals are loaded in any way.  Make sure this call stays
            // as soon as possible
            AppDomain.CurrentDomain.SetAppDomainPolicy(SecurityUtils.MakePolicyLevel(_pac.AssemblyDirectory));

            _usingNetwork = useNetwork;
            _populationData = new PopulationData(reportData, leds[(int) LedIndicators.ReportWebService]);

            // Should only happen if an exception prevented a previous attempt to start a game
            if (AppMgr.CurrentScheduler != null)
            {
                AppMgr.DestroyScheduler();
            }

            // Create a scheduler that manages giving the creatures timeslices
            _scheduler = AppMgr.CreateSameDomainScheduler(this);
            _scheduler.Quantum = _organismQuanta;

            if (useNetwork)
            {
                // Required to start up the network listeners
                _networkEngine = new NetworkEngine();
            }

            WorldState currentState;
            Boolean successfulDeserialization = false;
            if (deserializeState && File.Exists(fileName))
            {
                try
                {
                    if (_pac.LastRun.Length != 0)
                    {
                        // The process was killed while an organism was being run, blacklist it
                        // Since this potentially means the animal hung the game.
                        _pac.BlacklistAssemblies(new string[] {_pac.LastRun});
                    }
                    this.deserializeState(fileName);
                    currentState = CurrentVector.State;
                    _scheduler.CurrentState = currentState;
                    _scheduler.CompleteOrganismDeserialization();
                    successfulDeserialization = true;
                }
                catch (Exception e)
                {
                    ErrorLog.LogHandledException(e);
                }
            }

            if (successfulDeserialization) return;
            
            // Set up initial world state
            currentState = new WorldState(GridWidth, GridHeight);
            currentState.TickNumber = 0;
            currentState.StateGuid = Guid.NewGuid();
            currentState.Teleporter = new Teleporter(AnimalCount/EngineSettings.NumberOfAnimalsPerTeleporter);
            currentState.MakeImmutable();

            WorldVector vector = new WorldVector(currentState);
            CurrentVector = vector;
            _scheduler.CurrentState = currentState;
        }

        /// <summary>
        /// NetworkEngine is a reference to the network object that does all peer to peer communication
        /// for this instance of the game.
        /// </summary>
        /// <returns>A reference to the network engine.</returns>
        public NetworkEngine NetworkEngine
        {
            get { return _networkEngine; }
        }

        /// <summary>
        ///  Provides a managed location for creature assemblies.
        /// </summary>
        public PrivateAssemblyCache Pac
        {
            get { return _pac; }
        }

        /// <summary>
        ///  Returns the number of peers from the network engine.
        /// </summary>
        public int PeerCount
        {
            get { return _usingNetwork && _networkEngine != null ? _networkEngine.PeerCount : 0; }
        }

        /// <summary>
        ///  Gets the current peer channel from the network engine.  Also
        ///  allows the user to change the peer channel and network with
        ///  a different set of machines.
        /// </summary>
        public string PeerChannel
        {
            get { return _usingNetwork && _networkEngine != null ? _networkEngine.PeerChannel : "<not networked>"; }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    if (_usingNetwork && _networkEngine != null)
                    {
                        _networkEngine.ShutdownNetwork();
                        _networkEngine = null;
                        _usingNetwork = false;
                    }
                }
                else
                {
                    if (_usingNetwork && _networkEngine != null)
                    {
                        _networkEngine.ShutdownNetwork();
                        _networkEngine = null;
                    }

                    _usingNetwork = true;
                    _networkEngine = new NetworkEngine();
                    _networkEngine.InitializeNetwork(value, _ledIndicators);
                }
            }
        }

        /// <summary>
        ///  Determines if the engine is in EcoSystem mode or some other mode.
        /// </summary>
        public Boolean EcosystemMode
        {
            get { return _ecosystemMode; }
        }

        /// <summary>
        ///  Returns the name of the current state file.
        /// </summary>
        public string FileName
        {
            get { return _currentStateFileName; }

            set { _currentStateFileName = value; }
        }

        /// <summary>
        ///  Determines if game state should be logged after each tick.
        ///  This is useful for debugging mode only.
        /// </summary>
        public Boolean LogState
        {
            get { return _logState; }

            set { _logState = value; }
        }

        /// <summary>
        ///  Provides access to the game scheduler for scheduling creatures.
        /// </summary>
        public IGameScheduler Scheduler
        {
            get { return _scheduler; }
        }

        /// <summary>
        ///  Provides access to the current world vector
        /// </summary>
        public WorldVector CurrentVector
        {
            get { return _currentVector; }

            set
            {
                WorldVector oldVector = _currentVector;
                _currentVector = value;
                OnWorldVectorChanged(new WorldVectorChangedEventArgs(oldVector, _currentVector));
            }
        }

        /// <summary>
        ///  Static access to the current game engine.
        /// </summary>
        public static GameEngine Current
        {
            get { return _engine; }
        }

        /// <summary>
        ///  Used to notify the game engine that a shutdown level
        ///  error has occured.
        /// </summary>
        public static bool ShutdownError
        {
            get { return _shutdownError; }

            set
            {
                lock (typeof (GameEngine))
                {
                    _shutdownError = value;
                }
            }
        }

        /// <summary>
        ///  Used to notify the game engine that the peer is not
        ///  able to play the game networked.
        /// </summary>
        public static bool InvalidPeerError
        {
            get { return _invalidPeerError; }

            set
            {
                lock (typeof (GameEngine))
                {
                    _invalidPeerError = value;
                }
            }
        }

        /// <summary>
        ///  Provides access to the population data used
        ///  for reporting.
        /// </summary>
        public PopulationData PopulationData
        {
            get { return _populationData; }
        }

        /// <summary>
        ///  Provides the height of the given world in game units (pixels)
        /// </summary>
        public int WorldHeight
        {
            get { return _worldHeight; }
        }

        /// <summary>
        ///  Provides the width of the given world in game units (pixels)
        /// </summary>
        public int WorldWidth
        {
            get { return _worldWidth; }
        }

        /// <summary>
        ///  Converts world width to grid cells and returns the number of
        ///  grid cells.
        /// </summary>
        public int GridWidth
        {
            get { return WorldWidth >> EngineSettings.GridWidthPowerOfTwo; }
        }

        /// <summary>
        ///  Converts world height to grid cells and returns the number of
        ///  grid cells.
        /// </summary>
        public int GridHeight
        {
            get { return WorldHeight >> EngineSettings.GridHeightPowerOfTwo; }
        }

        /// <summary>
        ///  Identifies the maximum amount of Animal class creatures allowed
        ///  in the game engine.
        /// </summary>
        public int MaxAnimals
        {
            get { return _maxAnimals; }
        }

        /// <summary>
        ///  Identifies the maximum amount of Plant class creatures allowed
        ///  in the game engine.
        /// </summary>
        public static int MaxPlants
        {
            get { return _maxPlants; }
        }

        /// <summary>
        ///  Identifies the current count of Animal class creatures in the
        ///  game engine.
        /// </summary>
        public int AnimalCount
        {
            get { return _animalCount; }
        }

        /// <summary>
        ///  The current turn phase.  There are ten phases per tick.
        /// </summary>
        public int TurnPhase
        {
            get { return _turnPhase; }
        }

        /// <summary>
        /// Used to determine if the network is running
        /// </summary>
        public bool IsNetworkEnabled
        {
            get { return _usingNetwork; }
        }

        /// <summary>
        ///  Event that can be handled by clients that notifies them when
        ///  the world vector is changed.
        /// </summary>
        public event WorldVectorChangedEventHandler WorldVectorChanged;

        /// <summary>
        ///  Event that can be handled by clients to be notified whenever
        ///  some interesting state in the engine has changed. Used for notifications in 
        ///  the UI.
        /// </summary>
        public event EngineStateChangedEventHandler EngineStateChanged;

        /// <summary>
        ///  Stops the game and determines if the final tick of data should be serialized.
        /// </summary>
        /// <param name="serializeState">Controls if the state is serialized.</param>
        public void StopGame(Boolean serializeState)
        {
            // This needs to be the first call in stop game because if it fails, the game needs to be able
            // to continue.  In addition, it may jump the engine ahead a few phases, and if the engine isn't
            // in the normal running state this could cause things to fail since the engine phases are expecting
            // things to be in a normal state.
            if (serializeState)
            {
                this.serializeState(_currentStateFileName);
            }

            if (_usingNetwork && _networkEngine != null)
            {
                _networkEngine.ShutdownNetwork();
                _networkEngine = null;
                _usingNetwork = false;
            }

            // Shut down populationData after serializing since serializing
            // may advance to the end of the tick
            if (_populationData != null)
            {
                _populationData.Close();
                _populationData = null;
            }

            AppMgr.DestroyScheduler();
            _pac.Close();
            _ledIndicators = null;
            _engine = null;
        }

        /// <summary>
        /// This routine attempts to figure out how fast the machine is by
        /// running some standardized tests to test code execution speed.  It
        /// then uses this information to determine how big the Terrarium world
        /// should be on this machine to maintain a decent frame rate. It also
        /// determines how big of a time slice we should give each animal.
        /// </summary>
        private void CalculateWorldSize()
        {
            const int Modifier = 40;

            // We need to get all the objects loaded and initialized.
            OrganismQuanta.TestAnimal(10);

            // Run tests
            OrganismQuanta.Clear();
            OrganismQuanta.TestAnimal(100);

            Trace.WriteLine(String.Format("Total {0}, Last {1}, Best {2}, Worst {3}, Average {4}",
                                          new Object[]
                                              {
                                                  OrganismQuanta.totalQuanta*Modifier,
                                                  OrganismQuanta.lastQuanta*Modifier,
                                                  OrganismQuanta.bestQuanta*Modifier,
                                                  OrganismQuanta.worstQuanta*Modifier,
                                                  (OrganismQuanta.totalQuanta*Modifier/OrganismQuanta.samples)
                                              }
                                ));

            // Figure out how much of a time slice to give each animal
            _organismQuanta = (int) (OrganismQuanta.totalQuanta/OrganismQuanta.samples);
            if (_organismQuanta < 500)
            {
                _organismQuanta *= Modifier;
            }
            Debug.Assert(_organismQuanta < EngineSettings.OrganismSchedulingMaximumOvertime,
                         "The computer is too slow to run Terrarium or obtained bad results from OrganismQuanta");

            estimateNumberOfAnimalsToSupport();
            computeWorldHeight();

            // We only need to normalize to GridCellHeight and GridCellWidth.  The
            // game view may add some extra pixels on the bottom and right of the
            // screen for tiling purposes, but this never hurts the game.  In this
            // way we don't need to know the size of the world tiles in the game
            // engine.
            if (_worldWidth%EngineSettings.GridCellWidth != 0)
            {
                _worldWidth += EngineSettings.GridCellWidth - (_worldWidth%EngineSettings.GridCellWidth);
            }

            if (_worldHeight%EngineSettings.GridCellHeight != 0)
            {
                _worldHeight += EngineSettings.GridCellHeight - (_worldHeight%EngineSettings.GridCellHeight);
            }

            // Worldheight and Height must be evenly divisible by our grid cells
            Debug.Assert(WorldHeight%EngineSettings.GridCellHeight == 0 &&
                         WorldWidth%EngineSettings.GridCellWidth == 0);

            // if we actually calculate it, tell the engine not to redo it since it only needs to be done once
            // if this gets changed because we load a file, we'll do it again
            _reloadSettings = false;
        }

        /// <summary>
        /// Now lets compute worldheight
        /// Each animal needs a certain amount of territory so we can compute a density.
        /// 10,000 pixels per plant/animal is the key at this point.
        /// </summary>
        private static void computeWorldHeight()
        {
            _worldHeight = (int) (Math.Sqrt(10000*(_maxAnimals + _maxPlants)) + 1);
            if (_worldHeight < 1024)
            {
                _worldHeight = 1024; // ensures at least an 800x800 area for viewing purposes
            }

            double a = _worldHeight/1024.0;

            a = Math.Round(a + 0.5);

            _worldHeight = 1024*(int) a;

            _worldWidth = _worldHeight;
        }

        /// <summary>
        /// This is where we attempt to estimate how many animals we can support on this machine.
        /// We assume we want 20 frames a second and we do two engine ticks per second which means we have
        /// 500 msec per engine tick.  We budget our paint time 200 mSec, and that leaves 300 mSec for all engine
        /// processing.
        /// We assume we'll allocate 4/5 of our available engine processing time to running animals, and 1/5 to plants.
        /// organismQuanta is how many microseconds each animal will get, so we have to divide by 1000
        /// to get milliseconds.
        /// We assume that we can use all engine processing time for running animals although this isn't strictly true
        /// since we need to run the engine code too.  It's OK, because animals can't use all their allocated time every
        /// tick -- they'll eventually get killed. Thus, we use the time they don't use for the engine processing.
        /// It's all basically an estimate, that we've tuned and works pretty well.
        /// </summary>
        private static void estimateNumberOfAnimalsToSupport()
        {
            float baseTime = (300/5)/((_organismQuanta)/((float) 1000));
            _maxAnimals = (int) (baseTime*4);
            _maxAnimals = (int) (_maxAnimals*(GameConfig.CpuThrottle/(double) 100));
            _maxPlants = _maxAnimals;
        }

        /// <summary>
        ///  Creates a new game engine that can be used for a Terrarium game.  Terrarium
        ///  games aren't networked and can load any creatures.
        /// </summary>
        /// <param name="dataPath">The path where the Terrarium game will be stored.</param>
        /// <param name="fileName">The path to the serialized Terrarium.</param>
        /// <param name="leds">A series of leds to be used for state reporting.</param>
        public static void NewTerrariumGame(string dataPath, string fileName, TerrariumLed[] leds)
        {
            if (_engine != null)
            {
                _engine.StopGame(false);
            }

            _engine = new GameEngine(dataPath, false, false, fileName, false, leds, true);
        }

        /// <summary>
        ///  Creates a game engine that loads an existing Terrarium game save state.
        /// </summary>
        /// <param name="dataPath">The path where the Terrarium game will be stored.</param>
        /// <param name="fileName">The path to the serialized Terrarium.</param>
        /// <param name="leds">A series of leds to be used for state reporting.</param>
        public static void LoadTerrariumGame(string dataPath, string fileName, TerrariumLed[] leds)
        {
            if (_engine != null)
            {
                _engine.StopGame(false);
            }

            _engine = new GameEngine(dataPath, false, true, fileName, false, leds, true);
        }

        /// <summary>
        ///  Creates a new game engine that can be used to play an EcoSystem game.
        /// </summary>
        /// <param name="dataPath">The path where the Terrarium game will be stored.</param>
        /// <param name="fileName">The path to the serialized Terrarium.</param>
        /// <param name="leds">A series of leds to be used for state reporting.</param>
        public static void NewEcosystemGame(string dataPath, string fileName, TerrariumLed[] leds)
        {
            if (_engine != null)
            {
                _engine.StopGame(false);
            }

            _engine = new GameEngine(dataPath, true, false, dataPath + fileName, true, leds, true);

            // Start the network after Current is set on GameEngine because the network
            // needs a current gameengine to receive teleportations
            if (_engine._usingNetwork)
            {
                _engine._networkEngine.InitializeNetwork("EcoSystem", leds);
            }

            _engine._ecosystemMode = true;
        }

        /// <summary>
        ///  Creates a new game engine that can be used to load a pre-existing
        ///  EcoSystem game.
        /// </summary>
        /// <param name="dataPath">The path where the Terrarium game will be stored.</param>
        /// <param name="fileName">The path to the serialized Terrarium.</param>
        /// <param name="leds">A series of leds to be used for state reporting.</param>
        public static void LoadEcosystemGame(string dataPath, string fileName, TerrariumLed[] leds)
        {
            if (_engine != null)
            {
                _engine.StopGame(false);
            }

            _engine = new GameEngine(dataPath, true, true, dataPath + fileName, true, leds, true);

            // Start the network after Current is set on GameEngine because the network
            // needs a current gameengine to receive teleportations
            if (_engine._usingNetwork)
            {
                _engine._networkEngine.InitializeNetwork("EcoSystem", leds);
            }

            _engine._ecosystemMode = true;
        }

        /// <summary>
        ///  Helper function for the WorldVectorChanged event to fire
        ///  the event given a new WorldVectorChangedEventArgs.
        /// </summary>
        /// <param name="e">References to the new and old vectors.</param>
        public void OnWorldVectorChanged(WorldVectorChangedEventArgs e)
        {
            if (WorldVectorChanged != null)
            {
                WorldVectorChanged(null, e);
            }
        }

        /// <summary>
        ///  Helper function for the EngineStateChanged event to fire
        ///  the event given a new EngineStateChangedEventArgs
        /// </summary>
        /// <param name="e">Properties identifying the change to the engine.</param>
        public void OnEngineStateChanged(EngineStateChangedEventArgs e)
        {
            if (EngineStateChanged != null)
            {
                EngineStateChanged(null, e);
            }
        }

        /// <summary>
        ///  Adds another organism to the count of organisms.  Depending on the organism
        ///  type either plants or animals will be incremented.
        /// </summary>
        /// <param name="state">The state of the creature.</param>
        /// <param name="reason">The reason the creature is being added.</param>
        private void countOrganism(OrganismState state, PopulationChangeReason reason)
        {
            Debug.Assert(state != null);

            _populationData.CountOrganism(((Species) state.Species).Name, reason);
            if (state is AnimalState)
            {
                _animalCount++;
            }
            else
            {
                _plantCount++;
            }
        }

        /// <summary>
        ///  Subtracts an organism from the count of organisms.  Depending on the 
        ///  organism type either plants or animals will be decremented.
        /// </summary>
        /// <param name="state">The state of the creature.</param>
        /// <param name="reason">The reason the creature is being removed.</param>
        private void uncountOrganism(OrganismState state, PopulationChangeReason reason)
        {
            Debug.Assert(state != null);

            _populationData.UncountOrganism(((Species) state.Species).Name, reason);
            if (state is AnimalState)
            {
                _animalCount--;
            }
            else
            {
                _plantCount--;
            }
        }

        /// <summary>
        ///  Serializes the game state to the given path.
        /// </summary>
        /// <param name="path">Path to the state file.</param>
        private void serializeState(string path)
        {
            // We should only serialize at the beginning of the 10 state turnphase so everything
            // can be started at zero when we deserialize
            while (_turnPhase != 0)
            {
                try
                {
                    ProcessTurn();
                }
                catch (GameEngineException e)
                {
                    // The game will throw exceptions like StateTimeOut, Blacklist, etc during process turn
                    // Since we are serializing, ignore them all. We will hit them again when we report again
                    ErrorLog.LogHandledException(e);
                }
            }

            BinaryFormatter b = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);
            try
            {
                b.Serialize(stream, WorldWidth);
                b.Serialize(stream, WorldHeight);
                b.Serialize(stream, MaxPlants);
                b.Serialize(stream, MaxAnimals);

                b.Serialize(stream, _plantCount);
                b.Serialize(stream, AnimalCount);
                b.Serialize(stream, CurrentVector);
                Scheduler.SerializeOrganisms(stream);
            }
            finally
            {
                stream.Close();
            }
        }

        /// <summary>
        ///  Deserializes the game state from the given path.
        /// </summary>
        /// <param name="path">Path to the serialized game state.</param>
        private void deserializeState(string path)
        {
            // We should only deserialize at the beginning of the 10 state turnphase because
            // that's when we serialized
            while (_turnPhase != 0)
            {
                try
                {
                    ProcessTurn();
                }
                catch (GameEngineException e)
                {
                    // The game will throw exceptions like StateTimeOut, Blacklist, etc during process turn
                    // Since we are serializing, ignore them all, we will hit them again when we report again
                    ErrorLog.LogHandledException(e);
                }
            }

            BinaryFormatter b = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            try
            {
                _worldWidth = (int) b.Deserialize(stream);
                _worldHeight = (int) b.Deserialize(stream);
                _maxPlants = (int) b.Deserialize(stream);
                _maxAnimals = (int) b.Deserialize(stream);

                // Since we may be changing our settings to non-optimal settings, tell the engine to recalculate
                // them next time
                _reloadSettings = true;

                _plantCount = (int) b.Deserialize(stream);
                _animalCount = (int) b.Deserialize(stream);
                CurrentVector = (WorldVector) b.Deserialize(stream);
                Scheduler.DeserializeOrganisms(stream);

                PopulationData.BeginTick(CurrentVector.State.TickNumber, CurrentVector.State.StateGuid);
                foreach (OrganismState state in CurrentVector.State.Organisms)
                {
                    PopulationData.CountOrganism(((Species) state.Species).Name, PopulationChangeReason.Initial);
                }
                PopulationData.EndTick(CurrentVector.State.TickNumber);
            }
            catch (Exception e)
            {
                // If we hit a problem, reset everything
                _plantCount = 0;
                _animalCount = 0;
                CurrentVector = null;

                // The scheduler should reset itself if it encounters an exception, so we don't need
                // to do anything for it here
                Trace.WriteLine("State could not be deserialized from " + path + ": " + e.Message + "\r\n" +
                                e.StackTrace);
                throw;
            }
            finally
            {
                stream.Close();
            }
        }

        /// <summary>
        ///  Finds a location within the current game world where no organisms exist.
        /// </summary>
        /// <param name="cellRadius">The cell radius of the new creature to be inserted.</param>
        /// <param name="preferredGridPoint">The perferred location for the new creature.</param>
        /// <returns>The empty point where the creature can be inserted.</returns>
        private Point findEmptyPosition(int cellRadius, Point preferredGridPoint)
        {
            Point newLocation = preferredGridPoint == Point.Empty
                                    ? new Point(_random.Next(cellRadius, GridWidth - 1 - cellRadius),
                                                _random.Next(cellRadius, GridHeight - 1 - cellRadius))
                                    : preferredGridPoint;

            int retry = 20;
            while (retry > 0 &&
                   _newWorldState.FindOrganismsInCells(newLocation.X - cellRadius, newLocation.X + cellRadius,
                                                      newLocation.Y - cellRadius, newLocation.Y + cellRadius).Count != 0)
            {
                newLocation = new Point(_random.Next(cellRadius, GridWidth - 1 - cellRadius),
                                        _random.Next(cellRadius, GridHeight - 1 - cellRadius));
                retry--;
            }

            // If we couldn't find a spot delete the organism
            return retry == 0
                       ? Point.Empty
                       : new Point(newLocation.X << EngineSettings.GridWidthPowerOfTwo,
                                   newLocation.Y << EngineSettings.GridHeightPowerOfTwo);
        }

        /// <summary>
        ///  Receieves a teleported state either from a remote peer or the local peer.
        /// </summary>
        /// <param name="teleportedObject">The teleported object.</param>
        /// <param name="teleportedToSelf">Whether the teleportation is local or from a remote peer.</param>
        public void ReceiveTeleportation(Object teleportedObject, bool teleportedToSelf)
        {
            if (null == teleportedObject)
            {
                throw new Exception("Null object passed into ReceiveTeleportation");
            }

            TeleportState teleportState = (TeleportState) teleportedObject;
            teleportState.TeleportedToSelf = teleportedToSelf;
            _newOrganismQueue.Enqueue(teleportState);
        }

        /// <summary>
        ///  Can be called with an OrganismState that needs to be teleported.
        ///  Cannot be called during MoveAnimals since it passes true to the clearOld
        ///  argument in RemoveOrganism.
        /// </summary>
        /// <param name="state">The state of the creature to be teleported.</param>
        public void Teleport(OrganismState state)
        {
            if (null == state)
            {
                throw new Exception("Null object passed into Teleport");
            }

            TeleportState teleportState = new TeleportState();

            Organism organism = Scheduler.GetOrganism(state.ID);
            state.MakeImmutable();
            teleportState.OrganismState = state;
            teleportState.Organism = organism;
            teleportState.OrganismWrapper = new OrganismWrapper(organism);
            teleportState.Originator = _newWorldState.StateGuid;
            teleportState.Country = GameConfig.UserCountry;
            teleportState.State = GameConfig.UserState;

            // Remove it from this world
            removeOrganism(new KilledOrganism(state.ID, PopulationChangeReason.TeleportedFrom));

            // Teleport the organism through the network engine
            if (_usingNetwork)
            {
                _networkEngine.Teleport(teleportState);
            }
            else
            {
                ReceiveTeleportation(teleportState, true);
            }
        }

        /// <summary>
        ///  Add a new organism given the species of the creature, and the
        ///  preferred location of insertion.
        /// </summary>
        /// <param name="species">The species for the new organism.</param>
        /// <param name="preferredLocation">The preferred insertion point.</param>
        public void AddNewOrganism(Species species, Point preferredLocation)
        {
            NewOrganism newOrganism;
            if (preferredLocation == Point.Empty)
            {
                newOrganism = new NewOrganism(species.InitializeNewState(new Point(0, 0), 0), null);
            }
            else
            {
                newOrganism = new NewOrganism(species.InitializeNewState(preferredLocation, 0), null);
                newOrganism.AddAtRandomLocation = false;
            }

            _newOrganismQueue.Enqueue(newOrganism);
        }

        /// <summary>
        ///  Add a new organism to the terrarium using the given assembly to generate
        ///  a species from, a preferred insertion point, and whether this is a reintroduction
        ///  or not.
        /// </summary>
        /// <param name="assemblyPath">The path to the assembly used for this creature.</param>
        /// <param name="preferredLocation">The preferred point of insertion.</param>
        /// <param name="reintroduction">Controls if this is a reintroduction.</param>
        /// <returns>A species object for the new organism.</returns>
        public Species AddNewOrganism(String assemblyPath, Point preferredLocation, Boolean reintroduction)
        {
            string fullPath = Path.GetFullPath(assemblyPath);
            string reportPath = Path.Combine(GameConfig.ApplicationDirectory, Guid.NewGuid() + ".xml");
            int validAssembly = PrivateAssemblyCache.checkAssemblyWithReporting(fullPath, reportPath);

            if (validAssembly == 0)
            {
                throw OrganismAssemblyFailedValidationException.GenerateExceptionFromXml(reportPath);
            }

            if (File.Exists(reportPath))
            {
                File.Delete(reportPath);
            }

            byte[] asm;
            using (FileStream sourceStream = File.OpenRead(assemblyPath))
            {
                if (sourceStream.Length > (100*1024))
                {
                    throw new GameEngineException(
                        "Your organism is greater than 100k in size.  Please try to reduce the size of your assembly.");
                }
                asm = new byte[sourceStream.Length];
                sourceStream.Read(asm, 0, (int) sourceStream.Length);
            }

            // Load the debugging information if it exists
            byte[] asmSymbols = null;
            if (File.Exists(Path.ChangeExtension(assemblyPath, ".pdb")))
            {
                using (FileStream sourceStream = File.OpenRead(Path.ChangeExtension(assemblyPath, ".pdb")))
                {
                    asmSymbols = new byte[sourceStream.Length];
                    sourceStream.Read(asmSymbols, 0, (int) sourceStream.Length);
                }
            }

            // Actually load the assembly from the bytes
            Species newSpecies;
            Assembly organismAssembly;
            try
            {
                organismAssembly = asmSymbols != null ? Assembly.Load(asm, asmSymbols) : Assembly.Load(asm);

                // Make sure organism isn't signed with Terrarium key (e.g. has full trust)
                if (SecurityUtils.AssemblyHasTerrariumKey(organismAssembly.GetName()))
                {
                    throw new GameEngineException("You can't introduce assemblies signed with the Terrarium key");
                }

                newSpecies = Species.GetSpeciesFromAssembly(organismAssembly);
            }
                // Catch two common fusion errors that occur if you build against the wrong
                // organismbase.dll
            catch (FileNotFoundException e)
            {
                if (Path.GetFileName(e.FileName).ToLower(CultureInfo.InvariantCulture) == "organismbase.dll")
                {
                    throw new GameEngineException(
                        "Your organism is built against the wrong version of organismbase.dll.  Build using the version in '" +
                        Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "'.");
                }
                throw;
            }
            catch (FileLoadException e)
            {
                if (Path.GetFileName(e.FileName).ToLower(CultureInfo.InvariantCulture).StartsWith("organismbase"))
                {
                    throw new GameEngineException(
                        "The organism was built using a different version of organismbase.dll than the current client.  You may need to change servers to one that supports the build of the client you are running.");
                }
                throw;
            }
            catch (BadImageFormatException)
            {
                throw new GameEngineException(
                    "Your organism doesn't appear to be a valid .NET Framework assembly.  It is possible you're loading an old assembly.");
            }

            SpeciesServiceStatus speciesResult = SpeciesServiceStatus.Success;
            if (!reintroduction)
            {
                // Load the species to make sure that its attributes conform to our rules (will throw an
                // exception if this isn't true
                SpeciesService service = new SpeciesService();
                service.Url = GameConfig.WebRoot + "/species/addspecies.asmx";
                service.Timeout = 60000;
                if (EcosystemMode)
                {
                    // If we're in ecosystem mode,
                    // make sure it has a unique assembly name (which is also the species name)
                    string speciesType;
                    if (newSpecies is AnimalSpecies)
                    {
                        speciesType = ((AnimalSpecies) newSpecies).IsCarnivore ? "Carnivore" : "Herbivore";
                    }
                    else
                    {
                        speciesType = "Plant";
                    }

                    if (!Regex.IsMatch(newSpecies.AssemblyInfo.ShortName, "[0-9a-zA-Z ]+"))
                    {
                        throw new GameEngineException(
                            "The assembly name of your organism must consist only of ASCII letters, digits, and spaces.");
                    }

                    speciesResult = service.Add(newSpecies.AssemblyInfo.ShortName,
                                                Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                                                speciesType, newSpecies.AuthorName, newSpecies.AuthorEmail,
                                                newSpecies.AssemblyInfo.FullName, asm);
                }
            }

            if (reintroduction || speciesResult == SpeciesServiceStatus.Success)
            {
                if (!_pac.Exists(organismAssembly.FullName))
                {
                    _pac.SaveOrganismAssembly(assemblyPath, Path.ChangeExtension(assemblyPath, ".pdb"),
                                             organismAssembly.FullName);
                }
                else if (!reintroduction)
                {
                    // If this is not a reintroduction, then the assembly should not be there.  If it is, we can't
                    // ignore it because it could be a user testing out their organism in terrarium mode and if they
                    // introduce twice with two different assemblies, they'll all end up using the one that's already there
                    // and not the new one if they save and reload.
                    if (!EcosystemMode)
                    {
                        throw new GameEngineException(
                            "Organism assemblies can only be introduced once into a Terrarium. \r\n - If you are just trying to add more of these animals, select your animal in the 'Species' dropdown below and push the 'add' button.\r\n - If you are developing an animal and have a new assembly to try, create a new terrarium and introduce it there.");
                    }
                    // The server should prevent
                    Debug.Assert(false);
                    throw new GameEngineException(
                        "Can't add this species because you already have an assembly with the same name in your Ecosystem.");
                }

                newSpecies = Species.GetSpeciesFromAssembly(_pac.LoadOrganismAssembly(organismAssembly.FullName));

                for (int i = 0; i < 10; i++)
                {
                    AddNewOrganism(newSpecies, Point.Empty);
                }
            }
            else switch (speciesResult)
            {
                case SpeciesServiceStatus.AlreadyExists:
                    throw new GameEngineException(
                        "Species name already exists in universe, please choose a new assembly name.");
                case SpeciesServiceStatus.FiveMinuteThrottle:
                    throw new GameEngineException(
                        "You have submitted another species within the last 5 minutes.  Please wait at least 5 minutes between adding new species.");
                case SpeciesServiceStatus.TwentyFourHourThrottle:
                    throw new GameEngineException(
                        "You have submitted 30 species within the last 24 hours.  You now have to wait up to 24 hours to introduce a new species.");
                case SpeciesServiceStatus.PoliCheckSpeciesNameFailure:
                    throw new GameEngineException(
                        "Your Species Name has failed our basic check for inflammatory terms.  Please resubmit using a new name, or if your name was flagged in error, please try using initials or other monikers.");
                case SpeciesServiceStatus.PoliCheckAuthorNameFailure:
                    throw new GameEngineException(
                        "Your Author Name has failed our basic check for inflammatory terms.  Please resubmit using a new name, or if your name was flagged in error, please try using initials or other monikers.");
                case SpeciesServiceStatus.PoliCheckEmailFailure:
                    throw new GameEngineException(
                        "Your Email has failed our basic check for inflammatory terms.  Please resubmit using a different email address.");
                default:
                    throw new GameEngineException("Terrarium is experience some server problems.  Please try again later.");
            }

            return newSpecies;
        }

        /// <summary>
        ///  Inserts all new organisms from the queue when it is safe to insert
        ///  them.  This happens so that all new organism are inserted at the
        ///  same time in a serial manner rather than in multiple phases.
        /// </summary>
        private void insertOrganismsFromQueue()
        {
            while (_newOrganismQueue.Count > 0)
            {
                Object queueObject = _newOrganismQueue.Dequeue();
                if (typeof (TeleportState).IsAssignableFrom(queueObject.GetType()))
                {
                    // Teleported organism
                    TeleportState teleportState = (TeleportState) queueObject;
                    if (teleportState.TeleportedToSelf)
                    {
                        // Make sure this wasn't teleported from a previous game on this machine
                        // if it was, destroy the organism by not handling it
                        if (teleportState.Originator != _newWorldState.StateGuid)
                        {
                            OnEngineStateChanged(new EngineStateChangedEventArgs(EngineStateChangeType.Other,
                                                                                 "Teleport attempted to an old game.",
                                                                                 "A teleport was attempted to an old game, the organism was lost in space."));
                            continue;
                        }
                    }

                    Organism organism;
                    try
                    {
                        organism = teleportState.TeleportedToSelf
                                       ? teleportState.Organism
                                       : teleportState.OrganismWrapper.Organism;
                    }
                    catch (FileLoadException e)
                    {
                        // Here we want to log how many organisms are getting killed
                        // FileLoadException will happen when an organism is blacklisted, so don't show a message because the message contains the name
                        ErrorLog.LogHandledException(e);
                        OnEngineStateChanged(new EngineStateChangedEventArgs(EngineStateChangeType.Other,
                                                                             "An organism died during teleportation"));
                        continue;
                    }
                    catch (Exception e)
                    {
                        // Here we want to log how many organisms are getting killed
                        ErrorLog.LogHandledException(e);
                        OnEngineStateChanged(new EngineStateChangedEventArgs(EngineStateChangeType.Other,
                                                                             "An organism died during teleportation: " +
                                                                             e.Message));
                        continue;
                    }

                    OrganismState organismState = teleportState.OrganismState;

                    // Make it mutable and change its position
                    organismState = organismState.CloneMutable();
                    Point newPosition = findEmptyPosition(organismState.CellRadius, Point.Empty);

                    if (newPosition != Point.Empty)
                    {
                        organismState.Position = newPosition;
                    }
                    else
                    {
                        // We couldn't find a place to put it, kill the organism
                        OnEngineStateChanged(new EngineStateChangedEventArgs(EngineStateChangeType.Other,
                                                                             "A '" +
                                                                             ((Species) organismState.Species).Name +
                                                                             "' died during teleportation: couldn't find an open spot quickly enough."));
                        continue;
                    }

                    try
                    {
                        // Host it and give it state in a random location
                        _scheduler.Add(organism, organismState.ID);
                    }
                    catch (OrganismAlreadyExistsException e)
                    {
                        // An organism can get duplicated if a peer teleports it, and then shuts down
                        // unexpectedly, restarts and the organism is alive on it again (because it reverts
                        // to the last saved state)
                        ErrorLog.LogHandledException(e);
                        continue;
                    }

                    try
                    {
                        if (!teleportState.TeleportedToSelf)
                        {
                            if (organism is Animal)
                            {
                                ((Animal) organism).DeserializeAnimal(organism.SerializedStream);
                            }
                            else
                            {
                                ((Plant) organism).DeserializePlant(organism.SerializedStream);
                            }
                        }
                        organism.SerializedStream = null;
                    }
                    catch (Exception e)
                    {
                        OnEngineStateChanged(new EngineStateChangedEventArgs(EngineStateChangeType.Other,
                                                                             "A '" +
                                                                             ((Species) organismState.Species).Name +
                                                                             "' lost its memory during teleportation: " +
                                                                             e.Message));
                    }

                    _newWorldState.AddOrganism(organismState);

                    // Tell the organism it was teleported
                    organismState.OrganismEvents.Teleported = new TeleportedEventArgs(teleportState.TeleportedToSelf);

                    countOrganism(organismState, PopulationChangeReason.TeleportedTo);

                    //The GameEngine state has changed so raise an event to that effect
                    if (!teleportState.TeleportedToSelf)
                    {
                        OnEngineStateChanged(EngineStateChangedEventArgs.AnimalArrived(organismState));
                    }
                }
                else
                {
                    // new organism
                    NewOrganism newOrganism = (NewOrganism) queueObject;

                    OrganismState organismState = newOrganism.State;
                    Point newPosition = newOrganism.AddAtRandomLocation
                                            ?
                                                findEmptyPosition(organismState.CellRadius, Point.Empty)
                                            :
                                                findEmptyPosition(organismState.CellRadius,
                                                                  new Point(
                                                                      organismState.Position.X >>
                                                                      EngineSettings.GridWidthPowerOfTwo,
                                                                      organismState.Position.Y >>
                                                                      EngineSettings.GridHeightPowerOfTwo));

                    if (newPosition != Point.Empty)
                    {
                        organismState.Position = newPosition;
                    }
                    else
                    {
                        OnEngineStateChanged(new EngineStateChangedEventArgs(EngineStateChangeType.Other,
                                                                             "A '" +
                                                                             ((Species) organismState.Species).Name +
                                                                             "' died at birth: not enough space in the world."));
                        continue;
                    }

                    // Host it and Give it state in a random location
                    Debug.Assert(((Species) organismState.Species).Type != null,
                                 "Type is null on organism '" + ((Species) organismState.Species).Name + "'");
                    try
                    {
                        _scheduler.Create(((Species) organismState.Species).Type, organismState.ID);
                    }
                    catch (Exception e)
                    {
                        ErrorLog.LogHandledException(e);
                        OnEngineStateChanged(new EngineStateChangedEventArgs(EngineStateChangeType.Other,
                                                                             "An organism died at birth: " + e.Message));
                        continue;
                    }
                    _newWorldState.AddOrganism(organismState);

                    // Tell the organism it was born
                    organismState.OrganismEvents.Born = new BornEventArgs(newOrganism.Dna);
                    countOrganism(organismState, PopulationChangeReason.Born);
                }
            }
        }

        /// <summary>
        ///  Method used to add an organism to the organism removal queue.
        /// </summary>
        /// <param name="killedOrganism">The organism to remove.</param>
        public void RemoveOrganismQueued(KilledOrganism killedOrganism)
        {
            // Queue it up to be removed at the proper point
            _removeOrganismQueue.Enqueue(killedOrganism);
        }

        /// <summary>
        ///  Method used to remove organisms that have been queued for removal.
        /// </summary>
        private void removeOrganismsFromQueue()
        {
            while (_removeOrganismQueue.Count > 0)
            {
                removeOrganism((KilledOrganism) _removeOrganismQueue.Dequeue());
            }
        }

        /// <summary>
        ///  Method used to remove an organism given a KilledOrganism object.
        ///  This method instantly removes the organism and so should not
        ///  be called by normal code, rather call RemoveOrganismQueued.
        /// </summary>
        /// <param name="killedOrganism">The organism to be removed.</param>
        private void removeOrganism(KilledOrganism killedOrganism)
        {
            OrganismState killedState = _newWorldState.GetOrganismState(killedOrganism.ID);

            if (killedOrganism.DeathReason == PopulationChangeReason.Error ||
                killedOrganism.DeathReason == PopulationChangeReason.Timeout ||
                killedOrganism.DeathReason == PopulationChangeReason.SecurityViolation ||
                killedOrganism.DeathReason == PopulationChangeReason.OrganismBlacklisted)
            {
                OnEngineStateChanged(EngineStateChangedEventArgs.AnimalDestroyed(killedState, killedOrganism.DeathReason));
            }

            // Tell the developer the details
            if (killedOrganism.DeathReason == PopulationChangeReason.Error ||
                killedOrganism.DeathReason == PopulationChangeReason.SecurityViolation)
            {
                if (!string.IsNullOrEmpty(killedOrganism.ExtraInformation))
                {
                    string developerMessage = "**** An exception occurred in a '" + ((Species) killedState.Species).Name +
                                              "': \r\n" + killedOrganism.ExtraInformation;
                    OnEngineStateChanged(new EngineStateChangedEventArgs(EngineStateChangeType.DeveloperInformation,
                                                                         developerMessage));
                }
            }

            if (killedOrganism.DeathReason == PopulationChangeReason.Error &&
                string.IsNullOrEmpty(killedOrganism.ExtraInformation))
            {
                OrganismState state = _newWorldState.GetOrganismState(killedOrganism.ID);
                state.Kill(killedOrganism.DeathReason);
            }
            else
            {
                uncountOrganism(killedState, killedOrganism.DeathReason);

                // Remove it from the Host
                Scheduler.Remove(killedOrganism.ID);

                // Remove it from the world state
                _newWorldState.RemoveOrganism(killedOrganism.ID);
            }
        }

        /// <summary>
        /// Processes turns in a phase manner.  After 10 calls to ProcessTurn
        /// all 10 phases will be complete and the method will have completed
        /// one game tick.
        /// </summary>
        /// <returns>True if a tick has been processed, false otherwise.</returns>
        public Boolean ProcessTurn()
        {
            // shutdownError and invalidPeerError are two errors that get set from
            // deep in the system.  We process them here because it is a clean place
            // to restart the game since we aren't in the middle of any phases.
            // This is just a mechanism for cleanly throwing an error from a known good
            // location.
            if (_shutdownError)
            {
                _shutdownError = false;
                throw new ShutdownFailureException();
            }

            if (_invalidPeerError)
            {
                _invalidPeerError = false;
                throw new InvalidPeerException();
            }

            // We break the main processing of the game into 10 phases that have been tuned to each
            // take roughly the same amount of time.  We paint the screen in between each one.  This way
            // We can have a constant frame rate and incrementally do the engine processing we need to
            // do.
            // After we do all 10 phases we've done one game "tick".  We can do two ticks a second which means
            // we have a frame rate of 20.
            switch (_turnPhase)
            {
                case 0:
                    // If we are in ecosystem mode, we want to save the game state periodically in case the user
                    // shuts down the screensaver by pressing ctl-alt-del which just kills the process.  This way
                    // we won't have lost too much processing time.
                    // We always want to save on the tick after we've reported so we don't get our state messed up
                    // on the server.  For example: never start the game in a state that was before the information
                    // was sent to the server because the server will think we're corrupted.
                    if (_logState ||
                        (_ecosystemMode && _populationData.IsReportingTick(CurrentVector.State.TickNumber - 1)))
                    {
                        Debug.WriteLine("Saving state.");
                        serializeState(_currentStateFileName);
                    }

                    // Give 1/5 of the animals a chance to do their processing
                    Scheduler.Tick();
                    break;

                case 1:
                    // Give 1/5 of the animals a chance to do their processing
                    Scheduler.Tick();
                    break;

                case 2:
                    // Give 1/5 of the animals a chance to do their processing
                    Scheduler.Tick();
                    break;

                case 3:
                    // Give 1/5 of the animals a chance to do their processing
                    Scheduler.Tick();
                    break;

                case 4:
                    // Give 1/5 of the animals a chance to do their processing
                    Scheduler.Tick();
                    break;

                case 5:
                    // Get all the actions that the animals want to perform in this tick.
                    TickActions act = _scheduler.GatherTickActions();

                    CurrentVector.Actions = act;

                    // Create a mutable version of the world state that we'll change to create the next
                    // world state.
                    _newWorldState = (WorldState) CurrentVector.State.DuplicateMutable();
                    _newWorldState.TickNumber = _newWorldState.TickNumber + 1;
                    _populationData.BeginTick(_newWorldState.TickNumber, CurrentVector.State.StateGuid);

                    // Remove any organisms queued to be removed
                    removeOrganismsFromQueue();

                    // We take a snapshot of the organism IDs since we do several foreach loops
                    // and change the state, which causes exceptions to occur
                    _organismIDList = new string[_newWorldState.OrganismIDs.Count];
                    _newWorldState.OrganismIDs.CopyTo(_organismIDList, 0);
                    killDiseasedOrganisms2();
                    break;

                case 6:
                    Debug.Assert(_newWorldState != null, "Worldstate did not get created for this tick");

                    // Do this first, since it always must happen so that things (like growing)
                    // won't happen if there isn't enough energy
                    burnBaseEnergy();

                    // Do attacks before movement so that when a carnivore asks if it can attack
                    // they actually get to hit the target before it moves
                    doAttacks();
                    doDefends();
                    changeMovementVectors();
                    break;

                case 7:
                    moveAnimals();
                    break;

                case 8:
                    doBites();
                    growAllOrganisms();
                    incubate();
                    startReproduction();
                    heal();
                    break;

                case 9:
                    // Do this last so that the plant always starts charged up,
                    // and has to operate with what it has for the next turn
                    giveEnergyToPlants();

                    // Teleport after all actions have been processed so that there are
                    // no single turn pending actions left for the organism.
                    teleportOrganisms();

                    // Insert any new organisms
                    insertOrganismsFromQueue();

                    // Set Antenna States
                    doAntennas();

                    // We're done changing the state, now make it immutable
                    _newWorldState.MakeImmutable();
                    Debug.Assert(_newWorldState.Organisms.Count == Scheduler.Organisms.Count);

                    WorldVector vector = new WorldVector(_newWorldState);
                    CurrentVector = vector;

                    _scheduler.CurrentState = _newWorldState;
                    _populationData.EndTick(_newWorldState.TickNumber);

                    _newWorldState = null;
                    break;
            }

            _turnPhase++;
            if (_turnPhase == 10)
            {
                _turnPhase = 0;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Helper function to change movement vectors for all creatures after
        /// they have processed their turn and determined new points of destination.
        /// </summary>
        private void changeMovementVectors()
        {
            ICollection moveToActions = CurrentVector.Actions.MoveToActions.Values;
            foreach (MoveToAction action in moveToActions)
            {
                OrganismState state = _newWorldState.GetOrganismState(action.OrganismID);
                if (state != null && state.IsAlive)
                {
                    state.CurrentMoveToAction = action;
                }
            }
        }

        /// <summary>
        /// Helper function to update all state objects with the latest antenna
        /// information after the creature's have processed their ticks.
        /// </summary>
        private void doAntennas()
        {
            foreach (OrganismState orgState in _newWorldState.Organisms)
            {
                if (!(orgState is AnimalState)) continue;
                Animal animal = (Animal) _scheduler.GetOrganism(orgState.ID);
                AntennaState antennaState = new AntennaState(null);
                if (animal != null)
                {
                    antennaState = new AntennaState((animal).Antennas);
                }
                antennaState.MakeImmutable();
                ((AnimalState) orgState).Antennas = antennaState;
            }
        }

        /// <summary>
        /// We only send these events so that animals can easily continue
        /// defending if they want to.  There is no information in them
        /// </summary>
        private void doDefends()
        {
            ICollection defendActions = CurrentVector.Actions.DefendActions.Values;
            foreach (DefendAction action in defendActions)
            {
                AnimalState defenderState = (AnimalState) _newWorldState.GetOrganismState(action.OrganismID);
                if (defenderState != null && defenderState.IsAlive)
                {
                    defenderState.OrganismEvents.DefendCompleted = new DefendCompletedEventArgs(action.ActionID, action);
                }
            }
        }

        private void doAttacks()
        {
            ICollection attackActions = CurrentVector.Actions.AttackActions.Values;
            foreach (AttackAction action in attackActions)
            {
                AnimalState attackerState = (AnimalState) _newWorldState.GetOrganismState(action.OrganismID);
                if (attackerState == null || !attackerState.IsAlive)
                {
                    continue;
                }

                AnimalState defenderState = (AnimalState) _newWorldState.GetOrganismState(action.TargetAnimal.ID);
                int damageCaused = 0;
                Boolean escaped = false;
                Boolean killed = false;

                if (defenderState == null)
                {
                    // They were killed and eaten before the attack occurred
                    escaped = true;
                }
                else
                {
                    // See if they are near enough to attack
                    // We let them attack if they are at most 1 rectangle away because
                    // it is hard to get closer than this
                    if (attackerState.IsWithinRect(1, defenderState))
                    {
                        // If the animal is dead, it's just zero damage
                        if (defenderState.IsAlive)
                        {
                            // Figure out the maximum possible attack damage
                            damageCaused = _random.Next(0, attackerState.AnimalSpecies.MaximumAttackDamagePerUnitRadius*
                                                          attackerState.Radius);

                            // Defense doesn't scale based on distance
                            DefendAction defendAction =
                                (DefendAction) CurrentVector.Actions.DefendActions[defenderState.ID];
                            int defendDiscount = 0;
                            if (defendAction != null && defendAction.TargetAnimal.ID == attackerState.ID)
                            {
                                defendDiscount = _random.Next(0,
                                                             defenderState.AnimalSpecies.
                                                                 MaximumDefendDamagePerUnitRadius*
                                                             defenderState.Radius);
                            }

                            if (damageCaused > defendDiscount)
                            {
                                damageCaused = damageCaused - defendDiscount;
                                defenderState.CauseDamage(damageCaused);
                            }
                            else
                            {
                                damageCaused = 0;
                            }

                            killed = !defenderState.IsAlive;
                            defenderState.OrganismEvents.AttackedEvents.Add(new AttackedEventArgs(attackerState));
                        }
                    }
                }

                // Tell the attacker what happened
                attackerState.OrganismEvents.AttackCompleted = new AttackCompletedEventArgs(action.ActionID, action,
                                                                                            killed, escaped,
                                                                                            damageCaused);
            }
        }

        private void teleportOrganisms()
        {
            foreach (string organismID in _organismIDList)
            {
                OrganismState organismState = _newWorldState.GetOrganismState(organismID);
                if (organismState == null) continue;

                if (organismState is AnimalState && !(organismState).IsAlive)
                {
                    continue;
                }

                if (_newWorldState.Teleporter.IsInTeleporter(organismState))
                {
                    Teleport(organismState);
                }
            }
        }

        private void moveAnimals()
        {
            // Move the teleporters if necessary
            _newWorldState.Teleporter.Move();

            GridIndex index = new GridIndex();

            foreach (string organismID in _organismIDList)
            {
                OrganismState organismState = _newWorldState.GetOrganismState(organismID);

                if (organismState == null) continue;

                if (organismState is AnimalState)
                {
                    AnimalState animalState = (AnimalState) organismState;

                    if (!animalState.IsStopped)
                    {
                        // Need to move
                        Vector vector = Vector.Subtract(animalState.Position,
                                                        animalState.CurrentMoveToAction.MovementVector.Destination);
                        Point newLocation;

                        Vector unitVector = vector.GetUnitVector();
                        if (vector.Magnitude <= animalState.CurrentMoveToAction.MovementVector.Speed)
                        {
                            // We've arrived
                            newLocation = animalState.CurrentMoveToAction.MovementVector.Destination;
                        }
                        else
                        {
                            Vector speedVector =
                                unitVector.Scale(animalState.CurrentMoveToAction.MovementVector.Speed);
                            newLocation = Vector.Add(animalState.Position, speedVector);
                        }

                        index.AddPath(animalState, animalState.Position, newLocation);
                    }
                    else
                    {
                        // Organism isn't moving but it needs to be added so that the grid is occupied
                        index.AddPath(animalState, animalState.Position, animalState.Position);
                    }
                }
                else
                {
                    // Plants don't move but they need to be added so that the grid is occupied
                    index.AddPath(organismState, organismState.Position, organismState.Position);
                }
            }

            index.ResolvePaths();

            // The index contains the old positions.  If you don't clear them, you will get collisions with old positions
            _newWorldState.ClearIndex();

            foreach (MovementSegment segment in index.StartSegments)
            {
                if (segment.IsStationarySegment)
                {
                    OrganismState stationaryState = segment.State;

                    if (stationaryState.CurrentMoveToAction != null)
                    {
                        // This organism wanted to go where it already was, so -- destination reached
                        stationaryState.OrganismEvents.MoveCompleted =
                            new MoveCompletedEventArgs(stationaryState.CurrentMoveToAction.ActionID,
                                                       stationaryState.CurrentMoveToAction,
                                                       ReasonForStop.DestinationReached, null);
                        stationaryState.CurrentMoveToAction = null;
                    }

                    continue;
                }

                MovementSegment endSegment = segment;

                // Find where the organism ended
                while (endSegment.Next != null)
                {
                    endSegment = endSegment.Next;
                }

                AnimalState newState = (AnimalState) endSegment.State;
                Vector moveVector = Vector.Subtract(endSegment.EndingPoint, endSegment.State.Position);
                newState.Position = endSegment.EndingPoint;

                // Make sure we're ending up where we think we are
                Debug.Assert(endSegment.GridX == newState.GridX && endSegment.GridY == newState.GridY);

                newState.BurnEnergy(newState.EnergyRequiredToMove(moveVector.Magnitude,
                                                                  newState.CurrentMoveToAction.MovementVector.Speed));

                // Burning energy may have killed the organism, in which case we can't
                // send it events because the CurrentMoveToAction is gone
                if (!newState.IsAlive) continue;

                if (endSegment.ExitTime != 0)
                {
                    Debug.Assert(newState != null);
                    Debug.Assert(newState.CurrentMoveToAction != null);

                    // If where they landed wasn't where they wanted to go, then they got blocked.  Stop movement and notify them
                    newState.OrganismEvents.MoveCompleted =
                        new MoveCompletedEventArgs(newState.CurrentMoveToAction.ActionID,
                                                   newState.CurrentMoveToAction, ReasonForStop.Blocked,
                                                   endSegment.BlockedByState);

                    newState.CurrentMoveToAction = null;
                }
                else if (endSegment.State.CurrentMoveToAction.MovementVector.Destination == newState.Position)
                {
                    // Destination reached
                    newState.OrganismEvents.MoveCompleted =
                        new MoveCompletedEventArgs(newState.CurrentMoveToAction.ActionID,
                                                   newState.CurrentMoveToAction, ReasonForStop.DestinationReached,
                                                   null);

                    newState.CurrentMoveToAction = null;
                }
            }

            _newWorldState.BuildIndex();
        }

        private void giveEnergyToPlants()
        {
            foreach (string organismID in _organismIDList)
            {
                OrganismState organismState = _newWorldState.GetOrganismState(organismID);

                if (organismState != null && organismState.GetType() == typeof (PlantState))
                {
                    PlantState plantState = (PlantState) organismState;
                    int availableLight = CurrentVector.State.GetAvailableLight(plantState);

                    if (plantState.IsAlive)
                    {
                        plantState.GiveEnergy(availableLight);
                    }
                }
            }
        }

        private void killDiseasedOrganisms2()
        {
            Hashtable plantsToKill = null;
            Hashtable animalsToKill = null;
            ArrayList deadAnimals = new ArrayList();

            if (_plantCount > MaxPlants)
            {
                plantsToKill = new Hashtable();
            }

            if (AnimalCount > MaxAnimals)
            {
                animalsToKill = new Hashtable();
            }

            if (plantsToKill == null && animalsToKill == null)
            {
                return;
            }

            sortOrganismsForDisease(plantsToKill, animalsToKill, deadAnimals);
            killOrganisms(plantsToKill, _plantCount - MaxPlants, _plantCount, false);
            killOrganisms(animalsToKill, AnimalCount - MaxAnimals - deadAnimals.Count, MaxAnimals - deadAnimals.Count,
                          true);

            // Next remove enough previously rotted animals to get us as close to the right level as possible
            // Make sure dead animals only represent 1/3 of our maximum
            // Leave around some corpses if possible so that carnivores can eat
            int corpseRemoveCount = deadAnimals.Count - (MaxAnimals/3);
            if (corpseRemoveCount <= 0) return;
            foreach (AnimalState state in deadAnimals)
            {
                // Don't ever remove animals that were killed, because it will affect
                // the carnivore population since we're removing their source of food
                if (state.DeathReason == PopulationChangeReason.Killed)
                {
                    continue;
                }

                removeOrganism(new KilledOrganism(state));
                corpseRemoveCount--;
                if (corpseRemoveCount == 0)
                {
                    break;
                }
            }
        }

        private void killOrganisms(IDictionary organismsToKill, int organismKillCount, int totalPopulation,
                                   bool killingAnimals)
        {
            const int diseaseBoundary = 10;
            int killedCount = 0;
            ArrayList largePopulationSpecies = new ArrayList();
            Random rand = new Random(DateTime.Now.Millisecond);

            if (organismsToKill == null || organismKillCount <= 0) return;

            // Find the species that have large populations on the system
            ICollection allSpeciesCollection = organismsToKill.Values;
            foreach (SortedList list in allSpeciesCollection)
            {
                // For large population species, we try to maintain their population percentages with respect to the other
                // large population species
                if (list.Count <= diseaseBoundary) continue;
                // Kill a proportional amount of this species
                int speciesKillTarget = (int) ((list.Count/(double) totalPopulation)*organismKillCount);
                while (list.Count > diseaseBoundary && speciesKillTarget > 0)
                {
                    killedCount++;
                    speciesKillTarget--;

                    // Get the oldest animal of this species
                    OrganismState state = (OrganismState) list.GetByIndex(0);
                    list.RemoveAt(0);
                    if (killingAnimals)
                    {
                        Debug.Assert(state is AnimalState);
                        state.Kill(PopulationChangeReason.Sick);
                    }
                    else
                    {
                        removeOrganism(new KilledOrganism(state.ID, PopulationChangeReason.Sick));
                    }
                }

                if (list.Count > diseaseBoundary)
                {
                    largePopulationSpecies.Add(list);
                }
            }

            organismKillCount -= killedCount;
            killedCount = 0;

            // If we still need to kill more organisms, we take it off the large populations first
            if (organismKillCount > 0)
            {
                int index = rand.Next(0, largePopulationSpecies.Count);
                while (organismKillCount > 0 && largePopulationSpecies.Count > 0)
                {
                    SortedList list = (SortedList) largePopulationSpecies[index];
                    OrganismState state = (OrganismState) list.GetByIndex(0);
                    list.RemoveAt(0);
                    if (killingAnimals)
                    {
                        Debug.Assert(state is AnimalState);
                        state.Kill(PopulationChangeReason.Sick);
                    }
                    else
                    {
                        removeOrganism(new KilledOrganism(state.ID, PopulationChangeReason.Sick));
                    }
                    organismKillCount--;
                    killedCount++;
                    if (list.Count <= diseaseBoundary)
                    {
                        largePopulationSpecies.RemoveAt(index);
                    }
                    else
                    {
                        index++;
                    }

                    if (index > largePopulationSpecies.Count - 1)
                    {
                        index = 0;
                    }
                }
            }

            killedCount = 0;

            // If we still need to kill more organisms, we take it off of anyone, one by one, randomly
            if (organismKillCount > 0)
            {
                SortedList[] allSpecies = new SortedList[organismsToKill.Count];
                allSpeciesCollection.CopyTo(allSpecies, 0);
                while (organismKillCount > 0)
                {
                    int index = rand.Next(0, allSpecies.Length);
                    SortedList list = allSpecies[index];
                    if (list.Count > 0)
                    {
                        OrganismState state = (OrganismState) list.GetByIndex(0);
                        list.RemoveAt(0);
                        if (killingAnimals)
                        {
                            Debug.Assert(state is AnimalState);
                            state.Kill(PopulationChangeReason.Sick);
                        }
                        else
                        {
                            removeOrganism(new KilledOrganism(state.ID, PopulationChangeReason.Sick));
                        }
                        organismKillCount--;
                        killedCount++;
                    }
                }
            }
        }

        /// <summary>
        /// Fills in the hashtables with sorted lists keyed on SpeciesName
        /// </summary>
        /// <param name="plantsToKill"></param>
        /// <param name="animalsToKill"></param>
        /// <param name="deadAnimals"></param>
        private void sortOrganismsForDisease(IDictionary plantsToKill, IDictionary animalsToKill, IList deadAnimals)
        {
            // This is just to ensure that each strength is unique so we can use the sorted list
            // We allow strengths to be from 0 - 9,999,000 and we never use numbers in the hundreds
            // we add the uniquifier which is a number from 0 - 999 just to break ties
            int uniquifier = 0;
            foreach (string organismID in _organismIDList)
            {
                OrganismState organismState = _newWorldState.GetOrganismState(organismID);
                if (organismState == null)
                {
                    continue;
                }

                // Old animals get sick first
                int strength;
                if (organismState.IsAlive)
                {
                    strength = (int) ((organismState.PercentLifespanRemaining)*1000)*1000 + uniquifier;
                    uniquifier++;
                    Debug.Assert(uniquifier < 1000);
                }
                else
                {
                    strength = -1;
                }

                string speciesName = ((Species) organismState.Species).Name;
                SortedList addList;
                if (plantsToKill != null && organismState is PlantState)
                {
                    if (plantsToKill.Contains(speciesName))
                    {
                        addList = (SortedList) plantsToKill[speciesName];
                    }
                    else
                    {
                        addList = new SortedList();
                        plantsToKill.Add(speciesName, addList);
                    }

                    if (strength == -1)
                    {
                        removeOrganism(new KilledOrganism(organismState));
                    }
                    else
                    {
                        Debug.Assert(strength > -1);
                        addList.Add(strength, organismState);
                    }
                }
                else if (animalsToKill != null && organismState is AnimalState)
                {
                    if (strength == -1)
                    {
                        deadAnimals.Add(organismState);
                    }
                    else
                    {
                        if (!((AnimalSpecies) organismState.Species).IsCarnivore)
                        {
                            if (animalsToKill.Contains(speciesName))
                            {
                                addList = (SortedList) animalsToKill[speciesName];
                            }
                            else
                            {
                                addList = new SortedList();
                                animalsToKill.Add(speciesName, addList);
                            }

                            addList.Add(strength, organismState);
                        }
                    }
                }
            }
        }

        private void growAllOrganisms()
        {
            foreach (string organismID in _organismIDList)
            {
                OrganismState organismState = _newWorldState.GetOrganismState(organismID);
                if (organismState == null || !organismState.IsAlive) continue;

                // grow it and check the index to see if it has room to grow
                // if it doesn't, just throw away the clone
                OrganismState grownOrganism = organismState.Grow();

                // The organism can only grow if there is space to grow without overlapping other organisms
                if (_newWorldState.OnlyOverlapsSelf(grownOrganism))
                {
                    // Remove it and read it so that the index gets updated
                    _newWorldState.RefreshOrganism(grownOrganism);
                }
            }
        }

        private void incubate()
        {
            foreach (string organismID in _organismIDList)
            {
                OrganismState organismState = _newWorldState.GetOrganismState(organismID);
                if (organismState == null || !organismState.IsAlive || !organismState.IsIncubating) continue;
                if (organismState.IncubationTicks == EngineSettings.TicksToIncubate)
                {
                    Point newPosition = findEmptyPosition(organismState.CellRadius, Point.Empty);
                    if (newPosition != Point.Empty)
                    {
                        // Only birth an organism if there is space
                        NewOrganism newOrganism = new NewOrganism(
                            ((Species) organismState.Species).InitializeNewState(newPosition,
                                                                                 organismState.Generation + 1),
                            (organismState.CurrentReproduceAction.Dna == null)
                                ? (new byte[0])
                                : ((byte[]) organismState.CurrentReproduceAction.Dna.Clone()));
                        _newOrganismQueue.Enqueue(newOrganism);
                    }
                    else
                    {
                        // Kill the organism since there is no place to put it
                        OnEngineStateChanged(new EngineStateChangedEventArgs(EngineStateChangeType.Other,
                                                                             "A '" +
                                                                             ((Species) organismState.Species).Name +
                                                                             "' died during birth: couldn't find an open spot quickly enough."));
                    }

                    organismState.OrganismEvents.ReproduceCompleted =
                        new ReproduceCompletedEventArgs(organismState.CurrentReproduceAction.ActionID,
                                                        organismState.CurrentReproduceAction);
                    organismState.ResetReproductionWait();
                    organismState.CurrentReproduceAction = null;
                }
                else
                {
                    // Only incubate if the organism isn't hungry
                    if (organismState.EnergyState >= EnergyState.Normal)
                    {
                        if (organismState is AnimalState)
                        {
                            organismState.BurnEnergy(organismState.Radius*
                                                     EngineSettings.AnimalIncubationEnergyPerUnitOfRadius);
                        }
                        else
                        {
                            Debug.Assert(organismState is PlantState);
                            organismState.BurnEnergy(organismState.Radius*
                                                     EngineSettings.PlantIncubationEnergyPerUnitOfRadius);
                        }

                        organismState.AddIncubationTick();
                    }
                }
            }
        }

        private void startReproduction()
        {
            ICollection reproduceActions = CurrentVector.Actions.ReproduceActions.Values;
            foreach (ReproduceAction action in reproduceActions)
            {
                OrganismState state = _newWorldState.GetOrganismState(action.OrganismID);
                if (state == null || !state.IsAlive) continue;
                Debug.Assert(state.IsMature);
                state.CurrentReproduceAction = action;
            }
        }

        /// <summary>
        /// Assumes the animal was dead (if an animal) at the time of the bite
        /// and that it is the proper food source for this animal (herbivore or carnivore)
        /// Also assumes that it is within biting range.
        /// </summary>
        private void doBites()
        {
            ICollection eatActions = CurrentVector.Actions.EatActions.Values;
            foreach (EatAction action in eatActions)
            {
                // Only animals can bite
                AnimalState attackerState = (AnimalState) _newWorldState.GetOrganismState(action.OrganismID);
                if (attackerState == null || !attackerState.IsAlive)
                {
                    continue;
                }

                Boolean biteSuccessful = true;
                OrganismState defenderState = _newWorldState.GetOrganismState(action.TargetOrganism.ID);
                if (defenderState == null)
                {
                    // the defender has been removed from the game
                    biteSuccessful = false;
                }
                else
                {
                    int energyToFill =
                        (int) ((attackerState.Radius*(double) attackerState.Species.MaximumEnergyPerUnitRadius) -
                               attackerState.StoredEnergy);
                    Debug.Assert(energyToFill > 0);

                    // Determine how many chunks it would take to fill the animal completely
                    int foodChunkCount;
                    if (defenderState is PlantState)
                    {
                        foodChunkCount = (int) (energyToFill/(double) EngineSettings.EnergyPerPlantFoodChunk);
                    }
                    else
                    {
                        foodChunkCount = (int) (energyToFill/(double) EngineSettings.EnergyPerAnimalFoodChunk);
                    }

                    if (foodChunkCount == 0)
                    {
                        foodChunkCount = 1;
                    }

                    // If this is more than the animal can eat in one bite, limit it to what they can eat
                    if (foodChunkCount > attackerState.AnimalSpecies.EatingSpeedPerUnitRadius*attackerState.Radius)
                    {
                        foodChunkCount = attackerState.AnimalSpecies.EatingSpeedPerUnitRadius*attackerState.Radius;
                    }

                    if (defenderState.FoodChunks <= foodChunkCount)
                    {
                        // remove the defender from the world if we ate them all
                        removeOrganism(new KilledOrganism(defenderState));
                        foodChunkCount = defenderState.FoodChunks;
                    }
                    else
                    {
                        // Shrink the meat or plant
                        defenderState.FoodChunks = defenderState.FoodChunks - foodChunkCount;
                    }

                    // Determine how much energy we got
                    int newEnergy;
                    if (defenderState is PlantState)
                    {
                        newEnergy = EngineSettings.EnergyPerPlantFoodChunk*foodChunkCount;
                    }
                    else
                    {
                        newEnergy = EngineSettings.EnergyPerAnimalFoodChunk*foodChunkCount;
                    }

                    attackerState.StoredEnergy = attackerState.StoredEnergy + newEnergy;
                }

                // Tell the attacker what happened
                attackerState.OrganismEvents.EatCompleted = new EatCompletedEventArgs(action.ActionID, action,
                                                                                      biteSuccessful);
            }
        }

        private void burnBaseEnergy()
        {
            foreach (string organismID in _organismIDList)
            {
                OrganismState organismState = _newWorldState.GetOrganismState(organismID);
                if (organismState == null) continue;
                if (organismState.IsAlive)
                {
                    // Age the organism
                    organismState.AddTickToAge();
                    if (organismState is AnimalState)
                    {
                        if (organismState.IsAlive)
                        {
                            organismState.BurnEnergy(organismState.Radius*
                                                     EngineSettings.BaseAnimalEnergyPerUnitOfRadius);
                        }
                    }
                    else
                    {
                        Debug.Assert(organismState is PlantState);
                        PlantState plantState = (PlantState) organismState;
                        if (organismState.IsAlive)
                        {
                            plantState.BurnEnergy(plantState.Radius*EngineSettings.BasePlantEnergyPerUnitOfRadius);
                        }

                        // When plants die they disappear
                        if (plantState.EnergyState == EnergyState.Dead)
                        {
                            removeOrganism(new KilledOrganism(plantState));
                        }
                    }
                }
                else
                {
                    if (organismState is AnimalState)
                    {
                        AnimalState animalState = (AnimalState) organismState;
                        animalState.AddRotTick();
                        if (animalState.RotTicks > EngineSettings.TimeToRot)
                        {
                            removeOrganism(new KilledOrganism(animalState));
                        }
                    }
                }
            }
        }

        private void heal()
        {
            foreach (string organismID in _organismIDList)
            {
                OrganismState organismState = _newWorldState.GetOrganismState(organismID);
                if (organismState != null && organismState.IsAlive)
                {
                    organismState.HealDamage();
                }
            }
        }
    }
}