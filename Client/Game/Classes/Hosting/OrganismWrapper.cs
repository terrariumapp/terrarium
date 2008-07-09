//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.Serialization;
using OrganismBase;

using Terrarium.Game;
using Terrarium.Tools;
using Terrarium.Forms;

namespace Terrarium.Hosting 
{
    // Each creature gets wrapped by an OrganismWrapper so that we have
    // a place to put timing and other status information
    [Serializable]
    internal class OrganismWrapper : ISerializable
    {
        Organism _org;
        Int64     _overage = 0;
        Int64     _lastTime = 0;
        Int64     _totalTime = 0;
        Int64     _totalActivations = 0;
        bool      _active = true;

        public OrganismWrapper(Organism org)
        {
            _org = org;
        }

        // ISerializable constructor
        private OrganismWrapper(SerializationInfo info, StreamingContext c)
        {
            try
            {
                Type t = Type.GetType((string) info.GetValue("OrganismType", typeof(string)));
                _org = (Organism) Activator.CreateInstance(t);
            }
            catch (Exception e)
            {
                ErrorLog.LogHandledException(e);

                // Something failed when we tried to deserialize.  This usually means that the
                // animal has been blacklisted and thus their assembly has been nulled out.
                // Replace with an instance of TerrariumOrganism so we don't abort the whole deserialization process.
                // TerrariumOrganism will simply throw an exception when we try to give it a time slice.
                _org = (Organism) Activator.CreateInstance(typeof(TerrariumOrganism));
                return; 
            }

            // Give the animal a chance to deserialize itself
            ((Organism) _org).InternalOrganismDeserialize(new MemoryStream((byte[]) info.GetValue("OrganismInfo", typeof(byte[]))));
            if (_org is Animal)
            {
                ((Animal)_org).InternalAnimalDeserialize(new MemoryStream((byte[]) info.GetValue("AnimalInfo", typeof(byte[]))));
                _org.SerializedStream = new MemoryStream((byte[]) info.GetValue("UserInfo", typeof(byte[])));
            }
            else
            {
                ((Plant)_org).InternalPlantDeserialize(new MemoryStream((byte[]) info.GetValue("PlantInfo", typeof(byte[]))));
                _org.SerializedStream = new MemoryStream((byte[]) info.GetValue("UserInfo", typeof(byte[])));
            }
        }

        void ISerializable.GetObjectData (SerializationInfo info, StreamingContext context)
        {
            if (_org != null)
            {
                info.AddValue("OrganismType", _org.GetType().AssemblyQualifiedName);

                MemoryStream m = new MemoryStream();
                ((Organism) _org).InternalOrganismSerialize(m);
                info.AddValue("OrganismInfo", m.ToArray());

                // Let the animal contribute to serialization.  We don't use BinarySerialization
                // here because we don't want animals to be able to put bogus items in their
                // serialization stream that we will deserialize on their behalf later and 
                // cause security problems.
                if (_org is Animal)
                {
                    m = new MemoryStream();
                    ((Animal)_org).InternalAnimalSerialize(m);
                    info.AddValue("AnimalInfo", m.ToArray());

                    m = new MemoryStream();
                
                    try
                    {
                        ((Animal)_org).SerializeAnimal(m);
                    }
                    catch (Exception e)
                    {
                        ErrorLog.LogHandledException(e);
                        if (GameEngine.Current != null)
                        {
                            GameEngine.Current.OnEngineStateChanged(new EngineStateChangedEventArgs(EngineStateChangeType.Other,
                                "Animal Serialization Failure.",
                                _org.GetType().Assembly.GetName().Name + "was frost bitten while being Cryogenically Frozen (aka Serialized)."));
                        }
                        m = new MemoryStream();                 
                    }
                    info.AddValue("UserInfo", m.ToArray());
                }
                else
                {
                    m = new MemoryStream();
                    ((Plant)_org).InternalPlantSerialize(m);
                    info.AddValue("PlantInfo", m.ToArray());

                    m = new MemoryStream();
                    try
                    {
                        ((Plant)_org).SerializePlant(m);
                    }
                    catch (Exception e)
                    {
                        ErrorLog.LogHandledException(e);
                        if (GameEngine.Current != null)
                        {
                            GameEngine.Current.OnEngineStateChanged(new EngineStateChangedEventArgs(EngineStateChangeType.Other,
                                "Plant Serialization Failure.",
                                _org.GetType().Assembly.GetName().Name + "was frost bitten while being Cryogenically Frozen (aka Serialized)."));
                        }
                        m = new MemoryStream();
                    }

                    info.AddValue("UserInfo", m.ToArray());
                }
            }
        }

        public Organism Organism
        {
            get
            {
                return _org;
            }
        }

        // ticks: 100 nanosec intervals
        public Int64 Overage
        {
            get
            {
                return _overage;
            }
        
            set 
            {
                _overage = value;
            }
        }

        // ticks: 100 nanosec intervals
        public Int64 TotalTime
        {
            get 
            {
                return _totalTime;
            }
        
            set
            {
                _totalTime = value;
            }
        }
        public Int64 LastTime 
        {
            get
            {
                return _lastTime;
            }
        
            set
            {
                _lastTime = value;
            }
        }

        public Int64 TotalActivations 
        {
            get 
            {
                return _totalActivations;
            }
        
            set
            {
                _totalActivations = value;
            }
        }

        public bool Active
        {
            get 
            {
                return _active;
            }
        
            set 
            {
                _active = value;
            }
        }
    }
}