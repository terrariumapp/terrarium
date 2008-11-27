//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    /// Determines how far your animal can see.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The more points you apply, the farther you can see.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class EyesightPointsAttribute : PointBasedCharacteristicAttribute
    {
        /// <param name="eyesightPoints">
        /// Specify the number of points (from 1 to 100) to apply to this attribute.
        /// </param>
        public EyesightPointsAttribute(int eyesightPoints) : base(eyesightPoints, EngineSettings.MaximumEyesightRadius)
        {
        }

        /// <internal/>
        public int EyesightRadius
        {
            get
            {
                return (int) (EngineSettings.BaseEyesightRadius +
                              PercentOfMaximum*EngineSettings.MaximumEyesightRadius);
            }
        }
    }
}