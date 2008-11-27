//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    /// Determines how large your organism will be.</summary>
    /// <remarks>
    /// <para>
    /// <list type="bullet">
    /// <listheader>Pros for larger organisms</listheader>
    /// <item><term>Attacks against smaller animals are more effective (damage inflicted is per unit of radius) which means there are more animals you can eat</term></item>
    /// <item><term>Defends against smaller animals are more effective (damage deterred is per unit of radius)</term></item>
    /// <item><term>Lifespan is longer</term></item>
    /// </list>
    /// </para>
    /// <para>
    /// <list type="bullet">
    /// <listheader>Cons for larger organisms</listheader>
    /// <item><term>Burn more energy per turn to stay alive, and to move (which means you need to eat more)</term></item>
    /// <item><term>Takes longer to reach maturity and thus to give birth</term></item>
    /// </list>
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class MatureSizeAttribute : Attribute
    {
        private readonly int _matureSize;

        /// <param name="matureSize">
        /// a size less than or equal to EngineSettings.MaxMatureSize
        /// and greater than or equal to EngineSettings.MinMatureSize
        /// </param>
        public MatureSizeAttribute(int matureSize)
        {
            if (matureSize > EngineSettings.MaxMatureSize || matureSize < EngineSettings.MinMatureSize)
            {
                throw new SizeOutOfRangeCharacteristicException();
            }

            _matureSize = matureSize;
        }

        ///<summary>
        ///</summary>
        public int MatureRadius
        {
            // Divide by two since we track radius
            get { return _matureSize/2; }
        }
    }
}