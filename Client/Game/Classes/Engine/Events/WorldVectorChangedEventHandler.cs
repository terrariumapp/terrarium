//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

namespace Terrarium.Game
{
    /// <summary>
    ///  Delegate used to define an event on the gaming engine
    ///  to notify clients of when the world vector has been
    ///  changed.
    /// </summary>
    public delegate void WorldVectorChangedEventHandler(object sender, WorldVectorChangedEventArgs e);
}