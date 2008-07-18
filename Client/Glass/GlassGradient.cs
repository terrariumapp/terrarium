//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;

namespace Terrarium.Glass
{
    internal class GradientConverter : ExpandableObjectConverter
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
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs args)
        {
            GlassGradient gradient = (GlassGradient) args.Value;

            LinearGradientBrush gradientBrush = new LinearGradientBrush(args.Bounds, gradient.Top, gradient.Bottom,
                                                                        90.0f);

            args.Graphics.FillRectangle(gradientBrush, args.Bounds);

            gradientBrush.Dispose();
        }
    }

    [Editor(typeof (GradientEditor), typeof (UITypeEditor))]
    [TypeConverter(typeof (GradientConverter))]
    public class GlassGradient
    {
        private Color bottom;
        private Color top;

        public GlassGradient()
        {
            top = Color.FromArgb(128, 128, 128);
            bottom = Color.FromArgb(0, 0, 0);
        }

        public GlassGradient(Color top, Color bottom)
        {
            this.top = top;
            this.bottom = bottom;
        }

        public Color Top
        {
            get { return top; }
            set { top = value; }
        }

        public Color Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }

        public Brush GetBrush(Rectangle rectangle)
        {
            try
            {
                return new LinearGradientBrush(rectangle, Top, Bottom, 90.0f);
            }
            catch
            {
                return Brushes.Transparent;
            }
        }
    }
}