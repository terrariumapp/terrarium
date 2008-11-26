//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///    The base class for all exceptions an organism will
    ///    receive from the game.
    /// </summary>
    [Serializable]
    public class GameEngineException : Exception
    {
        /// <summary>
        ///    <para>Constructs a new Exception</para>
        /// </summary>
        public GameEngineException(string message) : base(message)
        {
        }
    }
}