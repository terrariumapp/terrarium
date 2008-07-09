//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Diagnostics;

namespace Terrarium.PeerToPeer 
{
    class LeaseComparer : IComparer
    {
        public int Compare (object x, object y)
        {
            Debug.Assert(x is Peer && y is Peer);

            Peer peer1 = (Peer) x;
            Peer peer2 = (Peer) y;
            return peer1.LeaseTimeout.CompareTo(peer2.LeaseTimeout);
        }
    }
}