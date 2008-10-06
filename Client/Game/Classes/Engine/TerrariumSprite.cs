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
        private float _curFrame;
        private float _curFrameDelta;
        private bool _isPlant;
        private DisplayAction _prevAction;
        private bool _selected;
        private string _skinFamily;
        private string _spriteKey;
        private float _xDelta;
        private float _xPos;
        private float _yDelta;
        private float _yPos;

        /// <summary>
        ///  Controls if the rendered object is a plant or animal.
        /// </summary>
        public bool IsPlant
        {
            get { return _isPlant; }
            set { _isPlant = value; }
        }

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
        public string SkinFamily
        {
            get { return _skinFamily; }
            set { _skinFamily = value; }
        }

        /// <summary>
        ///  The current frame for this sprite.  Used to animate any animated
        ///  sprites.
        /// </summary>
        public float CurFrame
        {
            get { return _curFrame; }
            set { _curFrame = value; }
        }

        /// <summary>
        ///  The current frame delta for this sprite.  This is used to speed
        ///  up or slow down animations by changing the amount of frames issued
        ///  per AdvanceFrame method called.
        /// </summary>
        public float CurFrameDelta
        {
            get { return _curFrameDelta; }
            set { _curFrameDelta = value; }
        }

        /// <summary>
        ///  The current X position within the game world.
        /// </summary>
        public float XPosition
        {
            get { return _xPos; }
            set { _xPos = value; }
        }

        /// <summary>
        ///  The current Y position within the game world.
        /// </summary>
        public float YPosition
        {
            get { return _yPos; }
            set { _yPos = value; }
        }

        /// <summary>
        ///  The current amount of X to add to the position
        ///  per call to AdvanceFrame.
        /// </summary>
        public float XDelta
        {
            get { return _xDelta; }
            set { _xDelta = value; }
        }

        /// <summary>
        ///  The current amount of Y to add to the position
        ///  per call to AdvanceFrame.
        /// </summary>
        public float YDelta
        {
            get { return _yDelta; }
            set { _yDelta = value; }
        }

        /// <summary>
        ///  Determines if the current sprite is selected.
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        /// <summary>
        ///  The current frame type to display.
        /// </summary>
        public DisplayAction PreviousAction
        {
            get { return _prevAction; }
            set { _prevAction = value; }
        }

        /// <summary>
        ///  The key used to index sprite sheets.
        /// </summary>
        public string SpriteKey
        {
            get { return _spriteKey; }
            set { _spriteKey = value; }
        }

        /// <summary>
        ///  Controls movement and frame advancement for the sprite.
        /// </summary>
        public void AdvanceFrame()
        {
            if (_curFrame == 0)
            {
                // First run so we are in the right spot
            }
            else
            {
                _xPos += _xDelta;
                _yPos += _yDelta;
            }
            _curFrame = (_curFrame + _curFrameDelta)%10;
        }
    }
}