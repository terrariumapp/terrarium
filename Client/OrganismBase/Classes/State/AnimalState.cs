//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Represents a creature's state during a certain tick in time.  This
    ///   class provides information on the creature's species, how much
    ///   damage the creature has taken, and methods to compute energy
    ///   requirements.
    ///  </para>
    ///  <para>
    ///   For additional information you should look at the OrganismState
    ///   class which provides information that is shared between the
    ///   Plant/Animal states.
    ///  </para>
    /// </summary>
    [Serializable]
    public sealed class AnimalState : OrganismState
    {
        private int damage;
        private AntennaState state = new AntennaState(null);

        /// <internal/>
        public AnimalState(string id, ISpecies species, int generation, EnergyState initialEnergyState, int initialRadius)
            : base(id, species, generation, initialEnergyState, initialRadius)
        {
        }

        /// <summary>
        ///  <para>
        ///   Provides access to a read-only version of a creature's Antenna.  Each Antenna
        ///   has a specific set of positions that it may be in.  Setting states with this
        ///   information is possible as is passing numeric data.
        ///  </para>
        ///  <para>
        ///   This property is used to examine the Antenna state of other creatures.  If you
        ///   need to set the states of your own Antenna you should use the Antennas property
        ///   on the Animal base class.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  AntennaState for the organism to initiate communication.
        /// </returns>
        [TypeConverter((typeof (ExpandableObjectConverter)))]
        public AntennaState Antennas
        {
            get
            {
                if (IsImmutable)
                {
                    state.MakeImmutable();
                }

                return state;
            }

            set
            {
                if (!IsImmutable)
                {
                    if (value != null)
                    {
                        state = value;
                    }
                }
                else
                {
                    throw new GameEngineException(
                        "Antennas can not be set on the State object.  Use the Antennas property on your Creature class instead.");
                }
            }
        }

        /// <summary>
        ///  <para>
        ///   Provides information about the Species of the creature through the
        ///   IAnimalSpecies interface.  This should be used to determine the
        ///   stats of a creature for threat/food calculations.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  IAnimalSpecies representing the Species of the creature this state represents.
        /// </returns>
        public IAnimalSpecies AnimalSpecies
        {
            get { return (IAnimalSpecies) Species; }
        }

        /// <summary>
        ///  <para>
        ///   Provides the number of ticks a particular corpse has been rotting for.
        ///   This is useful to determine whether a corpse will be around long
        ///   enough to be used for food.
        ///  </para>
        ///  <para>
        ///   Under normal circumstances a corpse will remain for 100 ticks.  However,
        ///   this number may be shortened if the machine is under load and many
        ///   creatures are dying from sickness.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the number of ticks the creature has been dead.
        /// </returns>
        public int RotTicks { get; private set; }

        /// <summary>
        ///  <para>
        ///   Provides the percentage of damage taken versus total allowed damage
        ///   before the creature is killed.  This is useful in calculating which
        ///   creature makes the best target due to weakness.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Double decimal percentage of total injury.  0 Representing maximum
        ///  health and 1 representing death.
        /// </returns>
        public override double PercentInjured
        {
            get
            {
                Debug.Assert(((damage/(EngineSettings.DamageToKillPerUnitOfRadius*(double) Radius))*
                              100) <= 100,
                             "Percent Injured returned '" +
                             ((damage/(EngineSettings.DamageToKillPerUnitOfRadius*(double) Radius))*
                              100) + "', damage = " + damage + " Radius = " + Radius);

                return ((damage/(EngineSettings.DamageToKillPerUnitOfRadius*(double) Radius)));
            }
        }

        /// <internal/>
        public override DisplayAction PreviousDisplayAction
        {
            get
            {
                // If we *just* died, return DisplayAction.Died 
                if (!IsAlive && RotTicks == 0)
                {
                    return DisplayAction.Died;
                }

                return base.PreviousDisplayAction;
            }
        }

        /// <summary>
        ///  <para>
        ///   Provides the absolute amount of damage an organism has sustained.
        ///   Normally PercentInjured is more useful for determining how badly
        ///   hurt your creature is, but this absolute number can be used for
        ///   different types of calculations.
        ///  </para>
        ///  <para>
        ///   Some useful applications include examining the damage taken each
        ///   tick, determining which creature has the MOST damage, not the highest
        ///   percentage of damage.  Determining if your creature can sustain a hit
        ///   from another creature without dying.  Determining a creature's
        ///   statistical chance of winning a fight, along with updating this chance
        ///   each round.  Each of these can be done using the Damage property, but
        ///   not the PercentInjured property.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the absolute amount of damage taken.
        /// </returns>
        public int Damage
        {
            get { return damage; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override OrganismState CloneMutable()
        {
            var newInstance = new AnimalState(ID, AnimalSpecies, Generation, EnergyState, Radius);
            CopyStateInto(newInstance);

            return newInstance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newInstance"></param>
        protected override void CopyStateInto(OrganismState newInstance)
        {
            base.CopyStateInto(newInstance);

            ((AnimalState) newInstance).damage = damage;
            ((AnimalState) newInstance).RotTicks = RotTicks;
            ((AnimalState) newInstance).state = new AntennaState(state);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddRotTick()
        {
            if (IsImmutable)
            {
                throw new GameEngineException("Object is immutable.");
            }

            Debug.Assert(!IsAlive);

            RotTicks++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="incrementalDamage"></param>
        public void CauseDamage(int incrementalDamage)
        {
            if (IsImmutable)
            {
                throw new GameEngineException("Object is immutable.");
            }

            if (incrementalDamage < 0)
            {
                throw new GameEngineException("Damage must be positive.");
            }

            if (Damage + incrementalDamage >= EngineSettings.DamageToKillPerUnitOfRadius*Radius)
            {
                Kill(PopulationChangeReason.Killed);
                damage = EngineSettings.DamageToKillPerUnitOfRadius*Radius;
                return;
            }

            damage = damage + incrementalDamage;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void HealDamage()
        {
            if (IsImmutable)
            {
                throw new GameEngineException("Object is immutable.");
            }

            double maxHealing = EngineSettings.AnimalMaxHealingPerTickPerRadius*Radius;
            Debug.Assert(maxHealing > 0);

            var usableEnergy = StoredEnergy - UpperBoundaryForEnergyState(Species, EnergyState.Hungry, Radius);
            if (usableEnergy > 0)
            {
                var availableHealing = usableEnergy/
                                       (EngineSettings.AnimalRequiredEnergyPerUnitOfHealing);

                if (availableHealing < maxHealing)
                {
                    maxHealing = availableHealing;
                }

                double damageDelta = 0;
                if (damage - maxHealing < 0)
                {
                    damageDelta = damage;
                    damage = 0;
                }
                else
                {
                    damageDelta = maxHealing;
                    damage = (int) (damage - maxHealing);
                }

                BurnEnergy(damageDelta*EngineSettings.AnimalRequiredEnergyPerUnitOfHealing);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newRadius"></param>
        public override void IncreaseRadiusTo(int newRadius)
        {
            if (IsImmutable)
            {
                throw new GameEngineException("Object is immutable.");
            }

            var additionalRadius = newRadius - Radius;

            base.IncreaseRadiusTo(newRadius);

            currentFoodChunks += additionalRadius*EngineSettings.FoodChunksPerUnitOfRadius;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override OrganismState Grow()
        {
            if (IsImmutable)
            {
                throw new GameEngineException("Object is immutable.");
            }

            if (Radius < Species.MatureRadius && GrowthWait == 0)
            {
                if (EnergyState >= EnergyState.Normal)
                {
                    var newState = (AnimalState) CloneMutable();

                    // we have to set the events because the cloned object won't have them
                    // and the engine may have already stuck events in there
                    newState.OrganismEvents = OrganismEvents;
                    newState.IncreaseRadiusTo(Radius + 1);
                    newState.BurnEnergy(EngineSettings.AnimalRequiredEnergyPerUnitOfRadiusGrowth);
                    newState.ResetGrowthWait();

                    return newState;
                }
            }

            return this;
        }

        /// <summary>
        ///  <para>
        ///   Provides a method for creatures to determine how much energy a specific
        ///   movement action will cost.  The slower a creature moves the less energy
        ///   they expend moving over a specific distance.  With this in mind the developer
        ///   can determine the optimum speed at which to move to a given location versus
        ///   time and energy consumption.
        ///  </para>
        /// </summary>
        /// <param name='distance'>
        ///  The amount of distance your creature is going to move.
        /// </param>
        /// <param name=' speed'>
        ///  The speed your creature is going to move at.
        /// </param>
        /// <returns>
        ///  System.Double representing the amount of energy required to make the movement action.
        /// </returns>
        public double EnergyRequiredToMove(double distance, int speed)
        {
            return distance*Radius*
                   speed*EngineSettings.RequiredEnergyPerUnitOfRadiusSpeedDistance;
        }
    }
}