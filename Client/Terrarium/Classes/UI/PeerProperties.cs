//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Windows.Forms;

using Terrarium.Configuration;
using Terrarium.Tools;
using Terrarium.Forms;
using Terrarium.Services.Discovery;

using Terrarium.Glass;

namespace Terrarium.Client 
{
	/// <summary>
	///  PeerProperties dialog used to give access to various GameConfig values
	///  from a user interface.
	/// </summary>
	public class PeerProperties : Terrarium.Forms.TerrariumForm
	{
		#region Designer Generated Fields

		private System.Windows.Forms.CheckBox chkCommunityRegionInfo;
		private Terrarium.Glass.GlassButton cancelButton;
		private Terrarium.Glass.GlassButton okButton;
		private Terrarium.Glass.GlassLabel lblPerformanceDescription;
		private Terrarium.Glass.GlassLabel lblPerformanceSlider;
		private System.Windows.Forms.TrackBar tbrPerformanceSlider;
		private Terrarium.Glass.GlassLabel lblPerformanceSliderResult;
		private Terrarium.Glass.GlassLabel lblPerformanceEnableLogging;
		private System.Windows.Forms.RadioButton radPerformanceNoLogging;
		private System.Windows.Forms.RadioButton radPerformanceSelectedLogging;
		private System.Windows.Forms.RadioButton radPerformanceAllLogging;
		private Terrarium.Glass.GlassPanel performancePanel;
		private Terrarium.Glass.GlassButton graphicsButton;
		private Terrarium.Glass.GlassButton performanceButton;
		private Terrarium.Glass.GlassButton serverButton;
		private Terrarium.Glass.GlassLabel GlassLabel1;
		private Terrarium.Glass.GlassLabel GlassLabel2;
		private Terrarium.Glass.GlassLabel GlassLabel3;
		private GlassPanel serverPanel;
		private GlassLabel GlassLabel5;
		private TextBox txtPeerChannel;
		private GlassLabel lblServerDescription;
		private GlassLabel lblServerURI;
		private TextBox txtServerURI;
		private GlassLabel lblServerEnableNat;
		private CheckBox chkServerEnableNat;
		private GlassPanel graphicsPanel;
		private CheckBox chkStartFullscreen;
		private GlassLabel GlassLabel6;
		private ComboBox styleComboBox;
		private GlassLabel GlassLabel4;
		private GlassLabel lblGraphicsDescription;
		private CheckBox chkGraphicsDrawScreen;
		private CheckBox chkGraphicsBoundingBoxes;
		private CheckBox chkGraphicsBackgroundGrid;
		private CheckBox chkGraphicsDestinationLines;
		private GlassLabel lblGraphicsDrawScreen;
		private GlassLabel lblGraphicsBoundingBoxes;
		private GlassLabel lblGraphicsBackgroundGrid;
		private GlassLabel lblGraphicsDestinationLines;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		/// <summary>
		///  Creates a new instance of the peer properties form and enables
		///  the designer.
		/// </summary>
		public PeerProperties()
		{
			InitializeComponent();
			this.titleBar.ShowMaximizeButton = false;
			this.titleBar.ShowMinimizeButton = false;

			this.BackColor = GlassStyleManager.Active.DialogColor;
			this.serverButton_Click( this, new EventArgs() );

		}

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.chkCommunityRegionInfo = new System.Windows.Forms.CheckBox();
			this.cancelButton = new Terrarium.Glass.GlassButton();
			this.okButton = new Terrarium.Glass.GlassButton();
			this.graphicsButton = new Terrarium.Glass.GlassButton();
			this.performanceButton = new Terrarium.Glass.GlassButton();
			this.serverButton = new Terrarium.Glass.GlassButton();
			this.performancePanel = new Terrarium.Glass.GlassPanel();
			this.GlassLabel3 = new Terrarium.Glass.GlassLabel();
			this.GlassLabel2 = new Terrarium.Glass.GlassLabel();
			this.GlassLabel1 = new Terrarium.Glass.GlassLabel();
			this.lblPerformanceDescription = new Terrarium.Glass.GlassLabel();
			this.lblPerformanceSlider = new Terrarium.Glass.GlassLabel();
			this.tbrPerformanceSlider = new System.Windows.Forms.TrackBar();
			this.lblPerformanceSliderResult = new Terrarium.Glass.GlassLabel();
			this.lblPerformanceEnableLogging = new Terrarium.Glass.GlassLabel();
			this.radPerformanceNoLogging = new System.Windows.Forms.RadioButton();
			this.radPerformanceSelectedLogging = new System.Windows.Forms.RadioButton();
			this.radPerformanceAllLogging = new System.Windows.Forms.RadioButton();
			this.serverPanel = new Terrarium.Glass.GlassPanel();
			this.GlassLabel5 = new Terrarium.Glass.GlassLabel();
			this.txtPeerChannel = new System.Windows.Forms.TextBox();
			this.lblServerDescription = new Terrarium.Glass.GlassLabel();
			this.lblServerURI = new Terrarium.Glass.GlassLabel();
			this.txtServerURI = new System.Windows.Forms.TextBox();
			this.lblServerEnableNat = new Terrarium.Glass.GlassLabel();
			this.chkServerEnableNat = new System.Windows.Forms.CheckBox();
			this.graphicsPanel = new Terrarium.Glass.GlassPanel();
			this.chkStartFullscreen = new System.Windows.Forms.CheckBox();
			this.GlassLabel6 = new Terrarium.Glass.GlassLabel();
			this.styleComboBox = new System.Windows.Forms.ComboBox();
			this.GlassLabel4 = new Terrarium.Glass.GlassLabel();
			this.lblGraphicsDescription = new Terrarium.Glass.GlassLabel();
			this.chkGraphicsDrawScreen = new System.Windows.Forms.CheckBox();
			this.chkGraphicsBoundingBoxes = new System.Windows.Forms.CheckBox();
			this.chkGraphicsBackgroundGrid = new System.Windows.Forms.CheckBox();
			this.chkGraphicsDestinationLines = new System.Windows.Forms.CheckBox();
			this.lblGraphicsDrawScreen = new Terrarium.Glass.GlassLabel();
			this.lblGraphicsBoundingBoxes = new Terrarium.Glass.GlassLabel();
			this.lblGraphicsBackgroundGrid = new Terrarium.Glass.GlassLabel();
			this.lblGraphicsDestinationLines = new Terrarium.Glass.GlassLabel();
			this.bottomPanel.SuspendLayout();
			this.performancePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbrPerformanceSlider)).BeginInit();
			this.serverPanel.SuspendLayout();
			this.graphicsPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// titleBar
			// 
			this.titleBar.Size = new System.Drawing.Size(409, 32);
			this.titleBar.Title = "Game Settings";
			this.titleBar.CloseClicked += new System.EventHandler(this.Cancel_Click);
			// 
			// bottomPanel
			// 
			this.bottomPanel.Controls.Add(this.cancelButton);
			this.bottomPanel.Controls.Add(this.okButton);
			this.bottomPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.bottomPanel.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.bottomPanel.Location = new System.Drawing.Point(0, 374);
			this.bottomPanel.Size = new System.Drawing.Size(409, 40);
			// 
			// chkCommunityRegionInfo
			// 
			this.chkCommunityRegionInfo.Location = new System.Drawing.Point(0, 0);
			this.chkCommunityRegionInfo.Name = "chkCommunityRegionInfo";
			this.chkCommunityRegionInfo.Size = new System.Drawing.Size(104, 24);
			this.chkCommunityRegionInfo.TabIndex = 0;
			// 
			// cancelButton
			// 
			this.cancelButton.BackColor = System.Drawing.Color.Transparent;
			this.cancelButton.BorderColor = System.Drawing.Color.Black;
			this.cancelButton.Depth = 3;
			this.cancelButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.cancelButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.cancelButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancelButton.ForeColor = System.Drawing.Color.White;
			this.cancelButton.Highlight = false;
			this.cancelButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.cancelButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.cancelButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.cancelButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.cancelButton.IsGlass = true;
			this.cancelButton.Location = new System.Drawing.Point(322, 2);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.cancelButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.cancelButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.cancelButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.cancelButton.Size = new System.Drawing.Size(75, 36);
			this.cancelButton.TabIndex = 19;
			this.cancelButton.TabStop = false;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseStyles = true;
			this.cancelButton.UseVisualStyleBackColor = false;
			this.cancelButton.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// okButton
			// 
			this.okButton.BackColor = System.Drawing.Color.Transparent;
			this.okButton.BorderColor = System.Drawing.Color.Black;
			this.okButton.Depth = 3;
			this.okButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.okButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.okButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.okButton.ForeColor = System.Drawing.Color.White;
			this.okButton.Highlight = false;
			this.okButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.okButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.okButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.okButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.okButton.IsGlass = true;
			this.okButton.Location = new System.Drawing.Point(241, 2);
			this.okButton.Name = "okButton";
			this.okButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.okButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.okButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.okButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.okButton.Size = new System.Drawing.Size(75, 36);
			this.okButton.TabIndex = 18;
			this.okButton.TabStop = false;
			this.okButton.Text = "OK";
			this.okButton.UseStyles = true;
			this.okButton.UseVisualStyleBackColor = false;
			this.okButton.Click += new System.EventHandler(this.OK_Click);
			// 
			// graphicsButton
			// 
			this.graphicsButton.BackColor = System.Drawing.Color.Transparent;
			this.graphicsButton.BorderColor = System.Drawing.Color.Black;
			this.graphicsButton.Depth = 3;
			this.graphicsButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.graphicsButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.graphicsButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.graphicsButton.ForeColor = System.Drawing.Color.White;
			this.graphicsButton.Highlight = false;
			this.graphicsButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.graphicsButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.graphicsButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.graphicsButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.graphicsButton.IsGlass = true;
			this.graphicsButton.Location = new System.Drawing.Point(91, 63);
			this.graphicsButton.Name = "graphicsButton";
			this.graphicsButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.graphicsButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.graphicsButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.graphicsButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.graphicsButton.Size = new System.Drawing.Size(90, 52);
			this.graphicsButton.TabIndex = 21;
			this.graphicsButton.TabStop = false;
			this.graphicsButton.Text = "Graphics";
			this.graphicsButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.graphicsButton.UseStyles = true;
			this.graphicsButton.UseVisualStyleBackColor = false;
			this.graphicsButton.Click += new System.EventHandler(this.graphicsButton_Click);
			// 
			// performanceButton
			// 
			this.performanceButton.BackColor = System.Drawing.Color.Transparent;
			this.performanceButton.BorderColor = System.Drawing.Color.Black;
			this.performanceButton.Depth = 3;
			this.performanceButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.performanceButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.performanceButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.performanceButton.ForeColor = System.Drawing.Color.White;
			this.performanceButton.Highlight = false;
			this.performanceButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.performanceButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.performanceButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.performanceButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.performanceButton.IsGlass = true;
			this.performanceButton.Location = new System.Drawing.Point(183, 63);
			this.performanceButton.Name = "performanceButton";
			this.performanceButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.performanceButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.performanceButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.performanceButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.performanceButton.Size = new System.Drawing.Size(118, 52);
			this.performanceButton.TabIndex = 22;
			this.performanceButton.TabStop = false;
			this.performanceButton.Text = "Performance";
			this.performanceButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.performanceButton.UseStyles = true;
			this.performanceButton.UseVisualStyleBackColor = false;
			this.performanceButton.Click += new System.EventHandler(this.performanceButton_Click);
			// 
			// serverButton
			// 
			this.serverButton.BackColor = System.Drawing.Color.Transparent;
			this.serverButton.BorderColor = System.Drawing.Color.Black;
			this.serverButton.Depth = 3;
			this.serverButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.serverButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.serverButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.serverButton.ForeColor = System.Drawing.Color.White;
			this.serverButton.Highlight = false;
			this.serverButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.serverButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.serverButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.serverButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.serverButton.IsGlass = true;
			this.serverButton.Location = new System.Drawing.Point(13, 63);
			this.serverButton.Name = "serverButton";
			this.serverButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.serverButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.serverButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.serverButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.serverButton.Size = new System.Drawing.Size(76, 52);
			this.serverButton.TabIndex = 23;
			this.serverButton.TabStop = false;
			this.serverButton.Text = "Server";
			this.serverButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.serverButton.UseStyles = true;
			this.serverButton.UseVisualStyleBackColor = false;
			this.serverButton.Click += new System.EventHandler(this.serverButton_Click);
			// 
			// performancePanel
			// 
			this.performancePanel.Borders = ((Terrarium.Glass.GlassBorders)((((Terrarium.Glass.GlassBorders.Left | Terrarium.Glass.GlassBorders.Top)
						| Terrarium.Glass.GlassBorders.Right)
						| Terrarium.Glass.GlassBorders.Bottom)));
			this.performancePanel.Controls.Add(this.GlassLabel3);
			this.performancePanel.Controls.Add(this.GlassLabel2);
			this.performancePanel.Controls.Add(this.GlassLabel1);
			this.performancePanel.Controls.Add(this.lblPerformanceDescription);
			this.performancePanel.Controls.Add(this.lblPerformanceSlider);
			this.performancePanel.Controls.Add(this.tbrPerformanceSlider);
			this.performancePanel.Controls.Add(this.lblPerformanceSliderResult);
			this.performancePanel.Controls.Add(this.lblPerformanceEnableLogging);
			this.performancePanel.Controls.Add(this.radPerformanceNoLogging);
			this.performancePanel.Controls.Add(this.radPerformanceSelectedLogging);
			this.performancePanel.Controls.Add(this.radPerformanceAllLogging);
			this.performancePanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.performancePanel.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.performancePanel.IsGlass = true;
			this.performancePanel.IsSunk = false;
			this.performancePanel.Location = new System.Drawing.Point(13, 87);
			this.performancePanel.Name = "performancePanel";
			this.performancePanel.Size = new System.Drawing.Size(384, 272);
			this.performancePanel.TabIndex = 27;
			this.performancePanel.UseStyles = true;
			// 
			// GlassLabel3
			// 
			this.GlassLabel3.AutoSize = true;
			this.GlassLabel3.BackColor = System.Drawing.Color.Transparent;
			this.GlassLabel3.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GlassLabel3.ForeColor = System.Drawing.Color.White;
			this.GlassLabel3.Location = new System.Drawing.Point(96, 240);
			this.GlassLabel3.Name = "GlassLabel3";
			this.GlassLabel3.NoWrap = false;
			this.GlassLabel3.Size = new System.Drawing.Size(28, 12);
			this.GlassLabel3.TabIndex = 24;
			this.GlassLabel3.Text = "None";
			this.GlassLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// GlassLabel2
			// 
			this.GlassLabel2.AutoSize = true;
			this.GlassLabel2.BackColor = System.Drawing.Color.Transparent;
			this.GlassLabel2.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GlassLabel2.ForeColor = System.Drawing.Color.White;
			this.GlassLabel2.Location = new System.Drawing.Point(160, 240);
			this.GlassLabel2.Name = "GlassLabel2";
			this.GlassLabel2.NoWrap = false;
			this.GlassLabel2.Size = new System.Drawing.Size(72, 12);
			this.GlassLabel2.TabIndex = 23;
			this.GlassLabel2.Text = "Selected Only";
			this.GlassLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// GlassLabel1
			// 
			this.GlassLabel1.AutoSize = true;
			this.GlassLabel1.BackColor = System.Drawing.Color.Transparent;
			this.GlassLabel1.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GlassLabel1.ForeColor = System.Drawing.Color.White;
			this.GlassLabel1.Location = new System.Drawing.Point(264, 240);
			this.GlassLabel1.Name = "GlassLabel1";
			this.GlassLabel1.NoWrap = false;
			this.GlassLabel1.Size = new System.Drawing.Size(21, 12);
			this.GlassLabel1.TabIndex = 22;
			this.GlassLabel1.Text = "Full";
			this.GlassLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblPerformanceDescription
			// 
			this.lblPerformanceDescription.BackColor = System.Drawing.Color.Transparent;
			this.lblPerformanceDescription.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPerformanceDescription.ForeColor = System.Drawing.Color.White;
			this.lblPerformanceDescription.Location = new System.Drawing.Point(16, 16);
			this.lblPerformanceDescription.Name = "lblPerformanceDescription";
			this.lblPerformanceDescription.NoWrap = false;
			this.lblPerformanceDescription.Size = new System.Drawing.Size(360, 72);
			this.lblPerformanceDescription.TabIndex = 16;
			this.lblPerformanceDescription.Text = "Settings here allow you to affect how the terrarium performs on your machine.  No" +
				"te some settings may be useful for debugging, but may affect performance.";
			// 
			// lblPerformanceSlider
			// 
			this.lblPerformanceSlider.BackColor = System.Drawing.Color.Transparent;
			this.lblPerformanceSlider.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPerformanceSlider.ForeColor = System.Drawing.Color.White;
			this.lblPerformanceSlider.Location = new System.Drawing.Point(8, 104);
			this.lblPerformanceSlider.Name = "lblPerformanceSlider";
			this.lblPerformanceSlider.NoWrap = false;
			this.lblPerformanceSlider.Size = new System.Drawing.Size(368, 16);
			this.lblPerformanceSlider.TabIndex = 15;
			this.lblPerformanceSlider.Text = "World Size (Min 25%, Max 400%):";
			this.lblPerformanceSlider.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tbrPerformanceSlider
			// 
			this.tbrPerformanceSlider.BackColor = System.Drawing.Color.Gray;
			this.tbrPerformanceSlider.LargeChange = 10;
			this.tbrPerformanceSlider.Location = new System.Drawing.Point(56, 120);
			this.tbrPerformanceSlider.Maximum = 400;
			this.tbrPerformanceSlider.Minimum = 25;
			this.tbrPerformanceSlider.Name = "tbrPerformanceSlider";
			this.tbrPerformanceSlider.Size = new System.Drawing.Size(272, 45);
			this.tbrPerformanceSlider.TabIndex = 17;
			this.tbrPerformanceSlider.TickFrequency = 10;
			this.tbrPerformanceSlider.Value = 100;
			this.tbrPerformanceSlider.ValueChanged += new System.EventHandler(this.PerformanceSlider_ValueChanged);
			// 
			// lblPerformanceSliderResult
			// 
			this.lblPerformanceSliderResult.BackColor = System.Drawing.Color.Transparent;
			this.lblPerformanceSliderResult.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPerformanceSliderResult.ForeColor = System.Drawing.Color.White;
			this.lblPerformanceSliderResult.Location = new System.Drawing.Point(8, 168);
			this.lblPerformanceSliderResult.Name = "lblPerformanceSliderResult";
			this.lblPerformanceSliderResult.NoWrap = false;
			this.lblPerformanceSliderResult.Size = new System.Drawing.Size(368, 16);
			this.lblPerformanceSliderResult.TabIndex = 14;
			this.lblPerformanceSliderResult.Text = "Current: 100%";
			this.lblPerformanceSliderResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblPerformanceEnableLogging
			// 
			this.lblPerformanceEnableLogging.BackColor = System.Drawing.Color.Transparent;
			this.lblPerformanceEnableLogging.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPerformanceEnableLogging.ForeColor = System.Drawing.Color.White;
			this.lblPerformanceEnableLogging.Location = new System.Drawing.Point(8, 208);
			this.lblPerformanceEnableLogging.Name = "lblPerformanceEnableLogging";
			this.lblPerformanceEnableLogging.NoWrap = false;
			this.lblPerformanceEnableLogging.Size = new System.Drawing.Size(368, 16);
			this.lblPerformanceEnableLogging.TabIndex = 18;
			this.lblPerformanceEnableLogging.Text = "Enable Animal Trace Logging for use with DBMon:";
			this.lblPerformanceEnableLogging.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// radPerformanceNoLogging
			// 
			this.radPerformanceNoLogging.BackColor = System.Drawing.Color.Transparent;
			this.radPerformanceNoLogging.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radPerformanceNoLogging.ForeColor = System.Drawing.Color.White;
			this.radPerformanceNoLogging.Location = new System.Drawing.Point(80, 240);
			this.radPerformanceNoLogging.Name = "radPerformanceNoLogging";
			this.radPerformanceNoLogging.Size = new System.Drawing.Size(12, 12);
			this.radPerformanceNoLogging.TabIndex = 19;
			this.radPerformanceNoLogging.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.radPerformanceNoLogging.UseVisualStyleBackColor = false;
			// 
			// radPerformanceSelectedLogging
			// 
			this.radPerformanceSelectedLogging.BackColor = System.Drawing.Color.Transparent;
			this.radPerformanceSelectedLogging.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radPerformanceSelectedLogging.ForeColor = System.Drawing.Color.White;
			this.radPerformanceSelectedLogging.Location = new System.Drawing.Point(144, 240);
			this.radPerformanceSelectedLogging.Name = "radPerformanceSelectedLogging";
			this.radPerformanceSelectedLogging.Size = new System.Drawing.Size(12, 12);
			this.radPerformanceSelectedLogging.TabIndex = 20;
			this.radPerformanceSelectedLogging.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.radPerformanceSelectedLogging.UseVisualStyleBackColor = false;
			// 
			// radPerformanceAllLogging
			// 
			this.radPerformanceAllLogging.BackColor = System.Drawing.Color.Transparent;
			this.radPerformanceAllLogging.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radPerformanceAllLogging.ForeColor = System.Drawing.Color.White;
			this.radPerformanceAllLogging.Location = new System.Drawing.Point(248, 240);
			this.radPerformanceAllLogging.Name = "radPerformanceAllLogging";
			this.radPerformanceAllLogging.Size = new System.Drawing.Size(12, 12);
			this.radPerformanceAllLogging.TabIndex = 21;
			this.radPerformanceAllLogging.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.radPerformanceAllLogging.UseVisualStyleBackColor = false;
			// 
			// serverPanel
			// 
			this.serverPanel.Borders = ((Terrarium.Glass.GlassBorders)((((Terrarium.Glass.GlassBorders.Left | Terrarium.Glass.GlassBorders.Top)
						| Terrarium.Glass.GlassBorders.Right)
						| Terrarium.Glass.GlassBorders.Bottom)));
			this.serverPanel.Controls.Add(this.GlassLabel5);
			this.serverPanel.Controls.Add(this.txtPeerChannel);
			this.serverPanel.Controls.Add(this.lblServerDescription);
			this.serverPanel.Controls.Add(this.lblServerURI);
			this.serverPanel.Controls.Add(this.txtServerURI);
			this.serverPanel.Controls.Add(this.lblServerEnableNat);
			this.serverPanel.Controls.Add(this.chkServerEnableNat);
			this.serverPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.serverPanel.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.serverPanel.IsGlass = true;
			this.serverPanel.IsSunk = false;
			this.serverPanel.Location = new System.Drawing.Point(13, 87);
			this.serverPanel.Name = "serverPanel";
			this.serverPanel.Size = new System.Drawing.Size(384, 272);
			this.serverPanel.TabIndex = 29;
			this.serverPanel.UseStyles = true;
			// 
			// GlassLabel5
			// 
			this.GlassLabel5.BackColor = System.Drawing.Color.Transparent;
			this.GlassLabel5.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GlassLabel5.ForeColor = System.Drawing.Color.White;
			this.GlassLabel5.Location = new System.Drawing.Point(8, 161);
			this.GlassLabel5.Name = "GlassLabel5";
			this.GlassLabel5.NoWrap = false;
			this.GlassLabel5.Size = new System.Drawing.Size(104, 16);
			this.GlassLabel5.TabIndex = 14;
			this.GlassLabel5.Text = "Peer Channel:";
			this.GlassLabel5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// txtPeerChannel
			// 
			this.txtPeerChannel.BackColor = System.Drawing.SystemColors.Window;
			this.txtPeerChannel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPeerChannel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPeerChannel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtPeerChannel.Location = new System.Drawing.Point(112, 160);
			this.txtPeerChannel.Name = "txtPeerChannel";
			this.txtPeerChannel.Size = new System.Drawing.Size(240, 18);
			this.txtPeerChannel.TabIndex = 15;
			// 
			// lblServerDescription
			// 
			this.lblServerDescription.BackColor = System.Drawing.Color.Transparent;
			this.lblServerDescription.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblServerDescription.ForeColor = System.Drawing.Color.White;
			this.lblServerDescription.Location = new System.Drawing.Point(16, 16);
			this.lblServerDescription.Name = "lblServerDescription";
			this.lblServerDescription.NoWrap = false;
			this.lblServerDescription.Size = new System.Drawing.Size(360, 80);
			this.lblServerDescription.TabIndex = 10;
			this.lblServerDescription.Text = "The Terrarium gaming client can be pointed at an alternate Terrarium server.  Fee" +
				"l free to change your game to another server.";
			// 
			// lblServerURI
			// 
			this.lblServerURI.BackColor = System.Drawing.Color.Transparent;
			this.lblServerURI.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblServerURI.ForeColor = System.Drawing.Color.White;
			this.lblServerURI.Location = new System.Drawing.Point(6, 129);
			this.lblServerURI.Name = "lblServerURI";
			this.lblServerURI.NoWrap = false;
			this.lblServerURI.Size = new System.Drawing.Size(104, 16);
			this.lblServerURI.TabIndex = 9;
			this.lblServerURI.Text = "Server URL:";
			this.lblServerURI.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// txtServerURI
			// 
			this.txtServerURI.BackColor = System.Drawing.SystemColors.Window;
			this.txtServerURI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtServerURI.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtServerURI.ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtServerURI.Location = new System.Drawing.Point(112, 128);
			this.txtServerURI.Name = "txtServerURI";
			this.txtServerURI.Size = new System.Drawing.Size(240, 18);
			this.txtServerURI.TabIndex = 11;
			// 
			// lblServerEnableNat
			// 
			this.lblServerEnableNat.BackColor = System.Drawing.Color.Transparent;
			this.lblServerEnableNat.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblServerEnableNat.ForeColor = System.Drawing.Color.White;
			this.lblServerEnableNat.Location = new System.Drawing.Point(104, 208);
			this.lblServerEnableNat.Name = "lblServerEnableNat";
			this.lblServerEnableNat.NoWrap = false;
			this.lblServerEnableNat.Size = new System.Drawing.Size(160, 16);
			this.lblServerEnableNat.TabIndex = 12;
			this.lblServerEnableNat.Text = "Enable Nat/Firewall Support";
			this.lblServerEnableNat.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// chkServerEnableNat
			// 
			this.chkServerEnableNat.BackColor = System.Drawing.Color.Transparent;
			this.chkServerEnableNat.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkServerEnableNat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.chkServerEnableNat.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkServerEnableNat.Location = new System.Drawing.Point(264, 208);
			this.chkServerEnableNat.Name = "chkServerEnableNat";
			this.chkServerEnableNat.Size = new System.Drawing.Size(13, 13);
			this.chkServerEnableNat.TabIndex = 13;
			this.chkServerEnableNat.UseVisualStyleBackColor = false;
			// 
			// graphicsPanel
			// 
			this.graphicsPanel.Borders = ((Terrarium.Glass.GlassBorders)((((Terrarium.Glass.GlassBorders.Left | Terrarium.Glass.GlassBorders.Top)
						| Terrarium.Glass.GlassBorders.Right)
						| Terrarium.Glass.GlassBorders.Bottom)));
			this.graphicsPanel.Controls.Add(this.chkStartFullscreen);
			this.graphicsPanel.Controls.Add(this.GlassLabel6);
			this.graphicsPanel.Controls.Add(this.styleComboBox);
			this.graphicsPanel.Controls.Add(this.GlassLabel4);
			this.graphicsPanel.Controls.Add(this.lblGraphicsDescription);
			this.graphicsPanel.Controls.Add(this.chkGraphicsDrawScreen);
			this.graphicsPanel.Controls.Add(this.chkGraphicsBoundingBoxes);
			this.graphicsPanel.Controls.Add(this.chkGraphicsBackgroundGrid);
			this.graphicsPanel.Controls.Add(this.chkGraphicsDestinationLines);
			this.graphicsPanel.Controls.Add(this.lblGraphicsDrawScreen);
			this.graphicsPanel.Controls.Add(this.lblGraphicsBoundingBoxes);
			this.graphicsPanel.Controls.Add(this.lblGraphicsBackgroundGrid);
			this.graphicsPanel.Controls.Add(this.lblGraphicsDestinationLines);
			this.graphicsPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.graphicsPanel.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.graphicsPanel.IsGlass = true;
			this.graphicsPanel.IsSunk = false;
			this.graphicsPanel.Location = new System.Drawing.Point(13, 87);
			this.graphicsPanel.Name = "graphicsPanel";
			this.graphicsPanel.Size = new System.Drawing.Size(384, 272);
			this.graphicsPanel.TabIndex = 30;
			this.graphicsPanel.UseStyles = true;
			// 
			// chkStartFullscreen
			// 
			this.chkStartFullscreen.BackColor = System.Drawing.Color.Transparent;
			this.chkStartFullscreen.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkStartFullscreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.chkStartFullscreen.Location = new System.Drawing.Point(248, 144);
			this.chkStartFullscreen.Name = "chkStartFullscreen";
			this.chkStartFullscreen.Size = new System.Drawing.Size(13, 13);
			this.chkStartFullscreen.TabIndex = 31;
			this.chkStartFullscreen.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.chkStartFullscreen.UseVisualStyleBackColor = false;
			// 
			// GlassLabel6
			// 
			this.GlassLabel6.BackColor = System.Drawing.Color.Transparent;
			this.GlassLabel6.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GlassLabel6.ForeColor = System.Drawing.Color.White;
			this.GlassLabel6.Location = new System.Drawing.Point(16, 142);
			this.GlassLabel6.Name = "GlassLabel6";
			this.GlassLabel6.NoWrap = false;
			this.GlassLabel6.Size = new System.Drawing.Size(234, 16);
			this.GlassLabel6.TabIndex = 30;
			this.GlassLabel6.Text = "Fullscreen on Start";
			this.GlassLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// styleComboBox
			// 
			this.styleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.styleComboBox.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.styleComboBox.FormattingEnabled = true;
			this.styleComboBox.Location = new System.Drawing.Point(112, 112);
			this.styleComboBox.Name = "styleComboBox";
			this.styleComboBox.Size = new System.Drawing.Size(216, 20);
			this.styleComboBox.TabIndex = 29;
			// 
			// GlassLabel4
			// 
			this.GlassLabel4.BackColor = System.Drawing.Color.Transparent;
			this.GlassLabel4.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GlassLabel4.ForeColor = System.Drawing.Color.White;
			this.GlassLabel4.Location = new System.Drawing.Point(16, 112);
			this.GlassLabel4.Name = "GlassLabel4";
			this.GlassLabel4.NoWrap = false;
			this.GlassLabel4.Size = new System.Drawing.Size(88, 24);
			this.GlassLabel4.TabIndex = 27;
			this.GlassLabel4.Text = "Style:";
			this.GlassLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblGraphicsDescription
			// 
			this.lblGraphicsDescription.BackColor = System.Drawing.Color.Transparent;
			this.lblGraphicsDescription.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblGraphicsDescription.ForeColor = System.Drawing.Color.White;
			this.lblGraphicsDescription.Location = new System.Drawing.Point(16, 16);
			this.lblGraphicsDescription.Name = "lblGraphicsDescription";
			this.lblGraphicsDescription.NoWrap = false;
			this.lblGraphicsDescription.Size = new System.Drawing.Size(360, 80);
			this.lblGraphicsDescription.TabIndex = 18;
			this.lblGraphicsDescription.Text = "These settings affect the graphic display of the Terrarium.  Note some settings m" +
				"ay be useful for debugging, but may affect Graphics performance.";
			// 
			// chkGraphicsDrawScreen
			// 
			this.chkGraphicsDrawScreen.BackColor = System.Drawing.Color.Transparent;
			this.chkGraphicsDrawScreen.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkGraphicsDrawScreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.chkGraphicsDrawScreen.Location = new System.Drawing.Point(248, 168);
			this.chkGraphicsDrawScreen.Name = "chkGraphicsDrawScreen";
			this.chkGraphicsDrawScreen.Size = new System.Drawing.Size(13, 13);
			this.chkGraphicsDrawScreen.TabIndex = 20;
			this.chkGraphicsDrawScreen.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.chkGraphicsDrawScreen.UseVisualStyleBackColor = false;
			// 
			// chkGraphicsBoundingBoxes
			// 
			this.chkGraphicsBoundingBoxes.BackColor = System.Drawing.Color.Transparent;
			this.chkGraphicsBoundingBoxes.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkGraphicsBoundingBoxes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.chkGraphicsBoundingBoxes.Location = new System.Drawing.Point(248, 192);
			this.chkGraphicsBoundingBoxes.Name = "chkGraphicsBoundingBoxes";
			this.chkGraphicsBoundingBoxes.Size = new System.Drawing.Size(13, 13);
			this.chkGraphicsBoundingBoxes.TabIndex = 22;
			this.chkGraphicsBoundingBoxes.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.chkGraphicsBoundingBoxes.UseVisualStyleBackColor = false;
			// 
			// chkGraphicsBackgroundGrid
			// 
			this.chkGraphicsBackgroundGrid.BackColor = System.Drawing.Color.Transparent;
			this.chkGraphicsBackgroundGrid.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkGraphicsBackgroundGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.chkGraphicsBackgroundGrid.Location = new System.Drawing.Point(248, 216);
			this.chkGraphicsBackgroundGrid.Name = "chkGraphicsBackgroundGrid";
			this.chkGraphicsBackgroundGrid.Size = new System.Drawing.Size(13, 13);
			this.chkGraphicsBackgroundGrid.TabIndex = 24;
			this.chkGraphicsBackgroundGrid.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.chkGraphicsBackgroundGrid.UseVisualStyleBackColor = false;
			// 
			// chkGraphicsDestinationLines
			// 
			this.chkGraphicsDestinationLines.BackColor = System.Drawing.Color.Transparent;
			this.chkGraphicsDestinationLines.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkGraphicsDestinationLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.chkGraphicsDestinationLines.Location = new System.Drawing.Point(248, 240);
			this.chkGraphicsDestinationLines.Name = "chkGraphicsDestinationLines";
			this.chkGraphicsDestinationLines.Size = new System.Drawing.Size(13, 13);
			this.chkGraphicsDestinationLines.TabIndex = 26;
			this.chkGraphicsDestinationLines.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.chkGraphicsDestinationLines.UseVisualStyleBackColor = false;
			// 
			// lblGraphicsDrawScreen
			// 
			this.lblGraphicsDrawScreen.BackColor = System.Drawing.Color.Transparent;
			this.lblGraphicsDrawScreen.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblGraphicsDrawScreen.ForeColor = System.Drawing.Color.White;
			this.lblGraphicsDrawScreen.Location = new System.Drawing.Point(16, 166);
			this.lblGraphicsDrawScreen.Name = "lblGraphicsDrawScreen";
			this.lblGraphicsDrawScreen.NoWrap = false;
			this.lblGraphicsDrawScreen.Size = new System.Drawing.Size(234, 16);
			this.lblGraphicsDrawScreen.TabIndex = 19;
			this.lblGraphicsDrawScreen.Text = "Enable Game View";
			this.lblGraphicsDrawScreen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblGraphicsBoundingBoxes
			// 
			this.lblGraphicsBoundingBoxes.BackColor = System.Drawing.Color.Transparent;
			this.lblGraphicsBoundingBoxes.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblGraphicsBoundingBoxes.ForeColor = System.Drawing.Color.White;
			this.lblGraphicsBoundingBoxes.Location = new System.Drawing.Point(18, 190);
			this.lblGraphicsBoundingBoxes.Name = "lblGraphicsBoundingBoxes";
			this.lblGraphicsBoundingBoxes.NoWrap = false;
			this.lblGraphicsBoundingBoxes.Size = new System.Drawing.Size(234, 16);
			this.lblGraphicsBoundingBoxes.TabIndex = 21;
			this.lblGraphicsBoundingBoxes.Text = "Enable Bounding Boxes";
			this.lblGraphicsBoundingBoxes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblGraphicsBackgroundGrid
			// 
			this.lblGraphicsBackgroundGrid.BackColor = System.Drawing.Color.Transparent;
			this.lblGraphicsBackgroundGrid.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblGraphicsBackgroundGrid.ForeColor = System.Drawing.Color.White;
			this.lblGraphicsBackgroundGrid.Location = new System.Drawing.Point(18, 215);
			this.lblGraphicsBackgroundGrid.Name = "lblGraphicsBackgroundGrid";
			this.lblGraphicsBackgroundGrid.NoWrap = false;
			this.lblGraphicsBackgroundGrid.Size = new System.Drawing.Size(234, 16);
			this.lblGraphicsBackgroundGrid.TabIndex = 23;
			this.lblGraphicsBackgroundGrid.Text = "Enable Background Grid";
			this.lblGraphicsBackgroundGrid.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblGraphicsDestinationLines
			// 
			this.lblGraphicsDestinationLines.BackColor = System.Drawing.Color.Transparent;
			this.lblGraphicsDestinationLines.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblGraphicsDestinationLines.ForeColor = System.Drawing.Color.White;
			this.lblGraphicsDestinationLines.Location = new System.Drawing.Point(16, 238);
			this.lblGraphicsDestinationLines.Name = "lblGraphicsDestinationLines";
			this.lblGraphicsDestinationLines.NoWrap = false;
			this.lblGraphicsDestinationLines.Size = new System.Drawing.Size(234, 16);
			this.lblGraphicsDestinationLines.TabIndex = 25;
			this.lblGraphicsDestinationLines.Text = "Enable Destination Lines";
			this.lblGraphicsDestinationLines.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// PeerProperties
			// 
			this.BackColor = System.Drawing.Color.Magenta;
			this.ClientSize = new System.Drawing.Size(409, 414);
			this.Controls.Add(this.graphicsPanel);
			this.Controls.Add(this.serverPanel);
			this.Controls.Add(this.performancePanel);
			this.Controls.Add(this.serverButton);
			this.Controls.Add(this.performanceButton);
			this.Controls.Add(this.graphicsButton);
			this.Description = "View and change various Terrarium Client Game settings";
			this.Location = new System.Drawing.Point(0, 24);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PeerProperties";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Title = "Game Settings";
			this.Load += new System.EventHandler(this.PeerProperties_Load);
			this.Controls.SetChildIndex(this.graphicsButton, 0);
			this.Controls.SetChildIndex(this.performanceButton, 0);
			this.Controls.SetChildIndex(this.serverButton, 0);
			this.Controls.SetChildIndex(this.bottomPanel, 0);
			this.Controls.SetChildIndex(this.titleBar, 0);
			this.Controls.SetChildIndex(this.performancePanel, 0);
			this.Controls.SetChildIndex(this.serverPanel, 0);
			this.Controls.SetChildIndex(this.graphicsPanel, 0);
			this.bottomPanel.ResumeLayout(false);
			this.performancePanel.ResumeLayout(false);
			this.performancePanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbrPerformanceSlider)).EndInit();
			this.serverPanel.ResumeLayout(false);
			this.serverPanel.PerformLayout();
			this.graphicsPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		///  Implement panel visiblity changes.  This makes a kind of Tab
		///  control.
		/// </summary>
		/// <param name="sender">Button</param>
		/// <param name="e">Null</param>
		private void PanelChange_Click(object sender, System.EventArgs e)
		{
			if ( sender is Button )
			{
				SwitchPanel(((Control) sender).Name.Substring(3));
			}
		}

		/// <summary>
		///  Enables programmatic panel switching given the name of the panel.
		/// </summary>
		/// <param name="panel">The name of the panel minus the pnl prefix.</param>
		/// <returns>True if the requested panel could be set, false otherwise.</returns>
		public bool SwitchPanel(string panel)
		{
			if( panel == null )
				return false;

			switch( panel.ToLower() )
			{
				case "server":
					serverButton_Click( this, new EventArgs() );
					return true;
				case "registration":
					registrationButton_Click( this, new EventArgs() );
					return true;
				case "community":
					communityButton_Click( this, new EventArgs() );
					return true;
				case "graphics":
					graphicsButton_Click( this, new EventArgs() );
					return true;
				case "performance":
					performanceButton_Click( this, new EventArgs() );
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		///  Updates the slider label whenever the slider value changes.
		/// </summary>
		/// <param name="sender">TrackBar</param>
		/// <param name="e">Null</param>
		private void PerformanceSlider_ValueChanged(object sender, System.EventArgs e)
		{
			this.lblPerformanceSliderResult.Text = "Current: " + this.tbrPerformanceSlider.Value + "%";
		}
    
		/// <summary>
		///  Updates the configuration variables when the user clicks the OK button.
		/// </summary>
		/// <param name="sender">Button</param>
		/// <param name="e">Null</param>
		private void OK_Click(object sender, System.EventArgs e)
		{
			// Validate the Peer Channel setting.  We do this first since if it is
			// invalid, we want to keep the dialog open and let the user fix
			if ( Terrarium.Game.GameEngine.Current != null )
			{
				if ( Terrarium.Game.GameEngine.Current.EcosystemMode == false )
				{
					if (this.txtPeerChannel.Text.ToLower( System.Globalization.CultureInfo.InvariantCulture) == "ecosystem")
					{
						MessageBox.Show(this, "Only Ecosystem mode can use that Peer Channel, please choose a different one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						this.SwitchPanel( "Server" );
						this.txtPeerChannel.Focus();
						return;
					}

					try
					{
						Terrarium.Game.GameEngine.Current.PeerChannel = this.txtPeerChannel.Text;
					}
					catch (Exception exception)
					{
						MessageBox.Show(this, "Could not connect to Peer Channel '" + this.txtPeerChannel.Text + "'.\r\n\r\nReason: " + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						this.SwitchPanel( "Server" );
						this.txtPeerChannel.Focus();
						return;
					}
				}
			}
	
			bool restartRequired = false;
    
			GameConfig.EnableNat = chkServerEnableNat.Checked;
			GameConfig.BoundingBoxes = chkGraphicsBoundingBoxes.Checked;
			GameConfig.BackgroundGrid = chkGraphicsBackgroundGrid.Checked;
			GameConfig.DrawScreen = chkGraphicsDrawScreen.Checked;
			GameConfig.DestinationLines = chkGraphicsDestinationLines.Checked;
        
			GameConfig.LoggingMode = radPerformanceNoLogging.Text;
			if (radPerformanceAllLogging.Checked)
			{
				GameConfig.LoggingMode = radPerformanceAllLogging.Text;
			}
			if (radPerformanceSelectedLogging.Checked)
			{
				GameConfig.LoggingMode = radPerformanceSelectedLogging.Text;
			}
        
			if (GameConfig.UserEmail.Length == 0)
			{
				GameConfig.UserEmail = "<not set>";
			}

			if (GameConfig.CpuThrottle != tbrPerformanceSlider.Value)
			{
				restartRequired = true;
				GameConfig.CpuThrottle = tbrPerformanceSlider.Value;
			}
        
			if (GameConfig.WebRoot != txtServerURI.Text && txtServerURI.Text.Length != 0)
			{
				if (!RegisterServer())
				{
					return;
				}
				else
				{
					restartRequired = true;
				}
			}

			GameConfig.StartFullscreen = this.chkStartFullscreen.Checked;

			if (restartRequired)
			{
				MessageBox.Show(this, "You'll need to shut down and restart the game for your changes to take effect.");
			}
        
			if ( this.styleComboBox.SelectedItem != null )
				GameConfig.StyleName = ((GlassStyle)this.styleComboBox.SelectedItem).Name;
			else
				GameConfig.StyleName = "";

			// Let's go ahead and try to change styles now
			Terrarium.Glass.GlassStyleManager.SetStyle( GameConfig.StyleName );

			this.Hide();
		}

		/// <summary>
		///  Cancels out the dialog without updating configuration values.
		/// </summary>
		/// <param name="sender">Button</param>
		/// <param name="e">Null</param>
		private void Cancel_Click(object sender, System.EventArgs e)
		{
			if (GameConfig.UserEmail.Length == 0 || GameConfig.UserEmail == "<not set>")
			{
				GameConfig.UserEmail = "<not set>";
			}

			this.Hide();
		}

		/// <summary>
		///  Sets initial control values after the form loads.
		/// </summary>
		/// <param name="sender">Form</param>
		/// <param name="e">Null</param>
		private void PeerProperties_Load(object sender, System.EventArgs e)
		{       
        
			tbrPerformanceSlider.Value = GameConfig.CpuThrottle;
			txtServerURI.Text = GameConfig.WebRoot;
			chkServerEnableNat.Checked = GameConfig.EnableNat;
        
			chkGraphicsBoundingBoxes.Checked = GameConfig.BoundingBoxes;
			chkGraphicsBackgroundGrid.Checked = GameConfig.BackgroundGrid;
			chkGraphicsDrawScreen.Checked = GameConfig.DrawScreen;
			chkGraphicsDestinationLines.Checked = GameConfig.DestinationLines;
        
			switch (GameConfig.LoggingMode)
			{
				case "Selected Only":
					radPerformanceSelectedLogging.Checked = true;
					break;
				case "Full":
					radPerformanceAllLogging.Checked = true;
					break;
				default:
					radPerformanceNoLogging.Checked = true;
					break;
			}

			// Set up the Styles Combo box
			this.styleComboBox.Items.Clear();
			this.styleComboBox.DisplayMember = "Name";

			GlassStyleManager.Refresh();

			foreach( GlassStyle style in GlassStyleManager.Styles )
			{
				this.styleComboBox.Items.Add( style );
			}

			foreach( GlassStyle style in this.styleComboBox.Items )
			{
				if ( style.Name == GameConfig.StyleName )
				{
					this.styleComboBox.SelectedItem = style;
					break;
				}
			}

			// Set up the peer channel text box
			if ( Terrarium.Game.GameEngine.Current != null )
			{
				if ( Terrarium.Game.GameEngine.Current.EcosystemMode == true )
				{
					this.txtPeerChannel.Text = "Ecosystem";
					this.txtPeerChannel.Enabled = false;
				}
				else
				{
					this.txtPeerChannel.Text = Terrarium.Game.GameEngine.Current.PeerChannel;
				}			
			}

			this.chkStartFullscreen.Checked = GameConfig.StartFullscreen;					
		}
    
		/// <summary>
		///  Controls email registration with the Terrarium server.
		/// </summary>
		/// <returns>True if the email was sent successfully, false otherwise.</returns>
		private bool RegisterEmail()
		{
			//GameConfig.UserEmail = txtRegistrationEmail.Text;

			//PeerDiscoveryService peerService = new PeerDiscoveryService();
			//peerService.Url = GameConfig.WebRoot + "/discovery/discoverydb.asmx";
			//peerService.Timeout = 60000; // 5 minute timeout
        
			//try
			//{
			//    peerService.RegisterUser(this.txtRegistrationEmail.Text);
			//}
			//catch (Exception exception)
			//{
			//    ErrorLog.LogHandledException(exception);
			//    MessageBox.Show("Problems registering: " + exception.Message);
			//    return false;
			//}
        
			return true;
		}
    
		/// <summary>
		///  Tries to validate a new server whenever the server changes.
		/// </summary>
		/// <returns>True if the server is a valid Terrarium server, false otherwise.</returns>
		private bool RegisterServer()
		{
			txtServerURI.Text = txtServerURI.Text.Trim();
        
			try
			{
				Uri uri = new Uri(txtServerURI.Text);
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message);
				return false;
			}
        
			this.Cursor = Cursors.WaitCursor;
			try
			{
				PeerDiscoveryService peerService = new PeerDiscoveryService();
				peerService.Url = txtServerURI.Text + "/discovery/discoverydb.asmx";
				peerService.Timeout = 15000; // 15 seconds

				peerService.ValidatePeer();
				GameConfig.WebRoot = txtServerURI.Text;
			}
			catch (Exception exception)
			{
				ErrorLog.LogHandledException(exception);
            
				DialogResult result = MessageBox.Show("Error accessing '" + txtServerURI.Text + "':\nThe remote server is either unavailable or is not a valid Terrarium Server.\n\nWould you still like to set this address as your Terrarium server?", "Verify Terrarium Server", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (result == DialogResult.Yes)
				{
					GameConfig.WebRoot = txtServerURI.Text;
					return true;
				}
				else
				{
					return false;
				}
			}
			finally
			{
				this.Cursor = Cursors.Default;      
			}
        
			return true;
		}

		private void registrationPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void registrationButton_Click(object sender, System.EventArgs e)
		{
			this.graphicsPanel.Visible = false;
			this.performancePanel.Visible = false;
			this.serverPanel.Visible = false;

			this.graphicsButton.Highlight = false;
			this.performanceButton.Highlight = false;
			this.serverButton.Highlight = false;
		}

		private void communityButton_Click(object sender, System.EventArgs e)
		{
			this.graphicsPanel.Visible = false;
			this.performancePanel.Visible = false;
			this.serverPanel.Visible = false;		

			this.graphicsButton.Highlight = false;
			this.performanceButton.Highlight = false;
			this.serverButton.Highlight = false;		
		
		}

		private void graphicsButton_Click(object sender, System.EventArgs e)
		{
			this.graphicsPanel.Visible = true;
			this.performancePanel.Visible = false;
			this.serverPanel.Visible = false;		

			this.graphicsButton.Highlight = true;
			this.performanceButton.Highlight = false;
			this.serverButton.Highlight = false;		
		
		}

		private void performanceButton_Click(object sender, System.EventArgs e)
		{
			this.performancePanel.Visible = true;
			this.graphicsPanel.Visible = false;
			this.serverPanel.Visible = false;		

			this.performanceButton.Highlight = true;
			this.graphicsButton.Highlight = false;
			this.serverButton.Highlight = false;				
		}

		private void serverButton_Click(object sender, System.EventArgs e)
		{
			this.serverPanel.Visible = true;		
			this.performancePanel.Visible = false;
			this.graphicsPanel.Visible = false;

			this.serverButton.Highlight = true;		
			this.performanceButton.Highlight = false;
			this.graphicsButton.Highlight = false;
		}
	}
}