//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    /// Base class for all exceptions that the game throws to organisms.
    /// </summary>
    public class OrganismException : GameEngineException
    {
        internal OrganismException(string message) : base(message)
        {
        }
    }
}