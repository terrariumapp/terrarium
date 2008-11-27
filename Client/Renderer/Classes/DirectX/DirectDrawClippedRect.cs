//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using DxVBLib;

namespace Terrarium.Renderer.DirectX 
{
    /// <summary>
    ///  Represents a sprite clipping structure that can
    ///  be used to draw sprites between surfaces will
    ///  full edge clipping.
    ///  These are public members instead of property accessors because they sometimes
    ///  need to be passed as ref or out arguments and these aren't supported on accessors.
    /// </summary>
    public struct DirectDrawClippedRect
    {
        /// <summary>
        ///  The destination rectangle
        /// </summary>
        public RECT Destination;
        /// <summary>
        ///  The source rectangle
        /// </summary>
        public RECT Source;
        /// <summary>
        ///  Has the sprite been clipped outside of the view
        /// </summary>
        public bool Invisible;
        /// <summary>
        ///  Has the sprite been clipped along the top
        /// </summary>
        public bool ClipTop;
        /// <summary>
        ///  Has the sprite been clipped along the bottom
        /// </summary>
        public bool ClipBottom;
        /// <summary>
        ///  Has the sprite been clipped along the left
        /// </summary>
        public bool ClipLeft;
        /// <summary>
        ///  Has the sprite been clipped along the right.
        /// </summary>
        public bool ClipRight;
    }
}