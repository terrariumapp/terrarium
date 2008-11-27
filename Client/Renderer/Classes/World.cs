//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using DxVBLib;

namespace Terrarium.Renderer
{
    /// <summary>
    ///  Creates a Terrarium mapping world given a target height and width.
    ///  Used by the TerrariumDirectDrawGameView to render the background
    /// </summary>
    internal class World
    {
        /// <summary>
        ///  Holds height information for the world.  This is how the height
        ///  map is created.
        /// </summary>
        private int[,] worldMap;

        static World()
        {
            TileXSize = 1024;
        }

        public World()
        {
            TileYSize = 1024;
        }

        /// <summary>
        ///  The size of a tile in the X direction
        /// </summary>
        internal static int TileXSize { get; private set; }

        /// <summary>
        ///  The size of a tile in the Y direction
        /// </summary>
        internal int TileYSize { get; private set; }

        /// <summary>
        ///  Provides access to the generated mini map.
        /// </summary>
        internal Bitmap MiniMap { get; private set; }

        /// <summary>
        ///  Provides access to the array of tile structures.
        /// </summary>
        internal TileInfo[,] Map { get; private set; }

        /// <summary>
        ///  The number of map tiles in the X direction
        /// </summary>
        internal int XMapTiles { get; private set; }

        /// <summary>
        ///  The number of map tiles in the Y direction
        /// </summary>
        internal int YMapTiles { get; private set; }

        /// <summary>
        ///  Initializes the mapping using a target number pixels.
        /// </summary>
        /// <param name="xPixels">The width of the world in pixels.</param>
        /// <param name="yPixels">The height of the world in pixels.</param>
        /// <returns>An updated size that has been rounded up to the nearest world boundary.</returns>
        internal RECT CreateWorld(int xPixels, int yPixels)
        {
            var worldSize = new RECT();

            if (xPixels > 1 && xPixels%TileXSize == 0)
            {
                worldSize.Right = xPixels;
            }
            else
            {
                xPixels += TileXSize - (xPixels%TileXSize);
                worldSize.Right = xPixels;
            }

            if (yPixels > 1 && yPixels%TileYSize == 0)
            {
                worldSize.Bottom = yPixels;
            }
            else
            {
                yPixels += TileYSize - (yPixels%TileYSize);
                worldSize.Bottom = yPixels;
            }

            XMapTiles = (xPixels/TileYSize) + 1;
            YMapTiles = (yPixels/TileYSize) + 1;

            // Generate the world map
            worldMap = new int[XMapTiles,YMapTiles];
            new HeightMap(worldMap, XMapTiles, YMapTiles);

            // Based on the last worldMap, lets create the tileMap
            Map = new TileInfo[XMapTiles,YMapTiles];
            TileFilterPass();
            TileOffsetPass();
            MiniMapPass();

            return worldSize;
        }

        /// <summary>
        ///  Runs a MinMap pass over the World object to create a
        ///  small version of the world.
        /// </summary>
        internal void MiniMapPass()
        {
            MiniMap = new Bitmap(XMapTiles, YMapTiles);

            for (var j = 0; j < YMapTiles; j++)
            {
                for (var i = 0; i < XMapTiles; i++)
                {
                    if (worldMap[i, j] == 0)
                    {
                        MiniMap.SetPixel(i, j, Color.Tan);
                    }
                    else
                    {
                        MiniMap.SetPixel(i, j, Color.PaleGreen);
                    }
                }
            }
        }

        /// <summary>
        ///  Turns the map into a series of tiles that can index into
        ///  the background sprite sheet.
        /// </summary>
        internal void TileFilterPass()
        {
            var rand = new Random();

            for (var j = 0; j < YMapTiles; j++)
            {
                for (var i = 0; i < XMapTiles; i++)
                {
                    Map[i, j].Transition = (short) (rand.Next(100) < 85 ? 0 : 1);

                    var p1 = rand.Next(0, 2);
                    var p2 = rand.Next(0, 2);
                    var p3 = rand.Next(0, 2);
                    var p4 = rand.Next(0, 2);

                    Map[i, j].Tile = (short) ((p1) + (p2 << 1) + (p3 << 2) + (p4 << 3));
                }
            }
        }

        /// <summary>
        ///  Computes the viewport offsets for tiles so that the information
        ///  is available during rendering and doesn't have to be computed
        ///  on the fly.
        /// </summary>
        internal void TileOffsetPass()
        {
            for (var j = 0; j < YMapTiles; j++)
            {
                for (var i = 0; i < XMapTiles; i++)
                {
                    Map[i, j].YOffset = (j*TileYSize);
                    Map[i, j].XOffset = (i*TileXSize);
                }
            }
        }
    }
}