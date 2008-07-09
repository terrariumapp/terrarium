//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Terrarium.Client 
{
    /// <summary>
    ///  Creates a new multi-monitor blocking form.  This blocks off non-primary
    ///  monitors when running as as screen saver
    /// </summary>
    internal class Block : Form
    {
        /// <summary>
        ///  Create a new Block Form that represents the primary screen.
        /// </summary>
        internal Block()
        {
			InitializeComponent();
        }

		private void InitializeComponent()
		{
			// 
			// Block
			// 

            this.AutoScaleDimensions = new System.Drawing.SizeF(5, 13);
            this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(640, 480);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Block";
			this.ShowInTaskbar = false;

		}
    
		/// <summary>
		/// Enumerates screens and creates an instance of Block for
		/// all screens other than primary. Primary is usually the 
		/// screen that the screen saver will run on.
		/// </summary>
		/// <param name="primary">The screen you wish to not block.</param>
		internal static void BlockScreens( Screen primary )
		{
			foreach( Screen alternate in Screen.AllScreens )
			{
				if ( alternate.Bounds == primary.Bounds )
					continue;

				Block b = new Block();
				b.ClientSize = alternate.Bounds.Size;
				b.BackColor = Color.Black;
				b.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
				b.Show();
				b.Left = alternate.Bounds.Left;
			}
		}
    }
}