//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Describes the event handler required in order to hook into
    ///   a creature's AttackCompleted event.  The sender will always be your
    ///   creature, and AttackCompletedEventArgs will be filled with information
    ///   to help your creature process it's turn.
    ///  </para>
    ///  <para>
    ///   This event will be fired after every attack your creature initiates.
    ///   The returned AttackCompletedEventArgs will contain information about
    ///   damage dealt, whether the creature is dead, or whether it got away.
    ///  </para>
    /// </summary>
    public delegate void AttackCompletedEventHandler(object sender, AttackCompletedEventArgs e);
}