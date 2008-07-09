//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                                
//------------------------------------------------------------------------------

using System;

using OrganismBase;
using Terrarium.Hosting;

namespace Terrarium.Game
{
    /// <summary>
    ///  Represents a world boundary for a plant. See description of AnimalWorldBoundary to see how this hides information
    ///  from creatures.
    /// </summary>
    public class PlantWorldBoundary : OrganismWorldBoundary, IPlantWorldBoundary
    {
        /// <summary>
        ///  Creates a new plant world boundary given the owning
        ///  plant and the plant's unique ID.
        /// </summary>
        /// <param name="plant">The plant used to initialize the world boundary.</param>
        /// <param name="ID">The plant's Unique ID</param>
        internal PlantWorldBoundary(Plant plant, string ID) : base(plant, ID)
        {
        }

        /// <summary>
        ///  Returns a state object representing the plant's current state in the world.
        /// </summary>
        public PlantState CurrentPlantState
        {
            get
            {
                return (PlantState) AppMgr.CurrentScheduler.CurrentState.GetOrganismState(ID);
            }
        }
    }
}