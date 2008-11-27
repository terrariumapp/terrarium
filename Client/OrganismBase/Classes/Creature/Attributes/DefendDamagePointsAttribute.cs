//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    /// Maximum damage your animal can defend against.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Points applied to this attribute mean your animal can defend itself against stronger attacks.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class DefendDamagePointsAttribute : PointBasedCharacteristicAttribute
    {
        /// <param name="defensePoints">
        /// Specify the number of points (from 1 to 100) to apply to this attribute.
        /// </param>
        public DefendDamagePointsAttribute(int defensePoints)
            : base(defensePoints, EngineSettings.MaximumDefendedDamagePerUnitOfRadius)
        {
        }

        // Returns food chunks per bite
        /// <internal/>
        public int MaximumDefendDamagePerUnitRadius
        {
            get
            {
                return (int) ((EngineSettings.BaseDefendedDamagePerUnitOfRadius +
                               PercentOfMaximum*EngineSettings.MaximumDefendedDamagePerUnitOfRadius) + 0.001f);
            }
        }
    }
}