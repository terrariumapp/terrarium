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
	public class GlassTitleBar : UserControl
    {
		private System.Windows.Forms.Panel panel1;
		
		/// <summary>
		/// 
		/// </summary>
		protected Terrarium.Glass.GlassButton closeButton;
		
		/// <summary>
		/// 
		/// </summary>
		protected Terrarium.Glass.GlassButton maximizeButton;
		
		/// <summary>
		/// 
		/// </summary>
        protected Terrarium.Glass.GlassButton minimizeButton;

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
        /// 
        /// </summary>
        public event EventHandler BugClicked;


		/// <summary>
		///  Used to store dragging information
		/// </summary>
		private bool bDragging = false;
		/// <summary>
		///  Used to determine the mouse down origin
		/// </summary>
		private Point mouseDown;
		private Terrarium.Glass.GlassPanel backgroundPanel;
        private Terrarium.Glass.GlassButton bugButton;
        private Panel imagePanel;
        private PictureBox titleBarImage;
        private Terrarium.Glass.GlassLabel titleBarTitle;
		/// <summary>
		///  Initialized during construction to the parentForm if one exists
		/// </summary>
		private Form parentForm;


		/// <summary>
		/// 
		/// </summary>
		public GlassTitleBar()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlassTitleBar));
            this.backgroundPanel = new Terrarium.Glass.GlassPanel();
            this.titleBarTitle = new Terrarium.Glass.GlassLabel();
            this.imagePanel = new System.Windows.Forms.Panel();
            this.titleBarImage = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bugButton = new Terrarium.Glass.GlassButton();
            this.minimizeButton = new Terrarium.Glass.GlassButton();
            this.maximizeButton = new Terrarium.Glass.GlassButton();
            this.closeButton = new Terrarium.Glass.GlassButton();
            this.backgroundPanel.SuspendLayout();
            this.imagePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titleBarImage)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundPanel
            // 
            this.backgroundPanel.Borders = ((Terrarium.Glass.GlassBorders)((((Terrarium.Glass.GlassBorders.Left | Terrarium.Glass.GlassBorders.Top)
                        | Terrarium.Glass.GlassBorders.Right)
                        | Terrarium.Glass.GlassBorders.Bottom)));
            this.backgroundPanel.Controls.Add(this.titleBarTitle);
            this.backgroundPanel.Controls.Add(this.imagePanel);
            this.backgroundPanel.Controls.Add(this.panel1);
            this.backgroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.backgroundPanel.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.backgroundPanel.IsGlass = true;
            this.backgroundPanel.IsSunk = false;
            this.backgroundPanel.Location = new System.Drawing.Point(0, 0);
            this.backgroundPanel.Name = "backgroundPanel";
            this.backgroundPanel.Size = new System.Drawing.Size(640, 32);
            this.backgroundPanel.TabIndex = 8;
            this.backgroundPanel.UseStyles = true;
            this.backgroundPanel.DoubleClick += new System.EventHandler(this.backgroundPanel_DoubleClick);
            this.backgroundPanel.MouseLeave += new System.EventHandler(this.backgroundPanel_MouseLeave);
            this.backgroundPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseMove);
            this.backgroundPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseUp);
            this.backgroundPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseDown);
            // 
            // titleBarTitle
            // 
            this.titleBarTitle.BackColor = System.Drawing.Color.Transparent;
            this.titleBarTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleBarTitle.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleBarTitle.ForeColor = System.Drawing.Color.White;
            this.titleBarTitle.Location = new System.Drawing.Point(32, 0);
            this.titleBarTitle.Name = "titleBarTitle";
            this.titleBarTitle.NoWrap = false;
            this.titleBarTitle.Size = new System.Drawing.Size(442, 32);
            this.titleBarTitle.TabIndex = 8;
            this.titleBarTitle.Text = "Form Title";
            this.titleBarTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.titleBarTitle.DoubleClick += new System.EventHandler(this.backgroundPanel_DoubleClick);
            this.titleBarTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseMove);
            this.titleBarTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseUp);
            this.titleBarTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.backgroundPanel_MouseDown);
            // 
            // imagePanel
            // 
            this.imagePanel.BackColor = System.Drawing.Color.Transparent;
            this.imagePanel.Controls.Add(this.titleBarImage);
            this.imagePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.imagePanel.Location = new System.Drawing.Point(0, 0);
            this.imagePanel.Name = "imagePanel";
            this.imagePanel.Size = new System.Drawing.Size(32, 32);
            this.imagePanel.TabIndex = 7;
            // 
            // titleBarImage
            // 
            this.titleBarImage.BackColor = System.Drawing.Color.Transparent;
            this.titleBarImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleBarImage.Image = ((System.Drawing.Image)(resources.GetObject("titleBarImage.Image")));
            this.titleBarImage.Location = new System.Drawing.Point(0, 0);
            this.titleBarImage.Name = "titleBarImage";
            this.titleBarImage.Size = new System.Drawing.Size(32, 32);
            this.titleBarImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.titleBarImage.TabIndex = 5;
            this.titleBarImage.TabStop = false;
            this.titleBarImage.DoubleClick += new System.EventHandler(this.closeButton_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.bugButton);
            this.panel1.Controls.Add(this.minimizeButton);
            this.panel1.Controls.Add(this.maximizeButton);
            this.panel1.Controls.Add(this.closeButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(474, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(166, 32);
            this.panel1.TabIndex = 6;
            // 
            // bugButton
            // 
            this.bugButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.bugButton.BackColor = System.Drawing.Color.Transparent;
            this.bugButton.BorderColor = System.Drawing.Color.Black;
            this.bugButton.Depth = 3;
            this.bugButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.bugButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.bugButton.Highlight = false;
            this.bugButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.bugButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bugButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.bugButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.bugButton.IsGlass = true;
            this.bugButton.Location = new System.Drawing.Point(4, 2);
            this.bugButton.Name = "bugButton";
            this.bugButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bugButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.bugButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.bugButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.bugButton.Size = new System.Drawing.Size(75, 28);
            this.bugButton.TabIndex = 9;
            this.bugButton.TabStop = false;
            this.bugButton.Text = "File a Bug";
            this.bugButton.UseStyles = true;
            this.bugButton.UseVisualStyleBackColor = false;
            this.bugButton.Visible = false;
            this.bugButton.Click += new System.EventHandler(this.glassButton1_Click);
            // 
            // minimizeButton
            // 
            this.minimizeButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.minimizeButton.BackColor = System.Drawing.Color.Transparent;
            this.minimizeButton.BorderColor = System.Drawing.Color.Black;
            this.minimizeButton.Depth = 3;
            this.minimizeButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.minimizeButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.minimizeButton.Highlight = false;
            this.minimizeButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.minimizeButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.minimizeButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.minimizeButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.minimizeButton.Image = ((System.Drawing.Image)(resources.GetObject("minimizeButton.Image")));
            this.minimizeButton.IsGlass = true;
            this.minimizeButton.Location = new System.Drawing.Point(79, 2);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.minimizeButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.minimizeButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.minimizeButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.minimizeButton.Size = new System.Drawing.Size(28, 28);
            this.minimizeButton.TabIndex = 8;
            this.minimizeButton.TabStop = false;
            this.minimizeButton.UseStyles = true;
            this.minimizeButton.UseVisualStyleBackColor = false;
            this.minimizeButton.Click += new System.EventHandler(this.minimizeButton_Click);
            // 
            // maximizeButton
            // 
            this.maximizeButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.maximizeButton.BackColor = System.Drawing.Color.Transparent;
            this.maximizeButton.BorderColor = System.Drawing.Color.Black;
            this.maximizeButton.Depth = 3;
            this.maximizeButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.maximizeButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.maximizeButton.Highlight = false;
            this.maximizeButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.maximizeButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.maximizeButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.maximizeButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.maximizeButton.Image = ((System.Drawing.Image)(resources.GetObject("maximizeButton.Image")));
            this.maximizeButton.IsGlass = true;
            this.maximizeButton.Location = new System.Drawing.Point(107, 2);
            this.maximizeButton.Name = "maximizeButton";
            this.maximizeButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.maximizeButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.maximizeButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.maximizeButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.maximizeButton.Size = new System.Drawing.Size(28, 28);
            this.maximizeButton.TabIndex = 7;
            this.maximizeButton.TabStop = false;
            this.maximizeButton.UseStyles = true;
            this.maximizeButton.UseVisualStyleBackColor = false;
            this.maximizeButton.Click += new System.EventHandler(this.maximizeButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.BorderColor = System.Drawing.Color.Black;
            this.closeButton.Depth = 3;
            this.closeButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.closeButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.closeButton.Highlight = false;
            this.closeButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.closeButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.closeButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.closeButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.closeButton.Image = ((System.Drawing.Image)(resources.GetObject("closeButton.Image")));
            this.closeButton.IsGlass = true;
            this.closeButton.Location = new System.Drawing.Point(135, 2);
            this.closeButton.Name = "closeButton";
            this.closeButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.closeButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.closeButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.closeButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.closeButton.Size = new System.Drawing.Size(28, 28);
            this.closeButton.TabIndex = 6;
            this.closeButton.TabStop = false;
            this.closeButton.UseStyles = true;
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // GlassTitleBar
            // 
            this.Controls.Add(this.backgroundPanel);
            this.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "GlassTitleBar";
            this.Size = new System.Drawing.Size(640, 32);
            this.backgroundPanel.ResumeLayout(false);
            this.imagePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.titleBarImage)).EndInit();
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
        /// <param name="args"></param>
        protected virtual void OnBugClicked(EventArgs args)
        {
            if (this.BugClicked != null)
            {
                this.BugClicked(this, args);
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
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool ShowBugButton
        {
            get
            {
                return this.bugButton.Visible;
            }
            set
            {
                this.bugButton.Visible = value;
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

        private void glassButton1_Click(object sender, EventArgs e)
        {
            this.OnBugClicked(e);
        }

	}
}
