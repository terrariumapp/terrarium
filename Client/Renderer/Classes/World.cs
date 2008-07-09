//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------


#define SQUARE_TILES

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

        /// <summary>
        ///  Tile mapping from the height map to actual tiles used in the
        ///  game.  This maps the height field information to the graphics
        ///  used within the game.
        /// </summary>
        private TileInfo[,] tileMap;

        /// <summary>
        ///  Holds a bitmap representation of the world.
        /// </summary>
        private Bitmap miniMap;

        /// <summary>
        ///  The number of horizontal map tiles.
        /// </summary>
        private int xMapTiles;

        /// <summary>
        ///  The number of vertical map tiles.
        /// </summary>
        private int yMapTiles;

        /// <summary>
        ///  The size of a may tile on the vertical axis.
        /// </summary>
        private int tileY = 1024;

        /// <summary>
        ///  The size of a map tile on the horizontal axis.
        /// </summary>
		private int tileX = 1024;

        /// <summary>
        ///  Creates a new instance of the World class used to create backgrounds
        ///  in the Terrarium.
        /// </summary>
        internal World()
        {
        }

        /// <summary>
        ///  Initializes the mapping using a target number pixels.
        /// </summary>
        /// <param name="xPixels">The width of the world in pixels.</param>
        /// <param name="yPixels">The height of the world in pixels.</param>
        /// <returns>An updated size that has been rounded up to the nearest world boundary.</returns>
        internal RECT CreateWorld(int xPixels, int yPixels)
        {
            RECT worldSize = new RECT();

            if (xPixels > 1 && xPixels % tileX == 0)
            {
                worldSize.Right = xPixels;
            }
            else
            {
                xPixels += tileX - (xPixels % tileX);
                worldSize.Right = xPixels;
            }

            if (yPixels > 1 && yPixels % tileY == 0)
            {
                worldSize.Bottom = yPixels;
            }
            else
            {
                yPixels += tileY - (yPixels % tileY);
                worldSize.Bottom = yPixels;
            }

            xMapTiles = (xPixels / tileY) + 1;
            yMapTiles = (yPixels / tileY) + 1;

            // Generate the world map
            worldMap = new int[xMapTiles, yMapTiles];
            new HeightMap(ref worldMap, xMapTiles, yMapTiles);

            // Based on the last worldMap, lets create the tileMap
            tileMap = new TileInfo[xMapTiles, yMapTiles];
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
            miniMap = new Bitmap(xMapTiles, yMapTiles);

            for (int j = 0; j < yMapTiles; j++)
            {
                for (int i = 0; i < xMapTiles; i++)
                {
                    if (worldMap[i,j] == 0)
                    {
                        miniMap.SetPixel(i,j,Color.Tan);
                    }
                    else
                    {
                        miniMap.SetPixel(i,j,Color.PaleGreen);
                    }
                }
            }
        }

        /// <summary>
        ///  The size of a tile in the X direction
        /// </summary>
        internal Int32 TileXSize
        {
            get
            {
                return tileX;
            }
        }

        /// <summary>
        ///  The size of a tile in the Y direction
        /// </summary>
        internal Int32 TileYSize
        {
            get
            {
                return tileY;
            }
        }

        /// <summary>
        ///  Provides access to the generated mini map.
        /// </summary>
        internal Bitmap MiniMap
        {
            get
            {
                return miniMap;
            }
        }

        /// <summary>
        ///  Provides access to the array of tile structures.
        /// </summary>
        internal TileInfo[,] Map
        {
            get
            {
                return tileMap;
            }
        }

        /// <summary>
        ///  The number of map tiles in the X direction
        /// </summary>
        internal int XMapTiles
        {
            get
            {
                return xMapTiles;
            }
        }

        /// <summary>
        ///  The number of map tiles in the Y direction
        /// </summary>
        internal int YMapTiles
        {
            get
            {
                return yMapTiles;
            }
        }

        /// <summary>
        ///  Turns the map into a series of tiles that can index into
        ///  the background sprite sheet.
        /// </summary>
        internal void TileFilterPass()
        {
            Random rand = new Random();

            for (int j = 0; j < yMapTiles; j++)
            {
                for (int i = 0; i < xMapTiles; i++)
                {
                    int transition = worldMap[i,j];
                    int p1 = 0; int p2 = 0; int p3 = 0; int p4 = 0;

// *** BEGIN OLD TILE SYSTEM ***
//                    if (transition == 0)
//                    {
//                        if (rand.Next(0,2) == 1)
//                        {
//                            p1 = rand.Next(0,2);
//                            p2 = rand.Next(0,2);
//                            p3 = rand.Next(0,2);
//                            p4 = rand.Next(0,2);
//                        }
//                        else
//                        {
//                            p1 = 0;
//                            p2 = 0;
//                            p3 = 0;
//                            p4 = 0;
//                        }
//                    }
//                    else
//                    {
//                        if ((i % 2) == 0)
//                        {
//                            if (i > 0)
//                            {
//                                if (j > 0)
//                                {
//                                    p1 = (worldMap[i-1,j-1] == transition) ? 0 : 1;
//                                }
//                                p4 = (worldMap[i-1,j] == transition) ? 0 : 1;
//                            }
//                        
//                            if (i < (xMapTiles-1))
//                            {
//                                if (j > 0)
//                                {
//                                    p2 = (worldMap[i+1,j-1] == transition) ? 0 : 1;
//                                }
//                                p3 = (worldMap[i+1,j] == transition) ? 0 : 1;
//                            }
//                        }
//                        else
//                        {
//                            if (i > 0)
//                            {
//                                p1 = (worldMap[i-1,j] == transition) ? 0 : 1;
//                                if (j < (yMapTiles-1))
//                                {
//                                    p4 = (worldMap[i-1,j+1] == transition) ? 0 : 1;
//                                }
//                            }
//                        
//                            if (i < (xMapTiles-1))
//                            {
//                                p2 = (worldMap[i+1,j] == transition) ? 0 : 1;
//                                if (j < (yMapTiles-1))
//                                {
//                                    p3 = (worldMap[i+1,j+1] == transition) ? 0 : 1;
//                                }
//                            }
//                        }
//                    }
//
//                    tileMap[i,j].Transition = (short) transition;
//                    tileMap[i,j].Tile = (short) ((p1) + (p2<<1) + (p3<<2) + (p4<<3));
// *** END OLD TILE SYSTEM ***

					if (rand.Next(100) < 85)
					{
						tileMap[i, j].Transition = 0;
					}
					else
					{
						tileMap[i, j].Transition = 1;
					}

					p1 = rand.Next(0, 2);
					p2 = rand.Next(0, 2);
					p3 = rand.Next(0, 2);
					p4 = rand.Next(0, 2);

					tileMap[i, j].Tile = (short)((p1) + (p2 << 1) + (p3 << 2) + (p4 << 3));

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
            for (int j = 0; j < yMapTiles; j++)
            {
                for (int i = 0; i < xMapTiles; i++)
                {
#if SQUARE_TILES
                    tileMap[i,j].YOffset = (j*tileY);
					tileMap[i, j].XOffset = (i * tileX);
#else
					if ((i%2) == 0)
					{
						tileMap[i, j].YOffset = (j * tileY)-(tileY / 2);
					}
					else
					{
						tileMap[i,j].YOffset = (j*tileY);
					}
					tileMap[i, j].XOffset = (i * tileY) - tileY;
#endif

				}
            }
        }
    }
}