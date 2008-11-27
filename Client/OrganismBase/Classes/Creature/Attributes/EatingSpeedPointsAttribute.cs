//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    /// Determines how quickly your animal can eat.  The higher the value the faster food can be eaten.
    /// </summary>
    /// <remarks>
    /// <para>
    /// High values applied to this attribute mean your animal can eat before other animals get there to benefit from the kill
    /// if you're a carnivore or it means you can eat quickly and hide out if you're an herbivore.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class EatingSpeedPointsAttribute : PointBasedCharacteristicAttribute
    {
        /// <param name="eatingSpeedPoints">
        /// Specify the number of points (from 1 to 100) to apply to this attribute.
        /// </param>
        public EatingSpeedPointsAttribute(int eatingSpeedPoints)
            : base(eatingSpeedPoints, EngineSettings.MaximumEatingSpeedPerUnitOfRadius)
        {
        }

        // Returns food chunks per bite
        /// <internal/>
        public int EatingSpeedPerUnitRadius
        {
            get
            {
                return
                    (int)
                    (EngineSettings.BaseEatingSpeedPerUnitOfRadius +
                     PercentOfMaximum*EngineSettings.MaximumEatingSpeedPerUnitOfRadius);
            }
        }
    }
}