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
        // This is used to speed up scanning for organisms.  It is a giant array that
        // contains an element for every cell in the game grid.  If the cell is not null,
        // it points to the animal that is occupying the cell.
        private readonly OrganismState[,] _cellOrganisms;

        private readonly int _gridHeight;
        private readonly int _gridWidth;

        // This member is here just so we don't create the visibility matrix for every call
        // to FindOrganismsInView.
        private readonly Byte[,] _invisible =
            new Byte[
                (EngineSettings.MaximumEyesightRadius + EngineSettings.BaseEyesightRadius + EngineSettings.MaxGridRadius)*
                2 + 1,
                (EngineSettings.MaximumEyesightRadius + EngineSettings.BaseEyesightRadius + EngineSettings.MaxGridRadius)*
                2 + 1];

        private readonly Hashtable _organisms = new Hashtable();
        private bool _indexBuilt;
        private bool _isImmutable;
        [NonSerialized] private ArrayList _orgs;
        private Guid _stateGuid;
        private Teleporter _teleporter;
        private int _tickNumber;

        /// <summary>
        ///  Creates a new world state with the given number of grid cells.
        /// </summary>
        /// <param name="gridWidth">The width of the world state in grid cells.</param>
        /// <param name="gridHeight">The height of the world state in grid cells.</param>
        public WorldState(int gridWidth, int gridHeight)
        {
            _gridWidth = gridWidth;
            _gridHeight = gridHeight;

            _cellOrganisms = new OrganismState[gridWidth,gridHeight];
        }

        /// <summary>
        ///  Determines if the cell index has been built.
        /// </summary>
        public bool IndexBuilt
        {
            get { return _indexBuilt; }
        }

        /// <summary>
        ///  Returns the collection of organisms.
        /// </summary>
        public ICollection Organisms
        {
            get { return _organisms.Values; }
        }

        /// <summary>
        ///  Returns the collection or organisms ZOrdered using
        ///  their Y value.
        /// </summary>
        public ICollection ZOrderedOrganisms
        {
            get
            {
                if (_orgs == null)
                {
                    _orgs = new ArrayList(_organisms.Values);
                    _orgs.Sort();
                }
                return _orgs;
            }
        }

        /// <summary>
        ///  Returns the GUID for the world state.
        /// </summary>
        public Guid StateGuid
        {
            get { return _stateGuid; }

            set
            {
                if (IsImmutable)
                {
                    throw new ApplicationException("WorldState is immutable.");
                }

                _stateGuid = value;
            }
        }

        /// <summary>
        ///  Returns the tick number of the current world state.
        /// </summary>
        public int TickNumber
        {
            get { return _tickNumber; }

            set
            {
                if (IsImmutable)
                {
                    throw new ApplicationException("WorldState is immutable.");
                }

                _tickNumber = value;
            }
        }

        /// <summary>
        ///  Provides access to the teleporters.
        /// </summary>
        public Teleporter Teleporter
        {
            get { return _teleporter; }

            set
            {
                if (IsImmutable)
                {
                    throw new ApplicationException("WorldState is immutable.");
                }

                _teleporter = value;
            }
        }

        /// <summary>
        ///  Provides the collection of organism IDs
        /// </summary>
        public ICollection OrganismIDs
        {
            get { return _organisms.Keys; }
        }

        /// <summary>
        ///  Determines if the current state is immutable.
        /// </summary>
        public Boolean IsImmutable
        {
            get { return _isImmutable; }
        }

        /// <summary>
        ///  Copies the object, but not the isImmutable bit.  Makes a newly
        ///  immutable copy.
        /// </summary>
        /// <returns>A new WorldState object that is newly mutable.</returns>
        public object DuplicateMutable()
        {
            var newState = new WorldState(_gridWidth, _gridHeight);

            foreach (OrganismState state in Organisms)
            {
                var newOrganismState = state.CloneMutable();
                Debug.Assert(newOrganismState != null, "Organism State is null in WorldState.DuplicateMutable()");
                Debug.Assert(newOrganismState.ID != null, "Organism State ID is null in WorldState.DuplicateMutable()");
                newState._organisms.Add(newOrganismState.ID, newOrganismState);
            }

            newState._tickNumber = _tickNumber;
            newState._stateGuid = _stateGuid;
            if (_teleporter != null)
            {
                newState._teleporter = _teleporter.Clone();
            }

            return newState;
        }

        /// <summary>
        ///  Clear the current cell index.
        /// </summary>
        public void ClearIndex()
        {
            if (_isImmutable)
            {
                throw new ApplicationException("WorldState must be mutable to re-add organisms.");
            }

            for (var x = 0; x < _gridWidth; x++)
            {
                for (var y = 0; y < _gridHeight; y++)
                {
                    _cellOrganisms[x, y] = null;
                }
            }

            _indexBuilt = false;
        }

        /// <summary>
        ///  Build the cell index.
        /// </summary>
        public void BuildIndex()
        {
            buildIndexInternal(false);
        }

        /// <summary>
        ///  Builds the cell index.
        /// </summary>
        /// <param name="isDeserializing">Determines if the engine is in the state of deserializing.</param>
        private void buildIndexInternal(bool isDeserializing)
        {
            if (!isDeserializing && _isImmutable)
            {
                throw new ApplicationException("WorldState must be mutable to rebuild index.");
            }

            if (Organisms.Count > 0)
            {
                foreach (OrganismState state in Organisms)
                {
                    Debug.Assert(state.GridX >= 0 && state.GridX < _gridWidth &&
                                 state.GridY >= 0 && state.GridY < _gridHeight);
                    Debug.Assert(_cellOrganisms[state.GridX, state.GridY] == null);

                    FillCells(state, state.GridX, state.GridY, state.CellRadius, false);

                    // Lock the size and position of the organism so that the index doesn't get out of sync
                    if (!isDeserializing)
                    {
                        state.LockSizeAndPosition();
                    }
                }
            }

            _indexBuilt = true;
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

            for (var x = cellX - cellRadius; x <= cellX + cellRadius; x++)
            {
                for (var y = cellY - cellRadius; y <= cellY + cellRadius; y++)
                {
                    if (clear)
                    {
                        // Make sure we are only clearing ourselves, the value may be null because clearindex
                        // may have been called
                        if (!(_cellOrganisms[x, y] == null || _cellOrganisms[x, y].ID == state.ID))
                        {
                            Debug.Assert(_cellOrganisms[x, y] == null || _cellOrganisms[x, y].ID == state.ID);
                        }
                        _cellOrganisms[x, y] = null;
                    }
                    else
                    {
                        // Make sure there was no one else here
                        if (!(_cellOrganisms[x, y] == null))
                        {
                            Debug.Assert(_cellOrganisms[x, y] == null);
                        }
                        _cellOrganisms[x, y] = state;
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

            Debug.Assert(state.GridX >= 0 && state.GridX < _gridWidth && state.GridY >= 0 && state.GridY < _gridHeight);
            Debug.Assert(_cellOrganisms[state.GridX, state.GridY] == null);

            if (_organisms.ContainsKey(state.ID))
            {
                throw new OrganismAlreadyExistsException();
            }

            FillCells(state, state.GridX, state.GridY, state.CellRadius, false);

            // Lock the size and position since we've added it to the index and these shouldn't be changed now
            state.LockSizeAndPosition();

            _organisms.Add(state.ID, state);
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

            var organismID = state.ID;

            // Clear the index if it's built
            if (IndexBuilt)
            {
                var oldState = GetOrganismState(organismID);
                FillCells(oldState, oldState.GridX, oldState.GridY, oldState.CellRadius, true);
            }

            _organisms.Remove(organismID);
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
                var state = GetOrganismState(organismID);
                FillCells(state, state.GridX, state.GridY, state.CellRadius, true);
            }

            _organisms.Remove(organismID);
        }

        /// <summary>
        ///  Retrieve the organism state from the world state for the given ID.
        /// </summary>
        /// <param name="organismID">The ID to match an organism state.</param>
        /// <returns>The state of the organism with the given ID.</returns>
        public OrganismState GetOrganismState(string organismID)
        {
            if (_organisms != null)
            {
                return (OrganismState) _organisms[organismID];
            }

            return null;
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

            _isImmutable = true;
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
            if (minX > _gridWidth - 1)
            {
                minX = _gridWidth - 1;
            }

            maxX >>= EngineSettings.GridWidthPowerOfTwo;
            if (maxX > _gridWidth - 1)
            {
                maxX = _gridWidth - 1;
            }

            minY >>= EngineSettings.GridHeightPowerOfTwo;
            if (minY > _gridHeight - 1)
            {
                minY = _gridHeight - 1;
            }

            maxY >>= EngineSettings.GridHeightPowerOfTwo;
            if (maxY > _gridHeight - 1)
            {
                maxY = _gridHeight - 1;
            }

            return FindOrganismsInCells(minX, maxX, minY, maxY);
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
            var maxX = plant.GridX + plant.CellRadius + 25;
            if (maxX > _gridWidth - 1)
            {
                maxX = _gridWidth - 1;
            }
            var overlappingPlantsEast = FindOrganismsInCells(plant.GridX + plant.CellRadius,
                                                             maxX, plant.GridY - plant.CellRadius,
                                                             plant.GridY + plant.CellRadius);

            var minX = plant.GridX - plant.CellRadius - 25;
            if (minX < 0)
            {
                minX = 0;
            }
            var overlappingPlantsWest = FindOrganismsInCells(minX, plant.GridX - plant.CellRadius,
                                                             plant.GridY - plant.CellRadius,
                                                             plant.GridY + plant.CellRadius);

            double maxAngleEast = 0;
            foreach (OrganismState targetPlant in overlappingPlantsEast)
            {
                if (!(targetPlant is PlantState)) continue;
                var currentAngle = Math.Atan2(((PlantState) targetPlant).Height,
                                              targetPlant.Position.X - plant.Position.X);
                if (currentAngle > maxAngleEast)
                {
                    maxAngleEast = currentAngle;
                }
            }

            double maxAngleWest = 0;
            foreach (OrganismState targetPlant in overlappingPlantsWest)
            {
                if (!(targetPlant is PlantState)) continue;
                var currentAngle = Math.Atan2(((PlantState) targetPlant).Height,
                                              plant.Position.X - targetPlant.Position.X);
                if (currentAngle > maxAngleWest)
                {
                    maxAngleWest = currentAngle;
                }
            }

            return (int) (((Math.PI - maxAngleEast + maxAngleWest)/Math.PI)*100);
        }

        /// <summary>
        ///  Make sure the organism only overlaps itself and not other organisms.
        /// </summary>
        /// <param name="state">The state of the organism to check.</param>
        /// <returns>True if the creature is safe, false otherwise.</returns>
        public Boolean OnlyOverlapsSelf(OrganismState state)
        {
            Debug.Assert(IndexBuilt);

            var minGridX = state.GridX - state.CellRadius;
            var maxGridX = state.GridX + state.CellRadius;
            var minGridY = state.GridY - state.CellRadius;
            var maxGridY = state.GridY + state.CellRadius;

            // If it would be out of bounds, return false.
            if (minGridX < 0 || maxGridX > _gridWidth - 1 ||
                minGridY < 0 || maxGridY > _gridHeight - 1)
            {
                return false;
            }

            for (var x = minGridX; x <= maxGridX; x++)
            {
                for (var y = minGridY; y <= maxGridY; y++)
                {
                    if (_cellOrganisms[x, y] == null) continue;
                    if (_cellOrganisms[x, y].ID != state.ID)
                    {
                        return false;
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
            Debug.Assert(IndexBuilt);

            Debug.Assert(minGridX <= maxGridX && minGridY <= maxGridY);
            Debug.Assert(minGridX >= 0 && maxGridX < _gridWidth && minGridY >= 0 && maxGridY < _gridHeight);

            OrganismState lastFound = null;

            // Since organisms are represented at multiple places in the grid, make
            // sure we only get one instance
            var foundHash = new Hashtable();
            var foundOrganisms = new ArrayList();
            for (var x = minGridX; x <= maxGridX; x++)
            {
                for (var y = minGridY; y <= maxGridY; y++)
                {
                    var current = _cellOrganisms[x, y];
                    if (current == null) continue;

                    // If it's the same as the last one, skip the hashable lookup
                    // since it's expensive and we'll often find the same organism over and over
                    // in a row
                    if (lastFound != null && lastFound == current) continue;
                    if (foundHash[current] == null)
                    {
                        foundHash[current] = current;
                        foundOrganisms.Add(current);
                    }

                    lastFound = current;
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
            Debug.Assert(IndexBuilt);

            // Make sure we have enough space in our visibility matrix
            Debug.Assert((state.CellRadius + radius)*2 + 1 <= _invisible.GetLength(0));

            var foundOrganisms = new ArrayList();
            var foundHash = new Hashtable();
            var originX = state.GridX;
            var originY = state.GridY;
            var middleX = -originX + state.CellRadius + radius;
            var middleY = -originY + state.CellRadius + radius;
            int xIncrement = 0, yIncrement = 0;

            // The first ring is all visible
            var currentRadius = state.CellRadius + 1;
            var currentX = originX - currentRadius;
            var currentY = originY - currentRadius;
            for (var side = 0; side < 4; side++)
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

                for (var count = 0; count < currentRadius << 1; count++)
                {
                    if (currentX >= 0 && currentY >= 0 && currentX < _gridWidth && currentY < _gridHeight)
                    {
                        var currentOrganism = _cellOrganisms[currentX, currentY];
                        if (currentOrganism != null)
                        {
                            if (foundHash[currentOrganism] == null)
                            {
                                foundHash[currentOrganism] = currentOrganism;
                                foundOrganisms.Add(currentOrganism);
                            }
                        }

                        _invisible[currentX + middleX, currentY + middleY] = 0;
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
                currentX = originX - currentRadius;
                currentY = originY - currentRadius;
                for (var side = 0; side < 4; side++)
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

                    for (var count = 0; count < currentRadius << 1; count++)
                    {
                        if (currentX >= 0 && currentY >= 0 && currentX < _gridWidth && currentY < _gridHeight)
                        {
                            var i = currentX - originX;
                            var j = currentY - originY;

                            // These actually calculate -1 * sign not just the sign function
                            int signI;
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

                            int signJ;
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

                            int absI;
                            if (i < 0)
                            {
                                absI = -i;
                            }
                            else
                            {
                                absI = i;
                            }

                            int absJ;
                            if (j < 0)
                            {
                                absJ = -j;
                            }
                            else
                            {
                                absJ = j;
                            }

                            // Check first point which is the diagonal direction from [x,y] to [originX,originY]
                            int p1X;
                            int p1Y;
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
                            int p2X;
                            int p2Y;
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
                            if (_invisible[p1X + middleX, p1Y + middleY] == 1 ||
                                _invisible[p2X + middleX, p2Y + middleY] == 1)
                            {
                                _invisible[currentX + middleX, currentY + middleY] = 1;
                            }
                            else
                            {
                                var currentOrganism = _cellOrganisms[currentX, currentY];
                                if (currentOrganism != null)
                                {
                                    // if there is an organism here, mark this spot as invisible
                                    // (even though it really isn't)
                                    // so the outer cells will be invisible too
                                    _invisible[currentX + middleX, currentY + middleY] = 1;

                                    if (foundHash[currentOrganism] == null)
                                    {
                                        foundHash[currentOrganism] = currentOrganism;
                                        foundOrganisms.Add(currentOrganism);
                                    }
                                }
                                else
                                {
                                    _invisible[currentX + middleX, currentY + middleY] = 0;
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
            Debug.Assert(IndexBuilt);
            Debug.Assert(cellX < _gridWidth && cellY < _gridHeight && cellX >= 0 && cellY >= 0);
            return _cellOrganisms[cellX, cellY] != null;
        }
    }
}