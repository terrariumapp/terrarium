//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System.Collections;

namespace OrganismBase
{
    /// <summary>
    ///  This class represents an Animal's view of the world.  An interface is
    ///  used because the creature needs something to link against, while the
    ///  actual object exists within the Terrarium Client executable.
    /// </summary>
    public interface IAnimalWorldBoundary : IOrganismWorldBoundary
    {
        /// <summary>
        ///  Retrieves the current state of a creature within the world.  This
        ///  state will be immutable and can't be changed, and actually represents
        ///  an animal state for the previous tick and not the currently executed
        ///  tick.
        /// </summary>
        AnimalState CurrentAnimalState { get; }

        /// <summary>
        ///  Provides a method for the creature to scan the surrounding area
        ///  and discover both hidden and visible creatures within your site
        ///  range.
        /// </summary>
        ArrayList Scan();

        /// <summary>
        ///  Used to look for a creature given an organism state.  The organism
        ///  state can be stale, and the LookFor function will attempt to return
        ///  an updated state for the latest tick.
        /// </summary>
        OrganismState LookFor(OrganismState organismState);

        /// <internal/>
        OrganismState LookForNoCamouflage(OrganismState organismState);

        /// <summary>
        ///  Used to look for a creature given an organism ID.  If the creature
        ///  is visible the state will be returned.  If not null should be
        ///  returned.
        /// </summary>
        OrganismState RefreshState(string organismID);
    }
}