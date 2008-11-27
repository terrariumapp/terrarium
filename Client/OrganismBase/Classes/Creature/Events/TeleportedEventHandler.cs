//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Describes the event handler required in order to hook into
    ///   a creature's Teleported event.  The sender will always be your
    ///   creature, and TeleportedEventArgs will be filled with information
    ///   to help your creature process it's turn.
    ///  </para>
    ///  <para>
    ///   This event may be fired whenever your creature is teleported
    ///   internally in the same Terrarium or externally to another peer.
    ///   This can be used to determine if special world related values should
    ///   be reset and reinitialized.
    ///  </para>
    /// </summary>
    public delegate void TeleportedEventHandler(object sender, TeleportedEventArgs e);
}