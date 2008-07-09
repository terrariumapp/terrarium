//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Text;
using System.Net;
using System.Diagnostics;
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
        // Make sure we get a threadsafe version of this so that it can be modified safely from
        // the listener threads
        private Hashtable knownPeers = Hashtable.Synchronized(new Hashtable());
        private Hashtable knownBadPeers = Hashtable.Synchronized(new Hashtable());
        private const int        maxKnownBadPeers = 30;
        private const int        secsBetweenPeerReceives = 30;

        /// <summary>
        /// Constructor for PeerManager.
        /// </summary>
		public PeerManager()
		{
		}
        
        // The set of peers this client knows about and considers valid and active
        internal Hashtable KnownPeers
        {
            get
            {
                return knownPeers;
            }

            set
            {
                knownPeers = value;
            }
        }

        // Allows a bad peer to be used again if they updated their lease on the server.
        // Returns true if the peer is OK, false if they are blacklisted
        internal bool ClearBadPeer(string ipAddress, DateTime peerLease)
        {
            // Lock so we can check for existence and remove atomically
            // We always lock knownPeers even though we are changing
            // bad peers so we don't deadlock
            lock (knownPeers.SyncRoot)
            {
                Peer badPeer = (Peer) knownBadPeers[ipAddress];
                if (badPeer != null)
                {
                    // If the bad peer has updated their lease, then they
                    // are online again, count them as good
                    // Consider: When do we blacklist them permanently if we can never
                    // get to them
                    if (badPeer.LeaseTimeout < peerLease)
                    {
                        knownBadPeers.Remove(ipAddress);
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

            lock (knownBadPeers.SyncRoot)
            {
                return knownBadPeers.ContainsKey(ipAddress);
            }
        }

        // Record Now as the time we last received something from this peer.
        // this is threadsafe
        internal void SetReceive(string ipAddress)
        {
            lock (knownPeers.SyncRoot)
            {
                if (knownPeers.ContainsKey(ipAddress))
                {
                    ((Peer) knownPeers[ipAddress]).LastReceipt = DateTime.UtcNow;
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

            bool receive = false;
            lock (knownPeers.SyncRoot)
            {
                if (knownPeers.ContainsKey(ipAddress))
                {
                    if (((Peer) knownPeers[ipAddress]).LastReceipt.AddSeconds(secsBetweenPeerReceives) < DateTime.UtcNow)
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
            lock (knownPeers.SyncRoot)
            {
                if (knownPeers.ContainsKey(ipAddress))
                {
                    if (!knownBadPeers.ContainsKey(ipAddress))
                    {
                        Peer badPeer = new Peer(ipAddress, ((Peer) knownPeers[ipAddress]).LeaseTimeout.AddHours(1));
                        knownBadPeers.Add(ipAddress, badPeer);
                    }

                    knownPeers.Remove(ipAddress);
                }
            }
        }

        /// <summary>
        /// Empty the collection of peers that had issues for some reason.
        /// </summary>
        public void ClearBadPeers()
        {
            // We always lock knownPeers even though we are changing
            // bad peers so we don't deadlock
            lock (knownPeers.SyncRoot)
            {
                // Copy from bad peers back to known peers,
                // or we'll quickly get into a blacklisted
                // situation
                foreach (string key in knownBadPeers.Keys)
                {
                    knownPeers[key] = knownBadPeers[key];
                }

                knownBadPeers = Hashtable.Synchronized(new Hashtable());
            }
        }

        // Keeps the bad peer list down to a reasonable size
        internal void TruncateBadPeerList()
        {
            // We always lock knownPeers even though we are changing
            // bad peers so we don't deadlock
            lock (knownPeers.SyncRoot)
            {
                if (knownBadPeers.Count > maxKnownBadPeers)
                {
                    // Remove the peers with the oldest leases
                    Peer [] peers = new Peer[knownBadPeers.Count];
                    knownBadPeers.Values.CopyTo(peers, 0);
                    Array.Sort(peers, new LeaseComparer());
                    for (int index = 0; index < knownBadPeers.Count - maxKnownBadPeers; index++)
                    {
                        knownBadPeers.Remove(peers[index].IPAddress);
                    }
                }
            }
        }

        // Returns an XML fragment useful for debugging.
        internal string GetReport()
        {
            StringBuilder builder = new StringBuilder();

            // Lock the hashtable so that we don't get an exception if something is removed
            lock (knownPeers.SyncRoot)
            {
                builder.Append("<goodPeerCount>");
                builder.Append(knownPeers.Count.ToString());
                builder.Append("</goodPeerCount>");
                builder.Append("<badPeerCount>");
                builder.Append(knownBadPeers.Count.ToString());
                builder.Append("</badPeerCount>");
            }

            return builder.ToString();
        }

        internal IPAddress GetRandomPeer()
        {
            // Lock the hashtable so it doesn't change out from under us and cause an exception
            Random randGen = new Random(DateTime.Now.Millisecond);
            int location = 0;
            IPAddress sendAddress = null;
            lock (knownPeers.SyncRoot)
            {
                location = randGen.Next(0, knownPeers.Count);
                Peer selectedPeer = null;
                int peerCount = 0;
                foreach (Peer peer in knownPeers.Values)
                {
                    if (peerCount == location)
                    {
                        selectedPeer = peer;
                        break;
                    }
                    peerCount++;
                }
                Debug.Assert(peerCount < knownPeers.Count);
                sendAddress = IPAddress.Parse(selectedPeer.IPAddress);
            }

            return sendAddress;
        }
	}
}
