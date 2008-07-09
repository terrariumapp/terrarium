//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using OrganismBase;

namespace Terrarium.Game
{
    /// <summary>
    ///  This exception is thrown when an organism is inserted into
    ///  the game engine, but another organism with the same Unique
    ///  ID already exists.
    /// </summary>
    [Serializable]
    public class OrganismAlreadyExistsException : GameEngineException
    {
        /// <summary>
        ///  Creates a default instance of the OrganismAlreadyExistsException
        /// </summary>
        public OrganismAlreadyExistsException() : base("This organism ID already exists in the world.")
        {
        }
    }
}