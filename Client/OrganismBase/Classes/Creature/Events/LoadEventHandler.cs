//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Describes the event handler required in order to hook into
    ///   a creature's Load event.  The sender will always be your
    ///   creature, and LoadEventArgs will be filled with information
    ///   to help your creature process it's turn.
    ///  </para>
    /// </summary>
    public delegate void LoadEventHandler(object sender, LoadEventArgs e);
}