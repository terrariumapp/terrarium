//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Describes the event handler required in order to hook into
    ///   a creature's Born event.  The sender will always be your
    ///   creature, and BornEventArgs will be filled with information
    ///   to help your creature process it's turn.
    ///  </para>
    ///  <para>
    ///   This event may be fired once for your creature when it is
    ///   first born, in which case the BornEventArgs might have some
    ///   Dna from the parent, or none if the creatures were
    ///   made by the Terrarium game engine during introduction.
    ///  </para>
    /// </summary>
    public delegate void BornEventHandler(object sender, BornEventArgs e);
}