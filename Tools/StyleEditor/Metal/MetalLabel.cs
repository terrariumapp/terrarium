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
	public class MetalLabel : System.Windows.Forms.Label
	{
		protected bool		noWrap = false;

		public MetalLabel()
		{
			this.BackColor = Color.Transparent;
			this.ForeColor = Color.White;
			this.Font = new Font( "Verdana", 6.75f, FontStyle.Bold );
		}

		public bool NoWrap
		{
			get
			{
				return this.noWrap;
			}
			set
			{
				this.noWrap = value;
				this.Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			MetalHelper.DrawText( this.Text, this.ClientRectangle, this.TextAlign, e.Graphics, this.noWrap, Color.Empty, Color.Empty );
		}

	}
}
