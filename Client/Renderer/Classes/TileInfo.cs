//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace Terrarium.Renderer 
{
    /// <summary>
    ///  Represents a game tile within the Terrarium world board.
    /// </summary>
    internal struct TileInfo
    {
        /// <summary>
        ///  The transition type for this tile.
        /// </summary>
        internal short Transition;
        /// <summary>
        ///  The tile index for this tile.
        /// </summary>
        internal short Tile;
        /// <summary>
        ///  The X Offset location computed using the tileX
        ///  constant.
        /// </summary>
        internal int XOffset;
        /// <summary>
        ///  The Y Offset location computed using the tileY
        ///  constant.
        /// </summary>
        internal int YOffset;
    }
}