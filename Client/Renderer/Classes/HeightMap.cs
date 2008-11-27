//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace Terrarium.Renderer
{
    /// <summary>
    ///  Seeds a given array with a division/offset based height map.
    ///  Used by Terrarium for generating variable terrarain.
    ///  Additional features include computation of a sea level
    ///  based on percentage area and clamping of height values
    ///  via a granularity.  The current height map supports 256
    ///  height values, but can be modified to produce much more
    ///  detailed terrain.
    /// </summary>
    internal class HeightMap
    {
        /// <summary>
        ///  A random number generator used throughout the process
        ///  of fractal height map generation.
        /// </summary>
        private readonly Random rand;

        /// <summary>
        ///  The actual height map being generated.  By the time
        ///  the height map is complete the values are already
        ///  clamped.
        /// </summary>
        private readonly int[,] worldMap;

        /// <summary>
        ///  The number of horizontal map locations.
        /// </summary>
        private readonly int xMapTiles;

        /// <summary>
        /// 
        ///  The number of vertical map locations.
        /// </summary>
        private readonly int yMapTiles;

        /// <summary>
        ///  This controls how far the algorithm recurses.
        ///  Whenever the granularity is greater than the
        ///  distance between points, the algorithm stops.
        /// </summary>
        private const int Granularity = 2;

        /// <summary>
        ///  Make sure we can break out of the sea level creation
        ///  routines in the case we get into a loop where we can't
        ///  get our percentages correct.  This happens on smaller
        ///  maps quite easily.
        /// </summary>
        private int lastSeaLevel;

        /// <summary>
        ///  The maximum number assigned to any point on the map.
        /// </summary>
        private const int MaxLevel = 256;

        /// <summary>
        ///  Gives an initial sea level value to begin processing
        ///  the grid for a sea level percentage.
        /// </summary>
        private int seaLevel = 100;

        /// <summary>
        ///  Gives a sea level percentage for renegotiating which
        ///  points are sea level.  This dynamically modifies the
        ///  seaLevel value to achieve consistent results.
        /// </summary>
        private const int SeaPercent = 15;

        /// <summary>
        ///  The allowable variance in sea level area.  This makes
        ///  sea level between (seaPercent - seaVariance),(seaPercent
        ///  + seaVariance).
        /// </summary>
        private const int SeaVariance = 2;

        /// <summary>
        ///  The bumpiness factor for terrain computations.
        /// </summary>
        private const int BumpinessFactor = 10;

        /// <summary>
        ///  Computes a new height map and fills in the by ref integer
        ///  array with world map values.
        /// </summary>
        /// <param name="worldMap">A by ref int array that holds the world values.</param>
        /// <param name="xMapTiles">The number of horizontal map locations.</param>
        /// <param name="yMapTiles">The number of vertical map locations.</param>
        internal HeightMap(int[,] worldMap, int xMapTiles, int yMapTiles)
        {
            this.worldMap = worldMap;
            this.xMapTiles = xMapTiles;
            this.yMapTiles = yMapTiles;

            rand = new Random();

            // Setup the initial seed values for the height map
            InitMap();

            // Begin diamond square average fractal
            MapArea(0, 0, xMapTiles - 1, yMapTiles - 1);

            // Populate all unpopulated cells
            NormalizeMap();

            // Compute Sea Level
            var seaLevelChange = 0;
            while (!ComputeSeaLevel())
            {
                if (seaLevelChange == 0)
                {
                    seaLevelChange = lastSeaLevel;
                    continue;
                }

                if (seaLevelChange != lastSeaLevel)
                {
                    // We just reversed
                    break;
                }
            }

            NormalizeMapToBaseTiles();
        }

        /// <summary>
        ///  Attempts to lock sea level into a specific amount of
        ///  area within the map.  This method either raises or lowers
        ///  the sea level value to achieve a consistent amount of water
        ///  in the final map.  Note that this method doesn't change the
        ///  height field, only how the normalization process clamps
        ///  height values to tile values.
        /// </summary>
        /// <returns>True if the iteration of the method achieved the desired results, false if it should be run again.</returns>
        internal bool ComputeSeaLevel()
        {
            var total = 0;
            var water = 0;

            for (var j = 0; j < yMapTiles; j++)
            {
                for (var i = 0; i < xMapTiles; i++)
                {
                    total++;
                    if (worldMap[i, j] <= seaLevel)
                    {
                        water++;
                    }
                }
            }

            var percentWater = (water*100)/total;
            if (percentWater > (SeaPercent + SeaVariance))
            {
                lastSeaLevel = -1;
            }

            if (percentWater < (SeaPercent - SeaVariance))
            {
                lastSeaLevel = 1;
            }

            if (lastSeaLevel != 0)
            {
                seaLevel += lastSeaLevel;
                return false;
            }

            return true;
        }

        /// <summary>
        ///  <para>
        ///   Clamps the map values from their original height field
        ///   values, to values used by the world mapping class.  This
        ///   should probably be moved into the World class, but for
        ///   more advanced height fields that support banding, this
        ///   method would actually be enhanced to normalize the height
        ///   field data to bands (deep sea, sea, shore, grass, hills,
        ///   rock, steep, mountain peak, etc..)
        ///  </para>
        ///  <para>
        ///   In the current iteration all land is set to 0, unless
        ///   it is below seaLevel, in which case it is set to 1.
        ///  </para>
        /// </summary>
        internal void NormalizeMapToBaseTiles()
        {
            for (var j = 0; j < yMapTiles; j++)
            {
                for (var i = 0; i < xMapTiles; i++)
                {
                    worldMap[i, j] = worldMap[i, j] > seaLevel ? 0 : 1;
                }
            }
        }

        /// <summary>
        ///  Diamond fractal generation only works optimally
        ///  when the size of the map is of a very specific
        ///  height and width, and it only works properly if
        ///  the map is square.  The NormalizeMap method attempts
        ///  to overcome this flaw in the algorithm by iteratively
        ///  checking points for set values and finally *finding*
        ///  points that can be used to compute the mid-point.
        /// </summary>
        internal void NormalizeMap()
        {
            for (var j = 0; j < yMapTiles; j++)
            {
                for (var i = 0; i < xMapTiles; i++)
                {
                    if (worldMap[i, j] == 0)
                    {
                        // We have to set these points up by hand
                        // since we have to account for areas where
                        // we are out of bounds
                        worldMap[i, j] = SquareAverage(
                            NearestPoint(i - 1, j - 1, -1, -1),
                            NearestPoint(i - 1, j + 1, -1, +1),
                            NearestPoint(i + 1, j - 1, +1, -1),
                            NearestPoint(i + 1, j + 1, +1, +1),
                            BumpinessFactor);
                    }
                }
            }
        }

        /// <summary>
        ///  Find the nearest point to the given point by using
        ///  x,y delta computations.  This method is used to facilitate
        ///  normalization of unset points in the map.
        /// </summary>
        /// <param name="x">The x coordinate of the point being checked.</param>
        /// <param name="y">The y coordinate of the point being checked.</param>
        /// <param name="xdelta">The direction to move on the x axis, should be -1,1</param>
        /// <param name="ydelta">The direction to move on the y axis, should be -1,1</param>
        /// <returns></returns>
        internal int NearestPoint(int x, int y, int xdelta, int ydelta)
        {
            if (x < 0)
            {
                xdelta = -xdelta;
                x = 0;
            }

            if (y < 0)
            {
                ydelta = -ydelta;
                y = 0;
            }

            if (x >= xMapTiles)
            {
                xdelta = -xdelta;
                x = xMapTiles - 1;
            }

            if (y >= yMapTiles)
            {
                ydelta = -ydelta;
                y = yMapTiles - 1;
            }

            if (worldMap[x, y] != 0)
            {
                return worldMap[x, y];
            }
            return NearestPoint(x + xdelta, y + ydelta, xdelta, ydelta);
        }

        /// <summary>
        ///  <para>
        ///   Computes a fractal grid in the given area.  Since this
        ///   is a fractal algorithm, this method is called recursively on
        ///   successively smaller map areas until the entire map has
        ///   been computed.
        ///  </para>
        ///  <para>
        ///   If the granularity of the distance between the points is less
        ///   than the established granularity (2 in this revision), then
        ///   the method quits out.  You can create a sparse map by giving
        ///   less of a granularity, however, the normalization function
        ///   will most likely populate all points on the map with valid
        ///   values anyway based on nearest point calculations.
        ///  </para>
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        internal void MapArea(int x0, int y0, int x1, int y1)
        {
            // We don't want to recurse too far.
            // Fractal algorithms are meant to work in a very defined
            // space.  Since we can't establish the grid on a 5x5 set
            // of boundaries we have to do some extra work.
            if (Granularity > (x1 - x0) || Granularity > (y1 - y0))
            {
                return;
            }

            int centerPoint;

            if (worldMap[(x1 + x0) >> 1, (y1 + y0) >> 1] != 0)
            {
                centerPoint = worldMap[(x1 + x0) >> 1, (y1 + y0) >> 1];
            }
            else
            {
                // Computer center of square
                centerPoint = SquareAverage(
                    worldMap[x0, y0],
                    worldMap[x0, y1],
                    worldMap[x1, y0],
                    worldMap[x1, y1],
                    BumpinessFactor);
                worldMap[(x1 + x0) >> 1, (y1 + y0) >> 1] = centerPoint;
            }

            // Compute Cross points to make a diamond
            // Make sure to take the center point twice
            // This should really be a TriangleAverage but
            // taking the center point twice weights us
            // to the midpoint of the landmass which isn't
            // that bad.
            worldMap[x0, (y1 + y0) >> 1] = SquareAverage(
                worldMap[x0, y0],
                worldMap[x0, y1],
                centerPoint,
                centerPoint,
                BumpinessFactor);

            worldMap[(x1 + x0) >> 1, y0] = SquareAverage(
                worldMap[x0, y0],
                worldMap[x1, y0],
                centerPoint,
                centerPoint,
                BumpinessFactor);

            worldMap[x1, (y1 + y0) >> 1] = SquareAverage(
                worldMap[x1, y0],
                worldMap[x1, y1],
                centerPoint,
                centerPoint,
                BumpinessFactor);

            worldMap[(x1 + x0) >> 1, y1] = SquareAverage(
                worldMap[x0, y1],
                worldMap[x1, y1],
                centerPoint,
                centerPoint,
                BumpinessFactor);

            MapArea(x0, y0, (x1 + x0) >> 1, (y1 + y0) >> 1);
            MapArea((x1 + x0) >> 1, y0, x1, (y1 + y0) >> 1);
            MapArea(x0, (y1 + y0) >> 1, (x1 + x0) >> 1, y1);
            MapArea((x1 + x0) >> 1, (y1 + y0) >> 1, x1, y1);
        }

        /// <summary>
        ///  Given 4 points, computes a center point based
        ///  on average height.  An additional random variance
        ///  factor is applied to the center point to enable
        ///  *bumpy* transitions.
        /// </summary>
        /// <param name="p1">One of 4 corner point heights.</param>
        /// <param name="p2">One of 4 corner point heights.</param>
        /// <param name="p3">One of 4 corner point heights.</param>
        /// <param name="p4">One of 4 corner point heights.</param>
        /// <param name="v">The variance factor used for bumpiness</param>
        /// <returns>The averaged height, +/- a random bumpy factor</returns>
        internal int SquareAverage(int p1, int p2, int p3, int p4, int v)
        {
            if (rand.Next(0, 2) == 1)
            {
                return ((p1 + p2 + p3 + p4) >> 2) + rand.Next(0, v);
            }
            return ((p1 + p2 + p3 + p4) >> 2) - rand.Next(0, v);
        }

        /// <summary>
        ///  Initialize a height field map with values for the initial corners
        ///  and center.  This ensures that the map will have a very specific
        ///  and consistent look and feel once it is complete.  This method
        ///  current places midPoint values in all 4 corners and the center.
        ///  Then dynamically computes a high point and low point that will
        ///  be used skew the map.  Note that InitMap sets very specific
        ///  points.  If the points set are selected randomly, then the method
        ///  of seeding won't necessarily work, since fractal subdivision uses
        ///  very specific points each iteration.
        /// </summary>
        internal void InitMap()
        {
            //  Here we are seeding values to get a nice map.
            //  We will be giving 1 high value, 2 mid values,
            //  and 1 low value.
            var highDirection = rand.Next(1, 5);
            var lowDirection = highDirection;

            var highValue = MaxLevel - (MaxLevel >> 2);
            var lowValue = (MaxLevel >> 2);
            var midValue = (MaxLevel >> 1);

            while (lowDirection == highDirection)
            {
                lowDirection = rand.Next(1, 6); // Add in a center point 1-4 corners, 5 center
            }

            worldMap[0, 0] = midValue;
            worldMap[0, yMapTiles - 1] = midValue;
            worldMap[xMapTiles - 1, 0] = midValue;
            worldMap[xMapTiles - 1, yMapTiles - 1] = midValue;
            worldMap[((xMapTiles - 1) >> 1), ((yMapTiles - 1) >> 1)] = midValue;

            switch (highDirection)
            {
                case 1:
                    worldMap[0, 0] = highValue;
                    break;
                case 2:
                    worldMap[0, yMapTiles - 1] = highValue;
                    break;
                case 3:
                    worldMap[xMapTiles - 1, 0] = highValue;
                    break;
                case 4:
                    worldMap[xMapTiles - 1, yMapTiles - 1] = highValue;
                    break;
                case 5:
                    worldMap[((xMapTiles - 1) >> 1), ((yMapTiles - 1) >> 1)] = highValue;
                    break;
            }

            switch (lowDirection)
            {
                case 1:
                    worldMap[0, 0] = lowValue;
                    break;
                case 2:
                    worldMap[0, yMapTiles - 1] = lowValue;
                    break;
                case 3:
                    worldMap[xMapTiles - 1, 0] = lowValue;
                    break;
                case 4:
                    worldMap[xMapTiles - 1, yMapTiles - 1] = lowValue;
                    break;
                case 5:
                    worldMap[((xMapTiles - 1) >> 1), ((yMapTiles - 1) >> 1)] = lowValue;
                    break;
            }
        }
    }
}