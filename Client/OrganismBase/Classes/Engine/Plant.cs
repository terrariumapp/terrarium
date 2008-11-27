//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

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
        internal IPlantWorldBoundary World
        {
            get { return (IPlantWorldBoundary) OrganismWorldBoundary; }
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
        public new PlantState State
        {
            get { return World.CurrentPlantState; }
        }

        ///<summary>
        ///</summary>
        ///<param name="m"></param>
        public abstract void SerializePlant(MemoryStream m);

        ///<summary>
        ///</summary>
        ///<param name="m"></param>
        public abstract void DeserializePlant(MemoryStream m);

        ///<summary>
        ///</summary>
        ///<param name="m"></param>
        public void InternalPlantSerialize(MemoryStream m)
        {
        }

        ///<summary>
        ///</summary>
        ///<param name="m"></param>
        public void InternalPlantDeserialize(MemoryStream m)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="clearOnly"></param>
        public override sealed void InternalMain(bool clearOnly)
        {
            var events = State.OrganismEvents;

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