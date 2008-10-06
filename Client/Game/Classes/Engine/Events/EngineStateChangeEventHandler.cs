//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

namespace Terrarium.Game
{
    /// <summary>
    ///     An EngineStateChangeEvent is raised when some interesting change 
    ///     in state occurs such as being teleported into or out of your world. 
    /// </summary>
    public delegate void EngineStateChangedEventHandler(object sender, EngineStateChangedEventArgs e);
}