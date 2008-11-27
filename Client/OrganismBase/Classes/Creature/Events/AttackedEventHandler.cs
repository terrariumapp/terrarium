//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Describes the event handler required in order to hook into
    ///   a creature's Attacked event.  The sender will always be your
    ///   creature, and AttackedEventArgs will be filled with information
    ///   to help your creature process it's turn.
    ///  </para>
    ///  <para>
    ///   This event may be fired more than once per turn and will be
    ///   fired once for each creature that initiates an attack against
    ///   your creature assuming your creature lives through the battle.
    ///  </para>
    /// </summary>
    public delegate void AttackedEventHandler(object sender, AttackedEventArgs e);
}