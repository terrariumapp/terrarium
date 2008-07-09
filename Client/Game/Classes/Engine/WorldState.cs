//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Diagnostics;

using OrganismBase;

namespace Terrarium.Game 
{
    /// <summary>
    ///  Contains the full state of the world at a given tick.  Contains
    ///  state objects for all organisms.  It is immutable once we have finished 
    ///  a game tick and can stay around for as long as needed to represent the state
    ///  of the world at that point in time.
    /// </summary>
    /// <immutable/>
    [Serializable]
    public sealed class WorldState 
    { 
        Guid stateGuid;
        int tickNumber = 0;
        bool isImmutable = false;
        Hashtable organisms = new Hashtable();

        // This is used to speed up scanning for organisms.  It is a giant array that
        // contains an element for every cell in the game grid.  If the cell is not null,
        // it points to the animal that is occupying the cell.
        OrganismState [,] CellOrganisms;

        Teleporter teleporter;
        bool indexBuilt = false;
        int gridWidth, gridHeight;
    
        [NonSerialized]
        ArrayList orgs;

        // This member is here just so we don't create the visibility matrix for every call
        // to FindOrganismsInView.
        Byte [,] invisible = new Byte[(EngineSettings.MaximumEyesightRadius + EngineSettings.BaseEyesightRadius + EngineSettings.MaxGridRadius) * 2 + 1,
            (EngineSettings.MaximumEyesightRadius + EngineSettings.BaseEyesightRadius + EngineSettings.MaxGridRadius) * 2 + 1];

        /// <summary>
        ///  Creates a new world state with the given number of grid cells.
        /// </summary>
        /// <param name="gridWidth">The width of the world state in grid cells.</param>
        /// <param name="gridHeight">The height of the world state in grid cells.</param>
        public WorldState(int gridWidth, int gridHeight)
        {
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;

            CellOrganisms = new OrganismState[gridWidth, gridHeight];
        }

        /// <summary>
        ///  Copies the object, but not the isImmutable bit.  Makes a newly
        ///  immutable copy.
        /// </summary>
        /// <returns>A new WorldState object that is newly mutable.</returns>
        public object DuplicateMutable() 
        {
            WorldState newState = new WorldState(gridWidth, gridHeight);

            foreach (OrganismState state in Organisms) 
            {
                OrganismState newOrganismState = state.CloneMutable();
                Debug.Assert(newOrganismState != null, "Organism State is null in WorldState.DuplicateMutable()");
                Debug.Assert(newOrganismState.ID != null, "Organism State ID is null in WorldState.DuplicateMutable()");
                newState.organisms.Add(newOrganismState.ID, newOrganismState);
            }

            newState.tickNumber = tickNumber;
            newState.stateGuid = stateGuid;
            if (teleporter != null)
            {
                newState.teleporter = teleporter.Clone();
            }

            return newState;
        }

        /// <summary>
        ///  Determines if the cell index has been built.
        /// </summary>
        public bool IndexBuilt
        {
            get
            {
                return indexBuilt;
            }
        }

        /// <summary>
        ///  Clear the current cell index.
        /// </summary>
        public void ClearIndex()
        {
            if (isImmutable)
            {
                throw new ApplicationException("WorldState must be mutable to re-add organisms.");
            }

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    CellOrganisms[x,y] = null;
                }
            }

            indexBuilt = false;
        }

        /// <summary>
        ///  Build the cell index.
        /// </summary>
        public void BuildIndex()
        {
            BuildIndexInternal(false);
        }

        /// <summary>
        ///  Builds the cell index.
        /// </summary>
        /// <param name="isDeserializing">Determines if the engine is in the state of deserializing.</param>
        void BuildIndexInternal(bool isDeserializing)
        {
            if (!isDeserializing && isImmutable)
            {
                throw new ApplicationException("WorldState must be mutable to rebuild index.");
            }

            if (Organisms.Count > 0 )
            {
                foreach (OrganismState state in Organisms) 
                {
                    Debug.Assert(state.GridX >= 0 && state.GridX < gridWidth && 
                        state.GridY >= 0 && state.GridY < gridHeight);
                    Debug.Assert(CellOrganisms[state.GridX, state.GridY] == null);

                    FillCells(state, state.GridX, state.GridY, state.CellRadius, false);

                    // Lock the size and position of the organism so that the index doesn't get out of sync
                    if (!isDeserializing)
                    {
                        state.LockSizeAndPosition();
                    }
                }
            }

            indexBuilt = true;
        }

        /// <summary>
        ///  Fills in the appropriate grid cells in our CellIndex given the organism state.
        /// </summary>
        /// <param name="state">The state of the organism being added.</param>
        /// <param name="cellX">The location of the organism in cells.</param>
        /// <param name="cellY">The location of the organism in cells.</param>
        /// <param name="cellRadius">The radius in cells of the organism.</param>
        /// <param name="clear">Determines if cells should be cleared or set.</param>
        public void FillCells(OrganismState state, int cellX, int cellY, int cellRadius, Boolean clear)
        {
            Debug.Assert(cellX - cellRadius >= 0 && cellY - cellRadius + 1 >= 0);

            for (int x = cellX - cellRadius; x <= cellX + cellRadius; x++)
            {
                for (int y = cellY - cellRadius; y <= cellY + cellRadius; y++)
                {
                    if (clear)
                    {
                        // Make sure we are only clearing ourselves, the value may be null because clearindex
                        // may have been called
                        if (!(CellOrganisms[x,y] == null || CellOrganisms[x,y].ID == state.ID))
                        {
                            Debug.Assert(CellOrganisms[x,y] == null || CellOrganisms[x,y].ID == state.ID);
                        }
                        CellOrganisms[x,y] = null;
                    }
                    else
                    {
                        // Make sure there was no one else here
                        if (!(CellOrganisms[x,y] == null))
                        {
                            Debug.Assert(CellOrganisms[x,y] == null);
                        }
                        CellOrganisms[x,y] = state;
                    }

                }
            }
        }

        /// <summary>
        ///  Should only be called by the GameEngine.
        /// </summary>
        /// <param name="state">The state of the organism to add.</param>
        public void AddOrganism(OrganismState state) 
        {
            if (IsImmutable)
            {
                throw new ApplicationException("WorldState must be mutable to add organisms.");
            }
        
            Debug.Assert(state.GridX >= 0 && state.GridX < gridWidth && state.GridY >= 0 && state.GridY < gridHeight);
            Debug.Assert(CellOrganisms[state.GridX, state.GridY] == null);

            if (organisms.ContainsKey(state.ID))
            {
                throw new OrganismAlreadyExistsException();
            }

            FillCells(state, state.GridX, state.GridY, state.CellRadius, false);
        
            // Lock the size and position since we've added it to the index and these shouldn't be changed now
            state.LockSizeAndPosition();

            organisms.Add(state.ID, state);
        }

        /// <summary>
        ///  Should only be called by the game engine.  Should be called if the state
        ///  of the organism changes.
        /// </summary>
        /// <param name="state">The state of the organism to refresh.</param>
        public void RefreshOrganism(OrganismState state) 
        {
            if (IsImmutable)
            {
                throw new ApplicationException("WorldState must be mutable to change.");
            }

            string organismID = state.ID;

            // Clear the index if it's built
            if (IndexBuilt)
            {
                OrganismState oldState = GetOrganismState(organismID);
                FillCells(oldState, oldState.GridX, oldState.GridY, oldState.CellRadius, true);
            }

            organisms.Remove(organismID);
            AddOrganism(state);
        }

        /// <summary>
        ///  Should only be called by the game engine.  Removes an organism from the world state.
        /// </summary>
        /// <param name="organismID">The ID of the organism that needs to be removed.</param>
        public void RemoveOrganism(string organismID) 
        {
            if (IsImmutable)
            {
                throw new ApplicationException("WorldState must be mutable to change.");
            }

            // Clear the index if it's built
            if (IndexBuilt)
            {
                OrganismState state = GetOrganismState(organismID);
                FillCells(state, state.GridX, state.GridY, state.CellRadius, true);
            }

            organisms.Remove(organismID);
        }

        /// <summary>
        ///  Retrieve the organism state from the world state for the given ID.
        /// </summary>
        /// <param name="organismID">The ID to match an organism state.</param>
        /// <returns>The state of the organism with the given ID.</returns>
        public OrganismState GetOrganismState(string organismID)
        {
            if (organisms != null)
            {
                return (OrganismState) organisms[organismID];
            }

            return null;
        }

        /// <summary>
        ///  Returns the collection of organisms.
        /// </summary>
        public ICollection Organisms 
        {
            get 
            {
                return (ICollection) organisms.Values;
            }
        }

        /// <summary>
        ///  Returns the collection or organisms ZOrdered using
        ///  their Y value.
        /// </summary>
        public ICollection ZOrderedOrganisms
        {
            get
            {
                if (orgs == null)
                {
                    orgs = new ArrayList(organisms.Values);
                    orgs.Sort();
                }
                return (ICollection) orgs;
            }
        }

        /// <summary>
        ///  Returns the GUID for the world state.
        /// </summary>
        public Guid StateGuid 
        {
            get 
            {
                return stateGuid; 
            }
        
            set
            {
                if (IsImmutable)
                {
                    throw new ApplicationException("WorldState is immutable.");
                }

                stateGuid = value;
            }
        }

        /// <summary>
        ///  Returns the tick number of the current world state.
        /// </summary>
        public int TickNumber
        {
            get 
            {
                return tickNumber;
            }
        
            set
            {
                if (IsImmutable)
                {
                    throw new ApplicationException("WorldState is immutable.");
                }

                tickNumber = value;
            }
        }

        /// <summary>
        ///  Provides access to the teleporters.
        /// </summary>
        public Teleporter Teleporter
        {
            get 
            {
                return teleporter;
            }
        
            set
            {
                if (IsImmutable)
                {
                    throw new ApplicationException("WorldState is immutable.");
                }

                teleporter = value;
            }
        }

        /// <summary>
        ///  Provides the collection of organism IDs
        /// </summary>
        public ICollection OrganismIDs 
        {
            get 
            {
                return (ICollection) organisms.Keys; 
            }
        }

        /// <summary>
        ///  Makes all portions of the world state immutable including
        ///  all organism state objects.
        /// </summary>
        public void MakeImmutable()
        {
            foreach (OrganismState state in Organisms)
            {
                state.MakeImmutable();
            }

            isImmutable = true;
        }

        /// <summary>
        ///  Find organisms with the given rectangular area.
        /// </summary>
        /// <param name="x1">Part of the location rectangle.</param>
        /// <param name="x2">Part of the location rectangle.</param>
        /// <param name="y1">Part of the location rectangle.</param>
        /// <param name="y2">Part of the location rectangle.</param>
        /// <returns>A list of organisms in the given rectangular area.</returns>
        public ArrayList FindOrganisms(int x1, int x2, int y1, int y2) 
        {
            int minX, maxX, minY, maxY;
            if (x1 < x2)
            {
                minX = x1;
                maxX = x2;
            }
            else
            {
                minX = x2;
                maxX = x1;
            }

            if (y1 < y2)
            {
                minY = y1;
                maxY = y2;
            }
            else
            {
                minY = y2;
                maxY = y1;
            }

            if (minX < 0)
            {
                minX = 0;
            }
            if (maxX < 0)
            {
                maxX = 0;
            }
            if (minY < 0)
            {
                minY = 0;
            }
            if (maxY < 0)
            {
                maxY = 0;
            }

            minX >>= EngineSettings.GridWidthPowerOfTwo;
            if (minX > gridWidth - 1)
            {
                minX = gridWidth - 1;
            }

            maxX >>= EngineSettings.GridWidthPowerOfTwo;
            if (maxX > gridWidth - 1)
            {
                maxX = gridWidth - 1;
            }

            minY >>= EngineSettings.GridHeightPowerOfTwo;
            if (minY > gridHeight - 1)
            {
                minY = gridHeight - 1;
            }

            maxY >>= EngineSettings.GridHeightPowerOfTwo;
            if (maxY > gridHeight - 1)
            {
                maxY = gridHeight - 1;
            }

            return FindOrganismsInCells(minX, maxX, minY, maxY);
        }

        /// <summary>
        ///  Determines if the current state is immutable.
        /// </summary>
        public Boolean IsImmutable 
        {
            get 
            {
                return isImmutable; 
            }
        }

        /// <summary>
        ///  Percentage of light reaching this plant.
        ///  We assume the sun moves from east to west directly overhead
        ///
        ///  We get a rough estimation like this:
        ///  Get all plants with a certain radius whose radius blocks any East-West vector that intersects
        ///  any part of the radius of the plant in question -- assume they block it completely
        ///  Figure out which blocks it at the highest angle.
        ///  Discount the amount of light the plant sees by angle / 180
        /// </summary>
        /// <param name="plant">The plant to get light for.</param>
        /// <returns>The amount of available light for the plant.</returns>
        public int GetAvailableLight(PlantState plant)
        {
            int maxX = plant.GridX + plant.CellRadius + 25;
            if (maxX > gridWidth - 1)
            {
                maxX = gridWidth - 1;
            }
            ArrayList overlappingPlantsEast = FindOrganismsInCells(plant.GridX + plant.CellRadius,
                maxX, plant.GridY - plant.CellRadius, plant.GridY + plant.CellRadius);

            int minX = plant.GridX - plant.CellRadius - 25;
            if (minX < 0)
            {
                minX = 0;
            }
            ArrayList overlappingPlantsWest = FindOrganismsInCells(minX, plant.GridX - plant.CellRadius,
                plant.GridY - plant.CellRadius, plant.GridY + plant.CellRadius);

            double maxAngleEast = 0;
            foreach (OrganismState targetPlant in overlappingPlantsEast)
            {
                if (targetPlant is PlantState)
                {
                    double currentAngle = Math.Atan2(((PlantState)targetPlant).Height, targetPlant.Position.X - plant.Position.X);
                    if (currentAngle > maxAngleEast)
                    {
                        maxAngleEast = currentAngle;
                    }
                }
            }

            double maxAngleWest = 0;
            foreach (OrganismState targetPlant in overlappingPlantsWest)
            {
                if (targetPlant is PlantState)
                {
                    double currentAngle = Math.Atan2(((PlantState) targetPlant).Height, plant.Position.X - targetPlant.Position.X);
                    if (currentAngle > maxAngleWest)
                    {
                        maxAngleWest = currentAngle;
                    }
                }
            }

            return (int) (((Math.PI - maxAngleEast + maxAngleWest) / Math.PI) * 100);
        }

        /// <summary>
        ///  Make sure the organism only overlaps itself and not other organisms.
        /// </summary>
        /// <param name="state">The state of the organism to check.</param>
        /// <returns>True if the creature is safe, false otherwise.</returns>
        public Boolean OnlyOverlapsSelf(OrganismState state) 
        {
            Debug.Assert(this.IndexBuilt);

            int minGridX = state.GridX - state.CellRadius;
            int maxGridX = state.GridX + state.CellRadius;
            int minGridY = state.GridY - state.CellRadius;
            int maxGridY = state.GridY + state.CellRadius;

            // If it would be out of bounds, return false.
            if (minGridX < 0 || maxGridX > gridWidth - 1 ||
                minGridY < 0 || maxGridY > gridHeight - 1)
            {
                return false;
            }

            for (int x = minGridX; x <= maxGridX; x++)
            {
                for (int y = minGridY; y <= maxGridY; y++)
                {
                    if (CellOrganisms[x,y] != null)
                    {
                        if (CellOrganisms[x,y].ID != state.ID)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///  Finds all organisms within a range of cells.
        /// </summary>
        /// <param name="minGridX">Leftmost grid cell</param>
        /// <param name="maxGridX">Rightmost grid cell</param>
        /// <param name="minGridY">Topmost grid cell</param>
        /// <param name="maxGridY">Bottommost grid cell</param>
        /// <returns>A list of organisms within the cell range.</returns>
        public ArrayList FindOrganismsInCells(int minGridX, int maxGridX, int minGridY, int maxGridY) 
        {
            Debug.Assert(this.IndexBuilt);

            Debug.Assert(minGridX <= maxGridX && minGridY <= maxGridY);
            Debug.Assert(minGridX >= 0 && maxGridX < gridWidth && minGridY >= 0 && maxGridY < gridHeight);

            OrganismState lastFound = null;
            OrganismState current = null;

            // Since organisms are represented at multiple places in the grid, make
            // sure we only get one instance
            Hashtable foundHash = new Hashtable();
            ArrayList foundOrganisms = new ArrayList();
            for (int x = minGridX; x <= maxGridX; x++)
            {
                for (int y = minGridY; y <= maxGridY; y++)
                {
                    current = CellOrganisms[x,y];
                    if (current != null)
                    {
                        // If it's the same as the last one, skip the hashable lookup
                        // since it's expensive and we'll often find the same organism over and over
                        // in a row
                        if (lastFound == null || lastFound != current)
                        {
                            if (foundHash[current] == null)
                            {
                                foundHash[current] = current;
                                foundOrganisms.Add(current);
                            }

                            lastFound = current;
                        }
                    }
                }
            }

            return foundOrganisms;
        }

        /// <summary>
        ///  Find a list of organisms within the view of the given organism.
        /// </summary>
        /// <param name="state">The state of the organism to check.</param>
        /// <param name="radius">The radius of vision.</param>
        /// <returns>A list of found organisms.</returns>
        public ArrayList FindOrganismsInView(OrganismState state, int radius) 
        {
            Debug.Assert(this.IndexBuilt);

            // Make sure we have enough space in our visibility matrix
            Debug.Assert((state.CellRadius + radius) * 2 + 1 <= invisible.GetLength(0));

            ArrayList foundOrganisms = new ArrayList();
            Hashtable foundHash = new Hashtable();
            int originX = state.GridX;
            int originY = state.GridY;
            int middleX = -originX + state.CellRadius + radius;
            int middleY = -originY + state.CellRadius + radius;
            int currentX, currentY;
            int xIncrement = 0, yIncrement = 0;
            int i, j;
            int signI, signJ;
            int absI, absJ;

            // The first ring is all visible
            int currentRadius = state.CellRadius + 1;
            currentX = originX - currentRadius;
            currentY = originY - currentRadius;
            for (int side = 0; side < 4; side++)
            {
                switch (side) 
                {
                    case 0:
                        xIncrement = 1;
                        yIncrement = 0;
                        break;
                    case 1:
                        xIncrement = 0;
                        yIncrement = 1;
                        break;
                    case 2:
                        xIncrement = -1;
                        yIncrement = 0;
                        break;
                    case 3:
                        xIncrement = 0;
                        yIncrement = -1;
                        break;
                }

                for (int count=0; count < currentRadius << 1; count++)
                {
                    if (currentX >= 0 && currentY >= 0 && currentX < gridWidth && currentY < gridHeight)
                    {
                        OrganismState currentOrganism = CellOrganisms[currentX, currentY];
                        if (currentOrganism != null)
                        {
                            if (foundHash[currentOrganism] == null)
                            {
                                foundHash[currentOrganism] = currentOrganism;
                                foundOrganisms.Add(currentOrganism);
                            }
                        }

                        invisible[currentX + middleX, currentY + middleY] = 0;
                    }

                    currentX += xIncrement;
                    currentY += yIncrement;
                }
            }

            for (currentRadius = state.CellRadius + 2; currentRadius <= state.CellRadius + radius; currentRadius++)
            {
                // Look at each point on a square of radius currentRadius
                // Look at the two points that are between this point and the origin
                // if they are invisible, this point is invisible

                // p1 is in the diagonal direction from [x,y] to [originX,originY] and
                // p2 is in the horizontal/vertical direction from [x,y] to [originX, originY].
                // p2 collapses to point_1 if j = 0 or i = 0
                int p1X, p1Y, p2X, p2Y;
                currentX = originX - currentRadius;
                currentY = originY - currentRadius;
                for (int side = 0; side < 4; side++)
                {
                    switch (side)
                    {
                        case 0:
                            xIncrement = 1;
                            yIncrement = 0;
                            break;
                        case 1:
                            xIncrement = 0;
                            yIncrement = 1;
                            break;
                        case 2:
                            xIncrement = -1;
                            yIncrement = 0;
                            break;
                        case 3:
                            xIncrement = 0;
                            yIncrement = -1;
                            break;
                    }

                    for (int count=0; count < currentRadius << 1; count++)
                    {
                        if (currentX >= 0 && currentY >= 0 && currentX < gridWidth && currentY < gridHeight)
                        {
                            i = currentX - originX;
                            j = currentY - originY;

                            // These actually calculate -1 * sign not just the sign function
                            if (i < 0)
                            {
                                signI = 1;
                            }
                            else if (i > 0)
                            {
                                signI = -1;
                            }
                            else
                            {
                                signI = 0;
                            }

                            if (j < 0)
                            {
                                signJ = 1;
                            }
                            else if (j > 0)
                            {
                                signJ = -1;
                            }
                            else
                            {
                                signJ = 0;
                            }

                            if (i < 0)
                            {
                                absI = -i;
                            }
                            else
                            {
                                absI = i;
                            }


                            if (j < 0)
                            {
                                absJ = -j;
                            }
                            else
                            {
                                absJ = j;
                            }

                            // Check first point which is the diagonal direction from [x,y] to [originX,originY]
                            if (absJ > absI)
                            {
                                // We are in the  90 < theta < 45 degrees region of every quadrant
                                p1X = currentX;
                                p1Y = signJ + currentY;
                            }
                            else 
                            {
                                p1X = signI + currentX;
                                p1Y = signJ + currentY;
                            }

                            // Check second point
                            // if Abs(j) == Abs(i) then we are on a diagonal, so secondpoint is the same as first point
                            if (absJ != absI)
                            {
                                p2X = signI + currentX;
                                p2Y = signJ + currentY;
                            }
                            else
                            {
                                p2X = p1X;
                                p2Y = p1Y;
                            }

                            // if p1 or p2 was invisible or they were something that blocks visibility
                            if (invisible[p1X + middleX, p1Y + middleY] == 1 ||
                                invisible[p2X + middleX, p2Y + middleY] == 1 )
                            {
                                invisible[currentX + middleX, currentY + middleY] = 1;
                            }
                            else
                            {
                                OrganismState currentOrganism = CellOrganisms[currentX, currentY];
                                if (currentOrganism != null)
                                {
                                    // if there is an organism here, mark this spot as invisible
                                    // (even though it really isn't)
                                    // so the outer cells will be invisible too
                                    invisible[currentX + middleX, currentY + middleY] = 1;

                                    if (foundHash[currentOrganism] == null)
                                    {
                                        foundHash[currentOrganism] = currentOrganism;
                                        foundOrganisms.Add(currentOrganism);
                                    }
                                }
                                else
                                {
                                    invisible[currentX + middleX, currentY + middleY] = 0;
                                }
                            }
                        }

                        currentX += xIncrement;
                        currentY += yIncrement;
                    }
                }
            }

            return foundOrganisms;
        }
 
        /// <summary>
        ///  Used to determine if a grid cell is occupied.
        /// </summary>
        /// <param name="cellX">The cell index.</param>
        /// <param name="cellY">The cell index.</param>
        /// <returns>True if the cell is occupied, false otherwise.</returns>
        public Boolean IsGridCellOccupied(int cellX, int cellY)
        {
            Debug.Assert(this.IndexBuilt);
            Debug.Assert(cellX < gridWidth && cellY < gridHeight && cellX >= 0 && cellY >= 0);
            return CellOrganisms[cellX, cellY] != null;
        }
    }
}