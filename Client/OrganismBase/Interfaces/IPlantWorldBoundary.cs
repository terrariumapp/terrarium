//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  Represents a plant's view of the world.
    /// </summary>
    public interface IPlantWorldBoundary : IOrganismWorldBoundary
    {
        /// <summary>
        ///  Retrieves the most up to date plant state for the current
        ///  plant.
        /// </summary>
        PlantState CurrentPlantState { get; }
    }
}