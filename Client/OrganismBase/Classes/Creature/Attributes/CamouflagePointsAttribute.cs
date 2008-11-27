//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Determines how easily your animal can be seen by other animals
    ///  </para>
    ///  <para>
    ///   The more points you apply, the less likely it is that another animal will see you when they look around.
    ///  </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class CamouflagePointsAttribute : PointBasedCharacteristicAttribute
    {
        /// <summary>
        ///  Creates a new camouflage attribute given a number of points from 0 to 100 to
        ///  put into the characteristic
        /// </summary>
        /// <param name="camouflagePoints">Specify the number of points (from 0 to 100) to apply to this attribute</param>
        public CamouflagePointsAttribute(int camouflagePoints)
            : base(camouflagePoints, EngineSettings.InvisibleOddsMaximum)
        {
        }

        /// <summary>
        ///  Returns the calculated odds of a creature actually appearing invisible.
        ///  This is a direct factor as to whether the creature will actually be
        ///  seen or not and is not scaled by distance or the ability of sight of
        ///  the other creatures.
        /// </summary>
        public int InvisibleOdds
        {
            get
            {
                return (int) (EngineSettings.InvisibleOddsBase +
                              PercentOfMaximum*EngineSettings.InvisibleOddsMaximum);
            }
        }
    }
}