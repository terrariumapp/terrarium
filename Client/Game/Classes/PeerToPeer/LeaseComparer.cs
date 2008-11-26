//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System.Collections;
using System.Diagnostics;

namespace Terrarium.PeerToPeer
{
    internal class LeaseComparer : IComparer
    {
        #region IComparer Members

        public int Compare(object x, object y)
        {
            Debug.Assert(x is Peer && y is Peer);

            var peer1 = (Peer) x;
            var peer2 = (Peer) y;
            return peer1.LeaseTimeout.CompareTo(peer2.LeaseTimeout);
        }

        #endregion
    }
}