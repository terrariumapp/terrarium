using System;
using System.Collections;
using System.IO;

namespace OrganismBase
{
    /// <summary>
    ///  This is the class that you derive from when you create an animal.
    /// </summary>
    public abstract class Animal : Organism
    {
        private AntennaState antennaState = new AntennaState(null);

        /// <summary>
        ///  Provides access to the world boundary object.  This can
        ///  be used to investigate the world from an Animal's point
        ///  of view.
        /// </summary>
        internal IAnimalWorldBoundary World
        {
            get { return (IAnimalWorldBoundary) OrganismWorldBoundary; }
        }

        /// <summary>
        ///  <para>
        ///   Provides access to the creature's Antenna.  Each Antenna has a specific set of
        ///   positions that it may be in.  Setting states with this information is possible
        ///   as is passing numeric data.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  AntennaState for the organism to initiate communication.
        /// </returns>
        public AntennaState Antennas
        {
            get { return antennaState; }
            set { antennaState = value; }
        }

        /// <summary>
        ///  <para>
        ///   The width of the world in single points/pixels.  Use this to make sure
        ///   you don't try to move outside of the bounds of the Terrarium and to
        ///   help manage your creature's population size/density.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the width of the Terrarium in points/pixels.
        /// </returns>
        public int WorldWidth
        {
            get { return World.WorldWidth; }
        }

        /// <summary>
        ///  <para>
        ///   The height of the world in single points/pixels.  Use this to make sure
        ///   you don't try to move outside of the bounds of the Terrarium and to
        ///   help manage your creature's population size/density.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the height of the Terrarium in points/pixels.
        /// </returns>
        public int WorldHeight
        {
            get { return World.WorldHeight; }
        }

        /// <summary>
        ///  The current state of your own creature.  This is used to get the latest
        ///  information about your creature's health, damage, and other stats available
        ///  on the AnimalState object.
        /// </summary>
        /// <returns>
        ///  AnimalState representing the most current state of your creature.
        /// </returns>
        public new AnimalState State
        {
            get { return World.CurrentAnimalState; }
        }

        /// <summary>
        ///  Returns the immutable species object containing information about
        ///  your creature's species related information.  This includes how
        ///  many points were placed into creature attributes and other values
        ///  calculated from those points allocations.
        /// </summary>
        /// <returns>
        ///  IAnimalSpecies representing the immutable characteristics of your creature.
        /// </returns>
        public IAnimalSpecies Species
        {
            get { return (IAnimalSpecies) State.Species; }
        }

        /// <summary>
        ///  <para>
        ///   After your creature has begun moving you can get the MoveToAction object that
        ///   represents your creature's current movement action.  You can use this to examine
        ///   the movement location and speed that your creature moving to see if you'll need
        ///   to alter your course.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  A MoveToAction object representing the current movement and the values
        ///  passed into BeginMoving.
        /// </returns>
        public MoveToAction CurrentMoveToAction
        {
            get
            {
                if (PendingActions.MoveToAction != null)
                {
                    return PendingActions.MoveToAction;
                }
                if (InProgressActions.MoveToAction != null)
                {
                    return InProgressActions.MoveToAction;
                }
                return null;
            }
        }

        /// <summary>
        ///  <para>
        ///   Used to determine if your creature has been commanded to move.
        ///   You can also check the CurrentMoveToAction property to get
        ///   the actual movement vector for your creature.  Because moving
        ///   is asynchronous your creature might not have started moving
        ///   yet.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  True if your creature is or will be moving, False otherwise.
        /// </returns>
        public Boolean IsMoving
        {
            get { return CurrentMoveToAction != null; }
        }

        /// <summary>
        ///  <para>
        ///   After your creature has begun defending you can get the DefendAction object that
        ///   represents you're creatures current defend action.  You can use this to examine
        ///   the target creature you're defending against and determine if there might be a
        ///   more appropriate enemy.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  A DefendAction object representing the current defend action and the values
        ///  passed into BeginDefending.
        /// </returns>
        public DefendAction CurrentDefendAction
        {
            get
            {
                if (PendingActions.DefendAction != null)
                {
                    return PendingActions.DefendAction;
                }
                if (InProgressActions.DefendAction != null)
                {
                    return InProgressActions.DefendAction;
                }
                return null;
            }
        }

        /// <summary>
        ///  <para>
        ///   Used to determine if your creature has been commanded to defend.
        ///   You can also check the CurrentDefendAction property to get
        ///   the actual target creature you're defending against.  Because defending
        ///   is asynchronous your creature won't defend until the upcoming tick.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  True if your creature will defend the next tick, False otherwise.
        /// </returns>
        public Boolean IsDefending
        {
            get { return CurrentDefendAction != null; }
        }

        /// <summary>
        ///  <para>
        ///   After your creature has begun attacking you can get the AttackAction object that
        ///   represents your creature's current attack action.  You can use this to examine
        ///   the target creature you're attacking and determine if there might be a
        ///   more appropriate enemy.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  AttackAction object representing the current attack action and the values
        ///  passed into BeginAttacking.
        /// </returns>
        public AttackAction CurrentAttackAction
        {
            get
            {
                if (PendingActions.AttackAction != null)
                {
                    return PendingActions.AttackAction;
                }
                if (InProgressActions.AttackAction != null)
                {
                    return InProgressActions.AttackAction;
                }
                return null;
            }
        }

        /// <summary>
        ///  <para>
        ///   Used to determine if your creature has been commanded to attack.
        ///   You can also check the CurrentAttackAction property to get
        ///   the actual target creature you're attacking.  Because attacking
        ///   is asynchronous your creature won't attack until the upcoming tick.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  True if your creature will attack the next tick, False otherwise.
        /// </returns>
        public Boolean IsAttacking
        {
            get { return CurrentAttackAction != null; }
        }

        /// <summary>
        ///  <para>
        ///   After your creature has begun eating you can get the EatAction object that
        ///   represents you're creatures current eat action.  You can use this to examine
        ///   the target creature your eating and determine if their might be a better
        ///   target to eat.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  EatAction object representing the current eat action and the values
        ///  passed into BeginEating.
        /// </returns>
        public EatAction CurrentEatAction
        {
            get
            {
                return PendingActions.EatAction ?? InProgressActions.EatAction;
            }
        }

        /// <summary>
        ///  <para>
        ///   Used to determine if your creature has been commanded to eat.
        ///   You can also check the CurrentEatAction property to get
        ///   the actual target creature you're eating.  Because eating
        ///   is asynchronous your creature won't actually eat until the upcoming tick.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  True if your creature will eat the next tick, False otherwise.
        /// </returns>
        public Boolean IsEating
        {
            get { return CurrentEatAction != null; }
        }

        /// <summary>
        ///  <para>
        ///   Used to determine if your creature is capable of eating depending
        ///   on the creature's current energy state.  You can also trap the
        ///   AlreadyFullException from BeginEating.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  True if your creature is hungry enough to eat, False otherwise.
        /// </returns>
        public Boolean CanEat
        {
            get { return State.EnergyState <= EnergyState.Normal; }
        }

        /// <summary>
        ///  <para>
        ///   The MoveCompleted event is fired whenever your creature
        ///   has completed a movement operation.  This can either mean
        ///   the creature reached the destination or that the creature
        ///   was blocked and can't move anymore.
        ///  </para>
        /// </summary>
        public event MoveCompletedEventHandler MoveCompleted;

        /// <summary>
        ///  <para>
        ///   The AttackCompleted event is fired whenever your creature
        ///   has completed an attack operation.  Your creature should
        ///   hook this event to learn the results of the battle such
        ///   as how much damage was inflicted and whether the target
        ///   creature was killed.
        ///  </para>
        /// </summary>
        public event AttackCompletedEventHandler AttackCompleted;

        /// <summary>Fired when an EatAction is completed.</summary>
        public event EatCompletedEventHandler EatCompleted;

        /// <summary>Fired after all other events have been fired.</summary>
        public event IdleEventHandler Idle;

        /// <summary>Fired before all other events have been fired.</summary>
        public event LoadEventHandler Load;

        /// <summary>Fired after an organism has been teleported.</summary>
        public event TeleportedEventHandler Teleported;

        /// <summary>Fired on the parent when a ReproduceAction is completed.</summary>
        public event ReproduceCompletedEventHandler ReproduceCompleted;

        /// <summary>Fired on an organism when it is first born.</summary>
        public event BornEventHandler Born;

        /// <summary>Fired when a DefendAction is completed.</summary>
        public event DefendCompletedEventHandler DefendCompleted;

        /// <summary>Fired when an organism is being attacked by another organism.</summary>
        public event AttackedEventHandler Attacked;

        /// <summary>
        ///  <para>
        ///   This method should be overridden by any class inheriting from 
        ///   Animal.  This method is called with a MemoryStream that the
        ///   user can place any data on they wish to Serialize during
        ///   save games or while being teleported.
        ///  </para>
        ///  <para>
        ///   The complement of this method is the DeserializeAnimal method
        ///   which is called to deserialize the data when the creature
        ///   is restored.  Authors should be careful when writing to a
        ///   MemoryStream since it will be truncated at 8000bytes.
        ///  </para>
        /// </summary>
        /// <param name="m">
        ///  MemoryStream used by the creature to serialize data.
        /// </param>
        public abstract void SerializeAnimal(MemoryStream m);

        /// <summary>
        ///  <para>
        ///   This method should be overridden by any class inheriting from 
        ///   Animal.  This method is called with a MemoryStream that the
        ///   user can read any data from that was written during the
        ///   call to SerializeAnimal.
        ///  </para>
        ///  <para>
        ///   Care should be taken when reading from a MemoryStream since
        ///   the values may have been truncated at 8000bytes if more than
        ///   8000bytes were originally written.
        ///  </para>
        /// </summary>
        /// <param name="m">
        ///  MemoryStream used by the creature to deserialize data.
        /// </param>
        public abstract void DeserializeAnimal(MemoryStream m);

        /// <summary>
        ///  Implemented by the Animal class in order to allow
        ///  serialization of any private members required for the
        ///  class to operate properly after deserialization.
        /// </summary>
        /// <param name="m">A memory stream that can be written to.</param>
        /// <internal/>
        public void InternalAnimalSerialize(MemoryStream m)
        {
        }

        /// <summary>
        ///  Implemented by the Animal class in order to allow
        ///  deserialization of any private members required for the
        ///  class to operate properly after deserialization.
        /// </summary>
        /// <param name="m">A memory stream that can be written to.</param>
        /// <internal/>
        public void InternalAnimalDeserialize(MemoryStream m)
        {
        }

        /// <summary>
        ///  <para>
        ///   Scans the world around your creature's current location in a circular
        ///   area and returns an ArrayList of OrganismState objects representing
        ///   what was seen.
        ///  </para>
        ///  <para>
        ///   The radius scanned by your creature is dependent upon the number of points
        ///   placed into the EyesightPoints attribute.  Animals may also hide within your
        ///   radius by using camouflage.  This means that more points placed into the
        ///   EyesightPoints attribute will yield a better vision of hidden creatures.
        ///  </para>
        ///  <para>
        ///   Because of camouflage and the random aspect of hiding vs. being seen by another
        ///   creature, multiple calls to Scan might returns different results.  However,
        ///   each call to Scan also takes additional time from your creature's total timeslice.
        ///  </para>
        ///  <para>
        ///   It is recommended that you hold onto the OrganismState objects, determine your
        ///   target creature, and then use the LookFor method to update the state rather than
        ///   calling the Scan method again.  The LookFor method also takes into account camouflage
        ///   and may not work the first time, but is much less expensive timewise than Scan.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Collections.ArrayList of OrganismState objects your creature can see.
        /// </returns>
        public ArrayList Scan()
        {
            return World.Scan();
        }

        /// <summary>
        ///  <para>
        ///   Tries to return an updated OrganismState given a creature's ID.  This function
        ///   may return null if the creature can't be found or was hidden by camouflage.
        ///   You may call this method multiple times just like the LookFor method and get
        ///   different results.
        ///  </para>
        /// </summary>
        /// <param name="organismID">
        ///  GUID which represents a specific organism in the system.
        /// </param>
        /// <returns>
        ///  OrganismState representing the creature being looked for or null if not found.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///  Thrown if the organismID parameter is null.
        /// </exception>
        public OrganismState RefreshState(string organismID)
        {
            if (organismID == null)
            {
                throw new ArgumentNullException("organismID", "The argument 'organismID' cannot be null");
            }

            return World.RefreshState(organismID);
        }

        /// <summary>
        ///  <para>
        ///   Tries to return an updated OrganismState given a creature's state OrganismState.
        ///   This function may return null if the creature can't be found or was hidden by
        ///   camouflage.  You may call this method multiple times and get different results.
        ///  </para>
        /// </summary>
        /// <param name="organismState">
        ///  The stale OrganismState object you want to refresh.
        /// </param>
        /// <returns>
        ///  OrganismState representing the creature being looked for or null if not found.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///  Thrown if the organismState parameter is null.
        /// </exception>
        public OrganismState LookFor(OrganismState organismState)
        {
            if (organismState == null)
            {
                throw new ArgumentNullException("organismState", "The argument 'organismState' cannot be null");
            }

            return World.LookFor(organismState);
        }

        /// <summary>
        ///  <para>
        ///   Allows a creature to determine if the OrganismState of another creature
        ///   represents the same species.  This can be used to determine whether you
        ///   should attack/defend against another creature.
        ///  </para>
        ///  <para>
        ///   Creatures of the same species often don't fight one another, defend against
        ///   one another, and kill one another.  They often help their own species in
        ///   fights against other species.  Carnivores of the same species may sacrifice
        ///   themselves as food once they become too old to members of their species.
        ///  </para>
        /// </summary>
        /// <param name="targetState">
        ///  The OrganismState for the creature to be used in species comparison.
        /// </param>
        /// <returns>
        ///  True if the organism owning the state object is of the same species, false otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///  Thrown if the targetState parameter is null.
        /// </exception>
        public Boolean IsMySpecies(OrganismState targetState)
        {
            if (targetState == null)
            {
                throw new ArgumentNullException("targetState", "The argument 'targetState' cannot be null");
            }

            return State.Species.IsSameSpecies(targetState.Species);
        }

        /// <summary>
        ///  <para>
        ///   Clears any pending movement operations your creature might be performing.
        ///  </para>
        /// </summary>
        public void StopMoving()
        {
            lock (PendingActions)
            {
                PendingActions.SetMoveToAction(null);
                InProgressActions.SetMoveToAction(null);
            }
        }


        /// <summary>
        ///  <para>
        ///   Method used to command a creature to begin moving towards a specific location
        ///   at a specific speed.  The actual movement operation may take several turns,
        ///   but is always initiated using this method.  Your movement location should
        ///   be within the world boundary and your movement speed should be less than or
        ///   equal to your creature's Species.MaximumSpeed.
        ///  </para>
        ///  <para>
        ///   Once called the creature will begin moving towards the specified point.  This
        ///   movement will continue until you issue a different BeginMoving command to your
        ///   creature, it reaches its destination, or becomes blocked by something.  Any
        ///   calls to BeginMoving will clear out any previous calls, so care should be taken
        ///   when issuing multi-part path movements.
        ///  </para>
        ///  <para>
        ///   Once the movement is completed the MoveCompleted event will be fired and your
        ///   event handler for this function will be called if you've provided one.  The
        ///   event handler will provide full information about the results of an attempted
        ///   movement operation.
        ///  </para>
        /// </summary>
        /// <param name="vector">
        ///  The MovementVector that determines the point you are moving to and how fast to move there.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///  Thrown if the vector parameter is null.
        /// </exception>
        /// <exception cref="OutOfBoundsException">
        ///  Thrown if the destination is outside of the world boundaries.
        /// </exception>
        /// <exception cref="TooFastException">
        ///  Thrown if the speed defined in the vector is greater than Species.MaximumSpeed.
        /// </exception>
        public void BeginMoving(MovementVector vector)
        {
            if (vector == null)
            {
                throw new ArgumentNullException("vector", "The argument 'vector' cannot be null");
            }

            if (vector.Speed > State.AnimalSpecies.MaximumSpeed)
            {
                throw new TooFastException();
            }

            if (vector.Destination.X > World.WorldWidth - 1 ||
                vector.Destination.X < 0 ||
                vector.Destination.Y > World.WorldHeight - 1 ||
                vector.Destination.Y < 0)
            {
                throw new OutOfBoundsException();
            }

            var actionID = GetNextActionID();
            var action = new MoveToAction(ID, actionID, vector);
            lock (PendingActions)
            {
                PendingActions.SetMoveToAction(action);
                InProgressActions.SetMoveToAction(action);
            }
        }

        /// <summary>
        ///  <para>
        ///   Method used to command a creature to begin defending against a specific
        ///   target creature.  You can only defend against one creature at a time,
        ///   so only the final call to BeginDefending will actually be used 
        ///   in the upcoming turn.
        ///  </para>
        ///  <para>
        ///   Once your creature has finished defending, the DefendCompleted event will
        ///   be fired and your event handler will be called if you provided one.  You
        ///   can use this event to determine the results of your defense.
        ///  </para>
        /// </summary>
        /// <param name="targetAnimal">
        ///  The AnimalState that represents the animal you want to defend against.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///  Thrown if the targetAnimal parameter is null.
        /// </exception>
        public void BeginDefending(AnimalState targetAnimal)
        {
            if (targetAnimal == null)
            {
                throw new ArgumentNullException("targetAnimal", "The argument 'targetAnimal' cannot be null");
            }

            var actionID = GetNextActionID();
            var action = new DefendAction(ID, actionID, targetAnimal);
            lock (PendingActions)
            {
                PendingActions.SetDefendAction(action);
                InProgressActions.SetDefendAction(action);
            }
        }

        /// <summary>
        ///  <para>
        ///   Method used to command your creature to start attacking another
        ///   creature.  You can only attack one creature per round, and a single
        ///   call to BeginAttacking will only attack a target creature in the
        ///   upcoming tick.  Calling BeginAttacking multiple times in the same
        ///   turn will only result in your creature attacking the target specified
        ///   in the last call to BeginAttacking.
        ///  </para>
        ///  <para>
        ///   Attacking is asynchronous so you'll need to handle the AttackCompleted
        ///   event in order to get the status of your attack.  A single attack might
        ///   not kill a target enemy so you should detect if the enemy is still
        ///   alive and call BeginAttacking once per round until the target creature
        ///   is either dead or has escaped.
        ///  </para>
        /// </summary>
        /// <param name="targetAnimal">
        ///  The AnimalState object that represents the creature you want your creature to attack.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///  Thrown if the targetAnimal parameter is null.
        /// </exception>
        /// <exception cref="NotHungryException">
        ///  Thrown if the creature is not hungry enough to attack.
        /// </exception>
        public void BeginAttacking(AnimalState targetAnimal)
        {
            if (targetAnimal == null)
            {
                throw new ArgumentNullException("targetAnimal", "The argument 'targetAnimal' cannot be null");
            }

            if (!CanAttack(targetAnimal))
            {
                throw new NotHungryException();
            }

            var actionID = GetNextActionID();
            var action = new AttackAction(ID, actionID, targetAnimal);
            lock (PendingActions)
            {
                PendingActions.SetAttackAction(action);
                InProgressActions.SetAttackAction(action);
            }
        }

        /// <summary>
        ///  <para>
        ///   Used to determine if your creature is within range to eat another
        ///   target creature.
        ///  </para>
        ///  <para>
        ///   This method does not attempt to validate the position of the
        ///   organismState with respect to the current world state.  If you
        ///   pass a stale object in then you may get stale results.  Make sure
        ///   you use the LookFor method to get the most up-to-date results.
        ///  </para>
        /// </summary>
        /// <param name="targetOrganism">
        ///  OrganismState of the creature you're thinking of eating.
        /// </param>
        /// <returns>
        ///  True if the creature is within range to eat, False otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///  Thrown if the targetOrganism parameter is null.
        /// </exception>
        public Boolean WithinEatingRange(OrganismState targetOrganism)
        {
            if (targetOrganism == null)
            {
                throw new ArgumentNullException("targetOrganism", "The argument 'targetOrganism' cannot be null");
            }

            return State.IsAdjacentOrOverlapping(targetOrganism);
        }

        /// <summary>
        ///  <para>
        ///   Used to determine if your creature is within range to attack another
        ///   target creature.
        ///  </para>
        ///  <para>
        ///   This method does not attempt to validate the position of the
        ///   organismState with respect to the current world state.  If you
        ///   pass a stale object in then you may get stale results.  Make sure
        ///   you use the LookFor method to get the most up-to-date results.
        ///  </para>
        /// </summary>
        /// <param name="targetOrganism">
        ///  OrganismState of the creature you're thinking of attacking.
        /// </param>
        /// <returns>
        ///  True if the creature is within range to attack, False otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///  Thrown if the targetOrganims parameter is null
        /// </exception>
        public Boolean WithinAttackingRange(AnimalState targetOrganism)
        {
            if (targetOrganism == null)
            {
                throw new ArgumentNullException("targetOrganism", "The argument 'targetOrganism' cannot be null");
            }

            return State.IsWithinRect(1, targetOrganism);
        }


        /// <summary>
        ///  <para>
        ///   Method used to command your creature to start eating another creature.
        ///   You can only eat one target creature per round, and a single call to
        ///   BeginEating will only attack a target creature in the upcoming tick.
        ///   Calling BeginEating multiple times in the same turn will only result
        ///   in your creature eating the target specified in the last call to
        ///   BeginEating.
        ///  </para>
        ///  <para>
        ///   Eating is asynchronous so you'll need to handle the EatCompleted event
        ///   in order to get the status of the bite.  A single bite might not
        ///   produce enough energy for your creature so you'll have to make multiple
        ///   bites against the same target until it is completed eaten.
        ///  </para>
        /// </summary>
        /// <param name="targetOrganism">
        ///  OrganismState of the creature you wish to eat.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///  Thrown if the targetOrganism parameter is null.
        /// </exception>
        /// <exception cref="AlreadyFullException">
        ///  Thrown if your creature is not hungry enough to eat.
        /// </exception>
        /// <exception cref="NotVisibleException">
        ///  Thrown if the creature had disappeared from your creature's view.
        /// </exception>
        /// <exception cref="NotWithinDistanceException">
        ///  Thrown if your creature is not within eating distance.
        /// </exception>
        /// <exception cref="ImproperFoodException">
        ///  Thrown if a Carnivore tries to eat a plant or a Herbivore tries to eat an Animal
        /// </exception>
        /// <exception cref="NotDeadException">
        ///  Thrown if a Carnivore tries to eat a creature that isn't dead yet.
        /// </exception>
        public void BeginEating(OrganismState targetOrganism)
        {
            if (targetOrganism == null)
            {
                throw new ArgumentNullException("targetOrganism", "The argument 'targetOrganism' cannot be null");
            }

            if (State.EnergyState > EnergyState.Normal)
            {
                throw new AlreadyFullException();
            }

            // Get an up to date state -- this organism may be an old state
            var currentOrganism = World.LookForNoCamouflage(targetOrganism);

            if (currentOrganism == null)
            {
                throw new NotVisibleException();
            }
            if (!WithinEatingRange(currentOrganism))
            {
                throw new NotWithinDistanceException();
            }

            // Make sure it is edible
            if (State.AnimalSpecies.IsCarnivore)
            {
                if (currentOrganism is PlantState)
                {
                    throw new ImproperFoodException();
                }

                if (currentOrganism.IsAlive)
                {
                    throw new NotDeadException();
                }
            }
            else
            {
                if (currentOrganism is AnimalState)
                {
                    throw new ImproperFoodException();
                }
            }

            var actionID = GetNextActionID();
            var action = new EatAction(ID, actionID, targetOrganism);
            lock (PendingActions)
            {
                PendingActions.SetEatAction(action);
                InProgressActions.SetEatAction(action);
            }
        }

        /// <summary>
        ///  <para>
        ///   Used to determine if your creature can attack another creature.
        ///   This will return true all the time for a Carnivore since they
        ///   can always attack.  
        ///  </para>
        ///  <para>
        ///   For Herbivores this will return true if they are hungry enough
        ///   to be aggressive.  Herbivores may also attack a creature in
        ///   the upcoming round if that creature attacked them in the
        ///   previous round.  The best place to attack a creature that is
        ///   attacking you is to handle the Attacked event.
        ///  </para>
        /// </summary>
        /// <param name="targetAnimal">
        ///  AnimalState for the creature you would like to attack.
        /// </param>
        /// <returns>
        ///  True if your creature can attack the target creature, False otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///  Thrown if the targetAnimal parameter is null.
        /// </exception>
        public Boolean CanAttack(AnimalState targetAnimal)
        {
            if (State.AnimalSpecies.IsCarnivore)
            {
                return true;
            }

            if (targetAnimal == null)
            {
                throw new ArgumentNullException("targetAnimal", "The argument 'targetAnimal' cannot be null");
            }

            // You can attack back if you were just attacked
            var wasAttacked = false;
            foreach (AttackedEventArgs attackEvent in State.OrganismEvents.AttackedEvents)
            {
                if (attackEvent.Attacker.ID != targetAnimal.ID) continue;
                wasAttacked = true;
                break;
            }

            return wasAttacked || State.EnergyState <= EnergyState.Hungry;
        }

        /// <summary>
        ///  Provides all of the per tick processing for an Animal.  This method
        ///  fires all of the events that make a creature tick.  Some events
        ///  are fired every tick, while other events are only fired whenever
        ///  certain actions complete.  This method can be called in order to
        ///  process Animal code without processing the developer code in the
        ///  instance they are being skipped for using too much time.
        /// </summary>
        /// <param name="clearOnly">Used by the system to clear completed actions, but not fire events.</param>
        /// <internal/>
        public override sealed void InternalMain(bool clearOnly)
        {
            // Give the Initialized call once, and during main so that their state is around
            // In the constructor of the organism the state might not be available
            if (!IsInitialized)
            {
                Initialize();
                IsInitialized = true;
            }

            // Always give a load event
            WriteTrace("#Load");
            OnLoad(new LoadEventArgs(), clearOnly);

            var events = State.OrganismEvents;
            if (events != null)
            {
                if (events.MoveCompleted != null)
                {
                    OnMoveCompleted(events.MoveCompleted, clearOnly);
                }

                if (events.AttackCompleted != null)
                {
                    OnAttackCompleted(events.AttackCompleted, clearOnly);
                }

                if (events.EatCompleted != null)
                {
                    OnEatCompleted(events.EatCompleted, clearOnly);
                }

                if (events.Teleported != null)
                {
                    OnTeleported(events.Teleported, clearOnly);
                }

                if (events.ReproduceCompleted != null)
                {
                    OnReproduceCompleted(events.ReproduceCompleted, clearOnly);
                }

                if (events.Born != null)
                {
                    OnBorn(events.Born, clearOnly);
                }

                if (events.DefendCompleted != null)
                {
                    OnDefendCompleted(events.DefendCompleted, clearOnly);
                }

                if (events.AttackedEvents.Count > 0)
                {
                    foreach (AttackedEventArgs attackEvent in events.AttackedEvents)
                    {
                        OnAttacked(attackEvent, clearOnly);
                    }
                }
            }

            // Always give an idle event
            WriteTrace("#Idle");
            OnIdle(new IdleEventArgs(), clearOnly);

            if (clearOnly)
            {
                InternalTurnsSkipped++;
            }
            else
            {
                InternalTurnsSkipped = 0;
            }
        }

        /// <summary>
        ///  Helper function used to fire off born events.
        /// </summary>
        /// <param name="e">The born event arguments.</param>
        /// <param name="clearOnly">Only clear the action, don't fire the event.</param>
        private void OnBorn(BornEventArgs e, bool clearOnly)
        {
            if (!clearOnly && Born != null)
            {
                Born(this, e);
            }
        }

        /// <summary>
        ///  Helper function used to fire Attacked events.
        /// </summary>
        /// <param name="e">The attacked event arguments.</param>
        /// <param name="clearOnly">Only clear the action, don't fire the event.</param>
        private void OnAttacked(AttackedEventArgs e, bool clearOnly)
        {
            if (!clearOnly && Attacked != null)
            {
                Attacked(this, e);
            }
        }

        /// <summary>
        ///  Helper function used to fire idle events.
        /// </summary>
        /// <param name="e">The idle event arguments.</param>
        /// <param name="clearOnly">Only clear the action, don't fire the event.</param>
        private void OnIdle(IdleEventArgs e, bool clearOnly)
        {
            if (!clearOnly && Idle != null)
            {
                Idle(this, e);
            }
        }

        /// <summary>
        ///  Helper function used to fire load events.
        /// </summary>
        /// <param name="e">The load event arguments.</param>
        /// <param name="clearOnly">Only clear the action, don't fire the event.</param>
        private void OnLoad(LoadEventArgs e, bool clearOnly)
        {
            if (!clearOnly && Load != null)
            {
                Load(this, e);
            }
        }

        /// <summary>
        ///  Helper function used to fire reproduction events.
        /// </summary>
        /// <param name="e">The reproduction event arguments.</param>
        /// <param name="clearOnly">Only clear the action, don't fire the event.</param>
        private void OnReproduceCompleted(ReproduceCompletedEventArgs e, bool clearOnly)
        {
            if (InProgressActions.ReproduceAction != null &&
                e.ActionID == InProgressActions.ReproduceAction.ActionID)
            {
                InProgressActions.SetReproduceAction(null);
            }

            if (!clearOnly && ReproduceCompleted != null)
            {
                ReproduceCompleted(this, e);
            }
        }

        /// <summary>
        ///  Helper function used to fire defend events.
        /// </summary>
        /// <param name="e">The defend event arguments.</param>
        /// <param name="clearOnly">Only clear the action, don't fire the event.</param>
        private void OnDefendCompleted(DefendCompletedEventArgs e, bool clearOnly)
        {
            if (InProgressActions.DefendAction != null &&
                e.ActionID == InProgressActions.DefendAction.ActionID)
            {
                InProgressActions.SetDefendAction(null);
            }

            if (!clearOnly && DefendCompleted != null)
            {
                DefendCompleted(this, e);
            }
        }

        /// <summary>
        ///  Helper function used to fire teleported events.
        /// </summary>
        /// <param name="e">The teleported event arguments.</param>
        /// <param name="clearOnly">Only clear the action, don't fire the event.</param>
        private void OnTeleported(TeleportedEventArgs e, bool clearOnly)
        {
            if (!clearOnly && Teleported != null)
            {
                Teleported(this, e);
            }
        }

        /// <summary>
        ///  Helper function used to fire movement events.
        /// </summary>
        /// <param name="e">The movement event arguments.</param>
        /// <param name="clearOnly">Only clear the action, don't fire the event.</param>
        private void OnMoveCompleted(MoveCompletedEventArgs e, bool clearOnly)
        {
            if (InProgressActions.MoveToAction != null &&
                e.ActionID == InProgressActions.MoveToAction.ActionID)
            {
                // the action is no longer in progress. If this is the most recent MoveToAction,
                // remove it since there is no action in progress now
                InProgressActions.SetMoveToAction(null);
            }

            if (!clearOnly && MoveCompleted != null)
            {
                MoveCompleted(this, e);
            }
        }

        /// <summary>
        ///  Helper function used to fire attack events.
        /// </summary>
        /// <param name="e">The attack event arguments.</param>
        /// <param name="clearOnly">Only clear the action, don't fire the event.</param>
        private void OnAttackCompleted(AttackCompletedEventArgs e, bool clearOnly)
        {
            if (InProgressActions.AttackAction != null &&
                e.ActionID == InProgressActions.AttackAction.ActionID)
            {
                InProgressActions.SetAttackAction(null);
            }

            if (!clearOnly && AttackCompleted != null)
            {
                AttackCompleted(this, e);
            }
        }

        /// <summary>
        ///  Helper function used to fire eat events.
        /// </summary>
        /// <param name="e">The eat event arguments.</param>
        /// <param name="clearOnly">Only clear the action, don't fire the event.</param>
        private void OnEatCompleted(EatCompletedEventArgs e, bool clearOnly)
        {
            if (InProgressActions.EatAction != null &&
                e.ActionID == InProgressActions.EatAction.ActionID)
            {
                InProgressActions.SetEatAction(null);
            }

            if (!clearOnly && EatCompleted != null)
            {
                EatCompleted(this, e);
            }
        }
    }
}