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
	[ DefaultProperty( "Gradient") ]
	public class GlassPanel : Panel
	{
		protected	GlassGradient		gradient;
		protected	GlassBorders		borders;
		protected	bool				isSunk = false;
		protected	bool				useStyles = true;
		protected bool isGlass = true;

		protected Image texture;

		public GlassPanel()
		{
			this.SetStyle ( ControlStyles.OptimizedDoubleBuffer | 
				ControlStyles.AllPaintingInWmPaint | 
				ControlStyles.UserPaint | 
				ControlStyles.Opaque , true); 

			this.gradient = new GlassGradient( Color.FromArgb( 96,96,96 ), Color.FromArgb( 0,0,0 ) );
			this.borders = GlassBorders.All;
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

		[ Category( "Glass UI") ]
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

		[ Category( "Glass UI") ]
		public bool IsSunk
		{
			get
			{
				return this.isSunk;
			}
			set
			{
				this.isSunk = value;
				this.Invalidate();
			}
		}

		[ Category( "Glass UI") ]
		public GlassBorders Borders
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

		[ Category( "Glass UI" ) ]
		[ DesignerSerializationVisibility(DesignerSerializationVisibility.Content) ]
		public GlassGradient Gradient
		{
			get
			{
				return this.gradient;
			}
			set
			{
				this.gradient = value;
			}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

			if (this.BackgroundImage != null)
			{
				GlassHelper.FillTexturedRectangle(rect, this.BackgroundImage, true, e.Graphics);
			}
			else
			{
				GlassGradient activeGradient = this.Gradient;

				if (this.UseStyles == true)
					activeGradient = GlassStyleManager.Active.Panel;

				GlassHelper.FillRectangle(rect, activeGradient, this.IsSunk, this.UseStyles == true ? GlassStyleManager.Active.PanelIsGlass : this.IsGlass, e.Graphics);
			}

			GlassHelper.DrawBorder( rect, this.Borders, e.Graphics);
		}
	}
}
