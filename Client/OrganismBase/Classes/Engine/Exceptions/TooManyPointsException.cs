//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///    <para>All points supplied for an organism must add up to 100</para>
    /// </summary>
    public class TooManyPointsException : GameEngineException
    {
        /// <summary>
        ///    <para>All points supplied for an organism must add up to 100</para>
        /// </summary>
        public TooManyPointsException()
            : base("Total of point-based characteristics must be <= " + EngineSettings.MaxAvailableCharacteristicPoints)
        {
        }
    }
}