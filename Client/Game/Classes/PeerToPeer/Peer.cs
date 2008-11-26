//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;

namespace Terrarium.PeerToPeer
{
    /// <summary>
    ///  Represents information about a peer.  Currently this
    ///  information is used to control peer leasing and
    ///  throttling.
    /// </summary>
    internal class Peer
    {
        /// <summary>
        ///  Initialize a new peer using an IP Address and the timeout
        ///  date on the lease.  This peer object is then stored in
        ///  either the known peers or bad peers collections of the PeerManager.
        /// </summary>
        /// <param name="ipAddress">The IP address of this peer.</param>
        /// <param name="leaseTimeout">The timeout date on the peer's lease.</param>
        internal Peer(string ipAddress, DateTime leaseTimeout)
        {
            IPAddress = ipAddress;
            LeaseTimeout = leaseTimeout;
        }

        /// <summary>
        ///  Retrieves the read-only IP Address for this peer.  No
        ///  dynamically morphing a peer to work on another IP.  Each
        ///  IP is considered a distinct and new peer.
        /// </summary>
        internal string IPAddress { get; private set; }

        /// <summary>
        ///  The timeout on the peer's lease.  This can be used to
        ///  remove the peer from peer collections, move them
        ///  between peer collections, and to determine if the
        ///  peer is valid for a peer connection.
        /// </summary>
        internal DateTime LeaseTimeout { get; private set; }

        /// <summary>
        ///  Whenever a conversation completes this property is
        ///  used in order to store the time.  Other code can
        ///  check this property to determine if the current
        ///  conversation from the peer should be accepted.  If
        ///  the last receipt wasn't long enough ago, the peer
        ///  can abort the conversation.  This has no effect on
        ///  either peer except that the conversation doesn't
        ///  complete.
        ///  This is used to make sure people don't cheat by 
        ///  spamming peers with a custom Terrarium client
        /// </summary>
        internal DateTime LastReceipt { get; set; }
    }
}