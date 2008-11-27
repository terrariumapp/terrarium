//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  Represents the view available to an organism.
    /// </summary>
    public interface IOrganismWorldBoundary
    {
        /// <summary>
        ///  Gets the most up to date state object for the current
        ///  creature.
        /// </summary>
        OrganismState CurrentOrganismState { get; }

        /// <summary>
        ///  Retrieves the organism ID representing your creature within the world.
        /// </summary>
        string ID { get; }

        /// <summary>
        ///  Returns the width of the world in game units (pixels);
        /// </summary>
        int WorldWidth { get; }

        /// <summary>
        ///  Returns the height of the world in game units (pixels).
        /// </summary>
        int WorldHeight { get; }
    }
}