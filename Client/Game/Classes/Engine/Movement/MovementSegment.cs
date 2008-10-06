//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using OrganismBase;

namespace Terrarium.Game
{
    // Time zero is the start of the tick.  Time GridIndex.timeWindow is the end of the tick.  
    // Time is basically represented as a fraction of a tick.
    internal class MovementSegment
    {
        private Boolean _active;
        private OrganismState _blockedByState;
        private int _cellsLeftToResolve;
        private Point _endingPoint;
        private int _entryTime;
        private int _exitTime;
        private int _gridX;
        private int _gridY;
        private MovementSegment _nextMovementSegment;
        private OrganismState _organismState;
        private MovementSegment _previousMovementSegment;
        private Point _startingPoint;

        internal MovementSegment(MovementSegment previous, OrganismState state, Point startingPoint, int entryTime,
                                 int gridX, int gridY)
        {
            Debug.Assert((previous == null && entryTime == 0) || (previous != null && entryTime != 0));

            State = state;
            StartingPoint = startingPoint;
            EntryTime = entryTime;
            GridX = gridX;
            GridY = gridY;
            Previous = previous;
        }

        /// <summary>
        /// Since an organism actually occupies multiple cells, this member variable tracks how many cells
        /// it is occupying that we haven't 'resolved' yet (see notes in GridIndex for a description of "resolving".  
        /// Once this goes to zero, we know that the movement this segment represents can occur, because all of
        /// the cells are not occupied at this point in time
        /// </summary>
        public int CellsLeftToResolve
        {
            get { return _cellsLeftToResolve; }

            set
            {
                if (value == 0)
                {
                    // We're now resolved.  If we're not clipped, we're active
                    if (!IsClipped)
                    {
                        Active = true;
                        if (Previous != null)
                        {
                            // If previous isn't active, we shouldn't have gotten this far
                            Debug.Assert(Previous.Active);
                            Previous.Active = false;
                        }
                    }
                }
                else if (value < 0)
                {
                    value = 0;
                }

                _cellsLeftToResolve = value;
            }
        }

        public Boolean IsResolved
        {
            get { return CellsLeftToResolve == 0; }

            set
            {
                if (value)
                {
                    CellsLeftToResolve = 0;
                }
                else
                {
                    throw new ApplicationException("Should never get unresolved");
                }
            }
        }

        public Point StartingPoint
        {
            get { return _startingPoint; }

            set { _startingPoint = value; }
        }

        public Point EndingPoint
        {
            get { return _endingPoint; }

            set { _endingPoint = value; }
        }

        public int GridX
        {
            get { return _gridX; }

            set { _gridX = value; }
        }

        public int GridY
        {
            get { return _gridY; }

            set { _gridY = value; }
        }

        public int EntryTime
        {
            get { return _entryTime; }

            set { _entryTime = value; }
        }

        public int ExitTime
        {
            get { return _exitTime; }

            set { _exitTime = value; }
        }

        public OrganismState State
        {
            get { return _organismState; }

            set { _organismState = value; }
        }

        public MovementSegment Previous
        {
            get { return _previousMovementSegment; }

            set { _previousMovementSegment = value; }
        }

        public MovementSegment Next
        {
            get { return _nextMovementSegment; }

            set { _nextMovementSegment = value; }
        }

        public OrganismState BlockedByState
        {
            get { return _blockedByState; }

            set { _blockedByState = value; }
        }

        internal Boolean Active
        {
            get { return _active; }

            set
            {
                // Can't make a clipped segment active
                Debug.Assert((value && !IsClipped) | !value);
                _active = value;
            }
        }

        // True if this segment represents a stationary creature
        internal Boolean IsStationarySegment
        {
            get { return StartingPoint == EndingPoint && EntryTime == 0 && ExitTime == 0; }
        }

        internal Boolean IsClipped
        {
            get
            {
                // Segments are guaranteed to be able to stay in the cell they started in
                // so if EntryTime == 0 it can't be clipped
                // When a single segment is clipped, all subsequent segments get clipped too
                // so we only need to check one back to see if it is clipped
                return EntryTime != 0 && Previous.Next == null;
            }
        }

        internal Boolean IsStartingSegment
        {
            get { return EntryTime == 0; }
        }

        public override string ToString()
        {
            return GridX + ", " + GridY + "EntryTime=" + EntryTime + "ExitTime=" + ExitTime +
                   "StartingPoint=" + StartingPoint + "EndingPoint=" + EndingPoint;
        }

        public void ClipSegment(OrganismState blocker)
        {
            MovementSegment segment = this;
            Debug.Assert(segment.Previous != null);
            segment.Previous.BlockedByState = blocker;

            while (segment != null)
            {
                // the starting segment should never get clipped, therefore there should always be
                // a previous segment
                Debug.Assert(segment.Previous != null);
                segment.Previous.Next = null;

                // Once one cell clips it, it doesn't matter what the other cells get, they are clipped as well
                segment.CellsLeftToResolve = 0;

                // Clip all subsequent segments
                segment = segment.Next;
            }
        }
    }
}