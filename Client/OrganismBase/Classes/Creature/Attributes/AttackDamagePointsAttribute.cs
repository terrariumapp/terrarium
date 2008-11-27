//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    /// Maximum damage your animal can inflict with one attack
    /// </summary>
    /// <remarks>
    /// <para>
    /// Points applied to this attribute mean your animal can inflict more damage when it attacks
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class AttackDamagePointsAttribute : PointBasedCharacteristicAttribute
    {
        /// <summary>
        ///  Initializes the attribute with an attack value in the range of 0 to 100
        /// </summary>
        /// <param name="attackPoints">Specify the number of points (from 0 to 100) to apply to this attribute.</param>
        public AttackDamagePointsAttribute(int attackPoints)
            : base(attackPoints, EngineSettings.MaximumInflictedDamagePerUnitOfRadius)
        {
        }

        /// <summary>
        ///  Retrieves the amount of damage that can be inflicted per unit of a
        ///  creature's radius.
        /// </summary>
        public int MaximumAttackDamagePerUnitRadius
        {
            get
            {
                return (int) ((EngineSettings.BaseInflictedDamagePerUnitOfRadius +
                               PercentOfMaximum*EngineSettings.MaximumInflictedDamagePerUnitOfRadius) + 0.001f);
            }
        }
    }
}