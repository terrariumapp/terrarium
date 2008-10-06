//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using OrganismBase;

namespace Terrarium.Game
{
    /// <summary>
    ///  Exception is thrown whenever a required creature attribute
    ///  has not been supplied in the assembly.
    /// </summary>
    [Serializable]
    public sealed class AttributeRequiredException : GameEngineException
    {
        /// <summary>
        ///  Create a new required attribute exception for the given
        ///  characteristic.
        /// </summary>
        /// <param name="characteristic">The missing characteristic.</param>
        public AttributeRequiredException(string characteristic)
            : base(string.Format("You must define the '{0}' attribute on your organism", characteristic))
        {
        }
    }
}