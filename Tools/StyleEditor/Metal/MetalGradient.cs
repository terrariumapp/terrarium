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
	internal class GradientConverter : ExpandableObjectConverter//TypeConverter 
	{
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, 
			object value, 
			Attribute[] filter)
		{
			return TypeDescriptor.GetProperties(value, filter);
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}
	}

	internal class GradientEditor : UITypeEditor
	{
		public override bool GetPaintValueSupported( ITypeDescriptorContext context ) 
		{
			return true;
		}

		public override void PaintValue( PaintValueEventArgs args ) 
		{
			MetalGradient gradient = (MetalGradient)args.Value;
		
			LinearGradientBrush gradientBrush = new LinearGradientBrush( args.Bounds, gradient.Top, gradient.Bottom, 90.0f );

			args.Graphics.FillRectangle( gradientBrush, args.Bounds );

			gradientBrush.Dispose();
		}
	}

	[Editor(typeof(GradientEditor), typeof(System.Drawing.Design.UITypeEditor))]
	[TypeConverter(typeof(GradientConverter))]
	public class MetalGradient
	{
		private Color		top;
		private Color		bottom;

		public Color Top
		{
			get
			{
				return this.top;
			}
			set
			{
				this.top = value;
			}
		}

		public Color Bottom
		{
			get
			{
				return this.bottom;
			}
			set
			{
				this.bottom = value;
			}
		}

		public MetalGradient()
		{
			this.top = Color.FromArgb( 128,128,128 );
			this.bottom = Color.FromArgb( 0,0,0);
		}

		public MetalGradient( Color top, Color bottom )
		{
			this.top = top;
			this.bottom = bottom;
		}
	}
}
