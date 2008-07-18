//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System.Drawing;
using System.Windows.Forms;

namespace Terrarium.Glass
{
    public class GlassLabel : Label
    {
        protected bool noWrap;

        public GlassLabel()
        {
            BackColor = Color.Transparent;
            ForeColor = Color.White;
            Font = new Font("Verdana", 6.75f, FontStyle.Bold);
        }

        public bool NoWrap
        {
            get { return noWrap; }
            set
            {
                noWrap = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            GlassHelper.DrawText(Text, ClientRectangle, TextAlign, e.Graphics, noWrap, Color.Empty, Color.Empty);
        }
    }
}