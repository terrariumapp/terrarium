//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Defines the various reasons a creature can be stopped.
    ///   This is currently either DestinationReached or Blocked.
    ///  </para>
    /// </summary>
    public enum ReasonForStop
    {
        /// <summary>
        ///  <para>
        ///   Your creature has arrived at the destination used
        ///   in the call to BeginMoving.  During the traversal
        ///   of the game area no other creatures or blocking
        ///   items were encountered.
        ///  </para>
        /// </summary>
        DestinationReached,

        /// <summary>
        ///  <para>
        ///   Your creature was blocked from reaching its destination
        ///   by another creature.  This could either be a plant/inanimate
        ///   object, or another moving creature.
        ///  </para>
        /// </summary>
        Blocked
    }
}