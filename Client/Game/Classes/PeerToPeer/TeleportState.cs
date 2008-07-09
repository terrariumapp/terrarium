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
        OrganismState organismState;
    
        // Used for local teleporations
        [NonSerialized]
        Organism org;
        string countryInfo; 
        string stateInfo;   
        bool teleportedToSelf = false;
        Guid originator;

        // We can't hold on to the actual organism object here, because
        // when it gets deserialized on the other peer, it would look for the
        // assembly which may not be there.  Instead we serialize it into a 
        // byte array.
        byte[] serializedWrapper;
    
        internal Organism Organism 
        {
            get 
            {
                return org;
            }
        
            set 
            {
                org = value;
            }
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
                MemoryStream stream = new MemoryStream(serializedWrapper);
                return (OrganismWrapper) b.Deserialize(stream);
            }
        
            // Serialize the organism into a byte stream;
            set 
            {
                BinaryFormatter b = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                b.Serialize(stream, value);
                serializedWrapper = stream.GetBuffer();
            }
        }

        internal OrganismState OrganismState 
        {
            get 
            {
                return organismState;
            }

            set 
            {
                Debug.Assert(value.IsImmutable);
                organismState = value;
            }
        }

        internal Guid Originator 
        {
            get 
            {
                return originator;
            }

            set 
            {
                originator = value;
            }
        }

        internal bool TeleportedToSelf
        {
            get 
            {
                return teleportedToSelf;
            }

            set
            {
                teleportedToSelf = value;
            }
        }
    
        internal string Country 
        {
            get 
            {
                return countryInfo;
            }
        
            set
            {
                countryInfo = value;
            }
        }

        internal string State
        {
            get 
            {
                return stateInfo;
            }
        
            set 
            {
                stateInfo = value;
            }
        }
    }
}