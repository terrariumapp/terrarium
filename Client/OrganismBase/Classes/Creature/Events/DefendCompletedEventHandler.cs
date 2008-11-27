//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Describes the event handler required in order to hook into
    ///   a creature's DefendCompleted event.  The sender will always be your
    ///   creature, and DefendCompletedEventArgs will be filled with information
    ///   to help your creature process it's turn.
    ///  </para>
    ///  <para>
    ///   This event will be fired after your creature has defended against another
    ///   creature.  This can often be used to defend against the creature additional
    ///   times in the case they might continue attacking.
    ///  </para>
    /// </summary>
    public delegate void DefendCompletedEventHandler(object sender, DefendCompletedEventArgs e);
}