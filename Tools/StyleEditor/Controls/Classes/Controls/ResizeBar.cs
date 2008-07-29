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

namespace Terrarium.Forms
{
	/// <summary>
	/// 
	/// </summary>
	[ DefaultProperty( "Text") ]
	public class ResizeBar : UserControl
	{
		/// <summary>
		///  Used to store dragging information
		/// </summary>
		private bool bDragging = false;
		/// <summary>
		///  Used to determine the mouse down origin
		/// </summary>
		private Point mouseDown;
		
		private Terrarium.Metal.MetalPanel backgroundPanel;
		private System.Windows.Forms.PictureBox pictureBox1;
		/// <summary>
		///  Initialized during construction to the parentForm if one exists
		/// </summary>
		private Form parentForm;


		/// <summary>
		/// 
		/// </summary>
		public ResizeBar()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ResizeBar));
			this.backgroundPanel = new Terrarium.Metal.MetalPanel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.backgroundPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// backgroundPanel
			// 
			this.backgroundPanel.Borders = Terrarium.Metal.MetalBorders.None;
			this.backgroundPanel.Controls.Add(this.pictureBox1);
			this.backgroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.backgroundPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.backgroundPanel.Gradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.backgroundPanel.Location = new System.Drawing.Point(0, 0);
			this.backgroundPanel.Name = "backgroundPanel";
			this.backgroundPanel.Size = new System.Drawing.Size(16, 24);
			this.backgroundPanel.Sunk = false;
			this.backgroundPanel.TabIndex = 8;
			this.backgroundPanel.UseStyles = true;
			this.backgroundPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseUp);
			this.backgroundPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseMove);
			this.backgroundPanel.MouseLeave += new System.EventHandler(this.backgroundPanel_MouseLeave);
			this.backgroundPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseDown);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(16, 24);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseUp);
			this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseMove);
			this.pictureBox1.MouseLeave += new System.EventHandler(this.backgroundPanel_MouseLeave);
			this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseDown);
			// 
			// ResizeBar
			// 
			this.Controls.Add(this.backgroundPanel);
			this.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.Name = "ResizeBar";
			this.Size = new System.Drawing.Size(16, 24);
			this.backgroundPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged (e);
			
			Control c = this;
			while((c = c.Parent) != null && !(c is System.Windows.Forms.Form))
			{
			}
			if ( c is System.Windows.Forms.Form )
			{
				parentForm = c as System.Windows.Forms.Form;
			}		
		}

		private Rectangle oldRectangle = Rectangle.Empty;
		private void backgroundPanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs me)
		{
			Point p = new Point(me.X, me.Y);
			Rectangle r = this.ClientRectangle;
        
			if (bDragging)
			{
				if ( this.parentForm != null )
				{
					this.Cursor = Cursors.Hand;
					int width = this.parentForm.Size.Width;
					int height = this.parentForm.Size.Height;
					width += (me.X - mouseDown.X);
					height += (me.Y - mouseDown.Y);
					if ( oldRectangle != Rectangle.Empty )
					{
						ControlPaint.DrawReversibleFrame(oldRectangle, Color.Black, FrameStyle.Thick);
						oldRectangle = Rectangle.Empty;
					}
					oldRectangle = new Rectangle(this.parentForm.Location, new Size(width, height));
					if ( oldRectangle.Width < this.parentForm.MinimumSize.Width )
					{
						oldRectangle.Width = this.parentForm.MinimumSize.Width;
					}
					if ( oldRectangle.Height < this.parentForm.MinimumSize.Height )
					{
						oldRectangle.Height = this.parentForm.MinimumSize.Height;
					}

					ControlPaint.DrawReversibleFrame(oldRectangle, Color.Black, FrameStyle.Thick);
				}
			}
		}

		private void backgroundPanel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs me)
		{
			Point p = new Point(me.X, me.Y);
			Rectangle r = this.ClientRectangle;
        
			if (bDragging)
			{
				bDragging = false;

				if ( oldRectangle != Rectangle.Empty )
				{
					ControlPaint.DrawReversibleFrame(oldRectangle, Color.Black, FrameStyle.Thick);
					oldRectangle = Rectangle.Empty;
				}

				if ( this.parentForm != null )
				{
					this.Cursor = Cursors.Hand;
					int width = this.parentForm.Size.Width;
					int height = this.parentForm.Size.Height;
					width += (me.X - mouseDown.X);
					height += (me.Y - mouseDown.Y);
					this.parentForm.Size = new Size( width, height );
				}
			}

			this.parentForm.Invalidate( true );
			this.backgroundPanel.Capture = false;
		}

		private void backgroundPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs me)
		{
			this.backgroundPanel.Capture = true;
			this.bDragging = true;
			this.mouseDown = new Point( me.X,me.Y );
		}

		private void backgroundPanel_MouseLeave(object sender, System.EventArgs e)
		{
		}

		/// <summary>
		///  Determine if the control is in the state of being
		///  resized.  This can be used to optimize any invalidation
		///  routines.
		/// </summary>
		public bool Dragging 
		{
			get
			{
				return this.bDragging;
			}
		}
	}
}