//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System.Windows.Forms;
using DxVBLib;

namespace Terrarium.Renderer.DirectX
{
    /// <summary>
    ///  Contains the logic required to use a PictureBox
    ///  surface as a DirectDraw rendering surface.
    /// </summary>
    public class DirectDrawPictureBox : PictureBox
    {
        /// <summary>
        ///  Creates a new DirectDrawPictureBox and performs any
        ///  initial setup.
        /// </summary>
        public DirectDrawPictureBox()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  The Clipper object assigned to the PictureBox
        /// </summary>
        protected DirectDrawClipper Clipper { get; set; }

        /// <summary>
        ///  The primary screen surface for the picture box.
        /// </summary>
        protected DirectDrawSurface ScreenSurface { get; set; }

        /// <summary>
        ///  The back buffer surface used with the picture box.
        /// </summary>
        protected DirectDrawSurface BackBufferSurface { get; set; }

        private void InitializeComponent()
        {
        }

        /// <summary>
        ///  Overrides the Painting logic because painting will be handled
        ///  using timers and DirectX.  If the control is in design mode, then
        ///  clear it because DirectX isn't available yet.
        /// </summary>
        /// <param name="e">Graphics context objects</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
            {
                e.Graphics.Clear(BackColor);
            }
        }

        /// <summary>
        ///  Don't paint the background when a background erase is requested.
        ///  Hurts performance and causes flicker.
        /// </summary>
        /// <param name="e">Graphics context objects</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
    }
}