//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using OrganismBase;

namespace Terrarium.Renderer
{
    /// <summary>
    ///  An object used by the Graphics Engine to render a creature.
    /// </summary>
    public class TerrariumSprite
    {
        /// <summary>
        ///  Controls if the rendered object is a plant or animal.
        /// </summary>
        public bool IsPlant { get; set; }

        /// <summary>
        ///  Returns the frame height of the sprite.  This value isn't really
        ///  required and so is set at a default of 48.
        /// </summary>
        public int FrameHeight
        {
            get { return 48; }
        }

        /// <summary>
        ///  Returns the frame width of the sprite.  This value isn't really
        ///  required and so is set at a default of 48.
        /// </summary>
        public int FrameWidth
        {
            get { return 48; }
        }

        /// <summary>
        ///  The skin family for this sprite.  Used to determine which graphics
        ///  to display.
        /// </summary>
        public string SkinFamily { get; set; }

        /// <summary>
        ///  The current frame for this sprite.  Used to animate any animated
        ///  sprites.
        /// </summary>
        public float CurFrame { get; set; }

        /// <summary>
        ///  The current frame delta for this sprite.  This is used to speed
        ///  up or slow down animations by changing the amount of frames issued
        ///  per AdvanceFrame method called.
        /// </summary>
        public float CurFrameDelta { get; set; }

        /// <summary>
        ///  The current X position within the game world.
        /// </summary>
        public float XPosition { get; set; }

        /// <summary>
        ///  The current Y position within the game world.
        /// </summary>
        public float YPosition { get; set; }

        /// <summary>
        ///  The current amount of X to add to the position
        ///  per call to AdvanceFrame.
        /// </summary>
        public float XDelta { get; set; }

        /// <summary>
        ///  The current amount of Y to add to the position
        ///  per call to AdvanceFrame.
        /// </summary>
        public float YDelta { get; set; }

        /// <summary>
        ///  Determines if the current sprite is selected.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        ///  The current frame type to display.
        /// </summary>
        public DisplayAction PreviousAction { get; set; }

        /// <summary>
        ///  The key used to index sprite sheets.
        /// </summary>
        public string SpriteKey { get; set; }

        /// <summary>
        ///  Controls movement and frame advancement for the sprite.
        /// </summary>
        public void AdvanceFrame()
        {
            if (CurFrame == 0)
            {
                // First run so we are in the right spot
            }
            else
            {
                XPosition += XDelta;
                YPosition += YDelta;
            }
            CurFrame = (CurFrame + CurFrameDelta)%10;
        }
    }
}