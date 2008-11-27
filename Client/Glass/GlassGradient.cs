//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;

namespace Terrarium.Glass
{
    [Editor(typeof (GradientEditor), typeof (UITypeEditor))]
    [TypeConverter(typeof (GradientConverter))]
    public class GlassGradient
    {
        public GlassGradient()
        {
            Top = Color.FromArgb(128, 128, 128);
            Bottom = Color.FromArgb(0, 0, 0);
        }

        public GlassGradient(Color top, Color bottom)
        {
            Top = top;
            Bottom = bottom;
        }

        public Color Top { get; set; }

        public Color Bottom { get; set; }

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