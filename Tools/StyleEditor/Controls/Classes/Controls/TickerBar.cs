//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Windows.Forms;
using Terrarium.Metal;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace Terrarium.Forms
{
	/// <summary>
	/// Holds the information about a scrolling message
	/// </summary>
	public class TickerMessage
	{
		/// <summary>
		/// The text to display
		/// </summary>
		public string	Text = "";
		/// <summary>
		/// What color to display this message in.
		/// Color.Empty will use the forecolor of the style
		/// </summary>
		public Color	Color = Color.White;
		/// <summary>
		/// The image to display, if any.  The image should have
		/// a 1:1 aspect ration.  It will be rendered small, at
		/// 24x24 or smaller.
		/// </summary>
		public Image	Image = null;

		/// <summary>
		/// Indicates that this callback should be removed after this message
		/// </summary>
		public bool		RemoveCallback = false;
	}

	/// <summary>
	/// The delegate to use to provide message data to the ticker
	/// </summary>
	public delegate TickerMessage GetMessageCallback();

	/// <summary>
	/// A statistics ticker bar control
	/// </summary>
	public class TickerBar : System.Windows.Forms.UserControl
	{	
		private Thread							backgroundThread = null;

		private TickerMessage					currentMessage = new TickerMessage();
		private	ArrayList						messageCallbacks;
		private int								currentCallbackIndex = 0;

		private bool							transition = false;
		private int								transitionAlpha = 255;

		private int								messageLife = 5000;
		private int								messageTime = Environment.TickCount + 10000;

		/// <summary>
		/// This gets fired whenever text scrolls off of the view
		/// </summary>
		public event EventHandler	ScrollComplete;

		/// <summary>
		/// Default Constructor
		/// </summary>
		public TickerBar()
		{
			// Design Time support
			InitializeComponent();

			this.SetStyle( ControlStyles.AllPaintingInWmPaint, true );
			this.SetStyle( ControlStyles.DoubleBuffer, true );
			this.SetStyle( ControlStyles.UserPaint, true );

			this.messageCallbacks = new ArrayList();
			
			// If we are in runtime mode, then set up the timer
			if ( this.DesignMode == false )
			{
				this.currentMessage.Text = "";
			}
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// TickerBar
			// 
			this.Name = "TickerBar";
			this.Size = new System.Drawing.Size(640, 24);
			this.ResumeLayout(false);

		}


		/// <summary>
		/// Starts the ticker.  Also performs a logical reset as it
		/// will always start with the first item
		/// </summary>
		public void Start()
		{
			if ( this.backgroundThread != null )
				this.Stop();

			this.currentCallbackIndex = -1;

			// This will force a OnScrollComplete as soon as the thread starts, which will
			// pump for the first callback.
			this.transitionAlpha = 0;
			this.transition = true;

			this.backgroundThread = new Thread( new ThreadStart(this.ThreadFunction) );
			this.backgroundThread.Priority = ThreadPriority.Highest;
			this.backgroundThread.IsBackground = false;
			this.backgroundThread.Start();
		}

		/// <summary>
		/// Stops the ticker
		/// </summary>
		public void Stop()
		{
			if ( this.backgroundThread != null )
			{
				this.backgroundThread.Abort();
				this.backgroundThread.Join( 2000 );
				this.backgroundThread = null;
			}
			this.currentMessage.Text = "";
		}

		/// <summary>
		/// Determines how long a message is displayed before
		/// changing
		/// </summary>
		public int MessageLife
		{
			get
			{
				return this.messageLife;
			}
			set
			{
				this.messageLife = value;
			}
		}

		/// <summary>
		/// Holds the message callbacks.  This really should be a 
		/// strongly typed collection to avoid someone putting
		/// anything in here.  OnScrollComplete() has safe guards
		/// against non TickMessageCallback delegates.
		/// </summary>
		public ArrayList MessageCallbacks
		{
			get
			{
				return this.messageCallbacks;
			}
		}

		/// <summary>
		/// Custom painting
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			MetalHelper.DrawGradient( this.ClientRectangle, MetalStyleManager.Active.PanelSunk, e.Graphics );
			MetalHelper.DrawBorder( this.ClientRectangle, MetalBorders.All, e.Graphics );

			Color alphaFore = Color.FromArgb( this.transitionAlpha, this.currentMessage.Color );
			Color alphaShadow = Color.FromArgb( this.transitionAlpha, Color.Black );

			MetalHelper.DrawText( this.currentMessage.Text, this.ClientRectangle, ContentAlignment.MiddleCenter, e.Graphics, true, alphaFore, alphaShadow );
		}

		/// <summary>
		/// Fired when the current text scrolls off of the bounds.  Increments
		/// the callback and executes it to retrieve the next text. 
		/// </summary>
		/// <param name="args">Standard EventArgs</param>
		protected virtual void OnScrollComplete( EventArgs args )
		{
			if ( this.messageCallbacks.Count == 0 )
			{
				this.currentCallbackIndex = -1;
				this.currentMessage.Text = "";
			}
			else
			{
				this.currentCallbackIndex++;
				if ( this.currentCallbackIndex >= this.messageCallbacks.Count )
					this.currentCallbackIndex = 0;

				Object callbackObject = this.messageCallbacks[this.currentCallbackIndex];
				if ( callbackObject is GetMessageCallback )
				{
					GetMessageCallback messageCallback = (GetMessageCallback)callbackObject;
					
					try
					{
						this.currentMessage = messageCallback();
						this.Invalidate();
						if ( this.currentMessage.RemoveCallback == true )
						{
							// Remove the callback
							this.messageCallbacks.Remove( messageCallback );

							// Push the index back one so that the next one
							// won't get skipped
							this.currentCallbackIndex--;
						}
					}
					catch( Exception ex )
					{
						this.currentMessage = new TickerMessage();
						this.currentMessage.Text = "*** TickerMessageCallback Exception: " + messageCallback.Method.Name + " ***";
						this.currentMessage.Color = Color.Red;
						Trace.WriteLine( this.currentMessage.Text );
						Trace.WriteLine( ex.ToString() );
					}
				}
				else
				{
					this.messageCallbacks.Remove( callbackObject );
					this.currentMessage = new TickerMessage();
					this.currentMessage.Text = "";
					this.currentCallbackIndex--;
				}
			}
				 
			if( this.ScrollComplete != null )
			{
				this.ScrollComplete( this, args );
			}
		}

		/// <summary>
		/// The main timer callback.  Performs the animation and determining
		/// when the text has scrolled off of view.  It fires the OnScrollComplete()
		/// event when this happens.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mainTimer_Tick(object sender, EventArgs e)
		{
		}

		private void ThreadFunction()
		{
			while( true )
			{
				if ( this.transition == true )
				{
					this.transitionAlpha -= 10;
					if ( this.transitionAlpha < 0 )
					{
						this.transitionAlpha = 0;
						this.OnScrollComplete( new EventArgs() );
						this.transitionAlpha = 255;
						this.transition = false;
						this.messageTime = Environment.TickCount;
					}
					
					this.Invalidate();

					Thread.Sleep( 50 );
				}
				else
				{						
					if ( Environment.TickCount - this.messageTime > this.messageLife )
					{
						this.transition = true;
					}
					
					Thread.Sleep( 500 );
				}

			}
		}
	}
}
