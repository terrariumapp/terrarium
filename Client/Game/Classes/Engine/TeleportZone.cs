//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Drawing;

using OrganismBase;

namespace Terrarium.Game 
{
    /// <summary>
    ///  Creatures a new TeleportZone object.
    /// </summary>
    [Serializable]
    public class TeleportZone
    {
        /// <summary>
        ///  The location and size of the rectangle.
        /// </summary>
        Rectangle rectangle;

        /// <summary>
        ///  The vector of movement for this zone.
        /// </summary>
        MovementVector vector;

        /// <summary>
        ///  The ID of this teleporter.
        /// </summary>
        int teleportID;

        /// <summary>
        ///  Creates a new teleportzone with all fields initialized.
        /// </summary>
        /// <param name="rectangle">The location and size of the zone.</param>
        /// <param name="vector">The movement vector of the zone.</param>
        /// <param name="ID">The ID for this zone.</param>
        public TeleportZone(Rectangle rectangle, MovementVector vector, int ID)
        {
            this.rectangle = rectangle;
            this.vector = vector;
            this.teleportID = ID;
        }
    
        /// <summary>
        ///  Clones a TeleportZone object.
        /// </summary>
        /// <returns>A copy of the TeleportZone</returns>
        public TeleportZone Clone() 
        {
            TeleportZone zone = new TeleportZone(rectangle, vector, ID);
            return zone;
        }

        /// <summary>
        ///  Returns the size and location of the teleport zone.
        /// </summary>
        /// <returns>A new rectangle containing the size and location of the zone.</returns>
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
            }
        }
    
        /// <summary>
        ///  Sets the size and location of the teleport zone.
        /// </summary>
        /// <param name="rectangle">The new size and location of this teleporter.</param>
        /// <returns>A cloned teleport zone.</returns>
        public TeleportZone SetRectangle(Rectangle rectangle)
        {
            TeleportZone zone = Clone();
            zone.rectangle = rectangle;
            return zone;
        }

        /// <summary>
        ///  The ID for this teleporter zone.
        /// </summary>
        public int ID 
        {
            get 
            {
                return teleportID;
            }
        }

        /// <summary>
        ///  Determines if the given organism state is within teleport zone.
        /// </summary>
        /// <param name="state">The state being checked.</param>
        /// <returns>True if the state is in the zone, false otherwise.</returns>
        public Boolean Contains(OrganismState state)
        {
            int difference = rectangle.Left - (state.Position.X - state.Radius);
            if (difference < 0) 
            {
                // Negative means rectangle boundary < state boundary
                if (-difference > rectangle.Width + 1)
                {
                    // X isn't overlapping or adjacent
                    return false;
                }
            }
            else
            {
                // state boundary <=  rectangle boundary
                if (difference > (state.Radius * 2) + 1)
                {
                    // X isn't overlapping or adjacent
                    return false;
                }
            }

            difference = rectangle.Top - (state.Position.Y - state.Radius);
            if (difference < 0)
            {
                // Negative means rectangle boundary < state boundary
                if (-difference > rectangle.Height + 1)
                {
                    // Y isn't overlapping or adjacent
                    return false;
                }
            }
            else
            {
                // state boundary <=  rectangle boundary
                if (difference > (state.Radius * 2) + 1)
                {
                    // Y isn't overlapping or adjacent
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///  The current movement vector for this zone.
        /// </summary>
        public MovementVector Vector 
        {
            get
            {
                return vector;
            }
        }

        /// <summary>
        ///  Get a new teleport zone that is initialized with a different
        ///  movement vector.
        /// </summary>
        /// <param name="vector">The new movement vector.</param>
        /// <returns>The update teleport zone.</returns>
        public TeleportZone SetVector(MovementVector vector)
        {
            TeleportZone zone = Clone();
            zone.vector = vector;
            return zone;
        }
    }
}