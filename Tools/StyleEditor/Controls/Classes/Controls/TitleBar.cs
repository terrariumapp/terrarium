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
	public class TitleBar : UserControl
	{
		/// <summary>
		/// 
		/// </summary>
		protected System.Windows.Forms.PictureBox titleBarImage;
		private System.Windows.Forms.Panel panel1;
		
		/// <summary>
		/// 
		/// </summary>
		protected Terrarium.Metal.MetalButton closeButton;
		
		/// <summary>
		/// 
		/// </summary>
		protected Terrarium.Metal.MetalButton maximizeButton;
		
		/// <summary>
		/// 
		/// </summary>
		protected Terrarium.Metal.MetalButton minimizeButton;
		
		/// <summary>
		/// 
		/// </summary>
		protected Terrarium.Metal.MetalLabel titleBarTitle;

		/// <summary>
		/// 
		/// </summary>
		public event EventHandler	CloseClicked;
		
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler	MaximizeClicked;
		
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler	MinimizeClicked;

		/// <summary>
		///  Used to store dragging information
		/// </summary>
		private bool bDragging = false;
		/// <summary>
		///  Used to determine the mouse down origin
		/// </summary>
		private Point mouseDown;
		private Terrarium.Metal.MetalPanel backgroundPanel;
		/// <summary>
		///  Initialized during construction to the parentForm if one exists
		/// </summary>
		private Form parentForm;


		/// <summary>
		/// 
		/// </summary>
		public TitleBar()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TitleBar));
			this.titleBarImage = new System.Windows.Forms.PictureBox();
			this.titleBarTitle = new Terrarium.Metal.MetalLabel();
			this.backgroundPanel = new Terrarium.Metal.MetalPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.minimizeButton = new Terrarium.Metal.MetalButton();
			this.maximizeButton = new Terrarium.Metal.MetalButton();
			this.closeButton = new Terrarium.Metal.MetalButton();
			this.backgroundPanel.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// titleBarImage
			// 
			this.titleBarImage.BackColor = System.Drawing.Color.Transparent;
			this.titleBarImage.Image = ((System.Drawing.Image)(resources.GetObject("titleBarImage.Image")));
			this.titleBarImage.Location = new System.Drawing.Point(8, 3);
			this.titleBarImage.Name = "titleBarImage";
			this.titleBarImage.Size = new System.Drawing.Size(16, 32);
			this.titleBarImage.TabIndex = 4;
			this.titleBarImage.TabStop = false;
			this.titleBarImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseUp);
			this.titleBarImage.DoubleClick += new System.EventHandler(this.backgroundPanel_DoubleClick);
			this.titleBarImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseMove);
			this.titleBarImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseDown);
			// 
			// titleBarTitle
			// 
			this.titleBarTitle.BackColor = System.Drawing.Color.Transparent;
			this.titleBarTitle.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.titleBarTitle.ForeColor = System.Drawing.Color.White;
			this.titleBarTitle.Location = new System.Drawing.Point(32, -2);
			this.titleBarTitle.Name = "titleBarTitle";
			this.titleBarTitle.NoWrap = false;
			this.titleBarTitle.Size = new System.Drawing.Size(528, 24);
			this.titleBarTitle.TabIndex = 3;
			this.titleBarTitle.Text = "Form Title";
			this.titleBarTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.titleBarTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseUp);
			this.titleBarTitle.DoubleClick += new System.EventHandler(this.backgroundPanel_DoubleClick);
			this.titleBarTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseMove);
			this.titleBarTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseDown);
			// 
			// backgroundPanel
			// 
			this.backgroundPanel.Borders = Terrarium.Metal.MetalBorders.All;
			this.backgroundPanel.Controls.Add(this.panel1);
			this.backgroundPanel.Controls.Add(this.titleBarTitle);
			this.backgroundPanel.Controls.Add(this.titleBarImage);
			this.backgroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.backgroundPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.backgroundPanel.Gradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.backgroundPanel.Location = new System.Drawing.Point(0, 0);
			this.backgroundPanel.Name = "backgroundPanel";
			this.backgroundPanel.Size = new System.Drawing.Size(640, 24);
			this.backgroundPanel.Sunk = false;
			this.backgroundPanel.TabIndex = 8;
			this.backgroundPanel.UseStyles = true;
			this.backgroundPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseUp);
			this.backgroundPanel.DoubleClick += new System.EventHandler(this.backgroundPanel_DoubleClick);
			this.backgroundPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseMove);
			this.backgroundPanel.MouseLeave += new System.EventHandler(this.backgroundPanel_MouseLeave);
			this.backgroundPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseDown);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Controls.Add(this.minimizeButton);
			this.panel1.Controls.Add(this.maximizeButton);
			this.panel1.Controls.Add(this.closeButton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel1.Location = new System.Drawing.Point(570, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(70, 24);
			this.panel1.TabIndex = 6;
			// 
			// minimizeButton
			// 
			this.minimizeButton.Borders = Terrarium.Metal.MetalBorders.All;
			this.minimizeButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(64)), ((System.Byte)(64)));
			this.minimizeButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(64)), ((System.Byte)(64)));
			this.minimizeButton.Highlight = false;
			this.minimizeButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.minimizeButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(192)));
			this.minimizeButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(64)), ((System.Byte)(0)));
			this.minimizeButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(216)), ((System.Byte)(0)));
			this.minimizeButton.Image = ((System.Drawing.Image)(resources.GetObject("minimizeButton.Image")));
			this.minimizeButton.Location = new System.Drawing.Point(0, 0);
			this.minimizeButton.Name = "minimizeButton";
			this.minimizeButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.minimizeButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.minimizeButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(216)), ((System.Byte)(0)));
			this.minimizeButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(64)), ((System.Byte)(0)));
			this.minimizeButton.Size = new System.Drawing.Size(24, 24);
			this.minimizeButton.TabIndex = 8;
			this.minimizeButton.TabStop = false;
			this.minimizeButton.Click += new System.EventHandler(this.minimizeButton_Click);
			// 
			// maximizeButton
			// 
			this.maximizeButton.Borders = Terrarium.Metal.MetalBorders.All;
			this.maximizeButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(64)), ((System.Byte)(64)));
			this.maximizeButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(64)), ((System.Byte)(64)));
			this.maximizeButton.Highlight = false;
			this.maximizeButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.maximizeButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(192)));
			this.maximizeButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(64)), ((System.Byte)(0)));
			this.maximizeButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(216)), ((System.Byte)(0)));
			this.maximizeButton.Image = ((System.Drawing.Image)(resources.GetObject("maximizeButton.Image")));
			this.maximizeButton.Location = new System.Drawing.Point(23, 0);
			this.maximizeButton.Name = "maximizeButton";
			this.maximizeButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.maximizeButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.maximizeButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(216)), ((System.Byte)(0)));
			this.maximizeButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(64)), ((System.Byte)(0)));
			this.maximizeButton.Size = new System.Drawing.Size(24, 24);
			this.maximizeButton.TabIndex = 7;
			this.maximizeButton.TabStop = false;
			this.maximizeButton.Click += new System.EventHandler(this.maximizeButton_Click);
			// 
			// closeButton
			// 
			this.closeButton.Borders = Terrarium.Metal.MetalBorders.All;
			this.closeButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(64)), ((System.Byte)(64)));
			this.closeButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(64)), ((System.Byte)(64)));
			this.closeButton.Highlight = false;
			this.closeButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.closeButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(192)));
			this.closeButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(64)), ((System.Byte)(0)));
			this.closeButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(216)), ((System.Byte)(0)));
			this.closeButton.Image = ((System.Drawing.Image)(resources.GetObject("closeButton.Image")));
			this.closeButton.Location = new System.Drawing.Point(46, 0);
			this.closeButton.Name = "closeButton";
			this.closeButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.closeButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.closeButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(216)), ((System.Byte)(0)));
			this.closeButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(64)), ((System.Byte)(0)));
			this.closeButton.Size = new System.Drawing.Size(24, 24);
			this.closeButton.TabIndex = 6;
			this.closeButton.TabStop = false;
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// TitleBar
			// 
			this.Controls.Add(this.backgroundPanel);
			this.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.Name = "TitleBar";
			this.Size = new System.Drawing.Size(640, 24);
			this.backgroundPanel.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		private void closeButton_Click(object sender, System.EventArgs e)
		{
			this.OnCloseClicked( e );
		}

		private void maximizeButton_Click(object sender, System.EventArgs e)
		{
			this.OnMaximizeClicked( e );
		}

		private void minimizeButton_Click(object sender, System.EventArgs e)
		{
			this.OnMinimizeClicked( e );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		protected virtual void OnCloseClicked( EventArgs args )
		{
			if ( this.CloseClicked != null )
			{
				this.CloseClicked( this, args );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		protected virtual void OnMaximizeClicked( EventArgs args )
		{
			if ( this.MaximizeClicked != null )
			{
				this.MaximizeClicked( this, args );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		protected virtual void OnMinimizeClicked( EventArgs args )
		{
			if ( this.MinimizeClicked != null )
			{
				this.MinimizeClicked( this, args );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Image Image
		{
			get
			{
				return this.titleBarImage.Image;
			}
			set
			{
				this.titleBarImage.Image = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			get
			{
				return this.titleBarTitle.Text;
			}
			set
			{
				this.titleBarTitle.Text = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[ DefaultValue(true) ]
		[ DesignerSerializationVisibility( DesignerSerializationVisibility.Content ) ]
		public bool ShowCloseButton
		{
			get
			{
				return this.closeButton.Visible;
			}
			set
			{
				this.closeButton.Visible = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[ DefaultValue(true) ]
		[ DesignerSerializationVisibility( DesignerSerializationVisibility.Content ) ]
		public bool ShowMaximizeButton
		{
			get
			{
				return this.maximizeButton.Visible;
			}
			set
			{
				this.maximizeButton.Visible = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[ DefaultValue(true) ]
		[ DesignerSerializationVisibility( DesignerSerializationVisibility.Content ) ]
		public bool ShowMinimizeButton
		{
			get
			{
				return this.minimizeButton.Visible;
			}
			set
			{
				this.minimizeButton.Visible = value;
			}
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

		private void backgroundPanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs me)
		{        
			if (bDragging)
			{
				if ( this.parentForm != null )
				{
					this.Cursor = Cursors.Hand;
					this.parentForm.Location = new Point((me.X - mouseDown.X) + this.parentForm.Location.X, (me.Y - mouseDown.Y) + this.parentForm.Location.Y);
				}
			}
		}

		private void backgroundPanel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if ( sender is Control )
			{
				// We handle the label and icon mouse movements in this handler as well, so
				// we need to act against the sender, rather than a specific control
				((Control)sender).Capture = false;
				bDragging = false;
			}
		}

		private void backgroundPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs me)
		{
			if ( sender is Control )
			{
				// We handle the label and icon mouse movements in this handler as well, so
				// we need to act against the sender, rather than a specific control
				((Control)sender).Capture = true;
				this.bDragging = true;
				this.mouseDown = new Point( me.X,me.Y );
			}
		}

		private void backgroundPanel_MouseLeave(object sender, System.EventArgs e)
		{
		}

		// Bubble the double click so that the parent could maximize, if they choose
		private void backgroundPanel_DoubleClick(object sender, System.EventArgs e)
		{
			this.OnDoubleClick(e);
		}

	}
}
