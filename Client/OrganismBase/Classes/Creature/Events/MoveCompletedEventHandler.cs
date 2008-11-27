//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Describes the event handler required in order to hook into
    ///   a creature's MoveCompleted event.  The sender will always be your
    ///   creature, and MoveCompletedEventArgs will be filled with information
    ///   to help your creature process it's turn.
    ///  </para>
    ///  <para>
    ///   This event will fire when a move has been completed.  This can mean
    ///   your creature was blocked or made it to its destination.  It may
    ///   be several ticks from the BeginMoving function call to initiate movement
    ///   until your creature's move is actually complete.
    ///  </para>
    /// </summary>
    public delegate void MoveCompletedEventHandler(object sender, MoveCompletedEventArgs e);
}