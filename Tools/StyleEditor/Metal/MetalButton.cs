//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace Terrarium.Metal
{
	[ DefaultProperty( "NormalGradient") ]
	public class MetalButton : System.Windows.Forms.ButtonBase
	{
		protected MetalGradient		normalGradient;
		protected MetalGradient		hoverGradient;
		protected MetalGradient		pressedGradient;
		protected MetalGradient		disabledGradient;
		protected MetalGradient		highlightGradient;
		protected MetalBorders		borders;

		protected bool			mouseOver = false;
		protected bool			mouseDown = false;

		protected bool			highlight = false;

		public MetalButton()
		{
			this.normalGradient = new MetalGradient( Color.FromArgb( 96,96,96 ), Color.FromArgb( 0,0,0 ) );
			this.hoverGradient = new MetalGradient( Color.FromArgb( 0,216,0 ), Color.FromArgb( 0,64,0 ) );
			this.pressedGradient = new MetalGradient( Color.FromArgb( 0,64,0 ), Color.FromArgb( 0,216,0 ) );
			this.disabledGradient = new MetalGradient( Color.FromArgb( 64, 64, 64 ), Color.FromArgb( 64, 64, 64 ) );
			this.highlightGradient = new MetalGradient( Color.FromArgb( 192, 192, 192 ), Color.FromArgb( 96, 96, 96 ) );
		
			this.TabStop = false;
			this.SetStyle( ControlStyles.Selectable, false );
		}

		[ Category("Metal UI") ]
		public bool Highlight
		{
			get
			{
				return this.highlight;
			}
			set
			{
				this.highlight = value;
				this.Invalidate();
			}
		}

		[ Category("Metal UI") ]
		public MetalBorders Borders
		{
			get
			{
				return this.borders;
			}
			set
			{
				this.borders = value;
				this.Invalidate();
			}
		}

		[ Category("Metal UI") ]
		[ DesignerSerializationVisibility(DesignerSerializationVisibility.Content) ]
		public MetalGradient NormalGradient
		{
			get
			{
				return this.normalGradient;
			}
			set
			{
				this.normalGradient = value;
				this.Invalidate();
			}
		}

		[ Category("Metal UI") ]
		[ DesignerSerializationVisibility(DesignerSerializationVisibility.Content) ]
		public MetalGradient HoverGradient
		{
			get
			{
				return this.hoverGradient;
			}
			set
			{
				this.hoverGradient = value;
				this.Invalidate();
			}
		}

		[ Category("Metal UI") ]
		[ DesignerSerializationVisibility(DesignerSerializationVisibility.Content) ]
		public MetalGradient PressedGradient
		{
			get
			{
				return this.pressedGradient;
			}
			set
			{
				this.pressedGradient = value;
				this.Invalidate();
			}
		}

		[ Category("Metal UI") ]
		[ DesignerSerializationVisibility(DesignerSerializationVisibility.Content) ]
		public MetalGradient DisabledGradient
		{
			get
			{
				return this.disabledGradient;
			}
			set
			{
				this.disabledGradient = value;
				this.Invalidate();
			}
		}
		
		[ Category("Metal UI") ]
		[ DesignerSerializationVisibility(DesignerSerializationVisibility.Content) ]
		public MetalGradient HighlightGradient
		{
			get
			{
				return this.highlightGradient;
			}
			set
			{
				this.highlightGradient = value;
				this.Invalidate();
			}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			base.OnPaintBackground(pevent);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			MetalGradient activeGradient = normalGradient;

			if ( MetalStyleManager.UseStyles == true )
			{
				if ( this.Enabled == false )
					activeGradient = MetalStyleManager.Active.ButtonDisabled;
				else if ( this.mouseDown == true )
					activeGradient = MetalStyleManager.Active.ButtonPressed;
				else if ( this.mouseOver == true )
					activeGradient = MetalStyleManager.Active.ButtonHover;
				else if ( this.highlight == true )
					activeGradient = MetalStyleManager.Active.ButtonHighlight;
				else
					activeGradient = MetalStyleManager.Active.ButtonNormal;
			}
			else
			{
				if ( this.Enabled == false )
					activeGradient = disabledGradient;
				else if ( this.mouseDown == true )
					activeGradient = pressedGradient;
				else if ( this.mouseOver == true )
					activeGradient = hoverGradient;
				else if ( this.highlight == true )
					activeGradient = highlightGradient;
				else
					activeGradient = normalGradient;
			}

			MetalHelper.DrawGradient( this.ClientRectangle, activeGradient, e.Graphics );

//			StringFormat stringFormat = new StringFormat();
//			stringFormat.Alignment = StringAlignment.Center;
//			stringFormat.FormatFlags = StringFormatFlags.FitBlackBox | StringFormatFlags.NoWrap;
//			stringFormat.LineAlignment = StringAlignment.Center;
//			stringFormat.Trimming = StringTrimming.EllipsisCharacter;
//
//			RectangleF shadowRectangle = new RectangleF( this.ClientRectangle.X+1, this.ClientRectangle.Y+1, this.ClientRectangle.Width, this.ClientRectangle.Height );
//			RectangleF normalRectangle = new RectangleF( this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Height );
//
//			e.Graphics.DrawString( this.Text, this.Font, Brushes.Black, shadowRectangle, stringFormat );
//			
//			SolidBrush fontBrush = new SolidBrush( this.ForeColor );
//
//			e.Graphics.DrawString( this.Text, this.Font, fontBrush, normalRectangle, stringFormat );
//
//			fontBrush.Dispose();

			StringAlignment		vertical = StringAlignment.Center;
			StringAlignment		horizontal = StringAlignment.Center;

			if ( this.Image != null )
			{
				switch( this.ImageAlign )
				{
					case ContentAlignment.BottomCenter:
					{
						horizontal = StringAlignment.Center;
						vertical = StringAlignment.Far;
						break;
					}
					case ContentAlignment.BottomLeft:
					{
						horizontal = StringAlignment.Near;
						vertical = StringAlignment.Far;
						break;
					}
					case ContentAlignment.BottomRight:
					{
						horizontal = StringAlignment.Far;
						vertical = StringAlignment.Far;
						break;
					}

					case ContentAlignment.MiddleCenter:
					{
						horizontal = StringAlignment.Center;
						vertical = StringAlignment.Center;
						break;
					}
					case ContentAlignment.MiddleLeft:
					{
						horizontal = StringAlignment.Near;
						vertical = StringAlignment.Center;
						break;
					}
					case ContentAlignment.MiddleRight:
					{
						horizontal = StringAlignment.Far;
						vertical = StringAlignment.Center;
						break;
					}

					case ContentAlignment.TopCenter:
					{
						horizontal = StringAlignment.Center;
						vertical = StringAlignment.Near;
						break;
					}
					case ContentAlignment.TopLeft:
					{
						horizontal = StringAlignment.Near;
						vertical = StringAlignment.Near;
						break;
					}
					case ContentAlignment.TopRight:
					{
						horizontal = StringAlignment.Far;
						vertical = StringAlignment.Near;
						break;
					}
				}

				int imageLeft = 0; 
				int imageTop = 0; 

				switch( horizontal )
				{
					case StringAlignment.Near:
						imageLeft = 4;
						break;
					case StringAlignment.Center:
						imageLeft = this.ClientRectangle.Width/2 - this.Image.Width/2;
						break;
					case StringAlignment.Far:
						imageLeft = this.Width - this.Image.Width - 4;
						break;
				}

				switch( vertical )
				{
					case StringAlignment.Near:
						imageTop = 2;
						break;
					case StringAlignment.Center:
						imageTop = this.ClientRectangle.Height/2 - this.Image.Height/2;
						break;
					case StringAlignment.Far:
						imageTop = this.Height - this.Image.Height - 2;
						break;
				}

				if ( this.Enabled == false )
				{
					ControlPaint.DrawImageDisabled( e.Graphics, this.Image, imageLeft, imageTop, MetalStyleManager.Active.ButtonDisabled.Bottom );
				}
				else
				{
					e.Graphics.DrawImage( this.Image, imageLeft, imageTop, this.Image.Width, this.Image.Height );
				}
			}

			Rectangle textRectangle = this.ClientRectangle;

			if ( this.Image != null )
			{
				switch( horizontal )
				{
					case StringAlignment.Near:
						textRectangle = new Rectangle( this.Image.Width + 4, 0, this.ClientRectangle.Width - this.Image.Width - 2, this.ClientRectangle.Height );
						break;
					case StringAlignment.Center:
						textRectangle = new Rectangle( 0, this.Image.Height + 2, this.ClientRectangle.Width, this.ClientRectangle.Height - this.Image.Height - 2 );
						break;
					case StringAlignment.Far:
						textRectangle = new Rectangle( 0, 0, this.ClientRectangle.Width - this.Image.Width - 2, this.ClientRectangle.Height );
						break;
				}
			}
	
			if ( this.Enabled == false )
			{
				MetalHelper.DrawText( this.Text, textRectangle, this.TextAlign, e.Graphics, true, ControlPaint.Dark( MetalStyleManager.Active.ForeColor ), Color.Empty );
			}
			else
			{
				MetalHelper.DrawText( this.Text, textRectangle, this.TextAlign, e.Graphics, true, Color.Empty, Color.Empty );
			}

			MetalHelper.DrawBorder( this.ClientRectangle, this.borders, e.Graphics );

		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			this.mouseDown = true;
			base.OnMouseDown (e);
			this.Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			this.mouseDown = false;
			base.OnMouseUp (e);
			this.Invalidate();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			this.mouseOver = true;
			base.OnMouseEnter (e);
			this.Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			this.mouseOver = false;
			base.OnMouseLeave (e);
			this.Invalidate();
		}


	}
}
