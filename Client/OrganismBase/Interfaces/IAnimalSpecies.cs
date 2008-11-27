//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Species information properties that are applicable only to animals
    ///   and that should be made available to developers.  Use this to find
    ///   out general information about a particular animal species' capabilities.
    ///  </para>
    /// </summary>
    public interface IAnimalSpecies : ISpecies
    {
        /// <summary>
        ///  <para>
        ///   Returns the SkinFamily the creature will use when being displayed in the Terrarium.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.String identifying the creature's skin family.
        /// </returns>
        AnimalSkinFamily SkinFamily { get; }

        /// <summary>
        ///  <para>
        ///   Determines if a creature is a Carnivore.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  True if the creature is a Carnivore, False otherwise.
        /// </returns>
        Boolean IsCarnivore { get; }

        /// <summary>
        ///  <para>
        ///   The number of food chunks a creature can eat in one tick
        ///   per unit Radius.  This means larger creatures can eat more
        ///   than smaller creatures in one tick.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the food chunks that can be eaten per tick per unit radius.
        /// </returns>
        int EatingSpeedPerUnitRadius { get; }

        /// <summary>
        ///  <para>
        ///   The maximum amount of damage that can be inflicted by a creature
        ///   in a single tick per unit radius.  This means larger creatures can
        ///   attack harder than smaller creatures per tick.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the maximum damage that can be inflicted per unit radius
        /// </returns>
        int MaximumAttackDamagePerUnitRadius { get; }

        /// <summary>
        ///  <para>
        ///   The maximum amount of damage that can be absorbed by a creature
        ///   in a single tick per unit radius.  This means larger creatures can
        ///   defend better than smaller creatures per tick.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the maximum damage that can be absorbed per unit radius
        /// </returns>
        int MaximumDefendDamagePerUnitRadius { get; }

        /// <summary>
        ///  <para>
        ///   The maximum speed the creature is capable of moving.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the maximum speed the creature is capable of moving.
        /// </returns>
        int MaximumSpeed { get; }

        /// <summary>
        ///  <para>
        ///   The odds that a creature is invisible to another creature that uses the
        ///   Scan method.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the odds a creature will appear invisible.
        /// </returns>
        int InvisibleOdds { get; }

        /// <summary>
        ///  <para>
        ///   The distance that a creature can see.  This is used to determine the amount
        ///   of area to be evaluated in a call to the Scan method.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the distance a creature can see.
        /// </returns>
        int EyesightRadius { get; }
    }
}