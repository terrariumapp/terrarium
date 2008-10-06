//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;

using OrganismBase;
using Terrarium.Configuration;

namespace Terrarium.Game
{
    /// <summary>
    ///  Thrown when a peer is unable to connect to various remote
    ///  web services.  This demonstrates a peer that might not
    ///  have an internet connection or that is behind a highly
    ///  restrictive firewall.
    /// </summary>
    [Serializable]
    public class InvalidPeerException : GameEngineException
    {
        /// <summary>
        ///  Creates a default invalid peer exception object.
        /// </summary>
        public InvalidPeerException() : base(string.Format("Invalid Peer Issue\n\nYour peer appears to be behind a firewall or other network translation hardware.  Please go to our website for instructions on how to enable your system to run Terrarium in the peer to peer network.\n{0}", GameConfig.WebRoot))
        {
        }
    }
}