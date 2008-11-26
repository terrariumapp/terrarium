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
        private OrganismState _organismState;

        // We can't hold on to the actual organism object here, because
        // when it gets deserialized on the other peer, it would look for the
        // assembly which may not be there.  Instead we serialize it into a 
        // byte array.
        private byte[] _serializedWrapper;

        internal Organism Organism { get; set; }

        internal OrganismWrapper OrganismWrapper
        {
            // Deserialize the organism from the byte stream.
            get
            {
                // Use a binder to ensure that only a certain set of objects
                // can be deserialized that we know to be safe from a
                // Code Access Security perspective.
                var binder = new BinaryFormatter { Binder = new OrganismWrapperBinder() };
                var stream = new MemoryStream(_serializedWrapper);
                return (OrganismWrapper) binder.Deserialize(stream);
            }
            // Serialize the organism into a byte stream;
            set
            {
                var b = new BinaryFormatter();
                var stream = new MemoryStream();
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

        internal Guid Originator { get; set; }

        internal bool TeleportedToSelf { get; set; }

        internal string Country { get; set; }

        internal string State { get; set; }
    }
}