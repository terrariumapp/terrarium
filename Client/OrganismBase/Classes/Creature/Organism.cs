//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;

namespace OrganismBase
{
    // All 'Pending' methods don't guarantee that the action is actually occurring, just that it's in some state between
    // waiting for processing and in progress.  If it's done, it isn't pending.

    /// <summary>
    ///  <para>
    ///   The Organism class is the base class for the Animal/Plant classes.  It
    ///   contains functionality that is common to both the Animal and the Plant
    ///   class.
    ///  </para>
    /// </summary>
    public abstract class Organism
    {
        // Every action an organism makes has an ID that is unique only to the organism
        // so that its response event is trackable and tied to the originating action.

        // This is here to give the illusion that everything happens instantaneously
        // It always reflects any actions that are in progress
        private readonly PendingActions inProgressActions = new PendingActions();

        // Used to hold the serialized bytes that the user serialized until the world is
        // set up enough to allow them to deserialize them

        private readonly Random random;
        private int nextActionID;
        private PendingActions pendingActions = new PendingActions();

        /// <internal/>
        protected Organism()
        {
            random = new Random(GetHashCode());
        }

        /// <internal/>
        public TraceEventHandler Trace { get; set; }

        internal bool IsInitialized { get; set; }

        internal IOrganismWorldBoundary OrganismWorldBoundary { get; private set; }

        /// <internal/>
        public MemoryStream SerializedStream { get; set; }

        /// <summary>
        ///  <para>
        ///   To make random actions deterministic the creature should use this Random
        ///   object when in need of a random number or variable in the creature's code.
        ///   This can help aid in debugging so that strange behavior can be reproduced.
        ///  </para>
        /// </summary>
        /// <returns>
        /// System.Random object initialized by the Organism class.
        /// </returns>
        public Random OrganismRandom
        {
            get { return random; }
        }

        /// <summary>
        ///  <para>
        ///   Each creature is centered in the game world to a specific point.  The
        ///   Position property can be used to query for this location.
        ///  </para>
        /// </summary>
        /// <returns>
        /// System.Drawing.Point object initialized to the organisms current
        /// location in the game world.
        /// </returns>
        public Point Position
        {
            get { return OrganismWorldBoundary.CurrentOrganismState.Position; }
        }

        /// <summary>
        ///  <para>
        ///   Gets the OrganismState object representing your creature's current state in the world.
        ///   The OrganismState object contains all of the properties that specify your organism's
        ///   existence with properties like: Position, EnergyState, PercentInjured, etc...
        ///  </para>
        /// </summary>
        /// <returns>
        /// An OrganismState object representing your creature's current state.
        /// </returns>
        public OrganismState State
        {
            get { return OrganismWorldBoundary.CurrentOrganismState; }
        }

        /// <summary>
        ///  <para>
        ///   A creature can get skipped for a number of turns if it takes too long to execute.
        ///   There is a limit to the time an animal can use for processing every turn which is
        ///   calculated dynamically by the Terrarium each time it starts based on your computer's
        ///   processing power.
        ///  </para>
        ///  <para>
        ///   To see how long your creature takes to process, you can open the
        ///   Trace window, unselect "Show Organism Traces", and then select your creature.  Note
        ///   that this time will be different depending on the machine.
        ///  </para>
        ///  <para>
        ///   If you do break the limit Terrarium determines how many ticks worth of time you've gone
        ///   over the limit and sets this property.  You can examine this property to recover from
        ///   conditions where your creature is skipped for a given number of turns.  This is useful
        ///   since you may not receive events that fire while during the time your creature's turn
        ///   is being skipped.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  A System.Int32 value for the number of turns the creature was skipped before it was
        ///  assigned another time slice.
        /// </returns>
        public int TurnsSkipped { get; private set; }

        internal int InternalTurnsSkipped
        {
            get { return TurnsSkipped; }
            set { TurnsSkipped = value; }
        }

        /// <summary>
        ///  <para>
        ///   Determines whether all conditions are met for your organism to be able to reproduce.
        ///   These conditions include various state information like whether your creature is
        ///   mature, has enough energy, and is not already reproducing.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  True if you creature can reproduce, False otherwise.
        /// </returns>
        public Boolean CanReproduce
        {
            get
            {
                return State.ReadyToReproduce && !IsReproducing &&
                       State.IsMature && (State.EnergyState >= EnergyState.Normal);
            }
        }

        /// <summary>
        ///  <para>
        ///   After your creature has begun reproduction you can get the ReproduceAction object that
        ///   represents your creature's current reproduction.  You can use this to examine
        ///   the dna byte array that will be passed to the child.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  A ReproduceAction object representing the current reproduction and the values
        ///  passed into BeginReproduction.
        /// </returns>
        public ReproduceAction CurrentReproduceAction
        {
            get { return PendingActions.ReproduceAction ?? InProgressActions.ReproduceAction; }
        }

        /// <summary>
        ///  <para>
        ///   Determines if your organism is currently in the process of reproducing.  Because
        ///   reproducing is an asynchronous action, the organism may not actually be giving
        ///   birth yet.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  True if the creature is in the state of reproduction, False otherwise.
        /// </returns>
        public Boolean IsReproducing
        {
            get { return CurrentReproduceAction != null; }
        }

        /// <summary>
        ///  <para>
        ///   The unique GUID for an organism.  This is used to store plant/animal state
        ///   when being saved to disk, or when passing plant/animal information to children
        ///   during reproduction.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  A string value representing the unique GUID or ID for the organism.
        /// </returns>
        public string ID
        {
            get { return OrganismWorldBoundary.ID; }
        }

        internal PendingActions PendingActions
        {
            get { return pendingActions; }
        }

        internal PendingActions InProgressActions
        {
            get { return inProgressActions; }
        }

        internal Boolean IsTracing
        {
            get { return Trace != null; }
        }

        /// <summary>
        /// The Initialize method is called immediately after instantiating a new creature.
        /// The developer should override this method to set up event handlers for the
        /// creature and do any first time initialization that needs to be done to set
        /// up member variables.
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <internal/>
        public void SetWorldBoundary(IOrganismWorldBoundary boundary)
        {
            // This is so organisms can't mess around with it
            if (OrganismWorldBoundary != null)
            {
                return;
            }

            OrganismWorldBoundary = boundary;
        }

        /// <summary>
        ///  <para>
        ///   Calculates the linear distance between your creature and another using
        ///   various API's defined by the Vector class.
        ///  </para>
        /// </summary>
        /// <param name="organismState">
        /// The OrganismState object for the creature to
        /// use when computing linear distance from you're creature.
        /// </param>
        /// <returns>
        /// A System.Double representing the linear distance between your creature and another.
        /// </returns>
        public double DistanceTo(OrganismState organismState)
        {
            if (organismState == null)
            {
                throw new ArgumentNullException("organismState", "The argument 'organismState' cannot be null");
            }

            return Vector.Subtract(Position, organismState.Position).Magnitude;
        }

        /// <summary>
        ///  <para>
        ///   Use this function to command your creature to reproduce.  There are many
        ///   conditions on whether your creature can reproduce.  If these conditions
        ///   are not met, an exception will be thrown.  The easiest way to make
        ///   sure all pre-existing conditions have been met is to check the CanReproduce
        ///   property.
        ///  </para>
        ///  <para>
        ///   If you call this method multiple times in the same turn, then the last call
        ///   will be used, and all previous calls will be ignored.  This method is also
        ///   asynchronous, and a ReproduceCompletedEvent will be fired when your creature
        ///   has actually given birth.  The time between start and completion is 10 ticks.
        ///  </para>
        /// </summary>
        /// <param name="dna">
        ///  A byte array that gets passed to the child.  This can be any information you
        ///  want to pass to the child creature.  The byte array is truncated at 8000 bytes.
        /// </param>
        /// <exception cref="AlreadyReproducingException">
        ///  Thrown when your creature is already in the process of reproduction.
        /// </exception>
        /// <exception cref="NotMatureException">
        ///  Thrown when your creature is not yet mature and you try to reproduce.
        /// </exception>
        /// <exception cref="NotEnoughEnergyException">
        ///  Thrown when your creature does not have enough energy to start reproduction.
        /// </exception>
        /// <exception cref="NotReadyToReproduceException">
        ///  Thrown when your creature is not yet ready to reproduce because the appropriate number of ticks has not elapsed.
        /// </exception>
        public void BeginReproduction(byte[] dna)
        {
            if (IsReproducing)
            {
                throw new AlreadyReproducingException();
            }

            if (!State.IsMature)
            {
                throw new NotMatureException();
            }

            if (State.EnergyState < EnergyState.Normal)
            {
                throw new NotEnoughEnergyException();
            }

            if (!State.ReadyToReproduce)
            {
                throw new NotReadyToReproduceException();
            }

            var actionID = GetNextActionID();
            var action = new ReproduceAction(ID, actionID, dna);
            lock (PendingActions)
            {
                PendingActions.SetReproduceAction(action);
                InProgressActions.SetReproduceAction(action);
            }
        }

        internal int GetNextActionID()
        {
            return nextActionID++;
        }

        /// <internal/>
        public PendingActions GetThenErasePendingActions()
        {
            PendingActions detachedActions;
            lock (PendingActions)
            {
                detachedActions = pendingActions;
                pendingActions = new PendingActions();
            }
            detachedActions.MakeImmutable();
            return detachedActions;
        }

        /// <internal/>
        public abstract void InternalMain(bool clearOnly);

        private void InternalTrace(params object[] tracings)
        {
            var strings = new string[tracings.Length];
            for (var i = 0; i < tracings.Length; i++)
            {
                strings[i] = tracings[i].ToString();
                strings[i] = strings[i].Substring(0, (strings[i].Length > 8000) ? 8000 : strings[i].Length);
            }

            Trace(this, strings);
        }

        /// <summary>
        ///  <para>
        ///   Writes a trace to the Terrarium trace window for debugging.  The Tracing routines
        ///   take a *very* small amount of time if you're not monitoring them.  They are on the
        ///   order of 12 nSec per call.  To meet this performance requirement there are several
        ///   overloads taking a varying number of parameters rather than a single variable argument
        ///   parameter.
        ///  </para>
        /// </summary>
        /// <param name="item1">
        ///  An object containing information for the Trace Window.  ToString() will be called on the object.
        /// </param>
        public void WriteTrace(object item1)
        {
            if (Trace != null)
            {
                InternalTrace(item1);
            }
        }

        /// <summary>
        ///  <para>
        ///   Writes a trace to the Terrarium trace window for debugging.  The Tracing routines
        ///   take a *very* small amount of time if you're not monitoring them.  They are on the
        ///   order of 12 nSec per call.  To meet this performance requirement there are several
        ///   overloads taking a varying number of parameters rather than a single variable argument
        ///   parameter.
        ///  </para>
        /// </summary>
        /// <param name="item1">
        ///  An object containing information for the Trace Window.  ToString() will be called on the object.
        /// </param>
        /// <param name="item2">
        ///  An object containing information for the Trace Window.  ToString() will be called on the object.
        /// </param>
        public void WriteTrace(object item1, object item2)
        {
            if (Trace != null)
            {
                InternalTrace(item1, item2);
            }
        }

        /// <summary>
        ///  <para>
        ///   Writes a trace to the Terrarium trace window for debugging.  The Tracing routines
        ///   take a *very* small amount of time if you're not monitoring them.  They are on the
        ///   order of 12 nSec per call.  To meet this performance requirement there are several
        ///   overloads taking a varying number of parameters rather than a single variable argument
        ///   parameter.
        ///  </para>
        /// </summary>
        /// <param name="item1">
        ///  An object containing information for the Trace Window.  ToString() will be called on the object.
        /// </param>
        /// <param name="item2">
        ///  An object containing information for the Trace Window.  ToString() will be called on the object.
        /// </param>
        /// <param name="item3">
        ///  An object containing information for the Trace Window.  ToString() will be called on the object.
        /// </param>
        public void WriteTrace(object item1, object item2, object item3)
        {
            if (Trace != null)
            {
                InternalTrace(item1, item2, item3);
            }
        }

        /// <summary>
        ///  <para>
        ///   Writes a trace to the Terrarium trace window for debugging.  The Tracing routines
        ///   take a *very* small amount of time if you're not monitoring them.  They are on the
        ///   order of 12 nSec per call.  To meet this performance requirement there are several
        ///   overloads taking a varying number of parameters rather than a single variable argument
        ///   parameter.
        ///  </para>
        /// </summary>
        /// <param name="item1">
        ///  An object containing information for the Trace Window.  ToString() will be called on the object.
        /// </param>
        /// <param name="item2">
        ///  An object containing information for the Trace Window.  ToString() will be called on the object.
        /// </param>
        /// <param name="item3">
        ///  An object containing information for the Trace Window.  ToString() will be called on the object.
        /// </param>
        /// <param name="item4">
        ///  An object containing information for the Trace Window.  ToString() will be called on the object.
        /// </param>
        public void WriteTrace(object item1, object item2, object item3, object item4)
        {
            if (Trace != null)
            {
                InternalTrace(item1, item2, item3, item4);
            }
        }

        /// <internal/>
        public void InternalOrganismSerialize(MemoryStream m)
        {
        }

        /// <internal/>
        public void InternalOrganismDeserialize(MemoryStream m)
        {
        }
    }
}