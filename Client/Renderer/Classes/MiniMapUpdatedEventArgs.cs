//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Drawing;

namespace Terrarium.Renderer
{
    /// <summary>
    ///  Provides access to information about an updated
    ///  MiniMap (which is the small scaled down map that shows
    ///  a more global view of the terrarain.
    /// </summary>
    public class MiniMapUpdatedEventArgs : EventArgs
    {
        /// <summary>
        ///  Creates a new set of event arguments given the
        ///  new minimap bitmap.
        /// </summary>
        /// <param name="miniMap">The new minimap</param>
        public MiniMapUpdatedEventArgs(Bitmap miniMap)
        {
            MiniMap = miniMap;
        }

        /// <summary>
        ///  Provides access to the minimap.
        /// </summary>
        public Bitmap MiniMap { get; private set; }
    }
}