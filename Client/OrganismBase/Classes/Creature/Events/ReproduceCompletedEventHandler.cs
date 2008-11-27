//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Describes the event handler required in order to hook into
    ///   a creature's ReproduceCompleted event.  The sender will always be your
    ///   creature, and ReproduceCompletedEventArgs will be filled with information
    ///   to help your creature process it's turn.
    ///  </para>
    ///  <para>
    ///   This event will be fired for your creature immediately after it has given
    ///   birth to a child creature of your species.
    ///  </para>
    /// </summary>
    public delegate void ReproduceCompletedEventHandler(object sender, ReproduceCompletedEventArgs e);
}