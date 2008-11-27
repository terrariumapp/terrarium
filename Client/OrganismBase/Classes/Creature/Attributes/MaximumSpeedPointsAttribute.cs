//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    /// Determines the top speed your animal can attain.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The more points you apply to this attribute, the faster your animal will be able to move.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class MaximumSpeedPointsAttribute : PointBasedCharacteristicAttribute
    {
        /// <param name="speedPoints">
        /// Specify the number of points (from 1 to 100) to apply to this attribute.
        /// </param>
        public MaximumSpeedPointsAttribute(int speedPoints) : base(speedPoints, EngineSettings.SpeedMaximum)
        {
        }

        // Returns food chunks per bite
        /// <internal/>
        public int MaximumSpeed
        {
            get
            {
                return (int) (EngineSettings.SpeedBase +
                              PercentOfMaximum*EngineSettings.SpeedMaximum);
            }
        }
    }
}