//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using OrganismBase;

namespace Terrarium.Game
{
    /// <summary>
    ///  Exception thrown based on returns from the reporting
    ///  server to indicate that one of your creature's has
    ///  been blacklisted globally.
    /// </summary>
    [Serializable]
    public class OrganismBlacklistException : GameEngineException
    {
        /// <summary>
        ///  Generates a default OrganismBlacklistException
        /// </summary>
        public OrganismBlacklistException()
            : base("You have an organism that needs to be cleaned out because it was blacklisted.")
        {
        }
    }
}