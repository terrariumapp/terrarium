//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using OrganismBase;

namespace Terrarium.Game
{
    /// <summary>
    ///  Thrown by a special Terrarium creature class that is used to replace
    ///  an organism that has been blacklisted.  This enables the removal of
    ///  creatures without having to delete the entire EcoSystem.
    /// </summary>
    [Serializable]
    public class MaliciousOrganismException : GameEngineException
    {
        /// <summary>
        ///  Creates a default MaliciousOrganismException
        /// </summary>
        public MaliciousOrganismException() : base("An organism is trying to compromise Terrarium stability.")
        {
        }
    }
}