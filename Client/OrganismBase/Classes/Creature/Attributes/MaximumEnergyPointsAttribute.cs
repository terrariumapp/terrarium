//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>Points applied to the MaximumEnergyPoints Attribute determine how much energy your animal can store -- 
    /// it determines how often your organism has to eat.</summary>
    /// <remarks>
    /// <para>
    /// If you apply lots of points to this attribute, your animal will be like a snake: it can fill up and not have to eat for
    /// a long time.  Less points here means you'll be more like a hummingbird: you'll need to eat small amounts constantly.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class MaximumEnergyPointsAttribute : PointBasedCharacteristicAttribute
    {
        /// <param name="maximumEnergyPoints">
        /// Specify the number of points (from 1 to 100) to apply to this attribute.
        /// </param>
        public MaximumEnergyPointsAttribute(int maximumEnergyPoints)
            : base(maximumEnergyPoints, (int) EngineSettings.MaxEnergyMaximumPerUnitRadius)
        {
        }

        /// <internal/>
        public int MaximumEnergyPerUnitRadius
        {
            get
            {
                return (int) ((float) EngineSettings.MaxEnergyBasePerUnitRadius +
                              PercentOfMaximum*(float) EngineSettings.MaxEnergyMaximumPerUnitRadius);
            }
        }
    }
}