//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------


using System;
using System.IO;

namespace OrganismBase 
{

    /// <summary>
    ///  <para>
    ///   This is the base class used by any creatures that
    ///   want to become a Plant.  This class contains
    ///   all methods that define a plants actions and behaviors.
    ///  </para>
    /// </summary>
    public abstract class Plant : Organism
    {

        /// <internal/>
        abstract public void SerializePlant(MemoryStream m);

        /// <internal/>
        abstract public void DeserializePlant(MemoryStream m);

        /// <internal/>
        public void InternalPlantSerialize(MemoryStream m)
        {
        }
    
        /// <internal/>
        public void InternalPlantDeserialize(MemoryStream m)
        {
        }

        internal IPlantWorldBoundary World
        {
            get
            {
                return (IPlantWorldBoundary) OrganismWorldBoundary;
            }
        }

        /// <summary>
        ///  <para>
        ///   Gets the PlantState object that represents your creature's current
        ///   state in the world.  This PlantState contains all of the standard
        ///   OrganismState properties in addition to new properties defined
        ///   just for use in plants.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  PlantState object representing your plant's current world state.
        /// </returns>
        new public PlantState State
        {
            get
            {
                return World.CurrentPlantState;
            }
        }

        /// <internal/>
        public override sealed void InternalMain(bool clearOnly)
        {
            OrganismEventResults events = State.OrganismEvents;

            // Call Initialize once
            if (!IsInitialized)
            {
                Initialize();
                IsInitialized = true;
            }

            if (events != null)
            {
                if (events.ReproduceCompleted != null)
                {
                    if (InProgressActions.ReproduceAction != null &&
                        events.ReproduceCompleted.ActionID == InProgressActions.ReproduceAction.ActionID)
                    {

                        InProgressActions.SetReproduceAction(null);
                    }
                }
            }

            if (CanReproduce)
            {
                BeginReproduction(null);
            }

            if (clearOnly)
            {
                InternalTurnsSkipped++;
            }
            else
            {
                InternalTurnsSkipped = 0;
            }
        }
    }
}