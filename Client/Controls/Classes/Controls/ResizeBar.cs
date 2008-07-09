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
		private PictureBox pictureBox1;
        private Terrarium.Glass.GlassPanel glassPanel1;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResizeBar));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.glassPanel1 = new Terrarium.Glass.GlassPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.glassPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseUp);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseDown);
            // 
            // glassPanel1
            // 
            this.glassPanel1.Borders = ((Terrarium.Glass.GlassBorders)(((Terrarium.Glass.GlassBorders.Top | Terrarium.Glass.GlassBorders.Right)
                        | Terrarium.Glass.GlassBorders.Bottom)));
            this.glassPanel1.Controls.Add(this.pictureBox1);
            this.glassPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glassPanel1.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.glassPanel1.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.glassPanel1.IsGlass = true;
            this.glassPanel1.IsSunk = false;
            this.glassPanel1.Location = new System.Drawing.Point(0, 0);
            this.glassPanel1.Name = "glassPanel1";
            this.glassPanel1.Size = new System.Drawing.Size(20, 48);
            this.glassPanel1.TabIndex = 2;
            this.glassPanel1.UseStyles = true;
            // 
            // ResizeBar
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.glassPanel1);
            this.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "ResizeBar";
            this.Size = new System.Drawing.Size(20, 48);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.glassPanel1.ResumeLayout(false);
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
			this.Capture = false;
		}

		private void backgroundPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs me)
		{
			this.Capture = true;
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