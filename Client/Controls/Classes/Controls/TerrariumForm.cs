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

using Terrarium.Glass;

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
        protected Terrarium.Forms.GlassTitleBar titleBar;
        /// <summary>
        /// 
        /// </summary>
        protected GlassPanel bottomPanel;
        
        private GlassLabel dialogDescriptionLabel;

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

			this.BackColor = GlassStyleManager.Active.DialogColor;
        }

    #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerrariumForm));
            this.titleBar = new Terrarium.Forms.GlassTitleBar();
            this.dialogDescriptionLabel = new Terrarium.Glass.GlassLabel();
            this.bottomPanel = new Terrarium.Glass.GlassPanel();
            this.SuspendLayout();
            // 
            // titleBar
            // 
            this.titleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleBar.ForeColor = System.Drawing.Color.White;
            this.titleBar.Image = ((System.Drawing.Image)(resources.GetObject("titleBar.Image")));
            this.titleBar.Location = new System.Drawing.Point(0, 0);
            this.titleBar.Name = "titleBar";
            this.titleBar.Size = new System.Drawing.Size(261, 32);
            this.titleBar.TabIndex = 13;
            this.titleBar.Title = "Form Title";
            // 
            // dialogDescriptionLabel
            // 
            this.dialogDescriptionLabel.BackColor = System.Drawing.Color.Transparent;
            this.dialogDescriptionLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dialogDescriptionLabel.ForeColor = System.Drawing.Color.White;
            this.dialogDescriptionLabel.Location = new System.Drawing.Point(12, 35);
            this.dialogDescriptionLabel.Name = "dialogDescriptionLabel";
            this.dialogDescriptionLabel.NoWrap = false;
            this.dialogDescriptionLabel.Size = new System.Drawing.Size(237, 32);
            this.dialogDescriptionLabel.TabIndex = 14;
            this.dialogDescriptionLabel.Text = "Form Title";
            // 
            // bottomPanel
            // 
            this.bottomPanel.Borders = ((Terrarium.Glass.GlassBorders)((((Terrarium.Glass.GlassBorders.Left | Terrarium.Glass.GlassBorders.Top)
                        | Terrarium.Glass.GlassBorders.Right)
                        | Terrarium.Glass.GlassBorders.Bottom)));
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bottomPanel.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.bottomPanel.IsGlass = true;
            this.bottomPanel.IsSunk = false;
            this.bottomPanel.Location = new System.Drawing.Point(0, 302);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(261, 40);
            this.bottomPanel.TabIndex = 15;
            this.bottomPanel.UseStyles = true;
            // 
            // TerrariumForm
            // 
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.ClientSize = new System.Drawing.Size(261, 342);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.dialogDescriptionLabel);
            this.Controls.Add(this.titleBar);
            this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TerrariumForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Terrarium";
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
		public GlassTitleBar TitleBar
		{
			get
			{
				return this.titleBar;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public GlassPanel BottomBar
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
			Terrarium.Glass.GlassHelper.DrawBorder( this.ClientRectangle, GlassBorders.All, e.Graphics );
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.dialogDescriptionLabel.Width = this.Width - 24;
        }
	}
}