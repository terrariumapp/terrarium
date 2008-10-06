//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;

namespace Terrarium.Game
{
    /// <summary>
    ///  Delegate used to define an event for notifying clients
    ///  when assemblies within the PrivateAssemblyCache have changed.
    /// </summary>
    public delegate void PacAssembliesChangedEventHandler(object sender, EventArgs e);
}