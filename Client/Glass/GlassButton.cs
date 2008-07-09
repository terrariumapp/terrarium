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

namespace Terrarium.Glass
{
	[ DefaultProperty( "NormalGradient") ]
	public class GlassButton : System.Windows.Forms.ButtonBase
	{
		protected GlassGradient		normalGradient;
		protected GlassGradient		hoverGradient;
		protected GlassGradient		pressedGradient;
		protected GlassGradient		disabledGradient;
		protected GlassGradient		highlightGradient;

		protected bool			mouseOver = false;
		protected bool			mouseDown = false;

		protected bool			highlight = false;

		protected Color borderColor = Color.Black;
		protected int depth = 3;
		protected bool useStyles = true;
		protected bool isGlass = true;

		public GlassButton()
		{
			this.normalGradient = new GlassGradient( Color.FromArgb( 96,96,96 ), Color.FromArgb( 0,0,0 ) );
			this.hoverGradient = new GlassGradient( Color.FromArgb( 0,216,0 ), Color.FromArgb( 0,64,0 ) );
			this.pressedGradient = new GlassGradient( Color.FromArgb( 0,64,0 ), Color.FromArgb( 0,216,0 ) );
			this.disabledGradient = new GlassGradient( Color.FromArgb( 64, 64, 64 ), Color.FromArgb( 64, 64, 64 ) );
			this.highlightGradient = new GlassGradient( Color.FromArgb( 192, 192, 192 ), Color.FromArgb( 96, 96, 96 ) );
		
			this.TabStop = false;
			this.SetStyle( ControlStyles.Selectable, false );
			this.SetStyle(ControlStyles.Opaque, false);

			this.borderColor = Color.Black;
			this.depth = 3;

			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = Color.Transparent;
		}

		[ Category("Glass UI") ]
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

		[ Category("Glass UI") ]
		[ DesignerSerializationVisibility(DesignerSerializationVisibility.Content) ]
		public GlassGradient NormalGradient
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

		[ Category("Glass UI") ]
		[ DesignerSerializationVisibility(DesignerSerializationVisibility.Content) ]
		public GlassGradient HoverGradient
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

		[ Category("Glass UI") ]
		[ DesignerSerializationVisibility(DesignerSerializationVisibility.Content) ]
		public GlassGradient PressedGradient
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

		[ Category("Glass UI") ]
		[ DesignerSerializationVisibility(DesignerSerializationVisibility.Content) ]
		public GlassGradient DisabledGradient
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
		
		[ Category("Glass UI") ]
		[ DesignerSerializationVisibility(DesignerSerializationVisibility.Content) ]
		public GlassGradient HighlightGradient
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

		[Category("Glass UI")]
		public Color BorderColor
		{
			get
			{
				return this.borderColor;
			}
			set
			{
				this.borderColor = value;
				this.Invalidate();
			}
		}

		[Category("Glass UI")]
		public int Depth
		{
			get
			{
				return this.depth;
			}
			set
			{
				this.depth = value;
				this.Invalidate();
			}
		}

		[Category("Glass UI")]
		public bool UseStyles
		{
			get
			{
				return this.useStyles;
			}
			set
			{
				this.useStyles = value;
				this.Invalidate();
			}
		}

		[Category("Glass UI")]
		public bool IsGlass
		{
			get
			{
				return this.isGlass;
			}
			set
			{
				this.isGlass = value;
				this.Invalidate();
			}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			base.OnPaintBackground(pevent);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			GlassGradient activeGradient = normalGradient;

			if (this.UseStyles == true)
			{
				if (this.Enabled == false)
					activeGradient = GlassStyleManager.Active.ButtonDisabled;
				else if (this.mouseDown == true)
					activeGradient = GlassStyleManager.Active.ButtonPressed;
				else if (this.mouseOver == true)
					activeGradient = GlassStyleManager.Active.ButtonHover;
				else if (this.highlight == true)
					activeGradient = GlassStyleManager.Active.ButtonHighlight;
				else
					activeGradient = GlassStyleManager.Active.ButtonNormal;
			}
			else
			{
				if (this.Enabled == false)
					activeGradient = disabledGradient;
				else if (this.mouseDown == true)
					activeGradient = pressedGradient;
				else if (this.mouseOver == true)
					activeGradient = hoverGradient;
				else if (this.highlight == true)
					activeGradient = highlightGradient;
				else
					activeGradient = normalGradient;
			}

			e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			Rectangle clientRectangle = this.ClientRectangle;
			clientRectangle.Inflate(-1, -1);

			GlassGradient sunk = null;
			bool activeIsGlass = true;

			sunk = new GlassGradient(Color.FromArgb(48, Color.White), Color.FromArgb(160, Color.Black));
			if (this.Parent is GlassPanel)
			{
				GlassPanel parent = (GlassPanel)this.Parent;
				if (parent.UseStyles == true)
				{
					activeIsGlass = GlassStyleManager.Active.PanelIsGlass;
				}
				else
				{
					activeIsGlass = parent.IsGlass;
				}
			}
			else
			{
				activeIsGlass = false;
			}

			GlassHelper.FillRoundedRectangle(clientRectangle, sunk, true, activeIsGlass, e.Graphics);

			Rectangle rectangle = clientRectangle;
			rectangle.Inflate(-this.depth, -this.depth);

			if (this.UseStyles == true)
				activeIsGlass = GlassStyleManager.Active.ButtonIsGlass;
			else
				activeIsGlass = this.IsGlass;

			GlassHelper.FillRoundedRectangle(rectangle, activeGradient, false, activeIsGlass, e.Graphics);

			e.Graphics.SetClip(this.ClientRectangle);

			GlassHelper.DrawRoundedBorder(rectangle, this.BorderColor, this.Depth / 4.0f, e.Graphics);

			StringAlignment vertical = StringAlignment.Center;
			StringAlignment horizontal = StringAlignment.Center;

			if (this.Image != null)
			{
				switch (this.ImageAlign)
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

				switch (horizontal)
				{
					case StringAlignment.Near:
						imageLeft = 4;
						break;
					case StringAlignment.Center:
						imageLeft = 1 + (this.ClientRectangle.Width / 2 - this.Image.Width / 2);
						break;
					case StringAlignment.Far:
						imageLeft = this.Width - this.Image.Width - 4;
						break;
				}

				switch (vertical)
				{
					case StringAlignment.Near:
						imageTop = 2;
						break;
					case StringAlignment.Center:
						imageTop = 0 + (this.ClientRectangle.Height / 2 - this.Image.Height / 2);
						break;
					case StringAlignment.Far:
						imageTop = this.Height - this.Image.Height - 2;
						break;
				}

				if (this.Enabled == false)
				{
					ControlPaint.DrawImageDisabled(e.Graphics, this.Image, imageLeft, imageTop, GlassStyleManager.Active.ButtonDisabled.Bottom);
				}
				else
				{
					if (this.mouseDown == true)
					{
						imageLeft++;
						imageTop++;
					}
					
					e.Graphics.DrawImage(this.Image, imageLeft, imageTop, this.Image.Width, this.Image.Height);
				}
			}

			Rectangle textRectangle = this.ClientRectangle;
			int textPadding = this.Depth + (this.Depth/4) + 2;

			textRectangle.Inflate(-textPadding, -textPadding);

			if (this.Image != null)
			{
				switch (horizontal)
				{
					case StringAlignment.Near:
						textRectangle = new Rectangle(this.Image.Width + 4, 0, this.ClientRectangle.Width - this.Image.Width - 2, this.ClientRectangle.Height);
						break;
					case StringAlignment.Center:
						textRectangle = new Rectangle(0, this.Image.Height + 2, this.ClientRectangle.Width, this.ClientRectangle.Height - this.Image.Height - 2);
						break;
					case StringAlignment.Far:
						textRectangle = new Rectangle(0, 0, this.ClientRectangle.Width - this.Image.Width - 2, this.ClientRectangle.Height);
						break;
				}
			}

			if (this.Enabled == false)
			{
				GlassHelper.DrawText(this.Text, textRectangle, this.TextAlign, e.Graphics, true, ControlPaint.Dark(GlassStyleManager.Active.ForeColor), Color.Empty);
			}
			else
			{
				if (this.mouseDown == true)
				{
					textRectangle.Offset(1, 1);
				}

				GlassHelper.DrawText(this.Text, textRectangle, this.TextAlign, e.Graphics, true, Color.Empty, Color.Empty);
			}

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
