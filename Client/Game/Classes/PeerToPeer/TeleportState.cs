//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using OrganismBase;
using Terrarium.Hosting;

namespace Terrarium.PeerToPeer
{
    // This is the object that is sent between Terrariums to transport
    // an organism
    [Serializable]
    internal class TeleportState
    {
        private string _countryInfo;
        [NonSerialized] private Organism _org;
        private OrganismState _organismState;
        private Guid _originator;

        // We can't hold on to the actual organism object here, because
        // when it gets deserialized on the other peer, it would look for the
        // assembly which may not be there.  Instead we serialize it into a 
        // byte array.
        private byte[] _serializedWrapper;
        private string _stateInfo;
        private bool _teleportedToSelf;

        internal Organism Organism
        {
            get { return _org; }
            set { _org = value; }
        }

        internal OrganismWrapper OrganismWrapper
        {
            // Deserialize the organism from the byte stream.
            get
            {
                BinaryFormatter b = new BinaryFormatter();

                // Use a binder to ensure that only a certain set of objects
                // can be deserialized that we know to be safe from a
                // Code Access Security perspective.
                b.Binder = new OrganismWrapperBinder();
                MemoryStream stream = new MemoryStream(_serializedWrapper);
                return (OrganismWrapper) b.Deserialize(stream);
            }
            // Serialize the organism into a byte stream;
            set
            {
                BinaryFormatter b = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                b.Serialize(stream, value);
                _serializedWrapper = stream.GetBuffer();
            }
        }

        internal OrganismState OrganismState
        {
            get { return _organismState; }

            set
            {
                Debug.Assert(value.IsImmutable);
                _organismState = value;
            }
        }

        internal Guid Originator
        {
            get { return _originator; }
            set { _originator = value; }
        }

        internal bool TeleportedToSelf
        {
            get { return _teleportedToSelf; }
            set { _teleportedToSelf = value; }
        }

        internal string Country
        {
            get { return _countryInfo; }
            set { _countryInfo = value; }
        }

        internal string State
        {
            get { return _stateInfo; }
            set { _stateInfo = value; }
        }
    }
}