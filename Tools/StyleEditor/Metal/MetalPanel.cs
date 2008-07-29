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
	[ DefaultProperty( "Gradient") ]
	public class MetalPanel : Panel
	{
		protected	MetalGradient		gradient;
		protected	MetalBorders	borders;
		protected	bool			sunk = false;
		protected	bool			useStyles = true;

		public MetalPanel()
		{
			this.SetStyle ( ControlStyles.DoubleBuffer | 
				ControlStyles.AllPaintingInWmPaint | 
				ControlStyles.UserPaint | 
				ControlStyles.Opaque , true); 

			this.gradient = new MetalGradient( Color.FromArgb( 96,96,96 ), Color.FromArgb( 0,0,0 ) );
			this.borders = MetalBorders.All;
		}

		[ Category( "Metal UI") ]
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

		[ Category( "Metal UI") ]
		public bool Sunk
		{
			get
			{
				return this.sunk;
			}
			set
			{
				this.sunk = value;
				this.Invalidate();
			}
		}

		[ Category( "Metal UI") ]
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

		[ Category( "Metal UI" ) ]
		[ DesignerSerializationVisibility(DesignerSerializationVisibility.Content) ]
		public MetalGradient Gradient
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
			MetalGradient targetGradient;

			if ( MetalStyleManager.UseStyles == true && this.UseStyles == true )
			{
				if ( sunk == true )
					targetGradient = MetalStyleManager.Active.PanelSunk;
				else
					targetGradient = MetalStyleManager.Active.PanelRaised;
			}
			else
			{
				if ( sunk == true )
				{
					targetGradient = new MetalGradient();
					targetGradient.Top = this.gradient.Bottom;
					targetGradient.Bottom = this.gradient.Top;
				}
				else
				{
					targetGradient = new MetalGradient();
					targetGradient.Top = this.gradient.Top;
					targetGradient.Bottom = this.gradient.Bottom;
				}
			}

			Rectangle rect = new Rectangle( 0, 0, this.Width, this.Height );

			MetalHelper.DrawGradient( rect, targetGradient, e.Graphics );
				
			MetalHelper.DrawBorder( rect, this.Borders, e.Graphics );
		}
	}
}
