//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using OrganismBase;

namespace Terrarium.Renderer
{
    /// <summary>
    ///  Provides access to information about a click
    ///  in the TerrariumDirectDrawGameView.  A click
    ///  can correspond to a creature, and this class
    ///  provides access to the creature clicked.
    /// </summary>
    public class OrganismClickedEventArgs : EventArgs
    {
        /// <summary>
        ///  Creates event args that identify a clicked creature.
        /// </summary>
        /// <param name="state">State of the creature that was clicked.</param>
        public OrganismClickedEventArgs(OrganismState state)
        {
            OrganismState = state;
        }

        /// <summary>
        ///  Provides access to the state object of the clicked creature.
        /// </summary>
        public OrganismState OrganismState { get; private set; }
    }
}