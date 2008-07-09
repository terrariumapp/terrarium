//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using OrganismBase;

namespace Terrarium.Game
{
    /// <summary>
    ///  Exception is thrown whenever an organism needs to be blacklisted.
    /// </summary>
    [Serializable]
    public sealed class OrganismBlacklistedException : Exception
    {
        /// <summary>
        ///  Generates a default OrganismBlacklistedException
        /// </summary>
        public OrganismBlacklistedException()
        {
        }
    }
}