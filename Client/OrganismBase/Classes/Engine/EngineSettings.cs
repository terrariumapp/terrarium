//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System.Diagnostics;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   EngineSettings contains all of the various constants
    ///   that affect the game world, creature attributes, and
    ///   computed values for the Terrarium.  Creatures may
    ///   use these values in order to decisions or calculate
    ///   new values of their own.  These should be thought of
    ///   as the laws of physics and biology for the Terrarium.
    ///  </para>
    /// </summary>
    public class EngineSettings
    {
        /// <internal/>
        public const double AnimalIncubationEnergyMultiplier = 1.5;

        /// <summary>
        ///  <para>
        ///   The amount of energy required per tick in order to incubate
        ///   an offspring.  This is multiplied by the creature's Radius
        ///   so larger creatures require more energy to incubate than
        ///   smaller creatures.
        ///  </para>
        /// </summary>
        public const double AnimalIncubationEnergyPerUnitOfRadius =
            (AnimalMatureSizeProvidedEnergyPerUnitRadius/TicksToIncubate)*
            AnimalIncubationEnergyMultiplier;

        /// <summary>
        ///  <para>
        ///   The amount of time in game ticks that a creature can stay alive.  This
        ///   number is multiplied by the creature's MatureSize so smaller creatures
        ///   will not live as long as larger creatures.
        ///  </para>
        /// </summary>
        public const int AnimalLifeSpanPerUnitMaximumRadius = 50;

        /// <internal/>
        public const double AnimalMatureSizeProvidedEnergyPerUnitRadius =
            FoodChunksPerUnitOfRadius*EnergyPerAnimalFoodChunk;

        /// <summary>
        ///  <para>
        ///   The maximum number of healing points a creature can
        ///   heal per tick.  This is multiplied by the Radius of the
        ///   creature and so larger creatures can heal many more points
        ///   than smaller creatures.
        ///  </para>
        /// </summary>
        public const int AnimalMaxHealingPerTickPerRadius = 2;

        /// <summary>
        ///  <para>
        ///   The amount of time in game ticks that a creature must wait before being
        ///   able to reproduce.  This number is multiplied by the creature's Radius
        ///   and so larger creatures take longer between reproductions than smaller
        ///   creatures.
        ///  </para>
        /// </summary>
        public const int AnimalReproductionWaitPerUnitRadius = 8;

        /// <summary>
        ///  <para>
        ///   The amount of energy required to heal a creature by a single
        ///   health unit.
        ///  </para>
        /// </summary>
        public const double AnimalRequiredEnergyPerUnitOfHealing = .1;

        /// <summary>
        ///  <para>
        ///   The amount of energy burned by a creature in order to grow a single unit
        ///   of radius.  For smaller creatures this amount of energy will be quite
        ///   taxing, but as the creature grows larger the amount of energy taken
        ///   doesn't affect the creature as much.
        ///  </para>
        /// </summary>
        public const double AnimalRequiredEnergyPerUnitOfRadiusGrowth = MaxEnergyBasePerUnitRadius*(1/(double) 5);

        /// <summary>
        ///  <para>
        ///   The amount of energy burned by a creature each tick just for being
        ///   alive in the game.  This constant is multiplied by the creature's
        ///   radius so smaller creatures been less energy per tick than larger
        ///   creatures.
        ///  </para>
        /// </summary>
        public const double BaseAnimalEnergyPerUnitOfRadius = .001; // base energy burnt per turn

        /// <summary>
        ///  <para>
        ///   Represents the base amount of damage that can be absorbed by
        ///   a creature that places 0 points into the DefendDamagePointsAttribute.
        ///   The amount of damage absorption taken from this constant is multiplied by the current
        ///   radius of the creature.  This means both MatureSize and the DefendDamagePointsAttribute
        ///   can increase the amount of damage your creature can take.
        ///  </para>
        /// </summary>
        public const int BaseDefendedDamagePerUnitOfRadius = 50;

        /// <summary>
        ///  <para>
        ///   Represents the base food chunks per bite granted to
        ///   a creature that puts 0 points into the EatingSpeedPointsAttribute.
        ///   The number of food chunks taken from this constant is multiplied by the current
        ///   radius of the creature.  This means both MatureSize and the EatingSpeedPointsAttribute
        ///   can increase the number of food chunks taken per bite.
        ///  </para>
        /// </summary>
        public const int BaseEatingSpeedPerUnitOfRadius = 1;

        /// <summary>
        ///  <para>
        ///   Represents the base distance a creature can see if they place
        ///   0 points into the EyesightPointsAttribute.
        ///  </para>
        ///  <para>
        ///   This distance is in Terrarium Cells, so you have to multiply
        ///   by 8 to get the actual distance in Terrarium Units (pixels).
        ///  </para>
        /// </summary>
        public const int BaseEyesightRadius = 5;

        /// <summary>
        ///  <para>
        ///   Represents the base amount of damage that can be dealt by
        ///   a creature that places 0 points into the AttackDamagePointsAttribute.
        ///   The amount of damage taken from this constant is multiplied by the current
        ///   radius of the creature.  This means both MatureSize and the AttackDamagePointsAttribute
        ///   can increase the amount of damage your creature can dish out.
        ///  </para>
        /// </summary>
        public const int BaseInflictedDamagePerUnitOfRadius = 50;

        /// <summary>
        ///  <para>
        ///   The amount of energy burned by a plant each tick just for being
        ///   alive in the game.  This constant is multiplied by the plant's
        ///   radius so smaller plants been less energy per tick than larger
        ///   plants.
        ///  </para>
        /// </summary>
        public const int BasePlantEnergyPerUnitOfRadius = 1;

        /// <summary>
        ///  <para>
        ///   Attack and Defense modifier applied to Carnivores.  This
        ///   gives Carnivores an advantage in both attacking and defending
        ///   against Herbivores since they have to attack and expend extra
        ///   energy to obtain food.
        ///  </para>
        /// </summary>
        public const double CarnivoreAttackDefendMultiplier = 2;

        /// <summary>
        ///  <para>
        ///   This multiplier is used to modify the life span for Carnivores.
        ///   Since Carnivores have double the lifespan of Herbivores they get
        ///   twice as many opportunities for reproduction
        ///  </para>
        /// </summary>
        public const int CarnivoreLifeSpanMultiplier = 2;

        /// <summary>
        ///  <para>
        ///   The amount of damage required to before a creature
        ///   is killed.  This is multiplied by the Radius of the
        ///   creature and so larger creatures can take many more
        ///   hits than smaller creatures.
        ///  </para>
        /// </summary>
        public const int DamageToKillPerUnitOfRadius = 190;

        /// <summary>
        ///  <para>
        ///   The amount of energy given to a creature for
        ///   one food chunk from a corpse.
        ///  </para>
        /// </summary>
        public const int EnergyPerAnimalFoodChunk = 1;

        /// <summary>
        ///  <para>
        ///   The amount of energy given to a creature for
        ///   one food chunk from a plant.
        ///  </para>
        /// </summary>
        public const int EnergyPerPlantFoodChunk = 1;

        private const double EnergyRequiredToMoveMinimumRequirements =
            MinimumUnitsToMoveAtMinimumEnergy*MinimumSpeedToMoveAtMinimumEnergy*
            RequiredEnergyPerUnitOfRadiusSpeedDistance +
            (MinimumUnitsToMoveAtMinimumEnergy/MinimumSpeedToMoveAtMinimumEnergy)*
            BaseAnimalEnergyPerUnitOfRadius;

        /// <summary>
        ///  <para>
        ///   The amount of food chunks a creature amounts to once they
        ///   become a corpse.  Larger creatures will have a larger amount
        ///   of food chunks available to predators than smaller creatures.
        ///  </para>
        /// </summary>
        public const int FoodChunksPerUnitOfRadius = 25;

        /// <internal/>
        public const int GridCellHeight = 1 << GridHeightPowerOfTwo;

        /// <internal/>
        public const int GridCellWidth = 1 << GridWidthPowerOfTwo;

        /// <internal/>
        public const int GridHeightPowerOfTwo = 3;

        /// <internal/>
        public const int GridWidthPowerOfTwo = 3;

        /// <summary>
        ///  <para>
        ///   Represents the lowest attainable odds of appearing
        ///   invisible in the Terrarium.  In order to achieve this
        ///   minimum available camouflage you must apply 0 points
        ///   to the CamouflagePointsAttribute.
        ///  </para>
        /// </summary>
        public const int InvisibleOddsBase = 0;

        /// <summary>
        ///  <para>
        ///   Represents the highest attainable odds of appearing
        ///   invisible in the Terrarium.  In order to achieve
        ///   this maximum camouflage MaxAvailableCharacteristicPoints
        ///   must be applied to the CamouflagePointsAttribute.
        ///  </para>
        /// </summary>
        public const int InvisibleOddsMaximum = 90;

        /// <summary>
        ///  <para>
        ///   Represents the maximum number of characteristic points
        ///   available for creature developers to assign to characteristic
        ///   stats.
        ///  </para>
        /// </summary>
        public const int MaxAvailableCharacteristicPoints = 100;

        /// <summary>
        ///  <para>
        ///   Represents the amount of energy storage per unit radius
        ///   granted to a creature when 0 points have been placed into
        ///   the MaximumEnergyPointsAttribute.
        ///  </para>
        ///  <para>
        ///   Even if 0 points are placed into the MaximumEnergyPointsAttribute
        ///   a creature can still achieve higher energy storage by increasing
        ///   MatureSize.
        ///  </para>
        /// </summary>
        public const double MaxEnergyBasePerUnitRadius = (int) EnergyRequiredToMoveMinimumRequirements;

        /// <summary>
        ///  <para>
        ///   The maximum amount of energy a plant can gain per tick from
        ///   the natural light of the EcoSystem.
        ///  </para>
        /// </summary>
        public const int MaxEnergyFromLightPerTick = 550;

        /// <summary>
        ///  <para>
        ///   Represents the highest attainable energy storage
        ///   per unit radius.  In order to achieve this maximum
        ///   available energy you must apply MaxAvailableCharacteristicPoints
        ///   to the MaximumEnergyPointsAttribute.
        ///  </para>
        ///  <para>
        ///   Please note that this value multiplied by your current radius
        ///   is your actual total energy storage.  This means that putting
        ///   points into the MaximumEnergyPointsAttribute or increasing the
        ///   MatureSize of your creature will both increase the total
        ///   energy storage.
        ///  </para>
        /// </summary>
        public const double MaxEnergyMaximumPerUnitRadius = (int) (EnergyRequiredToMoveMinimumRequirements*20);

        /// <internal/>
        public const int MaxGridRadius = (MaxMatureSize/GridCellWidth/2) + 1;

        /// <summary>
        ///  <para>
        ///   Represents the maximum amount of damage that can be absorbed by
        ///   a creature that places MaxAvailableCharacteristicPoints into the
        ///   DefendDamagePointsAttribute.
        ///   The amount of damage absorption taken from this constant is multiplied by the current
        ///   radius of the creature.  This means both MatureSize and the DefendDamagePointsAttribute
        ///   can increase the amount of damage your creature can take.
        ///  </para>
        /// </summary>
        public const int MaximumDefendedDamagePerUnitOfRadius = 25;

        /// <summary>
        ///  <para>
        ///   Represents the maximum number of food chunks per bite that can be taken by a
        ///   creature that places MaxAvailableCharacteristicPoints into the EatingSpeedPointsAttribute.
        ///   The number of food chunks taken from this constant is multiplied by the current
        ///   radius of the creature.  This means both MatureSize and the EatingSpeedPointsAttribute
        ///   can increase the number of food chunks taken per bite.
        ///  </para>
        /// </summary>
        public const int MaximumEatingSpeedPerUnitOfRadius = 100;

        /// <summary>
        ///  <para>
        ///   Represents the maximum distance a creature can see if they place
        ///   MaxAvailableCharacteristicPoints into the EyesightPointsAttribute.
        ///  </para>
        ///  <para>
        ///   This distance is in Terrarium Cells, so you have to multiply
        ///   by 8 to get the actual distance in Terrarium Units (pixels).
        ///  </para>
        /// </summary>
        public const int MaximumEyesightRadius = 10;

        /// <summary>
        ///  <para>
        ///   Represents the maximum amount of damage that can be dealt by
        ///   a creature that places MaxAvailableCharacteristicPoints into the
        ///   AttackDamagePointsAttribute.
        ///   The amount of damage taken from this constant is multiplied by the current
        ///   radius of the creature.  This means both MatureSize and the AttackDamagePointsAttribute
        ///   can increase the amount of damage your creature can dish out.
        ///  </para>
        /// </summary>
        public const int MaximumInflictedDamagePerUnitOfRadius = 25;

        /// <summary>
        ///  <para>
        ///   Represents the largest possible value that can be placed
        ///   into the MatureSize attribute.  No creature may grow to
        ///   be larger than this constant.
        ///  </para>
        /// </summary>
        public const int MaxMatureSize = 48;

        /// <summary>
        ///  <para>
        ///   The maximum distance that a plant can spread its seeds when reproducing.
        ///  </para>
        /// </summary>
        public const int MaxSeedSpreadDistance = 1000;

        private const double MinimumSpeedToMoveAtMinimumEnergy = 5;
        private const double MinimumUnitsToMoveAtMinimumEnergy = 2000;

        /// <summary>
        ///  <para>
        ///   Represents the smallest possible value that can be placed
        ///   into the MatureSize attribute.  No creature may grow to
        ///   maturity and still be smaller than this constant.
        ///  </para>
        /// </summary>
        public const int MinMatureSize = 25;

        /// <internal/>
        public const int MonitorModeHeight = 600;

        /// <internal/>
        public const int MonitorModeWidth = 800;

        // We don't want too many teleporters or it really scrambles things up.  We base it on
        // the size of the world, which is related to the maximum number of animals alive.
        /// <internal/>
        public const int NumberOfAnimalsPerTeleporter = 100;

        /// <internal/>
        public const int OrganismSchedulingBlacklistOvertime = 500000;

        /// <internal/>
        public const int OrganismSchedulingMaximumOvertime = 50000;

        /// <summary>
        ///  <para>
        ///   The amount of food chunks a plant amounts to. Larger plants will
        ///   have a larger amount of food chunks available to herbivores than
        ///   smaller plants.
        ///  </para>
        /// </summary>
        public const int PlantFoodChunksPerUnitOfRadius = 50;

        /// <internal/>
        public const double PlantIncubationEnergyMultiplier = 1.5;

        /// <summary>
        ///  <para>
        ///   The amount of energy required per tick in order to incubate
        ///   an offspring.  This is multiplied by the plant's Radius
        ///   so larger plants require more energy to incubate than
        ///   smaller plants.
        ///  </para>
        /// </summary>
        public const double PlantIncubationEnergyPerUnitOfRadius =
            (PlantMatureSizeProvidedEnergyPerUnitRadius/TicksToIncubate)*
            PlantIncubationEnergyMultiplier;

        /// <summary>
        ///  <para>
        ///   The amount of time in game ticks that a plant can stay alive.  This
        ///   number is multiplied by the plant's MatureSize so smaller plants
        ///   will not live as long as larger plants.
        ///  </para>
        /// </summary>
        public const int PlantLifeSpanPerUnitMaximumRadius = 150;

        /// <internal/>
        public const double PlantMatureSizeProvidedEnergyPerUnitRadius =
            PlantFoodChunksPerUnitOfRadius*EnergyPerPlantFoodChunk;

        /// <summary>
        ///  <para>
        ///   The maximum number of healing points a plant can
        ///   heal per tick.  This is multiplied by the Radius of the
        ///   plant and so larger plants can heal many more points
        ///   than smaller plants.
        ///  </para>
        /// </summary>
        public const int PlantMaxHealingPerTickPerRadius = 1;

        /// <summary>
        ///  <para>
        ///   The amount of time in game ticks that a plant must wait before being
        ///   able to reproduce.  This number is multiplied by the plant's Radius
        ///   and so larger plants take longer between reproductions than smaller
        ///   plants.
        ///  </para>
        /// </summary>
        public const int PlantReproductionWaitPerUnitRadius = 25;

        /// <summary>
        ///  <para>
        ///   The amount of energy required to heal a plant by a single
        ///   health unit.
        ///  </para>
        /// </summary>
        public const int PlantRequiredEnergyPerUnitOfHealing = 100;

        /// <summary>
        ///  <para>
        ///   The amount of energy burned by a plant in order to grow a single unit
        ///   of radius.  For smaller plants this amount of energy will be quite
        ///   taxing, but as the plant grows larger the amount of energy taken
        ///   doesn't affect the plant as much.
        ///  </para>
        /// </summary>
        public const double PlantRequiredEnergyPerUnitOfRadiusGrowth = MaxEnergyBasePerUnitRadius*(1/(double) 5);

        /// <summary>
        ///  <para>
        ///   The amount of energy burned by a creature in order to move.  If you take
        ///   the speed the creature is moving, the radius, and the distance they are
        ///   moving along with this constant you can judge how much energy will be
        ///   expended.
        ///  </para>
        ///  <para>
        ///   Note that RadiusSpeedDistance is equal to Radius * Speed * Distance.
        ///   This means that the faster a creature moves the more energy will be
        ///   consumed for equal distances.  A creature should make sure only to
        ///   move as fast as necessary to conserve energy.
        ///  </para>
        /// </summary>
        public const double RequiredEnergyPerUnitOfRadiusSpeedDistance = .005;
                            // RadiusSpeedDistance = Radius * Speed * Distance

        /// <summary>
        ///  <para>
        ///   Represents the base achievable speed granted to a creature
        ///   that places 0 points into the MaximumSpeedAttribute.
        ///  </para>
        /// </summary>
        public const int SpeedBase = 5;

        /// <summary>
        ///  <para>
        ///   Represents the maximum achievable speed granted to a creature
        ///   that places MaxAvailableCharacteristicPoints into the MaximumSpeedAttribute.
        ///  </para>
        /// </summary>
        public const int SpeedMaximum = 100;

        /// <summary>
        ///  <para>
        ///   The amount of time in game ticks that is required between a call
        ///   to BeginReproduction and a new creature actually being born
        ///   from the parent.
        ///  </para>
        /// </summary>
        public const int TicksToIncubate = 10;

        /// <summary>
        ///  <para>
        ///   The amount of time in game ticks that a creature's corpse stays
        ///   in the EcoSystem before it rots away and is removed.  Carnivores
        ///   should make sure to quickly pounce on dead corpses as a food source
        ///   before they rot.
        ///  </para>
        /// </summary>
        public const int TimeToRot = 60;

        /// <internal/>
        public const int ViewPortHeight = 450;

        /// <internal/>
        public const int ViewPortWidth = 800;

        // Lots of the constants are interrelated.  The asserts below are checks we do to make sure
        // constants aren't set to something that causes bad side effects.
        /// <internal/>
        public static void EngineSettingsAsserts()
        {
            // For an animal of radius 1, moving 1 unit at speed 1 should always require more energy than just being alive
            // Otherwise, you end up in a world where, for a given amount of energy, someone who moves slowly can't go as far
            // as someone who moves quickly because the amount of energy burned just being alive penalizes them more
            Debug.Assert(RequiredEnergyPerUnitOfRadiusSpeedDistance > BaseAnimalEnergyPerUnitOfRadius);

            // If we expend enough energy in one shot to go from normal to below hungry it's a problem
            // Just growing could kill.
            Debug.Assert(PlantRequiredEnergyPerUnitOfRadiusGrowth <= (((MaxEnergyBasePerUnitRadius/5)*1)));
            Debug.Assert(AnimalRequiredEnergyPerUnitOfRadiusGrowth <= (((MaxEnergyBasePerUnitRadius/5)*1)));

            // If we expend enough energy in incubation tick to go from normal to below hungry it's a problem
            // Just growing could kill.
            Debug.Assert(AnimalIncubationEnergyPerUnitOfRadius <= (((MaxEnergyBasePerUnitRadius/5)*1)));
            Debug.Assert(PlantIncubationEnergyPerUnitOfRadius <= (((MaxEnergyBasePerUnitRadius/5)*1)));

            // Make sure that a full sized plant can survive with only half available light
            Debug.Assert((BasePlantEnergyPerUnitOfRadius*MaxMatureSize) <
                         (MaxEnergyFromLightPerTick/(float) 2));

            // If the base energy burn per turn is smaller than what you can eat, you can't
            // survive
            Debug.Assert((float) BaseAnimalEnergyPerUnitOfRadius*MaxMatureSize <
                         EnergyPerPlantFoodChunk*(float) BaseEatingSpeedPerUnitOfRadius);

            // 1.   An organism needs to be able to reproduce twice in its lifetime in order to have 
            //      it's population grow.
            // 2.   An organism can only reproduce when it is mature.  
            // 3.   An organism reaches mature size when it is halfway through it's life (at the earliest)
            // Thus: the time between when an organism matures, and when it dies (which is half its lifespan)
            // needs to be at least twice the ReproductionWaitPerUnitRadius of organisms or they
            // will never get a population above 1.  But that assumes perfect conditions.
            // 3 is a better number to give some slack.
            // Also: if AnimalLifeSpanPerUnitMaximumRadius <  PlantReproductionWaitPerUnitRadius, it means
            // that larger animals get a less chance to reproduce, if it's > it means that larger animals get
            // more of a chance to reproduce.
            Debug.Assert(((AnimalLifeSpanPerUnitMaximumRadius*MaxMatureSize)/(float) 2) >=
                         3*((float) AnimalReproductionWaitPerUnitRadius*MaxMatureSize));
            Debug.Assert(((PlantLifeSpanPerUnitMaximumRadius*MaxMatureSize)/(float) 2) >=
                         3*((float) PlantReproductionWaitPerUnitRadius*MaxMatureSize));

            Debug.Assert(OrganismSchedulingMaximumOvertime < OrganismSchedulingBlacklistOvertime);

            // We should never set this so max energy is zero
            Debug.Assert((int) MaxEnergyBasePerUnitRadius > 0);
        }
    }
}