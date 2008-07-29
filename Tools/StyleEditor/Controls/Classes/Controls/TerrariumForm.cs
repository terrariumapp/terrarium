//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;

using Terrarium.Metal;

namespace Terrarium.Forms
{
	/// <summary>
	///  Base Terrarium Form for use with Terrarium based dialogs.
	///  Controls painting of form borders, automatic set-up of
	///  a basic font, dragging functionality, and Terrarium style
	///  control boxes.
	/// </summary>
	public class TerrariumForm : System.Windows.Forms.Form
	{

		/// <summary>
		/// 
		/// </summary>
		protected Terrarium.Forms.TitleBar titleBar;
		
		/// <summary>
		/// 
		/// </summary>
		protected Terrarium.Metal.MetalPanel bottomPanel;

		/// <summary>
		/// 
		/// </summary>
		protected Terrarium.Metal.MetalPanel middlePanel;
		
		private Terrarium.Metal.MetalLabel dialogDescriptionLabel;

        /// <summary>
        ///  Creates a default TerrariumForm.  The TerrariumForm should be extended
        ///  and not created directly.
        /// </summary>
		public TerrariumForm() 
        {
			InitializeComponent();

			try
			{
				Assembly thisAssembly = typeof(TerrariumForm).Assembly;

				this.Text = "Terrarium v" + thisAssembly.GetName().Version.ToString(2);
			}
			catch{}
        }

    #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TerrariumForm));
			this.middlePanel = new Terrarium.Metal.MetalPanel();
			this.dialogDescriptionLabel = new Terrarium.Metal.MetalLabel();
			this.titleBar = new Terrarium.Forms.TitleBar();
			this.bottomPanel = new Terrarium.Metal.MetalPanel();
			this.middlePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// middlePanel
			// 
			this.middlePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.middlePanel.Borders = Terrarium.Metal.MetalBorders.None;
			this.middlePanel.Controls.Add(this.dialogDescriptionLabel);
			this.middlePanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.middlePanel.Gradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.middlePanel.Location = new System.Drawing.Point(1, 24);
			this.middlePanel.Name = "middlePanel";
			this.middlePanel.Size = new System.Drawing.Size(413, 293);
			this.middlePanel.Sunk = true;
			this.middlePanel.TabIndex = 14;
			this.middlePanel.UseStyles = true;
			// 
			// dialogDescriptionLabel
			// 
			this.dialogDescriptionLabel.BackColor = System.Drawing.Color.Transparent;
			this.dialogDescriptionLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dialogDescriptionLabel.ForeColor = System.Drawing.Color.White;
			this.dialogDescriptionLabel.Location = new System.Drawing.Point(16, 8);
			this.dialogDescriptionLabel.Name = "dialogDescriptionLabel";
			this.dialogDescriptionLabel.Size = new System.Drawing.Size(376, 32);
			this.dialogDescriptionLabel.TabIndex = 1;
			this.dialogDescriptionLabel.Text = "Form Title";
			// 
			// titleBar
			// 
			this.titleBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.titleBar.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.titleBar.ForeColor = System.Drawing.Color.White;
			this.titleBar.Image = ((System.Drawing.Image)(resources.GetObject("titleBar.Image")));
			this.titleBar.Location = new System.Drawing.Point(0, 0);
			this.titleBar.Name = "titleBar";
			this.titleBar.Size = new System.Drawing.Size(416, 24);
			this.titleBar.TabIndex = 13;
			this.titleBar.Title = "Form Title";
			// 
			// bottomPanel
			// 
			this.bottomPanel.Borders = Terrarium.Metal.MetalBorders.All;
			this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.bottomPanel.Gradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.bottomPanel.Location = new System.Drawing.Point(0, 318);
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Size = new System.Drawing.Size(416, 24);
			this.bottomPanel.Sunk = false;
			this.bottomPanel.TabIndex = 12;
			this.bottomPanel.UseStyles = true;
			// 
			// TerrariumForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Fuchsia;
			this.ClientSize = new System.Drawing.Size(416, 342);
			this.Controls.Add(this.middlePanel);
			this.Controls.Add(this.titleBar);
			this.Controls.Add(this.bottomPanel);
			this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "TerrariumForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Terrarium";
			this.middlePanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Represents the Title text displayed at the top of the dialog
		/// </summary>
		public string Title
		{
			get
			{
				return this.titleBar.Text;
			}
			set
			{
				this.titleBar.Text = value;
			}
		}

		/// <summary>
		/// Represents the brief description text displayed at the top of the dialog
		/// </summary>
		public string Description
		{
			get
			{
				return this.dialogDescriptionLabel.Text;
			}
			set
			{
				this.dialogDescriptionLabel.Text = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public TitleBar TitleBar
		{
			get
			{
				return this.titleBar;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public MetalPanel BottomBar
		{
			get
			{
				return this.bottomPanel;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
			Terrarium.Metal.MetalHelper.DrawBorder( this.ClientRectangle, MetalBorders.All, e.Graphics );
		}

	}
}