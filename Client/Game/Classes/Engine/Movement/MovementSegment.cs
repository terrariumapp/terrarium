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
        private int _cellsLeftToResolve;

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

        public Point StartingPoint { get; set; }

        public Point EndingPoint { get; set; }

        public int GridX { get; set; }

        public int GridY { get; set; }

        public int EntryTime { get; set; }

        public int ExitTime { get; set; }

        public OrganismState State { get; set; }

        public MovementSegment Previous { get; set; }

        public MovementSegment Next { get; set; }

        public OrganismState BlockedByState { get; set; }

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
            return string.Format("{0}, {1}EntryTime={2}ExitTime={3}StartingPoint={4}EndingPoint={5}", GridX, GridY, EntryTime, ExitTime, StartingPoint, EndingPoint);
        }

        public void ClipSegment(OrganismState blocker)
        {
            var segment = this;
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