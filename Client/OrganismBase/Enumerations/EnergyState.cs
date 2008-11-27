//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Contains the different energy levels a creature goes through from Full
    ///   to Dead.  Normal represents the desired energy state for most creatures.
    ///  </para>
    /// </summary>
    public enum EnergyState
    {
        /// <summary>
        ///  <para>
        ///   The creature has no energy at this point and can't sustain existence.
        ///   A creature that reaches this state will die due to Starvation.
        ///  </para>
        /// </summary>
        Dead,

        /// <summary>
        ///  <para>
        ///   The creature has little energy and will soon die from Starvation.
        ///   When a creature reaches this point it should quickly find some food.
        ///  </para>
        /// </summary>
        Deterioration,

        /// <summary>
        ///  <para>
        ///   The creature is starting to get low on energy.  Most creatures should
        ///   look for food at this point if they aren't already.  Any Herbivore that
        ///   is at Hungry or below is capable of unprovoked attacks on other creatures.
        ///  </para>
        /// </summary>
        Hungry,

        /// <summary>
        ///  <para>
        ///   This is the desired energy zone for most creatures.  This is the point
        ///   where creatures can heal themselves, grow, and reproduce.  Creatures
        ///   have plenty of energy at this point, but can still eat and obtain higher
        ///   energy levels.
        ///  </para>
        /// </summary>
        Normal,

        /// <summary>
        ///  <para>
        ///   The creature has more than enough energy and can no longer eat.  This
        ///   state can't be maintained for very long and the creature will soon slip
        ///   back into the normal range again.
        ///  </para>
        /// </summary>
        Full
    }
}