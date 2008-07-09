//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using DxVBLib;
using OrganismBase;

using Terrarium.Game;
using Terrarium.Tools;
using Terrarium.Renderer.DirectX;

namespace Terrarium.Renderer 
{
    /// <summary>
    ///  Encapsulates all of the drawing code for the Terrarium Game
    ///  View.  This class makes heavy use of the DirectDraw APIs in
    ///  order to provide high speed animation.
    /// </summary>
    public class TerrariumDirectDrawGameView : DirectDrawPictureBox
    {
        /// <summary>
        ///  Clients can connect to this event and be notified of
        ///  click events that correspond to creatures within the view.
        /// </summary>
        public event OrganismClickedEventHandler OrganismClicked;

        /// <summary>
        ///  Clients can connect to this event and be notified of
        ///  mini map changes that occur during map transitions.
        /// </summary>
        public event MiniMapUpdatedEventHandler MiniMapUpdated;

        private RECT clipRect;
        private RECT viewsize;
        private RECT actualsize;

        // Cursor
        private Point cursorPos;
        private int cursor;
        private bool enabledCursor;
        private bool drawCursor = true;
		private bool paused;


        // Scrolling
        private int scrollUp;
        private int scrollDown;
        private int scrollLeft;
        private int scrollRight;

        private TerrariumSpriteSurfaceManager tsm = new TerrariumSpriteSurfaceManager();
        private TerrariumTextSurfaceManager tfm = new TerrariumTextSurfaceManager();
        private World wld = null;

        // MiniMap Control
        private Bitmap miniMap = null;
        private int updateTicker = 0;
        private bool updateTickerChanged = false;
        private bool updateMiniMap = false;

        private DirectDrawSurface backgroundSurface;
        private DirectDrawSurface stagingSurface;

        private Int64 renderTime;
        private Int32 samples;

        private bool doubleBuffer = true;
        private bool drawing = false;
        private bool fullscreen = false;
        private bool viewchanged = true;
        private bool paintPlants = false;
        private bool videomemory = true;
        private bool skipframe = true;
        private bool drawBoundingBox = false;
        private bool drawDestinationLines = false;
        private bool drawBackgroundGrid = false;
        private bool drawScreen = true;
		private bool resizing = false;

        private bool drawtext = true;
        private string textMessage = null;

        private Hashtable hackTable = new Hashtable();
        private WorldVector wv;
#if TRACE
        private Profiler tddGameViewProf;

        /// <summary>
        ///  Provides access to the game profiler.
        /// </summary>
        public Profiler Profiler
        {
            get
            {
                if (tddGameViewProf == null)
                {
                    tddGameViewProf = new Profiler();
                }

                return tddGameViewProf;
            }
        }
#endif

        /// <summary>
        ///  Creates a new instance of the game view and initializes any properties.
        /// </summary>
        public TerrariumDirectDrawGameView() : base()
        {
            InitializeComponent();
        }
        
        /// <summary>
        ///  Initialize Component
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        ///  Returns the amount of time required to render a scene.
        /// </summary>
        public Int64 RenderTime
        {
            get
            {
                return renderTime;
            }
        }

        /// <summary>
        ///  Returns the number of samples (frames) obtained.
        /// </summary>
        public Int64 Samples
        {
            get
            {
                return samples;
            }
        }

        /// <summary>
        ///  Clear the profiler
        /// </summary>
        public void ClearProfiler()
        {
            renderTime = 0;
            samples = 0;
        }

        /// <summary>
        ///  Pauses the TerrariumGameView and stops rendering.  A call to 
        ///  RenderFrame will automatically unpause the animation.
        /// </summary>
        public bool Paused
        {
            set
            {
                paused = value;
            }
        }

        /// <summary>
        ///  Overrides OnMouseMove to enable custom cursor rendering.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.cursorPos = new Point(e.X, e.Y);
        }

		private Rectangle ComputeCursorRectangle()
		{
			int left = 0;
			int top = 0;
			int right = this.Width;
			int bottom = this.Height;

			if ( bltUsingBezel )
			{
				if ( clipRect.Right == actualsize.Right )
				{
					left = (right - clipRect.Right) / 2;
					right = left + clipRect.Right;
				}
				if ( clipRect.Bottom == actualsize.Bottom )
				{
					top = (bottom - clipRect.Bottom) / 2;
					bottom = top + clipRect.Bottom;
				}
			}

			return Rectangle.FromLTRB(left, top, right, bottom);
		}


        /// <summary>
        ///  Computes whether scrolling should occur based on mouse location and
        ///  hovering.
        /// </summary>
        private void CheckScroll()
        {
            if (!enabledCursor)
            {
                scrollUp = 0;
                scrollDown = 0;
                scrollLeft = 0;
                scrollRight = 0;
                return;
            }

			Rectangle cursorRect = ComputeCursorRectangle();
			int left = cursorRect.Left;
			int top = cursorRect.Top;
			int right = cursorRect.Right;
			int bottom = cursorRect.Bottom;

            if (cursorPos.Y <= bottom && cursorPos.Y > (bottom - 30))
            {
                scrollDown++;
                if (scrollDown > 4)
                {
                    ScrollDown(15);
                }
            }
            else
            {
                scrollDown = 0;
            }

            if (cursorPos.Y >= top && cursorPos.Y < (top + 30))
            {
                scrollUp++;
                if (scrollUp > 4)
                {
                    ScrollUp(15);
                }
            }
            else
            {
                scrollUp = 0;
            }

            if (cursorPos.X <= right && cursorPos.X > (right - 30))
            {
                scrollRight++;
                if (scrollRight > 4)
                {
                    ScrollRight(15);
                }
            }
            else
            {
                scrollRight = 0;
            }

            if (cursorPos.X >= left && cursorPos.X < (left + 30))
            {
                scrollLeft++;
                if (scrollLeft > 4)
                {
                    ScrollLeft(15);
                }
            }
            else
            {
                scrollLeft = 0;
            }

            if (scrollUp == 0 && scrollDown == 0 && scrollLeft == 0 && scrollRight == 0)
            {
                cursor = 8;
            }
            else if (scrollUp > 0 && scrollDown == 0 && scrollLeft == 0 && scrollRight == 0)
            {
                cursor = 0;
            }
            else if (scrollUp == 0 && scrollDown > 0 && scrollLeft == 0 && scrollRight == 0)
            {
                cursor = 4;
            }
            else if (scrollUp == 0 && scrollDown == 0 && scrollLeft > 0 && scrollRight == 0)
            {
                cursor = 6;
            }
            else if (scrollUp == 0 && scrollDown == 0 && scrollLeft == 0 && scrollRight > 0)
            {
                cursor = 2;
            }
            else if (scrollUp > 0 && scrollDown == 0 && scrollLeft > 0 && scrollRight == 0)
            {
                cursor = 7;
            }
            else if (scrollUp > 0 && scrollDown == 0 && scrollLeft == 0 && scrollRight > 0)
            {
                cursor = 1;
            }
            else if (scrollUp == 0 && scrollDown > 0 && scrollLeft == 0 && scrollRight > 0)
            {
                cursor = 3;
            }
            else if (scrollUp == 0 && scrollDown > 0 && scrollLeft > 0 && scrollRight == 0)
            {
                cursor = 5;
            }
            else
            {
                cursor = 8;
            }
        }

        /// <summary>
        ///  Overrides the OnMouseLeave event to provide custom cursor manipulation
        /// </summary>
        /// <param name="e">Null</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            this.enabledCursor = false;
        }

        /// <summary>
        ///  Overrides the OnMouseEnter event to provide custom cursor manipulation
        /// </summary>
        /// <param name="e">Null</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            this.enabledCursor = true;
        }

        /// <summary>
        ///  Provides access to the bitmap representing the minimap for the
        ///  currently loaded world.
        /// </summary>
        public Bitmap MiniMap
        {
            get
            {
                if (this.miniMap == null)
                {
                    if (wld != null)
                    {
                        //this.miniMap = new Bitmap(wld.MiniMap);
						this.miniMap = new Bitmap(wld.MiniMap, (actualsize.Right - actualsize.Left) / 4, (actualsize.Bottom - actualsize.Top) / 4);
						Graphics graphics = Graphics.FromImage(this.miniMap);
						graphics.Clear(Color.Transparent);
						graphics.Dispose();
					}
                    else
                    {
                        return null;
                    }
                }

                return this.miniMap;
            }
        }

        /// <summary>
        ///  Enables the textual display of a message over top of
        ///  the Terrarium view.  This property accepts newlines and
        ///  centers the text on the screen.
        /// </summary>
        public string TerrariumMessage
        {
            get
            {
                return textMessage;
            }
            set
            {
                textMessage = value;
            }
        }

        /// <summary>
        ///  Enables the drawing of organism destination lines
        ///  within the Terrarium Client
        /// </summary>
        public bool DrawDestinationLines
        {
            get
            {
                return drawDestinationLines;
            }
            set
            {
                drawDestinationLines = value;
            }
        }
    
        /// <summary>
        ///  Controls the rendering of the entire game view.
        /// </summary>
        public bool DrawScreen
        {
            get
            {
                return drawScreen;
            }
            set
            {
                drawScreen = value;
            }
        }
    
        /// <summary>
        ///  Controls the rendering of organism bounding boxes useful
        ///  for movement debugging.
        /// </summary>
        public bool DrawBoundingBox
        {
            get
            {
                return drawBoundingBox;
            }
            set
            {
                drawBoundingBox = value;
            }
        }
    
        /// <summary>
        ///  Controls rendering of a special background that contains
        ///  a grid overlay that mimics the Terrarium application's
        ///  cell grid.
        /// </summary>
        public bool DrawBackgroundGrid
        {
            get
            {
                return drawBackgroundGrid;
            }
            set
            {
                viewchanged = true;
                drawBackgroundGrid = value;
				this.AddBackgroundSlide();
            }
        }

        /// <summary>
        ///  Overrides the cursor property so that
        ///  custom cursors can be implement.
        /// </summary>
        public override Cursor Cursor
        {
            get
            {
                if (paused || !drawScreen)
                {
                    return Cursors.Default;
                }
				
				if (bltUsingBezel)
				{
					if ( !ComputeCursorRectangle().Contains(cursorPos) )
					{
						return Cursors.Default;
					}
				}

				return null;
            }
        }

        /// <summary>
        ///  Initializes DirectDraw rendering APIs.  The function can be called to initialize
        ///  both Windowed and FullScreen mode, but FullScreen mode isn't fully
        ///  implemented.
        /// </summary>
        /// <param name="fullscreen">If true then fullscreen will be enabled.</param>
        /// <returns>True if DirectDraw is initialized.</returns>
        public bool InitializeDirectDraw(bool fullscreen)
        {
            try
            {
                this.fullscreen = fullscreen;

                if (fullscreen)
                {
                    ManagedDirectX.DirectDraw.SetCooperativeLevel(
                        this.Parent.Handle.ToInt32(),
                        CONST_DDSCLFLAGS.DDSCL_FULLSCREEN |
                        CONST_DDSCLFLAGS.DDSCL_EXCLUSIVE |
                        CONST_DDSCLFLAGS.DDSCL_ALLOWREBOOT);

                    ManagedDirectX.DirectDraw.SetDisplayMode(640, 480, 16, 0, 0);
                    CreateFullScreenSurfaces();
                }
                else
                {
                    this.Parent.Show();
                    ManagedDirectX.DirectDraw.SetCooperativeLevel(
                        this.Parent.Handle.ToInt32(), 
                        CONST_DDSCLFLAGS.DDSCL_NORMAL);
                    CreateWindowedSurfaces();
                }

                return true;
            }
            catch (DirectXException de)
            {
                ErrorLog.LogHandledException(de);
            }
			return false;
        }
    
        /// <summary>
        ///  Method used to create the necessary surfaces required for windowed
        ///  mode.
        /// </summary>
        /// <returns>True if the surfaces are created, otherwise false.</returns>
        public bool CreateWindowedSurfaces()
        {
            DDSURFACEDESC2 tempDescr = new DDSURFACEDESC2();
            tempDescr.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS;
            tempDescr.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_PRIMARYSURFACE;
            ScreenSurface = new DirectDrawSurface(tempDescr);

            Clipper = ManagedDirectX.DirectDraw.CreateClipper(0);
            Clipper.SetHWnd(this.Handle.ToInt32());
            ScreenSurface.Surface.SetClipper(Clipper);
            Trace.WriteLine("Primary Surface InVideo? " + ScreenSurface.InVideo);

            if (ScreenSurface != null)
            {
				int workSurfaceWidth = Math.Min(this.Width, actualsize.Right);
				int workSurfaceHeight = Math.Min(this.Height, actualsize.Bottom);

                tempDescr = new DDSURFACEDESC2();
                tempDescr.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS | 
                    CONST_DDSURFACEDESCFLAGS.DDSD_HEIGHT | 
                    CONST_DDSURFACEDESCFLAGS.DDSD_WIDTH;
                tempDescr.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN;
                tempDescr.lWidth = workSurfaceWidth;
                tempDescr.lHeight = workSurfaceHeight;
                BackBufferSurface = new DirectDrawSurface(tempDescr);
                Trace.WriteLine("Back Buffer Surface InVideo? " + BackBufferSurface.InVideo);

                tempDescr = new DDSURFACEDESC2();
                tempDescr.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS | 
                    CONST_DDSURFACEDESCFLAGS.DDSD_HEIGHT | 
                    CONST_DDSURFACEDESCFLAGS.DDSD_WIDTH;
                tempDescr.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN;
                tempDescr.lWidth = workSurfaceWidth;
                tempDescr.lHeight = workSurfaceHeight;
                backgroundSurface = new DirectDrawSurface(tempDescr);
                Trace.WriteLine("Background Surface InVideo? " + backgroundSurface.InVideo);

                tempDescr = new DDSURFACEDESC2();
                tempDescr.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS | 
                    CONST_DDSURFACEDESCFLAGS.DDSD_HEIGHT | 
                    CONST_DDSURFACEDESCFLAGS.DDSD_WIDTH;
                tempDescr.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN;
                tempDescr.lWidth = workSurfaceWidth;
                tempDescr.lHeight = workSurfaceHeight;
                stagingSurface = new DirectDrawSurface(tempDescr);
                Trace.WriteLine("Staging Surface InVideo? " + stagingSurface.InVideo);
            }

            if (!ScreenSurface.InVideo || !BackBufferSurface.InVideo || !backgroundSurface.InVideo || !stagingSurface.InVideo)
            {
                videomemory = false;
                drawtext = true;  // For now, turn this to false for a perf increase on slower machines
            }

            ResetTerrarium();
            ClearBackground();
        
            return true;
        }
    
        /// <summary>
        ///  Creates the surfaces required for full screen operation.
        /// </summary>
        /// <returns>True if the surfaces are initialized, false otherwise.</returns>
        public bool CreateFullScreenSurfaces()
        {
            if (doubleBuffer)
            {
                DDSURFACEDESC2 tempDescr = new DDSURFACEDESC2();
                tempDescr.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS;
                tempDescr.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_PRIMARYSURFACE;
                ScreenSurface = new DirectDrawSurface(tempDescr);

                if (ScreenSurface != null)
                {
                    tempDescr.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS | CONST_DDSURFACEDESCFLAGS.DDSD_HEIGHT | CONST_DDSURFACEDESCFLAGS.DDSD_WIDTH;
                    tempDescr.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN;
                    tempDescr.lWidth = 640;
                    tempDescr.lHeight = 480;
                    BackBufferSurface = new DirectDrawSurface(tempDescr);
                }
            
                ClearBackground();
            }
            else
            {
                DDSURFACEDESC2 tempDescr = new DDSURFACEDESC2();
                tempDescr.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS | CONST_DDSURFACEDESCFLAGS.DDSD_BACKBUFFERCOUNT;
                tempDescr.lBackBufferCount = 1;
                tempDescr.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_PRIMARYSURFACE |
                    CONST_DDSURFACECAPSFLAGS.DDSCAPS_COMPLEX |
                    CONST_DDSURFACECAPSFLAGS.DDSCAPS_FLIP;
                ScreenSurface = new DirectDrawSurface(tempDescr);

                if (ScreenSurface != null)
                {
                    tempDescr.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_BACKBUFFER;
                    BackBufferSurface = new DirectDrawSurface(ScreenSurface.Surface.GetAttachedSurface(ref tempDescr.ddsCaps));
                }
            }

            ResetTerrarium();

            return true;
        }

        /// <summary>
        ///  Overrides OnMouseUp in order to enable creature selection.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        
            if (e.Button == MouseButtons.Right)
            {
                return;
            }
        
            bool selectThisAnimalOnly = true;
            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                selectThisAnimalOnly = false;
            }

            SelectAnimalFromPoint(new Point(e.X + viewsize.Left, e.Y + viewsize.Top), selectThisAnimalOnly);
        }

        /// <summary>
        ///  Attempts to select a creature given the world offset point.
        /// </summary>
        /// <param name="p">Point to check for creature intersection.</param>
        /// <param name="selectThisAnimalOnly">Determines if this creature should be added to the selection</param>
        public void SelectAnimalFromPoint(Point p, bool selectThisAnimalOnly)
        {
            if (wv != null)
            {
                foreach (OrganismState orgState in wv.State.Organisms)
                {
                    if (orgState.RenderInfo != null)
                    {
                        TerrariumSprite tsSprite = (TerrariumSprite) orgState.RenderInfo;
                        int radius = tsSprite.FrameWidth;
                        Rectangle rec = new Rectangle((int) tsSprite.XPosition - (tsSprite.FrameWidth>>1), (int) tsSprite.YPosition - (tsSprite.FrameWidth>>1), tsSprite.FrameWidth, tsSprite.FrameWidth);
                        if (rec.Contains(p))
                        {
                            tsSprite.Selected = !tsSprite.Selected;
                            OnOrganismClicked(new OrganismClickedEventArgs(orgState));
                        }
                        else if (selectThisAnimalOnly && tsSprite.Selected)
                        {
                            tsSprite.Selected = false;                      
                            OnOrganismClicked(new OrganismClickedEventArgs(orgState));
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  Helper function for firing the MiniMapUpdated event
        ///  whenever a new mini-map becomes available.
        /// </summary>
        /// <param name="e">Mini-map event arguments containing the newest map.</param>
        private void OnMiniMapUpdated(MiniMapUpdatedEventArgs e)
        {
            if (MiniMapUpdated != null)
            {
                MiniMapUpdated(this, e);
            }
        }

        /// <summary>
        ///  Helper function for firing the OrganismClicked event
        ///  whenever an organism is selected within the game
        ///  view.
        /// </summary>
        /// <param name="e">Event arguments detailing which organism was clicked.</param>
        private void OnOrganismClicked(OrganismClickedEventArgs e)
        {
            if (OrganismClicked != null)
            { 
                OrganismClicked(this, e);
            }
        }

        /// <summary>
        ///  Used to change the background slide of the Terrarium.
        ///  Only a single background slide can be added with this
        ///  revision.
        /// </summary>
        public void AddBackgroundSlide()
        {
            tsm.Remove("background");
            tsm.Add("background", 1, 1);

			if ( this.DrawBackgroundGrid == true )
			{
				DirectDrawSpriteSurface workSurface = tsm["background"].GetDefaultSurface();
				DirectDrawSurface7 surface = workSurface.SpriteSurface.Surface;

				for( int h = 0; h < workSurface.SpriteSurface.Rect.Bottom; h += 8 )
				{
					surface.SetForeColor( Color.Gray.ToArgb() );
					surface.SetFillColor( Color.Gray.ToArgb() );
					surface.DrawLine( 0, h, workSurface.SpriteSurface.Rect.Right, h );
					surface.DrawLine( 0, h-1, workSurface.SpriteSurface.Rect.Right, h-1 );
				}
				for( int w = 0; w < workSurface.SpriteSurface.Rect.Right; w += 8 )
				{
					surface.SetForeColor( Color.Gray.ToArgb() );
					surface.SetFillColor( Color.Gray.ToArgb() );
					surface.DrawLine( w, 0, w, workSurface.SpriteSurface.Rect.Bottom );
					surface.DrawLine( w-1, 0, w-1, workSurface.SpriteSurface.Rect.Bottom );
				}
			}
        }

        /// <summary>
        ///  Adds a generic 10frame by 40frame sprite surface that
        ///  is compatible with creature animation.
        /// </summary>
        /// <param name="name">The name of the sprite sheet.</param>
        public void AddSpriteSurface(string name)
        {
            tsm.Remove(name);
            tsm.Add(name, 10, 40);
        }

        /// <summary>
        ///  Add a complex sprite surface given the number of frames.
        /// </summary>
        /// <param name="name">The name of the sprite sheet.</param>
        /// <param name="xFrames">The number of frames width-wise.</param>
        /// <param name="yFrames">The number of frames height-wise.</param>
        public void AddComplexSpriteSurface(string name, int xFrames, int yFrames)
        {
            tsm.Remove(name);
            tsm.Add(name, xFrames, yFrames);
        }

        /// <summary>
        ///  Add a complex sprite surface that takes advantage of sized sprites
        /// </summary>
        /// <param name="name">The name of the sprite sheet.</param>
        /// <param name="xFrames">The number of frames width wise.</param>
        /// <param name="yFrames">The number of frames height wise.</param>
        public void AddComplexSizedSpriteSurface(string name, int xFrames, int yFrames)
        {
            tsm.Remove(name);
            tsm.AddSizedSurface(name, xFrames, yFrames);
        }

        /// <summary>
        ///  Tell the game view to create a new world and reset the rendering
        ///  surfaces.
        /// </summary>
        /// <param name="xPixels">The number of world pixels</param>
        /// <param name="yPixels">The number of world pixels</param>
        /// <returns></returns>
        public RECT CreateWorld(int xPixels, int yPixels)
        {
            wld = new World();
            actualsize = wld.CreateWorld(xPixels, yPixels);

            ResetTerrarium();
            this.viewchanged = true;
            return actualsize;
        }

        /// <summary>
        ///  Determines if sprite labels should be drawn.
        /// </summary>
        public bool DrawText
        {
            get
            {
                return drawtext;
            }
            set
            {
                drawtext = value;
            }
        }

        /// <summary>
        ///  Determines if the primary surfaces are in video memory
        ///  or not.
        /// </summary>
        public bool VideoMemory
        {
            get
            {
                return videomemory;
            }
        }

        /// <summary>
        ///  Reinitialize all surfaces after they have been lost.
        ///  This method invokes the garbage collector to make sure
        ///  that any COM references have been cleaned up, else surface
        ///  renewal won't work correctly.
        /// </summary>
        private void ReInitSurfaces()
        {
            ScreenSurface = null;
            BackBufferSurface = null;
            backgroundSurface = null;
            stagingSurface = null;

            tsm.Clear();
            tfm.Clear();
        
            GC.Collect(2);
            GC.WaitForPendingFinalizers();
        
            if (fullscreen)
            {
                CreateFullScreenSurfaces();
            }
            else
            {
                CreateWindowedSurfaces();
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			//this.ResizeViewer();
			base.OnResize(e);
		}

		/// <summary>
		/// Handles logic for resizing the game view
		/// </summary>
		public bool ResizeViewer()
		{
            try
            {
                this.resizing = true;
                this.viewchanged = true;

                // Reset and re-initialize all surfaces to the new size
                this.ResetTerrarium();
                this.ReInitSurfaces();

                // Add default surfaces
                this.AddBackgroundSlide();
                this.AddComplexSpriteSurface("cursor", 1, 9);
                this.AddComplexSpriteSurface("teleporter", 16, 1);
                this.AddComplexSizedSpriteSurface("plant", 1, 1);

                // Mark viewport as having changed.  This forces us to redraw
                // background surfaces that we have cached
                this.viewchanged = true;

                // Re-center the screen
                int centerX = viewsize.Left + ((viewsize.Right - viewsize.Left) / 2);
                int centerY = viewsize.Top + ((viewsize.Bottom - viewsize.Top) / 2);
                this.CenterTo(centerX, centerY);

                this.resizing = false;

                return true;
            }
            catch (Exception e)
            {
                ErrorLog.LogHandledException(e);
                this.resizing = false;
                return false;
            }
		}

		private bool bltUsingBezel = false;
		private RECT bezelRect = new RECT();
		/// <summary>
        ///  Renders a new frame of animation.  This is the entry point for drawing
        ///  code and is required every time a new frame is to be drawn.
        /// </summary>
        public void RenderFrame()
        {
#if TRACE
            this.Profiler.Start("TerrariumDirectDrawGameView.RenderFrame()");
#else
			TimeMonitor tm = new TimeMonitor();
			tm.Start();
#endif
			// Don't draw while we are resizing the viewer
			// This might happen on a secondary thread so
			// we need some minimal protection.
			if ( this.resizing )
			{
				return;
			}

            // If we are still drawing then skip this frame.
            // However, only skip one frame to prevent hangs.
            if ( this.drawing )
            {
                this.drawing = false;
                return;
            }

			try
			{
				paused = false;
				this.drawing = true;
				this.skipframe = !this.skipframe;
				if (fullscreen)
				{
					// No full screen mode
				}
				else
				{
					if (ScreenSurface == null || ScreenSurface.Surface.isLost() != 0 || 
						BackBufferSurface == null || BackBufferSurface.Surface.isLost() != 0)
					{
                        if (ResizeViewer() == false)
                        {
                            return;
                        }
					}

					if (updateTickerChanged && (updateTicker % 10) == 0)
					{
						updateTickerChanged = false;
						updateMiniMap = true;
						if (wld != null && this.miniMap == null)
						{
							//this.miniMap = new Bitmap(wld.MiniMap);
							this.miniMap = new Bitmap(wld.MiniMap, (actualsize.Right - actualsize.Left)/4, (actualsize.Bottom - actualsize.Top)/4);
						}
						if (miniMap != null)
						{
							Graphics graphics = Graphics.FromImage(this.miniMap);
							graphics.Clear(Color.Transparent);
							graphics.Dispose();
						}
					}

					CheckScroll();
#if TRACE
					this.Profiler.Start("TerrariumDirectDrawGameView.RenderFrame()::Primary Surface Blit");
#endif
					if (drawScreen)
					{
						if (videomemory || !skipframe)
						{
							PaintBackground();
							PaintSprites(BackBufferSurface, false);
							PaintMessage();
							PaintCursor();

							// Grab the Window RECT
							RECT windowRect = new RECT();
							ManagedDirectX.DirectX.GetWindowRect(
								this.Handle.ToInt32(),
								ref windowRect);

							// Set up a sample destination rectangle
							RECT destRect = new RECT();
							destRect.Left = windowRect.Left;
							destRect.Top = windowRect.Top;
							destRect.Right = windowRect.Right;
							destRect.Bottom = windowRect.Bottom;

							// Grab the Source Rectangle for the Bezel
							RECT srcRect = BackBufferSurface.Rect;
							int destWidth = destRect.Right - destRect.Left;
							int destHeight = destRect.Bottom - destRect.Top;
							int srcWidth = srcRect.Right;
							int srcHeight = srcRect.Bottom;

							bltUsingBezel = false;
							if ( srcWidth < destWidth ) {
								destRect.Left += (destWidth - srcWidth) / 2;
								destRect.Right = destRect.Left + srcWidth;
								bltUsingBezel = true;
							}
							if ( srcHeight < destHeight ) {
								destRect.Top += (destHeight - srcHeight) / 2;
								destRect.Bottom = destRect.Top + srcHeight;
								bltUsingBezel = true;
							}

							if ( bltUsingBezel ) {
								if ( destRect.Top != bezelRect.Top ||
									 destRect.Left != bezelRect.Left ||
									 destRect.Bottom != bezelRect.Bottom ||
									 destRect.Right != bezelRect.Right ) {
									ScreenSurface.Surface.BltColorFill(ref windowRect, 0);
								}
								bezelRect = destRect;
							}

							ScreenSurface.Surface.Blt(
								ref destRect, 
								BackBufferSurface.Surface, 
								ref srcRect, 
								CONST_DDBLTFLAGS.DDBLT_WAIT);
						}
					}
#if TRACE
					this.Profiler.End("TerrariumDirectDrawGameView.RenderFrame()::Primary Surface Blit");
#endif
					if (updateMiniMap)
					{
						updateMiniMap = false;
						OnMiniMapUpdated(new MiniMapUpdatedEventArgs(this.miniMap));
					}
				}
			}
			finally
			{
				this.drawing = false;
			}
#if TRACE
            this.Profiler.End("TerrariumDirectDrawGameView.RenderFrame()");
#else
			renderTime += tm.EndGetMicroseconds();
			samples++;
#endif
        }

        /// <summary>
        ///  ZIndexes the teleporters so they can be properly rendered
        ///  on the screen.
        /// </summary>
        /// <returns>The z-indices for the teleporters</returns>
        private int[] TeleporterZIndex()
        {
            int[] zIndices = new int[hackTable.Count];
            int index = 0;

            IDictionaryEnumerator spriteList = hackTable.GetEnumerator();
            while (spriteList.MoveNext())
            {
                TerrariumSprite tsSprite = (TerrariumSprite) spriteList.Value;
                tsSprite.AdvanceFrame();
                zIndices[index++] = (int) tsSprite.YPosition;
            }

            return zIndices;
        }

        /// <summary>
        ///  Renders any teleporters that exist between the given z
        ///  ordered locations.
        /// </summary>
        /// <param name="lowZ">The minimum z index.</param>
        /// <param name="highZ">The maximum z index.</param>
        private void RenderTeleporter(int lowZ, int highZ)
        {
            IDictionaryEnumerator spriteList = hackTable.GetEnumerator();
            DirectDrawSpriteSurface workSurface = tsm["teleporter"].GetDefaultSurface();
            if (workSurface != null)
            {
                while (spriteList.MoveNext())
                {
                    TerrariumSprite tsSprite = (TerrariumSprite) spriteList.Value;

                    if (!videomemory && skipframe)
                    {
                        continue;
                    }

                    if (tsSprite.SpriteKey != "teleporter")
                    {
                        continue;
                    }

                    if (tsSprite.YPosition <= lowZ || tsSprite.YPosition > highZ)
                    {
                        continue;
                    }

                    int radius = 48;  // This is actually diameter.  True diameter is 50
                    // but the size of the sprites are 48.
                    RECT dest    = new RECT();
                    dest.Top     = (int) tsSprite.YPosition - viewsize.Top;
                    dest.Bottom  = dest.Top + radius;
                    dest.Left    = (int) tsSprite.XPosition - viewsize.Left;
                    dest.Right   = dest.Left + radius;

                    if (updateMiniMap)
                    {
                        int miniMapX = (int) (tsSprite.XPosition * miniMap.Width) / actualsize.Right; miniMapX = (miniMapX > (miniMap.Width - 1)) ? (miniMap.Width - 1) : miniMapX;
                        int miniMapY = (int) (tsSprite.YPosition * miniMap.Height) / actualsize.Bottom; miniMapY = (miniMapY > (miniMap.Height - 1)) ? (miniMap.Height - 1) : miniMapY;
                        //this.miniMap.SetPixel(miniMapX, miniMapY, Color.Blue);
						Graphics miniMapGraphics = Graphics.FromImage(this.miniMap);
						miniMapGraphics.FillRectangle(Brushes.SkyBlue, miniMapX, miniMapY, 12, 12);
						miniMapGraphics.Dispose();
					}

                    DirectDrawClippedRect ddClipRect;
                    ddClipRect = workSurface.GrabSprite((int) tsSprite.CurFrame, 0, dest, clipRect);
                    if (!ddClipRect.Invisible)
                    {
                        BackBufferSurface.Surface.BltFast(ddClipRect.Destination.Left, ddClipRect.Destination.Top, workSurface.SpriteSurface.Surface, ref ddClipRect.Source, CONST_DDBLTFASTFLAGS.DDBLTFAST_SRCCOLORKEY);
                    }
                }
            }
        }
    
        /// <summary>
        ///  Controls the rendering of textual messages to the Terrarium
        ///  client screen.  Since DrawText is invoked each time, this method
        ///  is slow.
        /// </summary>
        private void PaintMessage()
        {
            if (textMessage == null)
            {
                return;
            }
        
            string[] lines = textMessage.Split('\n');
            int yOffset = (clipRect.Bottom - ((lines.Length - 1) * 12)) / 2;

			IntPtr dcHandle = new IntPtr(BackBufferSurface.Surface.GetDC());
			
			System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHdc( dcHandle );
			
			System.Drawing.Font font = new System.Drawing.Font( "Verdana", 6.75f, System.Drawing.FontStyle.Bold );			

			Rectangle rectangle = new Rectangle( 4, 4, this.ClientRectangle.Width - 8, this.ClientRectangle.Height - 8 );

			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Near;
			stringFormat.FormatFlags = StringFormatFlags.NoClip;
			stringFormat.LineAlignment = StringAlignment.Near;
			stringFormat.Trimming = StringTrimming.EllipsisCharacter;

			graphics.DrawString( textMessage, font, System.Drawing.Brushes.Black, rectangle, stringFormat );
			rectangle.Offset( -1, -1 );
			graphics.DrawString( textMessage, font, System.Drawing.Brushes.WhiteSmoke, rectangle, stringFormat );

			font.Dispose();
			
			graphics.Dispose();
			
			BackBufferSurface.Surface.ReleaseDC( dcHandle.ToInt32() );

        }

        /// <summary>
        ///  Paints a custom cursor rather than the default windows cursors.
        ///  Can be used to enable cursor animation, but in the current
        ///  revision simply paints a custom cursor based on mouse location.
        /// </summary>
        private void PaintCursor()
        {
            if (!enabledCursor || Cursor != null || !drawCursor)
            {
                return;
            }

            DirectDrawSpriteSurface workSurface = null;
            workSurface = tsm["cursor"].GetDefaultSurface();
            if (workSurface != null)
            {
                RECT dest    = new RECT();

				int xOffset = cursorPos.X;
				int yOffset = cursorPos.Y;

				if ( bltUsingBezel )
				{
					Rectangle cursorRectangle = ComputeCursorRectangle();
					xOffset -= cursorRectangle.Left;
					yOffset -= cursorRectangle.Top;
				}

                switch (cursor)
                {
                    case 1:
                        dest.Top     = yOffset;
                        dest.Bottom  = yOffset + 16;
                        dest.Left    = xOffset - 16;
                        dest.Right   = xOffset;
                        break;
                    case 2:
                        goto case 1;
                    case 3:
                        dest.Top     = yOffset - 16;
                        dest.Bottom  = yOffset;
                        dest.Left    = xOffset - 16;
                        dest.Right   = xOffset;
                        break;
                    case 4:
                        goto case 3;
                    case 5:
                        dest.Top     = yOffset - 16;
                        dest.Bottom  = yOffset;
                        dest.Left    = xOffset;
                        dest.Right   = xOffset + 16;
                        break;
                    case 6:
                        goto case 5;
                    default:
                        dest.Top     = yOffset;
                        dest.Bottom  = yOffset + 16;
                        dest.Left    = xOffset;
                        dest.Right   = xOffset + 16;
                        break;
                }

                DirectDrawClippedRect ddClipRect;
                ddClipRect = workSurface.GrabSprite(0, cursor, dest, clipRect);
                if (!ddClipRect.Invisible)
                {
                    BackBufferSurface.Surface.Blt(ref ddClipRect.Destination, workSurface.SpriteSurface.Surface, ref ddClipRect.Source, CONST_DDBLTFLAGS.DDBLT_KEYSRC);
                }
            }
        }

        /// <summary>
        ///  Paint sprites on the given surface.  This method is the meat
        ///  of the graphics engine.  Normally, creatures are painted to
        ///  the work surface using this method.  To increase performance plants
        ///  are rendered to the background surface only once every 10 frames.
        ///  Lots of work happening in this function so either read through the
        ///  code or examine the Terrarium Graphics Engine whitepaper for more
        ///  information.
        /// </summary>
        /// <param name="dds">The surface to render to.</param>
        /// <param name="PlantsOnly">True to render plants, false to render animals.</param>
        private void PaintSprites(DirectDrawSurface dds, bool PlantsOnly)
        {
#if TRACE
            this.Profiler.Start("TerrariumDirectDrawGameView.PaintSprites()");
#endif
            if (wv == null)
            {
                return;
            }
        
            if (tfm.Count > 100)
            {
                tfm.Clear();
            }

            int[] TeleporterZIndices = TeleporterZIndex();
            int lastTeleporterZIndex = 0;

            foreach (OrganismState orgState in wv.State.ZOrderedOrganisms)
            {
                if (orgState.RenderInfo != null)
                {
                    TerrariumSprite tsSprite = (TerrariumSprite) orgState.RenderInfo;

                    if ((PlantsOnly && !(orgState.Species is PlantSpecies)) ||
                        (!PlantsOnly && orgState.Species is PlantSpecies))
                    {
                        continue;
                    }

                    if (orgState.Species is AnimalSpecies)
                    {
                        tsSprite.AdvanceFrame();
                        orgState.RenderInfo = (Object) tsSprite;
                    }

                    if (!videomemory && skipframe)
                    {
                        continue;
                    }

                    TerrariumSpriteSurface workTss = null;

                    if (workTss == null && tsSprite.SpriteKey != null)
                    {
                        workTss = tsm[tsSprite.SpriteKey, orgState.Radius, (orgState.Species is AnimalSpecies)];
                    }
                
                    if (workTss == null && tsSprite.SkinFamily != null)
                    {
                        workTss = tsm[tsSprite.SkinFamily, orgState.Radius, (orgState.Species is AnimalSpecies)];
                    }
                
                    if (workTss == null)
                    {
                        if (orgState.Species is AnimalSpecies)
                        {
                            workTss = tsm["ant", orgState.Radius, (orgState.Species is AnimalSpecies)];
                        }
                        else
                        {
                            workTss = tsm["plant", orgState.Radius, (orgState.Species is AnimalSpecies)];
                        }
                    }

                    if (workTss != null)
                    {
                        int direction = orgState.ActualDirection;
                        int radius = orgState.Radius;
                        int framedir = 1;
                        DirectDrawSpriteSurface workSurface = workTss.GetClosestSurface((radius*2));
                        radius = workSurface.FrameWidth;

                        if (direction >= 68 && direction < 113)
                        {
                            framedir = 1;
                        }
                        else if (direction >= 113 && direction < 158)
                        {
                            framedir = 2;
                        }
                        else if (direction >= 158 && direction < 203)
                        {
                            framedir = 3;
                        }
                        else if (direction >= 203 && direction < 248)
                        {
                            framedir = 4;
                        }
                        else if (direction >= 248 && direction < 293)
                        {
                            framedir = 5;
                        }
                        else if (direction >= 293 && direction < 338)
                        {
                            framedir = 6;
                        }
                        else if (direction >= 338 && direction < 23)
                        {
                            framedir = 7;
                        }
                        else
                        {
                            framedir = 8;
                        }

                        RECT dest    = new RECT();
                        dest.Top     = (int) tsSprite.YPosition - (viewsize.Top + (radius>>1));
                        dest.Bottom  = dest.Top + radius;
                        dest.Left    = (int) tsSprite.XPosition - (viewsize.Left + (radius>>1));
                        dest.Right   = dest.Left + radius;

                        if (updateMiniMap)
                        {
                            int miniMapX = (int) (tsSprite.XPosition * miniMap.Width) / actualsize.Right; miniMapX = (miniMapX > (miniMap.Width - 1)) ? (miniMap.Width - 1) : miniMapX;
                            int miniMapY = (int) (tsSprite.YPosition * miniMap.Height) / actualsize.Bottom; miniMapY = (miniMapY > (miniMap.Height - 1)) ? (miniMap.Height - 1) : miniMapY;
                            
							Color brushColor = Color.Fuchsia;

							if (orgState.Species.GetType() == typeof(PlantSpecies))
							{
								brushColor = Color.Lime;
							}
							else if (orgState.IsAlive == false)
							{
								brushColor = Color.Black;
							}
							else
							{
								Species orgSpecies = (Species)orgState.Species;
								if (orgSpecies.MarkingColor == KnownColor.Green || orgSpecies.MarkingColor == KnownColor.Black)
								{
									brushColor = Color.Red;
								}
								else
								{
									brushColor = Color.FromKnownColor(orgSpecies.MarkingColor);
								}
							}

							Brush orgBrush = new SolidBrush(brushColor);

							Graphics miniMapGraphics = Graphics.FromImage(miniMap);
							miniMapGraphics.FillRectangle(orgBrush, miniMapX, miniMapY, 12, 12);
							miniMapGraphics.Dispose();
							orgBrush.Dispose();

							//this.miniMap.SetPixel(
							//    miniMapX,
							//    miniMapY,
							//    (orgState.Species is PlantSpecies) ? Color.Green : (!orgState.IsAlive) ? Color.Black : (((Species) orgState.Species).MarkingColor == KnownColor.Green || ((Species) orgState.Species).MarkingColor == KnownColor.Black) ? Color.Red : Color.FromKnownColor(((Species) orgState.Species).MarkingColor));
                        }

                        DirectDrawClippedRect ddClipRect;
                        if (orgState.Species is PlantSpecies)
                        {
                            ddClipRect = workSurface.GrabSprite((int) tsSprite.CurFrame, 0, dest, clipRect);
                        }
                        else
                        {
                            if (tsSprite.PreviousAction == DisplayAction.NoAction   ||
                                tsSprite.PreviousAction == DisplayAction.Reproduced ||
                                tsSprite.PreviousAction == DisplayAction.Teleported ||
                                tsSprite.PreviousAction == DisplayAction.Dead)
                            {
                                if (tsSprite.PreviousAction == DisplayAction.Dead)
                                {
                                    ddClipRect = workSurface.GrabSprite(
                                        (int) 9, 
                                        ((int) DisplayAction.Died) + framedir, 
                                        dest, 
                                        clipRect);
                                }
                                else
                                {
                                    ddClipRect = workSurface.GrabSprite((int) 0, ((int) DisplayAction.Moved) + framedir, dest, clipRect);
                                }
                            }
                            else
                            {
                                ddClipRect = workSurface.GrabSprite((int) tsSprite.CurFrame, ((int) tsSprite.PreviousAction) + framedir, dest, clipRect);
                            }
                        }
                    
                        if (!ddClipRect.Invisible)
                        {
                            if (!PlantsOnly)
                            {
                                RenderTeleporter(lastTeleporterZIndex, (int)tsSprite.YPosition);
                                lastTeleporterZIndex = (int)tsSprite.YPosition;
                            }

                            dds.Surface.BltFast(
                                ddClipRect.Destination.Left,
                                ddClipRect.Destination.Top,
                                workSurface.SpriteSurface.Surface,
                                ref ddClipRect.Source,
                                CONST_DDBLTFASTFLAGS.DDBLTFAST_SRCCOLORKEY);

                            if (drawtext && !PlantsOnly)
                            {
                                DirectDrawSurface textSurface = tfm[((Species) orgState.Species).Name];
                                if (textSurface != null && textSurface.Surface != null)
                                {
                                    dds.Surface.BltFast(
                                        ddClipRect.Destination.Left, 
                                        ddClipRect.Destination.Top - 14, 
                                        textSurface.Surface, 
                                        ref TerrariumTextSurfaceManager.StandardFontRect, 
                                        CONST_DDBLTFASTFLAGS.DDBLTFAST_SRCCOLORKEY);
                                }
                            }

                            if (!ddClipRect.ClipLeft  &&
                                !ddClipRect.ClipRight &&
                                !ddClipRect.ClipTop   &&
                                !ddClipRect.ClipBottom)
                            {
                                if (drawDestinationLines)
                                {
                                    if (orgState.CurrentMoveToAction != null)
                                    {
                                        Point start = orgState.Position;
                                        Point end = orgState.CurrentMoveToAction.MovementVector.Destination;
                                        dds.Surface.SetForeColor(0);
                                        dds.Surface.DrawLine(start.X - viewsize.Left, start.Y - viewsize.Top, end.X - viewsize.Left, end.Y - viewsize.Top);
                                    }
                                }
                        
                                if (drawBoundingBox)
                                {
                                    Rectangle boundingBox = GetBoundsOfState(orgState);
                                    dds.Surface.SetForeColor(0);
                                    dds.Surface.DrawBox(
                                        boundingBox.Left - viewsize.Left,
                                        boundingBox.Top - viewsize.Top,
                                        (boundingBox.Right + 1) - viewsize.Left,
                                        (boundingBox.Bottom + 1) - viewsize.Top
                                        );
                                }

                                if (tsSprite.Selected)
                                {
                                    dds.Surface.SetForeColor(0);
                                    dds.Surface.DrawBox(ddClipRect.Destination.Left, ddClipRect.Destination.Top, ddClipRect.Destination.Right, ddClipRect.Destination.Bottom);

                                    // red  Maybe we want some cool graphic here though
                                    int lineheight = (int) ((ddClipRect.Destination.Bottom - ddClipRect.Destination.Top) * orgState.PercentEnergy);
                                    dds.Surface.SetForeColor(63488);
                                    dds.Surface.DrawLine(ddClipRect.Destination.Left - 1, ddClipRect.Destination.Top, ddClipRect.Destination.Left - 1, ddClipRect.Destination.Top + lineheight);

                                    //green  Maybe we want some cool graphic here though (or an actual bar?)
                                    lineheight = (int) ((ddClipRect.Destination.Bottom - ddClipRect.Destination.Top) * orgState.PercentInjured);
                                    dds.Surface.SetForeColor(2016);
                                    dds.Surface.DrawLine(ddClipRect.Destination.Right + 1, ddClipRect.Destination.Top, ddClipRect.Destination.Right + 1, ddClipRect.Destination.Top + lineheight);
                                }
                            }
                        }
                    }
                }
            }

            RenderTeleporter(lastTeleporterZIndex, actualsize.Bottom);

#if TRACE
            this.Profiler.End("TerrariumDirectDrawGameView.PaintSprites()");
#endif
        }
    
        /// <summary>
        ///  Uses the bounding box computation methods to compute a
        ///  box that can be printed within the graphics engine.  This
        ///  is used for debugging creature pathing.
        /// </summary>
        /// <param name="orgState">The state of the creature to compute a bounding box for.</param>
        /// <returns>A bounding box.</returns>
        private Rectangle GetBoundsOfState(OrganismState orgState)
        {
            Point origin = orgState.Position;
            int cellRadius = orgState.CellRadius;

            Point p1 = new Point(
                (origin.X>>3)*8,
                (origin.Y>>3)*8
                );

            Rectangle bounds = new Rectangle(
                p1.X - (cellRadius*8),
                p1.Y - (cellRadius*8),
                (((cellRadius*2+1)*8)-1),
                (((cellRadius*2+1)*8)-1)
                );

            return bounds;
        }

        /// <summary>
        ///  Sets up the hack table with teleporter information.
        ///  The hack table is used to quickly implement new types
        ///  of sprites or implement sprites linked to immutable
        ///  objects (like the teleporter).
        /// </summary>
        /// <param name="zone">The teleporter zone to initialize.</param>
        private void InitTeleporter(TeleportZone zone)
        {
            TerrariumSprite tsZone = new TerrariumSprite();
            tsZone.CurFrame = 0; tsZone.CurFrameDelta = 1;
            tsZone.SpriteKey = "teleporter";
            tsZone.XPosition = zone.Rectangle.X;
            tsZone.YPosition = zone.Rectangle.Y;
            hackTable.Add(zone.ID, tsZone);
        }

        /// <summary>
        ///  Initializes a new organism state by computed and
        ///  attaching a TerrariumSprite class that can be used
        ///  to control on screen movement, animation skins, and
        ///  selection.
        /// </summary>
        /// <param name="orgState">The organism state to attach the sprite animation information to.</param>
        private void InitOrganism(OrganismState orgState)
        {
            TerrariumSprite tsSprite = new TerrariumSprite();
            tsSprite.CurFrame = 0;
            tsSprite.CurFrameDelta = 1;

            ISpecies species = orgState.Species;
            if (species is AnimalSpecies)
            {
                tsSprite.SpriteKey = ((AnimalSpecies) species).Skin;
                tsSprite.SkinFamily = ((AnimalSpecies) species).SkinFamily.ToString();
                tsSprite.IsPlant = false;

                if (tsSprite.SpriteKey == null && tsSprite.SkinFamily == null)
                {
                    tsSprite.SpriteKey = AnimalSkinFamily.Spider.ToString(); // This will be our default
                }
            }
            else
            {
                tsSprite.SpriteKey = ((PlantSpecies) species).Skin;
                tsSprite.SkinFamily = ((PlantSpecies) species).SkinFamily.ToString();
                tsSprite.IsPlant = true;

                if (tsSprite.SpriteKey == null && tsSprite.SkinFamily == null)
                {
                    tsSprite.SpriteKey = PlantSkinFamily.Plant.ToString(); // This will be our default
                }
            }

            tsSprite.XPosition = orgState.Position.X;
            tsSprite.YPosition = orgState.Position.Y;
            tsSprite.PreviousAction = orgState.PreviousDisplayAction;
            orgState.RenderInfo = (Object) tsSprite;
        }

        /// <summary>
        ///  Updates the sprites controlled by the game view by providing a new
        ///  world vector from the game engine.
        /// </summary>
        /// <param name="worldVector">The new world vector of organisms.</param>
        public void UpdateWorld(WorldVector worldVector)
        {
#if TRACE
            this.Profiler.Start("TerrariumDirectDrawGameView.UpdateWorld()");
#endif
            wv = worldVector;
            paintPlants = true;

            updateTicker++; updateTickerChanged = true;

            TeleportZone[] zones = wv.State.Teleporter.GetTeleportZones();
            foreach (TeleportZone zone in zones)
            {
                if (hackTable.ContainsKey(zone.ID))
                {
                    TerrariumSprite tsZone = (TerrariumSprite) hackTable[zone.ID];
                    tsZone.XDelta = (zone.Rectangle.X - tsZone.XPosition) / 10;
                    tsZone.YDelta = (zone.Rectangle.Y - tsZone.YPosition) / 10;
                    hackTable[zone.ID] = tsZone;
                }
                else
                {
                    InitTeleporter(zone);
                }
            }
        
            foreach (OrganismState orgState in wv.State.Organisms)
            {
                if (orgState.RenderInfo != null)
                {
                    TerrariumSprite tsSprite = (TerrariumSprite) orgState.RenderInfo;

                    if (orgState is AnimalState)
                    {
                        if (tsSprite.PreviousAction != orgState.PreviousDisplayAction)
                        {
                            tsSprite.CurFrame = 0;
                            tsSprite.PreviousAction = orgState.PreviousDisplayAction;
                        }
                        tsSprite.XDelta = (orgState.Position.X - tsSprite.XPosition) / 10;
                        tsSprite.YDelta = (orgState.Position.Y - tsSprite.YPosition) / 10;
                    }
                    else
                    {
                        tsSprite.CurFrame = 0;
                        tsSprite.PreviousAction = orgState.PreviousDisplayAction;
                        tsSprite.XPosition = orgState.Position.X;
                        tsSprite.YPosition = orgState.Position.Y;
                    }
                    orgState.RenderInfo = (TerrariumSprite) tsSprite;
                }
                else
                {
                    InitOrganism(orgState);
                }
            }
#if TRACE
            this.Profiler.End("TerrariumDirectDrawGameView.UpdateWorld()");
#endif
        }

        /// <summary>
        ///  Resets the Terrarium and prepares it for a new world, without
        ///  having to reboot the entire game.  
        /// </summary>
        private void ResetTerrarium()
        {
            // Lets reset the hackTable
            hackTable = new Hashtable();

            // Lets reset the view coordinates
            if (fullscreen)
            {
                viewsize.Top = 0;
                viewsize.Left = 0;
                viewsize.Right = 500;
                viewsize.Bottom = 400;

                clipRect = viewsize;
            }
            else
            {
                viewsize.Top = 0;
                viewsize.Left = 0;
                viewsize.Right = this.Width - 1;
                viewsize.Bottom = this.Height - 1;

				if ( viewsize.Right > actualsize.Right )
				{
					viewsize.Right = actualsize.Right;
				}
				if ( viewsize.Bottom > actualsize.Bottom )
				{
					viewsize.Bottom = actualsize.Bottom;
				}

                clipRect = viewsize;
            }
        }

        /// <summary>
        ///  Clear any background surfaces to black.  This helps
        ///  find rendering artifacts where portions of a scene
        ///  aren't updated.  Since the background is cleared to
        ///  black, the portions not updated are clearly visible.
        /// </summary>
        private void ClearBackground()
        {
            RECT clearRect = new RECT();
            BackBufferSurface.Surface.BltColorFill(ref clearRect, 0);

            clearRect = new RECT();
            backgroundSurface.Surface.BltColorFill(ref clearRect, 0);

            clearRect = new RECT();
            stagingSurface.Surface.BltColorFill(ref clearRect, 0);
        }

        /// <summary>
        ///  Renders the Terrarium background image.  Also renders
        ///  plants on the background for static plant drawing to
        ///  enable higher performance.
        /// </summary>
        private void PaintBackground()
        {
#if TRACE
            this.Profiler.Start("TerrariumDirectDrawGameView.PaintBackground()");
#endif
            if (!videomemory && skipframe )
            {
#if TRACE
                this.Profiler.End("TerrariumDirectDrawGameView.PaintBackground()");
#endif
                return;
            }

            RECT srcRect;
            RECT destRect;

            if (viewchanged)
            {
                DirectDrawSpriteSurface workSurface = null;

				workSurface = tsm["background"].GetDefaultSurface();

                if (workSurface != null)
                {
                    int xTileStart = viewsize.Left / wld.TileYSize;
                    int xTileEnd = (viewsize.Right / wld.TileYSize) + 1;

                    if ((viewsize.Right%wld.TileYSize) != 0)
                    {
                        xTileEnd++;
                    }

                    int yTileStart = viewsize.Top / wld.TileYSize;
                    int yTileEnd   = (viewsize.Bottom / wld.TileYSize) + 2;

                    for (int j = yTileStart; j < yTileEnd && j < wld.Map.GetLength(1); j++)
                    {
                        for (int i = xTileStart; i < xTileEnd; i++ )
                        {
                            RECT dest;
                            TileInfo tInfo = wld.Map[i,j];

                            /* Get world offset */
                            dest.Top = tInfo.YOffset;
                            dest.Left = tInfo.XOffset;

                            /* Compute viewport offset */
                            dest.Top -= viewsize.Top;
                            dest.Left -= viewsize.Left;

                            dest.Bottom = dest.Top + wld.TileYSize;
                            dest.Right = dest.Left + wld.TileXSize;

                            int iTrans =
                                (tInfo.Tile < 8) ?
                                (tInfo.Transition*2) :
                                (tInfo.Transition*2) + 1;
                            int iTile = 
                                (tInfo.Tile < 8) ? 
                                tInfo.Tile : 
                                tInfo.Tile - 8;

							iTile = 0;
							iTrans = 0;

                            DirectDrawClippedRect ddClipRect = 
                                workSurface.GrabSprite(iTile, iTrans, dest, clipRect);
                            if (!ddClipRect.Invisible)
                            {
                                backgroundSurface.Surface.BltFast(
                                    ddClipRect.Destination.Left, 
                                    ddClipRect.Destination.Top, 
                                    workSurface.SpriteSurface.Surface, 
                                    ref ddClipRect.Source, 
                                    CONST_DDBLTFASTFLAGS.DDBLTFAST_WAIT | 
                                    CONST_DDBLTFASTFLAGS.DDBLTFAST_SRCCOLORKEY);
                            }
                        }
                    }
                }
                viewchanged = false;
                paintPlants = true;
            }

            if (paintPlants)
            {
                srcRect = backgroundSurface.Rect;
                destRect = stagingSurface.Rect;
            
                stagingSurface.Surface.Blt(ref destRect, backgroundSurface.Surface, ref srcRect, CONST_DDBLTFLAGS.DDBLT_WAIT);
            
                PaintSprites(stagingSurface, true);
                paintPlants = false;
            }


            srcRect = stagingSurface.Rect;
            destRect = BackBufferSurface.Rect;
            BackBufferSurface.Surface.Blt(ref destRect, stagingSurface.Surface, ref srcRect, CONST_DDBLTFLAGS.DDBLT_WAIT);
#if TRACE
            this.Profiler.End("TerrariumDirectDrawGameView.PaintBackground()");
#endif
        }

        /// <summary>
        ///  Controls scrolling of the viewport around the world map.
        /// </summary>
        /// <param name="pixels">The number of pixels to scroll up</param>
        /// <returns>The number of pixels actually scrolled.</returns>
        public int ScrollUp(int pixels)
        {
            if ((viewsize.Top - pixels) < actualsize.Top)
            {
                pixels = viewsize.Top - actualsize.Top;
            }

            viewsize.Top -= pixels;
            viewsize.Bottom -= pixels;

            if (pixels != 0)
            {
                viewchanged = true;
            }

            return pixels;
        }

        /// <summary>
        ///  Controls scrolling of the viewport around the world map.
        /// </summary>
        /// <param name="pixels">The number of pixels to scroll down</param>
        /// <returns>The number of pixels actually scrolled.</returns>
        public int ScrollDown(int pixels)
        {
            if ((viewsize.Bottom + pixels) > actualsize.Bottom)
            {
                pixels = actualsize.Bottom - viewsize.Bottom;
            }

            viewsize.Top += pixels;
            viewsize.Bottom += pixels;

            if (pixels != 0)
            {
                viewchanged = true;
            }

            return pixels;
        }

        /// <summary>
        ///  Controls scrolling of the viewport around the world map.
        /// </summary>
        /// <param name="pixels">The number of pixels to scroll left</param>
        /// <returns>The number of pixels actually scrolled.</returns>
        public int ScrollLeft(int pixels)
        {
            if ((viewsize.Left - pixels) < actualsize.Left)
            {
                pixels = viewsize.Left - actualsize.Left;
            }

            viewsize.Left -= pixels;
            viewsize.Right -= pixels;

            if (pixels != 0)
            {
                viewchanged = true;
            }

            return pixels;
        }

        /// <summary>
        ///  Controls scrolling of the viewport around the world map.
        /// </summary>
        /// <param name="pixels">The number of pixels to scroll right</param>
        /// <returns>The number of pixels actually scrolled.</returns>
        public int ScrollRight(int pixels)
        {
            if ((viewsize.Right + pixels) > actualsize.Right)
            {
                pixels = actualsize.Right - viewsize.Right;
            }

            viewsize.Left += pixels;
            viewsize.Right += pixels;

            if (pixels != 0)
            {
                viewchanged = true;
            }

            return pixels;
        }

        /// <summary>
        ///  Controls scrolling of the viewport around the world map.  Attempts
        ///  to center the view to the defined point.
        /// </summary>
        /// <param name="xOffset">X location to center to.</param>
        /// <param name="yOffset">Y location to center to.</param>
        public void CenterTo(int xOffset, int yOffset)
        {
            int ViewportOffsetXSize = (viewsize.Right - viewsize.Left) / 2;
            int ViewportOffsetYSize = (viewsize.Bottom - viewsize.Top) / 2;

            if (xOffset < (viewsize.Left + ViewportOffsetXSize))
            {
                ScrollLeft((viewsize.Left + ViewportOffsetXSize) - xOffset);
            }
            else
            {
                ScrollRight(xOffset - (viewsize.Left + ViewportOffsetXSize));
            }

            if (yOffset < (viewsize.Top + ViewportOffsetYSize))
            {
                ScrollUp((viewsize.Top + ViewportOffsetYSize) - yOffset);
            }
            else
            {
                ScrollDown(yOffset - (viewsize.Top + ViewportOffsetYSize));
            }
        }

        /// <summary>
        ///  Returns the size of the viewport window.
        /// </summary>
        public RECT ViewSize
        {
            get
            {
                return viewsize;
            }
        }

        /// <summary>
        ///  Returns the full size of the world.
        /// </summary>
        public RECT ActualSize
        {
            get
            {
                return actualsize;
            }
        }

		/// <summary>
		/// Determines whether to 
		/// </summary>
		public bool DrawCursor
		{
			get
			{
				return this.drawCursor;
			}
			set
			{
				this.drawCursor = value;
			}
		}

    }
}