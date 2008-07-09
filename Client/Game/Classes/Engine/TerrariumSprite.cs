//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using OrganismBase;

namespace Terrarium.Renderer 
{
    /// <summary>
    ///  An object used by the Graphics Engine to render a creature.
    /// </summary>
    public class TerrariumSprite
    {
        // Animation information
        private float curFrame = 0;
        private float curFrameDelta = 0;
        private string spriteKey;
        private string skinFamily;

        // Movement information
        private float xPos = 0;
        private float yPos = 0;
        private float xDelta = 0;
        private float yDelta = 0;

        // Selection information
        private bool selected = false;

        // Reset information
        private DisplayAction prevAction;
        private bool isPlant;

        /// <summary>
        ///  Creates a new TerrariumSprite object.
        /// </summary>
        public TerrariumSprite()
        {
        }

        /// <summary>
        ///  Controls if the rendered object is a plant or animal.
        /// </summary>
        public bool IsPlant
        {
            get
            {
                return isPlant;
            }
            set
            {
                isPlant = value;
            }
        }
    
        /// <summary>
        ///  Controls movement and frame advancement for the sprite.
        /// </summary>
        public void AdvanceFrame()
        {
            if (curFrame == 0)
            {
                // First run so we are in the right spot
            }
            else
            {
                xPos += xDelta;
                yPos += yDelta;
            }
            curFrame = (curFrame + curFrameDelta) % 10;
        }

        /// <summary>
        ///  Returns the frame height of the sprite.  This value isn't really
        ///  required and so is set at a default of 48.
        /// </summary>
        public int FrameHeight
        {
            get
            {
                return 48;
            }
        }

        /// <summary>
        ///  Returns the frame width of the sprite.  This value isn't really
        ///  required and so is set at a default of 48.
        /// </summary>
        public int FrameWidth
        {
            get
            {
                return 48;
            }
        }

        /// <summary>
        ///  The skin family for this sprite.  Used to determine which graphics
        ///  to display.
        /// </summary>
        public string SkinFamily
        {
            get
            {
                return skinFamily;
            }
            set
            {
                skinFamily = value;
            }
        }

        /// <summary>
        ///  The current frame for this sprite.  Used to animate any animated
        ///  sprites.
        /// </summary>
        public float CurFrame
        {
            get
            {
                return curFrame;
            }
            set
            {
                curFrame = value;
            }
        }

        /// <summary>
        ///  The current frame delta for this sprite.  This is used to speed
        ///  up or slow down animations by changing the amount of frames issued
        ///  per AdvanceFrame method called.
        /// </summary>
        public float CurFrameDelta
        {
            get
            {
                return curFrameDelta;
            }
            set
            {
                curFrameDelta = value;
            }
        }

        /// <summary>
        ///  The current X position within the game world.
        /// </summary>
        public float XPosition
        {
            get
            {
                return xPos;
            }
            set
            {
                xPos = value;
            }
        }

        /// <summary>
        ///  The current Y position within the game world.
        /// </summary>
        public float YPosition
        {
            get
            {
                return yPos;
            }
            set
            {
                yPos = value;
            }
        }

        /// <summary>
        ///  The current amount of X to add to the position
        ///  per call to AdvanceFrame.
        /// </summary>
        public float XDelta
        {
            get
            {
                return xDelta;
            }
            set
            {
                xDelta = value;
            }
        }

        /// <summary>
        ///  The current amount of Y to add to the position
        ///  per call to AdvanceFrame.
        /// </summary>
        public float YDelta
        {
            get
            {
                return yDelta;
            }
            set
            {
                yDelta = value;
            }
        }

        /// <summary>
        ///  Determines if the current sprite is selected.
        /// </summary>
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
            }
        }
    
        /// <summary>
        ///  The current frame type to display.
        /// </summary>
        public DisplayAction PreviousAction
        {
            get
            {
                return prevAction;
            }
            set
            {
                prevAction = value;
            }
        }

        /// <summary>
        ///  The key used to index sprite sheets.
        /// </summary>
        public string SpriteKey
        {
            get
            {
                return spriteKey;
            }
            set
            {
                spriteKey = value;
            }
        }
    }
}