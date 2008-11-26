//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Text;
using Terrarium.Configuration;

namespace Terrarium.PeerToPeer
{
    /// <summary>
    /// Manages the list of peers that this Terrarium knows about.  Each
    /// Terrarium only knows about a small set of peers and the server
    /// sets this up to make sure it is a fully connected graph through
    /// the whole ecosystem.  This is to prevent any machine from knowing
    /// about too many active internet IP addresses for security reasons.
    /// </summary>
    public class PeerManager
    {
        private const int MaxKnownBadPeers = 30;
        private const int SecsBetweenPeerReceives = 30;
        private Hashtable _knownBadPeers = Hashtable.Synchronized(new Hashtable());

        ///<summary>
        ///</summary>
        public PeerManager()
        {
            KnownPeers = Hashtable.Synchronized(new Hashtable());
        }

        /// <summary>
        /// The set of peers this client knows about and considers valid and active
        /// </summary>
        internal Hashtable KnownPeers { get; set; }

        // Allows a bad peer to be used again if they updated their lease on the server.
        // Returns true if the peer is OK, false if they are blacklisted
        internal bool ClearBadPeer(string ipAddress, DateTime peerLease)
        {
            // Lock so we can check for existence and remove atomically
            // We always lock _knownPeers even though we are changing
            // bad peers so we don't deadlock
            lock (KnownPeers.SyncRoot)
            {
                var badPeer = (Peer) _knownBadPeers[ipAddress];
                if (badPeer != null)
                {
                    // If the bad peer has updated their lease, then they
                    // are online again, count them as good
                    // Consider: When do we blacklist them permanently if we can never
                    // get to them
                    if (badPeer.LeaseTimeout < peerLease)
                    {
                        _knownBadPeers.Remove(ipAddress);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // Returns true if this Peer is in our list of Bad Peers
        // which means we couldn't access them last time or for some
        // reason consider them invalid.
        // This is threadsafe
        internal bool BadPeer(string ipAddress)
        {
            // We never set bad peers if we're in demo mode
            if (GameConfig.DemoMode)
            {
                return false;
            }

            lock (_knownBadPeers.SyncRoot)
            {
                return _knownBadPeers.ContainsKey(ipAddress);
            }
        }

        // Record Now as the time we last received something from this peer.
        // this is threadsafe
        internal void SetReceive(string ipAddress)
        {
            lock (KnownPeers.SyncRoot)
            {
                if (KnownPeers.ContainsKey(ipAddress))
                {
                    ((Peer) KnownPeers[ipAddress]).LastReceipt = DateTime.UtcNow;
                }
            }
        }

        // Returns true if this peer hasn't sent to us in a while.
        // This is threadsafe
        internal bool ShouldReceive(string ipAddress)
        {
            // We always allow receiving if we're in demo mode
            if (GameConfig.DemoMode)
            {
                return true;
            }

            var receive = false;
            lock (KnownPeers.SyncRoot)
            {
                if (KnownPeers.ContainsKey(ipAddress))
                {
                    if (((Peer) KnownPeers[ipAddress]).LastReceipt.AddSeconds(SecsBetweenPeerReceives) <
                        DateTime.UtcNow)
                    {
                        receive = true;
                    }
                }
            }

            return receive;
        }

        // Mark this Peer as a bad peer.
        // This is threadsafe
        internal void RemovePeer(string ipAddress)
        {
            // We never remove bad peers from the list if we're using the configfile
            if (GameConfig.UseConfigForDiscovery)
            {
                return;
            }

            // Lock so we can test for contains, and then remove safely
            lock (KnownPeers.SyncRoot)
            {
                if (KnownPeers.ContainsKey(ipAddress))
                {
                    if (!_knownBadPeers.ContainsKey(ipAddress))
                    {
                        var badPeer = new Peer(ipAddress, ((Peer) KnownPeers[ipAddress]).LeaseTimeout.AddHours(1));
                        _knownBadPeers.Add(ipAddress, badPeer);
                    }

                    KnownPeers.Remove(ipAddress);
                }
            }
        }

        /// <summary>
        /// Empty the collection of peers that had issues for some reason.
        /// </summary>
        public void ClearBadPeers()
        {
            // We always lock _knownPeers even though we are changing
            // bad peers so we don't deadlock
            lock (KnownPeers.SyncRoot)
            {
                // Copy from bad peers back to known peers,
                // or we'll quickly get into a blacklisted
                // situation
                foreach (string key in _knownBadPeers.Keys)
                {
                    KnownPeers[key] = _knownBadPeers[key];
                }

                _knownBadPeers = Hashtable.Synchronized(new Hashtable());
            }
        }

        // Keeps the bad peer list down to a reasonable size
        internal void TruncateBadPeerList()
        {
            // We always lock _knownPeers even though we are changing
            // bad peers so we don't deadlock
            lock (KnownPeers.SyncRoot)
            {
                if (_knownBadPeers.Count <= MaxKnownBadPeers) return;

                // Remove the peers with the oldest leases
                var peers = new Peer[_knownBadPeers.Count];
                _knownBadPeers.Values.CopyTo(peers, 0);
                Array.Sort(peers, new LeaseComparer());
                for (var index = 0; index < _knownBadPeers.Count - MaxKnownBadPeers; index++)
                {
                    _knownBadPeers.Remove(peers[index].IPAddress);
                }
            }
        }

        // Returns an XML fragment useful for debugging.
        internal string GetReport()
        {
            var builder = new StringBuilder();

            // Lock the hashtable so that we don't get an exception if something is removed
            lock (KnownPeers.SyncRoot)
            {
                builder.Append("<goodPeerCount>");
                builder.Append(KnownPeers.Count.ToString());
                builder.Append("</goodPeerCount>");
                builder.Append("<badPeerCount>");
                builder.Append(_knownBadPeers.Count.ToString());
                builder.Append("</badPeerCount>");
            }

            return builder.ToString();
        }

        internal IPAddress GetRandomPeer()
        {
            // Lock the hashtable so it doesn't change out from under us and cause an exception
            var randGen = new Random(DateTime.Now.Millisecond);
            IPAddress sendAddress = null;
            lock (KnownPeers.SyncRoot)
            {
                var location = randGen.Next(0, KnownPeers.Count);
                Peer selectedPeer = null;
                var peerCount = 0;
                foreach (Peer peer in KnownPeers.Values)
                {
                    if (peerCount == location)
                    {
                        selectedPeer = peer;
                        break;
                    }
                    peerCount++;
                }
                Debug.Assert(peerCount < KnownPeers.Count);
                if (selectedPeer != null) sendAddress = IPAddress.Parse(selectedPeer.IPAddress);
            }

            return sendAddress;
        }
    }
}