//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace Terrarium.Metal
{
	public sealed class MetalHelper
	{
		private MetalHelper(){}

		public static void DrawGradient( Rectangle rectangle, MetalGradient gradient, Graphics graphics )
		{
			if ( rectangle.Width == 0 || rectangle.Height == 0 )
				return;

			LinearGradientBrush brush = new LinearGradientBrush( rectangle, gradient.Top, gradient.Bottom, 90.0f );

			graphics.FillRectangle( brush, rectangle );

			brush.Dispose();
		}

		public static void DrawBorder( Rectangle rectangle, MetalBorders borders, Graphics graphics )
		{
			if ( rectangle.Width == 0 || rectangle.Height == 0 )
				return;

			Pen borderPen = new Pen( MetalStyleManager.Active.BorderColor );

			if ( (borders & MetalBorders.Left) > 0 )
				graphics.DrawLine( borderPen, 0, 0, 0, rectangle.Height - 1 );
			if ( (borders & MetalBorders.Top) > 0 )
				graphics.DrawLine( borderPen, 0, 0, rectangle.Width - 1, 0 );
			if ( (borders & MetalBorders.Right) > 0 )
				graphics.DrawLine( borderPen, rectangle.Width - 1, 0, rectangle.Width - 1, rectangle.Height - 1 );
			if ( (borders & MetalBorders.Bottom) > 0 )
				graphics.DrawLine( borderPen, 0, rectangle.Height - 1, rectangle.Width - 1, rectangle.Height - 1 );

			borderPen.Dispose();

		}

		public static void DrawText( string text, Rectangle rectangle, ContentAlignment contentAlignment, Graphics graphics, bool noWrap, Color foreColor, Color shadowColor )
		{
			StringFormat stringFormat = new StringFormat();

			switch( contentAlignment )
			{
				case ContentAlignment.BottomCenter:
				{
					stringFormat.Alignment = StringAlignment.Center;
					stringFormat.LineAlignment = StringAlignment.Far;
					break;
				}
				case ContentAlignment.BottomLeft:
				{
					stringFormat.Alignment = StringAlignment.Near;
					stringFormat.LineAlignment = StringAlignment.Far;
					break;
				}
				case ContentAlignment.BottomRight:
				{
					stringFormat.Alignment = StringAlignment.Far;
					stringFormat.LineAlignment = StringAlignment.Far;
					break;
				}

				case ContentAlignment.MiddleCenter:
				{
					stringFormat.Alignment = StringAlignment.Center;
					stringFormat.LineAlignment = StringAlignment.Center;
					break;
				}
				case ContentAlignment.MiddleLeft:
				{
					stringFormat.Alignment = StringAlignment.Near;
					stringFormat.LineAlignment = StringAlignment.Center;
					break;
				}
				case ContentAlignment.MiddleRight:
				{
					stringFormat.Alignment = StringAlignment.Far;
					stringFormat.LineAlignment = StringAlignment.Center;
					break;
				}

				case ContentAlignment.TopCenter:
				{
					stringFormat.Alignment = StringAlignment.Center;
					stringFormat.LineAlignment = StringAlignment.Near;
					break;
				}
				case ContentAlignment.TopLeft:
				{
					stringFormat.Alignment = StringAlignment.Near;
					stringFormat.LineAlignment = StringAlignment.Near;
					break;
				}
				case ContentAlignment.TopRight:
				{
					stringFormat.Alignment = StringAlignment.Far;
					stringFormat.LineAlignment = StringAlignment.Near;
					break;
				}
			}

			if ( noWrap == true )
				stringFormat.FormatFlags |= StringFormatFlags.NoWrap;

			stringFormat.Trimming = StringTrimming.EllipsisCharacter;

			if ( MetalStyleManager.Active.FontShadow == true )
			{
				RectangleF shadowRectangle = new RectangleF( rectangle.X+1, rectangle.Y+1, rectangle.Width, rectangle.Height );
				
				SolidBrush shadowBrush = new SolidBrush( shadowColor == Color.Empty ? Color.Black : shadowColor );
	
				graphics.DrawString( text, MetalStyleManager.Active.Font, shadowBrush, shadowRectangle, stringFormat );

				shadowBrush.Dispose();
				shadowBrush = null;
			}

			RectangleF normalRectangle = new RectangleF( rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height );

			SolidBrush fontBrush = new SolidBrush( foreColor == Color.Empty ? MetalStyleManager.Active.ForeColor : foreColor );

			graphics.DrawString( text, MetalStyleManager.Active.Font, fontBrush, normalRectangle, stringFormat );

			fontBrush.Dispose();
		}
	}
}
