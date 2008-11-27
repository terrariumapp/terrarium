//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase 
{
    /// <summary>
    ///  <para>
    ///   This interface holds all species related information that is
    ///   common to both plants and animals.  Creature developers can access
    ///   all of this information by using the Species property of an OrganismState
    ///   object.
    ///  </para>
    ///  <para>
    ///   All species information is immutable.  A creature's species represents
    ///   its basic capabilities such as the mature size, time between growth
    ///   spurts, and the life span.
    ///  </para>
    /// </summary>
    public interface ISpecies
    {
        /// <summary>
        ///  <para>
        ///   The maximum radius that a creature can achieve once
        ///   they are fully matured.  Once a creature has fully
        ///   matured they may perform additional actions such
        ///   as reproduction, while others actions cease such
        ///   as growth.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the radius of the creature when fully mature.
        /// </returns>
        int MatureRadius
        {
            get;
        }

        /// <summary>
        ///  <para>
        ///   The amount of time in ticks a creature must wait between reproductions.
        ///   Creatures can normally reproduce more than once in a lifetime,
        ///   so the time limit between reproduction and the life span of the
        ///   creature are used to define the maximum number of reproductions.
        ///  </para>
        ///  <para>
        ///   Each tick ReproductionWait will drop by one.  Once the ReproductionWait
        ///   reaches 0 one of the requirements the CanReproduce property will be met.
        ///   Once a creature is capable of reproducing a call to BeginReproduction
        ///   must be made to start incubation.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the number of ticks between reproductions.
        /// </returns>
        int ReproductionWait
        {
            get;
        }

        /// <summary>
        ///  <para>
        ///   The amount of time in ticks a creature is capable of living.  Once
        ///   the creature reaches the full lifespan of the species it is killed
        ///   with the reason PopulationChangeReason.OldAge.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the number of ticks a creature can live.
        /// </returns>
        int LifeSpan
        {
            get;
        }

        /// <summary>
        ///  <para>
        ///   The amount of time in ticks a creature must wait before growing
        ///   another unit of radius in size.  The initial GrowthWait is defined in
        ///   such a way that a creature will reach full size or MatureRadius
        ///   after one half of the LifeSpan.
        ///  </para>
        ///  <para>
        ///   Each tick GrowthWait will drop by one.  Once the GrowthWait reaches 0
        ///   the creature will grow if it meets all growth requirements.  Growth
        ///   will happen automatically as long as the requirements for growth have
        ///   been met.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the number of ticks before a creature can grow.
        /// </returns>
        int GrowthWait
        {
            get;
        }

        /// <summary>
        ///  <para>
        ///   The maximum amount of energy a creature can store for every unit of Radius
        ///   it has grown.  This allows larger creatures to store more energy than
        ///   smaller creatures.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the maximum amount of energy a creature can store per unit radius.
        /// </returns>
        int MaximumEnergyPerUnitRadius
        {
            get;
        }

        /// <summary>
        ///  <para>
        ///   Returns the Skin the creature will use when being displayed in the Terrarium.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.String identifying the creature's skin.
        /// </returns>
        string Skin
        {
            get;
        }

        /// <summary>
        ///  <para>
        ///   Used to determine if a creature with the given Species information is the
        ///   same as another creature given their Species information.  This is useful
        ///   for assessing and remembering the strengths/weaknesses of other creatures.
        ///  </para>
        /// </summary>
        /// <param name="species">
        ///  ISpecies interface for the creature to be compared with this creature.
        /// </param>
        /// <returns>
        ///  True if the creature's species information is the same, False otherwise.
        /// </returns>
        bool IsSameSpecies(ISpecies species);
    }
}