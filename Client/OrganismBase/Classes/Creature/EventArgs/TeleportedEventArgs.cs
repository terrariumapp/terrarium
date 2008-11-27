//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Special object used to hold arguments passed to the
    ///   TeleportedEventHandler delegate.  Currently no information
    ///   is passed to a creature after teleportation.
    ///  </para>
    /// </summary>
    [Serializable]
    public class TeleportedEventArgs : OrganismEventArgs
    {
        /// <internal/>
        public TeleportedEventArgs(bool localOnly)
        {
            LocalTeleport = localOnly;
        }

        /// <summary>
        ///  <para>
        ///   Provides information on whether you were teleported
        ///   to a new machine or back to the local machine the
        ///   teleportation started from.  When you are teleported
        ///   locally you shouldn't have to set up your creature, it
        ///   should be in the same state as when the teleport began.
        ///  </para>
        ///  <para>
        ///   Note that your creature will be in a new location on the map.
        ///   Note also that your creature will have missed some time within the
        ///   game (not counted towards total age), because of the time
        ///   it takes to teleport.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  AnimalState representing the state of the creature attacking you.
        /// </returns>
        public bool LocalTeleport { get; private set; }

        /// <summary>
        ///  <para>
        ///   Used to get string information about this event args for
        ///   debugging purposes.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  A System.String containing the value '#Teleported'
        /// </returns>
        public override string ToString()
        {
            return string.Format("#Teleported - (Local = {0})", LocalTeleport);
        }
    }
}