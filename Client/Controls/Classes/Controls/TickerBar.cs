//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                            
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Windows.Forms;
using Terrarium.Glass;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;

namespace Terrarium.Forms
{
	/// <summary>
	/// A statistics ticker bar control
	/// </summary>
	public class TickerBar : System.Windows.Forms.UserControl
	{	
		private Thread							backgroundThread = null;

		private TickerMessage					currentMessage = new TickerMessage();
		private TickerMessageQueue messages = new TickerMessageQueue();
		private bool							transition = false;
		private int								transitionAlpha = 255;

		private int								messageLife = 5000;
		private int								messageTime = Environment.TickCount + 10000;

        private TickerHistoryForm tickerHistory = null;

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
			this.SetStyle( ControlStyles.OptimizedDoubleBuffer, true );
			this.SetStyle( ControlStyles.UserPaint, true );
			
			// If we are in runtime mode, then set up the timer
			if ( this.DesignMode == false )
			{
				this.currentMessage.Text = "";
			}

			this.ResizeRedraw = true;

            this.tickerHistory = new TickerHistoryForm(this);
            this.tickerHistory.Parent = this.ParentForm;
            Size temp = this.tickerHistory.Size;
            this.tickerHistory.TopMost = true;
            this.tickerHistory.Size = new Size(0, 0);
           
            this.tickerHistory.Show();
            this.tickerHistory.Hide();
            this.tickerHistory.Size = temp;
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
		/// Holds the messages.
		/// </summary>
		public TickerMessageQueue Messages
		{
			get
			{
				return this.messages;
			}
		}

		/// <summary>
		/// Custom painting
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
            GlassHelper.FillRectangle(this.ClientRectangle, GlassStyleManager.Active.Panel, false, GlassStyleManager.Active.PanelIsGlass, e.Graphics);

			GlassHelper.DrawBorder(this.ClientRectangle, GlassBorders.Top | GlassBorders.TopAndBottom, e.Graphics);

			Color alphaFore = Color.FromArgb( this.transitionAlpha, this.currentMessage.Color );
			Color alphaShadow = Color.FromArgb( this.transitionAlpha, Color.Black );

            Rectangle textRectangle = this.ClientRectangle;
            
            //textRectangle.Inflate(-2, -2);

            //if(this.currentMessage.Image != null)
            //{
            //    int top = 0;

            //    top = (this.Height/2) - (this.currentMessage.Image.Height/2);

            //    e.Graphics.DrawImage(this.currentMessage.Image, 2, top );

            //    textRectangle.Offset(4 + this.currentMessage.Image.Width, 0);
            //    textRectangle.Inflate(-(4 + this.currentMessage.Image.Width), 0);
            //}

			GlassHelper.DrawText( this.currentMessage.Text, textRectangle, ContentAlignment.MiddleCenter, e.Graphics, true, alphaFore, alphaShadow );
		}

		/// <summary>
		/// Fired when the current text scrolls off of the bounds.  Increments
		/// the callback and executes it to retrieve the next text. 
		/// </summary>
		/// <param name="args">Standard EventArgs</param>
		protected virtual void OnScrollComplete( EventArgs args )
		{
			if ( this.messages.Count == 0 )
			{
				this.currentMessage.Text = "";
			}
			else
			{
				this.currentMessage = this.messages.Dequeue();
			}

            if (this.currentMessage != null)
            {
                this.tickerHistory.AddHistoryItem(this.currentMessage);
            }

			if( this.ScrollComplete != null )
			{
				this.ScrollComplete( this, args );
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            // Display the history farm, in the right place.
            Rectangle rect = this.RectangleToScreen(this.ClientRectangle);
            this.tickerHistory.Left = rect.Left;
            this.tickerHistory.Top = rect.Top - this.tickerHistory.Height;
            this.tickerHistory.Width = this.Width;
            this.tickerHistory.Show();
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

	/// <summary>
	/// Holds the information about a scrolling message
	/// </summary>
	public class TickerMessage
	{
		/// <summary>
		/// The text to display
		/// </summary>
		public string Text = "";
		/// <summary>
		/// What color to display this message in.
		/// Color.Empty will use the forecolor of the style
		/// </summary>
		public Color Color = Color.White;
		/// <summary>
		/// The image to display, if any.  The image should have
		/// a 1:1 aspect ration.  It will be rendered small, at
		/// 24x24 or smaller.
		/// </summary>
		public Image Image = null;
	}

	/// <summary>
	/// A strongly typed collection of TickerMessages
	/// </summary>
	public class TickerMessageQueue : Queue<TickerMessage>
	{
		/// <summary>
		/// A helper overload
		/// </summary>
		/// <param name="text">The text for the message</param>
		public void Enqueue(string text)
		{
            string[] messages = text.Split('\r');

            foreach (string textMessage in messages)
            {
                TickerMessage message = new TickerMessage();
                message.Text = textMessage.Trim('\r', '\n');
                if (message.Text.Length > 0)
                    this.Enqueue(message);
            }

		}

		/// <summary>
		/// A helper overload
		/// </summary>
		/// <param name="text"></param>
		/// <param name="color"></param>
		/// <param name="image"></param>
		public void Enqueue(string text, Color color, Image image)
		{
            string[] messages = text.Split('\r');

            foreach (string textMessage in messages)
            {
                TickerMessage message = new TickerMessage();
                message.Text = textMessage.Trim('\r', '\n');
                message.Color = color;
                message.Image = image;
                if (message.Text.Length > 0)
                    this.Enqueue(message);
            }
		}
	}
}
