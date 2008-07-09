//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace Terrarium.Renderer 
{
    /// <summary>
    ///  Delegate used to define an event used to notify clients
    ///  that a creature has been clicked in the game view.
    /// </summary>
    public delegate void OrganismClickedEventHandler(object sender, OrganismClickedEventArgs e);
}