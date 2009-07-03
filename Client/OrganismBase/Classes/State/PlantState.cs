//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Represents a plant's state during a certain tick in time.  This
    ///   class provides information on how much damage the plant has taken,
    ///   how much energy it has left, and how tall it is.
    ///  </para>
    ///  <para>
    ///   For additional information you should look at the OrganismState
    ///   class which provides information that is shared between the
    ///   Plant/Animal states.
    ///  </para>
    /// </summary>
    [Serializable]
    public sealed class PlantState : OrganismState
    {
        /// <summary>
        ///  Determines how high a plant is compared to its base.  This can be
        ///  used to make squatty plants or tall plants.  This is currently not
        ///  used.  Height could be derived from this ratio, and it should probably
        ///  be made into a characteristic.
        /// </summary>
        private const double heightToRadiusRatio = 1;

        /// <summary>
        ///  This is incomplete, this should be a characteristic that enables
        ///  creatures to determine how much light they need to obtain maximum
        ///  energy per tick.  This can be used to create trees that require
        ///  lots of light or moss that requires very little.
        /// </summary>
        private const int optimalLightPercentage = 100;

        /// <summary>
        ///  Creates a brand new state object for a plant.
        /// </summary>
        /// <internal/>
        public PlantState(string id, ISpecies species, int generation, EnergyState initialEnergyState, int initialRadius) 
            : base(id, species, generation, initialEnergyState, initialRadius)
        {
        }

        /// <summary>
        ///  <para>
        ///   Returns the height of the plant.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the height of the plant.
        /// </returns>
        public int Height { get; private set; }

        /// <summary>
        ///  <para>
        ///   Returns the number of food chunks this plant represents if
        ///   it hasn't taken any damage.  This along with the PercentInjured
        ///   property can be used to compute the total remaining food chunks
        ///   for a plant.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the maximum food chunks the plant can hold.
        /// </returns>
        public int CurrentMaxFoodChunks
        {
            get { return Radius*EngineSettings.PlantFoodChunksPerUnitOfRadius; }
        }

        /// <summary>
        ///  <para>
        ///   Returns the amount of defoliation this plant has lived through.
        ///   This is a percentage total with 100 representing a full defoliated
        ///   or dead plant, and 0 representing a fully healthy plant.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Double representing the percentage of defoliation on a plant.
        /// </returns>
        public override double PercentInjured
        {
            get
            {
                Debug.Assert(1 - ((FoodChunks/(double) CurrentMaxFoodChunks)*100)
                             <= 100);

                return 1 - (FoodChunks/(double) CurrentMaxFoodChunks);
            }
        }

        /// <summary>
        ///  Clones the current plant state while resetting the immutability attribute
        ///  so that the new state can be updated with new information.
        /// </summary>
        /// <returns>An newly mutable OrganismState that can be casted to a PlantState.</returns>
        /// <internal/>
        public override OrganismState CloneMutable()
        {
            var newInstance = new PlantState(ID, Species, Generation, EnergyState, Radius);
            CopyStateInto(newInstance);

            return newInstance;
        }

        /// <summary>
        ///  Copies the value of the current state into a new state object.
        ///  Used by the CloneMutable method to make mutable copies of state
        ///  objects.
        /// </summary>
        /// <param name="newInstance">The new state object that will hold the values.</param>
        /// <internal/>
        protected override void CopyStateInto(OrganismState newInstance)
        {
            base.CopyStateInto(newInstance);

            ((PlantState) newInstance).Height = Height;
        }

        /// <summary>
        /// The amount of energy a plant gets decreases linearly as you get away from the optimal
        /// </summary>
        /// <param name="availableLightPercentage">The amount of light available to give to this plant.</param>
        /// <internal/>
        public void GiveEnergy(int availableLightPercentage)
        {
            if (IsImmutable)
            {
                throw new GameEngineException("Object is immutable.");
            }

            var percentageFromOptimal = availableLightPercentage - optimalLightPercentage;
            if (percentageFromOptimal < 0)
            {
                percentageFromOptimal = -percentageFromOptimal;
            }
            Debug.Assert(percentageFromOptimal <= 100);

            var energyGained = (int) ((1 -
                                       (percentageFromOptimal/
                                        (double) 100))*EngineSettings.MaxEnergyFromLightPerTick);

            StoredEnergy = StoredEnergy + energyGained;
        }

        /// <summary>
        ///  Increases the radius of the creature to a new radius.  The game
        ///  engine calls this whenever a plant grows.  Since plants have
        ///  other attributes that change the base implementation of this
        ///  method must be overriden to also update the foodChunks property.
        /// </summary>
        /// <param name="newRadius">The new radius of the creature.</param>
        /// <internal/>
        public override void IncreaseRadiusTo(int newRadius)
        {
            if (IsImmutable)
            {
                throw new GameEngineException("Object is immutable.");
            }

            var newHeight = (int) (newRadius*heightToRadiusRatio);
            var additionalRadius = newRadius - Radius;
            base.IncreaseRadiusTo(newRadius);
            Height = newHeight;

            currentFoodChunks += additionalRadius*EngineSettings.PlantFoodChunksPerUnitOfRadius;
        }

        /// <summary>
        ///  Used by the game engine in order to grow a creature.  This attempts
        ///  to grow the creature's radius by one unit.  It makes a copy of the
        ///  organism state rather than modifying it directly since the size and
        ///  location properties might be immutable.
        /// </summary>
        /// <returns>
        ///  A new organism state object, since the old object might have had
        ///  a locked state that couldn't be modified in size.
        /// </returns>
        /// <internal/>
        public override OrganismState Grow()
        {
            if (IsImmutable)
            {
                throw new GameEngineException("Object is immutable.");
            }

            if (Radius < Species.MatureRadius)
            {
                if (EnergyState >= EnergyState.Normal && GrowthWait == 0)
                {
                    var newState = (PlantState) CloneMutable();

                    // we have to set the events because the cloned object won't have them
                    // and the engine may have already stuck events in there
                    newState.OrganismEvents = OrganismEvents;

                    newState.IncreaseRadiusTo(Radius + 1);
                    newState.BurnEnergy(EngineSettings.PlantRequiredEnergyPerUnitOfRadiusGrowth);
                    newState.ResetGrowthWait();

                    return newState;
                }
            }

            return this;
        }

        /// <summary>
        ///  Plants heal just by getting their foodchunks back up to the maximum level -- kind of
        ///  like growing their old leaves back.  This should only be called by the game engine
        ///  and so the method is protected by the immutable property.
        /// </summary>
        /// <internal/>
        public override void HealDamage()
        {
            if (IsImmutable)
            {
                throw new GameEngineException("Object is immutable.");
            }

            var maxHealingChunks = EngineSettings.PlantMaxHealingPerTickPerRadius*Radius;

            var usableEnergy = StoredEnergy - UpperBoundaryForEnergyState(Species, EnergyState.Deterioration, Radius);
            if (usableEnergy <= 0) return;

            var availableHealingChunks = (int) (usableEnergy/
                                                (EngineSettings.PlantRequiredEnergyPerUnitOfHealing));
            if (availableHealingChunks < maxHealingChunks)
            {
                maxHealingChunks = availableHealingChunks;
            }

            var foodChunkDelta = 0;
            if (CurrentMaxFoodChunks - FoodChunks < maxHealingChunks)
            {
                foodChunkDelta = CurrentMaxFoodChunks - FoodChunks;
                currentFoodChunks = CurrentMaxFoodChunks;
            }
            else
            {
                foodChunkDelta = maxHealingChunks;
                currentFoodChunks += foodChunkDelta;
            }

            BurnEnergy(foodChunkDelta*
                       (double) EngineSettings.PlantRequiredEnergyPerUnitOfHealing);
        }
    }
}