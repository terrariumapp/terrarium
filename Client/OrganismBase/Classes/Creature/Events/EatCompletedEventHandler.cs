//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Describes the event handler required in order to hook into
    ///   a creature's EatCompleted event.  The sender will always be your
    ///   creature, and EatCompletedEventArgs will be filled with information
    ///   to help your creature process it's turn.
    ///  </para>
    ///  <para>
    ///   This event will be fired after your creature has taken
    ///   a bite of food.  Eating can be successful only if the target
    ///   creature had enough available food matter left for your creature
    ///   to eat.
    ///  </para>
    /// </summary>
    public delegate void EatCompletedEventHandler(object sender, EatCompletedEventArgs e);
}