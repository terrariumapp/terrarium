//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///    <para>Can only apply 100 points to any given characteristic.</para>
    /// </summary>
    public class TooManyPointsOnOneCharacteristicException : GameEngineException
    {
        /// <summary>
        ///    <para>Can only apply 100 points to any given characteristic.</para>
        /// </summary>
        public TooManyPointsOnOneCharacteristicException() : base("Point-based characteristics must be <= " +
                                                                  EngineSettings.MaxAvailableCharacteristicPoints)
        {
        }
    }
}