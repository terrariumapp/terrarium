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
        // The coordinates of the line segment where this MovementSegment entered the cell that created this segment.
        Point startingPoint;

        // The coordinates of where the line segment exited the cell.
        Point endingPoint = new Point();

        // Position in the grid that we are entering and exiting from
        int gridX, gridY;        

        // At time 'EntryTime' the segment is already in the cell
        // At time 'ExitTime' the segment has already left the cell
        int entryTime = 0, exitTime = 0;  

        // The organism that is doing the moving
        OrganismState state;

        // Pointers to the next and previous segments that make up the path the organism is moving on
        // for this turn
        MovementSegment previous = null;
        MovementSegment next = null;

        // Since an organism actually occupies multiple cells, this member variable tracks how many cells
        // it is occupying that we haven't 'resolved' yet (see notes in GridIndex for a description of "resolving".  
        // Once this goes to zero, we know that the movement this segment represents can occur, because all of
        // the cells are not occupied at this point in time
        int cellsLeftToResolve = 0;  

        // True if we have validated that the organism can validly move to the spot this segment represents
        Boolean active = false; 

        // The organism that blocked the movement of this segment
        OrganismState blockedByState = null;

        internal MovementSegment(MovementSegment previous, OrganismState state, Point startingPoint, int entryTime, int gridX, int gridY)
        {
            Debug.Assert((previous == null && entryTime == 0) || (previous != null && entryTime != 0));

            this.State = state;
            this.StartingPoint = startingPoint;
            this.EntryTime = entryTime;     
            this.GridX = gridX;
            this.GridY = gridY;
            this.Previous = previous;
        }
    
        
        public override string ToString()
        {
            return GridX.ToString() + ", " + GridY.ToString() + "EntryTime=" + EntryTime.ToString() + "ExitTime=" + ExitTime.ToString() +
                "StartingPoint=" + StartingPoint.ToString() + "EndingPoint=" + EndingPoint.ToString();
        }

        public int CellsLeftToResolve 
        {
            get 
            {
                return cellsLeftToResolve;
            }

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

                cellsLeftToResolve = value;
            }
        }

        public Boolean IsResolved
        {
            get 
            {
                return CellsLeftToResolve == 0;
            }

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
   
        public Point StartingPoint
        {
            get
            {
                return startingPoint;
            }

            set
            {
                startingPoint = value;
            }
        }

        public Point EndingPoint 
        {
            get
            {
                return endingPoint;
            }

            set
            {
                endingPoint = value;
            }
        }

        public int GridX
        {
            get
            {
                return gridX;
            }

            set
            {
                gridX = value;
            }
        }

        public int GridY
        {
            get
            {
                return gridY;
            }

            set
            {
                gridY = value;
            }
        }

        public int EntryTime 
        {
            get
            {
                return entryTime;
            }

            set
            {
                entryTime = value;
            }
        }

        public int ExitTime
        {
            get
            {
                return exitTime;
            }

            set
            {
                exitTime = value;
            }
        }

        public OrganismState State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }

        public MovementSegment Previous
        {
            get
            {
                return previous;
            }

            set
            {
                previous = value;
            }
        }

        public MovementSegment Next 
        {
            get
            {
                return next;
            }

            set
            {
                next = value;
            }
        }

        public OrganismState BlockedByState
        {
            get
            {
                return blockedByState;
            }

            set
            {
                blockedByState = value;
            }
        }

        internal Boolean Active 
        {
            get 
            {
                return active;
            }

            set
            {
                // Can't make a clipped segment active
                Debug.Assert((value && !IsClipped) | !value);
                active = value;
            }
        }

        // True if this segment represents a stationary creature
        internal Boolean IsStationarySegment 
        {
            get
            {
                return StartingPoint == EndingPoint && EntryTime == 0 && ExitTime == 0;
            }
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
            get 
            {
                return EntryTime == 0;
            }
        }
    }
}