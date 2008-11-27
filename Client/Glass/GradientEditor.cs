using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Drawing2D;

namespace Terrarium.Glass
{
    internal class GradientEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs args)
        {
            var gradient = (GlassGradient) args.Value;

            var gradientBrush = new LinearGradientBrush(args.Bounds, gradient.Top, gradient.Bottom,
                                                        90.0f);

            args.Graphics.FillRectangle(gradientBrush, args.Bounds);

            gradientBrush.Dispose();
        }
    }
}