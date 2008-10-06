//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using OrganismBase;

namespace Terrarium.Game
{
    /// <summary>
    ///  Exception thrown whenever an issue happens during peer communication and we want to
    ///  abort the communication and just teleport locally.
    /// </summary>
    [Serializable]
    public sealed class AbortPeerDiscussionException : GameEngineException
    {
        /// <summary>
        ///  Creates a new peer abort exception given a reason for the
        ///  communication being aborted.
        /// </summary>
        /// <param name="reason">The reason communicaton was terminated.</param>
        public AbortPeerDiscussionException(string reason) : base("Peer discussion aborted: " + reason)
        {
        }
    }
}