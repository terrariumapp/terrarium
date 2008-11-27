//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace Terrarium.Forms.Classes.Controls
{
	/// <summary>
	/// 
	/// </summary>
	partial class DeveloperPanel
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeveloperPanel));
            this.panel3 = new System.Windows.Forms.Panel();
            this.glassPanel2 = new Terrarium.Glass.GlassPanel();
            this.navigatePictureBox = new System.Windows.Forms.PictureBox();
            this.glassPanel3 = new Terrarium.Glass.GlassPanel();
            this.glassLabel3 = new Terrarium.Glass.GlassLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.webRootLabel = new Terrarium.Glass.GlassLabel();
            this.failedReceivesCountLabel = new Terrarium.Glass.GlassLabel();
            this.glassLabel11 = new Terrarium.Glass.GlassLabel();
            this.failedSendsCountLabel = new Terrarium.Glass.GlassLabel();
            this.glassLabel5 = new Terrarium.Glass.GlassLabel();
            this.teleportationsCountLabel = new Terrarium.Glass.GlassLabel();
            this.glassLabel13 = new Terrarium.Glass.GlassLabel();
            this.maximumAnimalCountLabel = new Terrarium.Glass.GlassLabel();
            this.glassLabel15 = new Terrarium.Glass.GlassLabel();
            this.animalCountLabel = new Terrarium.Glass.GlassLabel();
            this.glassLabel9 = new Terrarium.Glass.GlassLabel();
            this.peerCountLabel = new Terrarium.Glass.GlassLabel();
            this.glassLabel7 = new Terrarium.Glass.GlassLabel();
            this.gameModeLabel = new Terrarium.Glass.GlassLabel();
            this.glassLabel4 = new Terrarium.Glass.GlassLabel();
            this.ledFour = new Terrarium.Forms.TerrariumLed();
            this.ledThree = new Terrarium.Forms.TerrariumLed();
            this.ledTwo = new Terrarium.Forms.TerrariumLed();
            this.ledOne = new Terrarium.Forms.TerrariumLed();
            this.glassPanel1 = new Terrarium.Glass.GlassPanel();
            this.glassLabel1 = new Terrarium.Glass.GlassLabel();
            this.panel3.SuspendLayout();
            this.glassPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.navigatePictureBox)).BeginInit();
            this.glassPanel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.glassPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.glassPanel2);
            this.panel3.Controls.Add(this.glassPanel3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 292);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 220);
            this.panel3.TabIndex = 18;
            // 
            // glassPanel2
            // 
            this.glassPanel2.Borders = ((Terrarium.Glass.GlassBorders)((((Terrarium.Glass.GlassBorders.Left | Terrarium.Glass.GlassBorders.Top)
                        | Terrarium.Glass.GlassBorders.Right)
                        | Terrarium.Glass.GlassBorders.Bottom)));
            this.glassPanel2.Controls.Add(this.navigatePictureBox);
            this.glassPanel2.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.glassPanel2.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.glassPanel2.IsGlass = true;
            this.glassPanel2.IsSunk = false;
            this.glassPanel2.Location = new System.Drawing.Point(8, 30);
            this.glassPanel2.Name = "glassPanel2";
            this.glassPanel2.Size = new System.Drawing.Size(184, 184);
            this.glassPanel2.TabIndex = 11;
            this.glassPanel2.UseStyles = false;
            // 
            // navigatePictureBox
            // 
            this.navigatePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.navigatePictureBox.BackColor = System.Drawing.Color.Gray;
            this.navigatePictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("navigatePictureBox.BackgroundImage")));
            this.navigatePictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.navigatePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.navigatePictureBox.Location = new System.Drawing.Point(8, 8);
            this.navigatePictureBox.Name = "navigatePictureBox";
            this.navigatePictureBox.Size = new System.Drawing.Size(168, 168);
            this.navigatePictureBox.TabIndex = 11;
            this.navigatePictureBox.TabStop = false;
            this.navigatePictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.navigatePictureBox_Paint);
            // 
            // glassPanel3
            // 
            this.glassPanel3.Borders = ((Terrarium.Glass.GlassBorders)((((Terrarium.Glass.GlassBorders.Left | Terrarium.Glass.GlassBorders.Top)
                        | Terrarium.Glass.GlassBorders.Right)
                        | Terrarium.Glass.GlassBorders.Bottom)));
            this.glassPanel3.Controls.Add(this.glassLabel3);
            this.glassPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.glassPanel3.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.glassPanel3.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.glassPanel3.IsGlass = true;
            this.glassPanel3.IsSunk = false;
            this.glassPanel3.Location = new System.Drawing.Point(0, 0);
            this.glassPanel3.Name = "glassPanel3";
            this.glassPanel3.Size = new System.Drawing.Size(200, 24);
            this.glassPanel3.TabIndex = 3;
            this.glassPanel3.UseStyles = true;
            // 
            // glassLabel3
            // 
            this.glassLabel3.BackColor = System.Drawing.Color.Transparent;
            this.glassLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glassLabel3.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.glassLabel3.ForeColor = System.Drawing.Color.White;
            this.glassLabel3.Location = new System.Drawing.Point(0, 0);
            this.glassLabel3.Name = "glassLabel3";
            this.glassLabel3.NoWrap = false;
            this.glassLabel3.Size = new System.Drawing.Size(200, 24);
            this.glassLabel3.TabIndex = 0;
            this.glassLabel3.Text = " Mini Map";
            this.glassLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.webRootLabel);
            this.panel2.Controls.Add(this.failedReceivesCountLabel);
            this.panel2.Controls.Add(this.glassLabel11);
            this.panel2.Controls.Add(this.failedSendsCountLabel);
            this.panel2.Controls.Add(this.glassLabel5);
            this.panel2.Controls.Add(this.teleportationsCountLabel);
            this.panel2.Controls.Add(this.glassLabel13);
            this.panel2.Controls.Add(this.maximumAnimalCountLabel);
            this.panel2.Controls.Add(this.glassLabel15);
            this.panel2.Controls.Add(this.animalCountLabel);
            this.panel2.Controls.Add(this.glassLabel9);
            this.panel2.Controls.Add(this.peerCountLabel);
            this.panel2.Controls.Add(this.glassLabel7);
            this.panel2.Controls.Add(this.gameModeLabel);
            this.panel2.Controls.Add(this.glassLabel4);
            this.panel2.Controls.Add(this.ledFour);
            this.panel2.Controls.Add(this.ledThree);
            this.panel2.Controls.Add(this.ledTwo);
            this.panel2.Controls.Add(this.ledOne);
            this.panel2.Controls.Add(this.glassPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 292);
            this.panel2.TabIndex = 19;
            // 
            // webRootLabel
            // 
            this.webRootLabel.BackColor = System.Drawing.Color.Transparent;
            this.webRootLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.webRootLabel.ForeColor = System.Drawing.Color.White;
            this.webRootLabel.Location = new System.Drawing.Point(13, 97);
            this.webRootLabel.Name = "webRootLabel";
            this.webRootLabel.NoWrap = false;
            this.webRootLabel.Size = new System.Drawing.Size(174, 35);
            this.webRootLabel.TabIndex = 40;
            this.webRootLabel.Text = "XXXXX";
            // 
            // failedReceivesCountLabel
            // 
            this.failedReceivesCountLabel.BackColor = System.Drawing.Color.Transparent;
            this.failedReceivesCountLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.failedReceivesCountLabel.ForeColor = System.Drawing.Color.White;
            this.failedReceivesCountLabel.Location = new System.Drawing.Point(115, 212);
            this.failedReceivesCountLabel.Name = "failedReceivesCountLabel";
            this.failedReceivesCountLabel.NoWrap = false;
            this.failedReceivesCountLabel.Size = new System.Drawing.Size(82, 12);
            this.failedReceivesCountLabel.TabIndex = 35;
            this.failedReceivesCountLabel.Text = "0";
            this.failedReceivesCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.failedReceivesCountLabel.Click += new System.EventHandler(this.failedReceivesLabel_Click);
            // 
            // glassLabel11
            // 
            this.glassLabel11.BackColor = System.Drawing.Color.Transparent;
            this.glassLabel11.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.glassLabel11.ForeColor = System.Drawing.Color.White;
            this.glassLabel11.Location = new System.Drawing.Point(3, 210);
            this.glassLabel11.Name = "glassLabel11";
            this.glassLabel11.NoWrap = false;
            this.glassLabel11.Size = new System.Drawing.Size(106, 14);
            this.glassLabel11.TabIndex = 34;
            this.glassLabel11.Text = "Failed Receives:";
            this.glassLabel11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // failedSendsCountLabel
            // 
            this.failedSendsCountLabel.BackColor = System.Drawing.Color.Transparent;
            this.failedSendsCountLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.failedSendsCountLabel.ForeColor = System.Drawing.Color.White;
            this.failedSendsCountLabel.Location = new System.Drawing.Point(115, 196);
            this.failedSendsCountLabel.Name = "failedSendsCountLabel";
            this.failedSendsCountLabel.NoWrap = false;
            this.failedSendsCountLabel.Size = new System.Drawing.Size(82, 12);
            this.failedSendsCountLabel.TabIndex = 33;
            this.failedSendsCountLabel.Text = "0";
            this.failedSendsCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // glassLabel5
            // 
            this.glassLabel5.BackColor = System.Drawing.Color.Transparent;
            this.glassLabel5.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.glassLabel5.ForeColor = System.Drawing.Color.White;
            this.glassLabel5.Location = new System.Drawing.Point(3, 194);
            this.glassLabel5.Name = "glassLabel5";
            this.glassLabel5.NoWrap = false;
            this.glassLabel5.Size = new System.Drawing.Size(106, 16);
            this.glassLabel5.TabIndex = 32;
            this.glassLabel5.Text = "Failed Sends:";
            this.glassLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // teleportationsCountLabel
            // 
            this.teleportationsCountLabel.BackColor = System.Drawing.Color.Transparent;
            this.teleportationsCountLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.teleportationsCountLabel.ForeColor = System.Drawing.Color.White;
            this.teleportationsCountLabel.Location = new System.Drawing.Point(115, 180);
            this.teleportationsCountLabel.Name = "teleportationsCountLabel";
            this.teleportationsCountLabel.NoWrap = false;
            this.teleportationsCountLabel.Size = new System.Drawing.Size(82, 12);
            this.teleportationsCountLabel.TabIndex = 29;
            this.teleportationsCountLabel.Text = "0";
            this.teleportationsCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // glassLabel13
            // 
            this.glassLabel13.BackColor = System.Drawing.Color.Transparent;
            this.glassLabel13.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.glassLabel13.ForeColor = System.Drawing.Color.White;
            this.glassLabel13.Location = new System.Drawing.Point(3, 178);
            this.glassLabel13.Name = "glassLabel13";
            this.glassLabel13.NoWrap = false;
            this.glassLabel13.Size = new System.Drawing.Size(106, 14);
            this.glassLabel13.TabIndex = 28;
            this.glassLabel13.Text = "Teleportations:";
            this.glassLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // maximumAnimalCountLabel
            // 
            this.maximumAnimalCountLabel.BackColor = System.Drawing.Color.Transparent;
            this.maximumAnimalCountLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.maximumAnimalCountLabel.ForeColor = System.Drawing.Color.White;
            this.maximumAnimalCountLabel.Location = new System.Drawing.Point(115, 164);
            this.maximumAnimalCountLabel.Name = "maximumAnimalCountLabel";
            this.maximumAnimalCountLabel.NoWrap = false;
            this.maximumAnimalCountLabel.Size = new System.Drawing.Size(82, 12);
            this.maximumAnimalCountLabel.TabIndex = 27;
            this.maximumAnimalCountLabel.Text = "0";
            this.maximumAnimalCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // glassLabel15
            // 
            this.glassLabel15.BackColor = System.Drawing.Color.Transparent;
            this.glassLabel15.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.glassLabel15.ForeColor = System.Drawing.Color.White;
            this.glassLabel15.Location = new System.Drawing.Point(3, 162);
            this.glassLabel15.Name = "glassLabel15";
            this.glassLabel15.NoWrap = false;
            this.glassLabel15.Size = new System.Drawing.Size(129, 12);
            this.glassLabel15.TabIndex = 26;
            this.glassLabel15.Text = "Maximum Animals:";
            this.glassLabel15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // animalCountLabel
            // 
            this.animalCountLabel.BackColor = System.Drawing.Color.Transparent;
            this.animalCountLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.animalCountLabel.ForeColor = System.Drawing.Color.White;
            this.animalCountLabel.Location = new System.Drawing.Point(115, 148);
            this.animalCountLabel.Name = "animalCountLabel";
            this.animalCountLabel.NoWrap = false;
            this.animalCountLabel.Size = new System.Drawing.Size(82, 12);
            this.animalCountLabel.TabIndex = 25;
            this.animalCountLabel.Text = "0";
            this.animalCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // glassLabel9
            // 
            this.glassLabel9.BackColor = System.Drawing.Color.Transparent;
            this.glassLabel9.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.glassLabel9.ForeColor = System.Drawing.Color.White;
            this.glassLabel9.Location = new System.Drawing.Point(3, 148);
            this.glassLabel9.Name = "glassLabel9";
            this.glassLabel9.NoWrap = false;
            this.glassLabel9.Size = new System.Drawing.Size(120, 12);
            this.glassLabel9.TabIndex = 24;
            this.glassLabel9.Text = "Current Animals:";
            this.glassLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // peerCountLabel
            // 
            this.peerCountLabel.BackColor = System.Drawing.Color.Transparent;
            this.peerCountLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.peerCountLabel.ForeColor = System.Drawing.Color.White;
            this.peerCountLabel.Location = new System.Drawing.Point(115, 132);
            this.peerCountLabel.Name = "peerCountLabel";
            this.peerCountLabel.NoWrap = false;
            this.peerCountLabel.Size = new System.Drawing.Size(82, 12);
            this.peerCountLabel.TabIndex = 23;
            this.peerCountLabel.Text = "0";
            this.peerCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // glassLabel7
            // 
            this.glassLabel7.BackColor = System.Drawing.Color.Transparent;
            this.glassLabel7.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.glassLabel7.ForeColor = System.Drawing.Color.White;
            this.glassLabel7.Location = new System.Drawing.Point(3, 132);
            this.glassLabel7.Name = "glassLabel7";
            this.glassLabel7.NoWrap = false;
            this.glassLabel7.Size = new System.Drawing.Size(82, 12);
            this.glassLabel7.TabIndex = 22;
            this.glassLabel7.Text = "Peers:";
            this.glassLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gameModeLabel
            // 
            this.gameModeLabel.BackColor = System.Drawing.Color.Transparent;
            this.gameModeLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.gameModeLabel.ForeColor = System.Drawing.Color.White;
            this.gameModeLabel.Location = new System.Drawing.Point(115, 81);
            this.gameModeLabel.Name = "gameModeLabel";
            this.gameModeLabel.NoWrap = false;
            this.gameModeLabel.Size = new System.Drawing.Size(82, 12);
            this.gameModeLabel.TabIndex = 21;
            this.gameModeLabel.Text = "XXXXX";
            this.gameModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // glassLabel4
            // 
            this.glassLabel4.BackColor = System.Drawing.Color.Transparent;
            this.glassLabel4.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.glassLabel4.ForeColor = System.Drawing.Color.White;
            this.glassLabel4.Location = new System.Drawing.Point(3, 81);
            this.glassLabel4.Name = "glassLabel4";
            this.glassLabel4.NoWrap = false;
            this.glassLabel4.Size = new System.Drawing.Size(82, 12);
            this.glassLabel4.TabIndex = 20;
            this.glassLabel4.Text = "Game Mode:";
            this.glassLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.glassLabel4.Click += new System.EventHandler(this.glassLabel4_Click);
            // 
            // ledFour
            // 
            this.ledFour.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ledFour.BackColor = System.Drawing.Color.Transparent;
            this.ledFour.BorderColor = System.Drawing.Color.Black;
            this.ledFour.Depth = 4;
            this.ledFour.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ledFour.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ledFour.Highlight = false;
            this.ledFour.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.ledFour.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ledFour.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ledFour.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.ledFour.IsGlass = true;
            this.ledFour.LedName = "Received Peer-to-Peer Request";
            this.ledFour.LedState = Terrarium.Forms.LedStates.Waiting;
            this.ledFour.Location = new System.Drawing.Point(138, 30);
            this.ledFour.Name = "ledFour";
            this.ledFour.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.ledFour.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ledFour.Size = new System.Drawing.Size(32, 32);
            this.ledFour.TabIndex = 19;
            this.ledFour.TabStop = false;
            this.ledFour.UseStyles = false;
            this.ledFour.UseVisualStyleBackColor = false;
            // 
            // ledThree
            // 
            this.ledThree.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ledThree.BackColor = System.Drawing.Color.Transparent;
            this.ledThree.BorderColor = System.Drawing.Color.Black;
            this.ledThree.Depth = 4;
            this.ledThree.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ledThree.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ledThree.Highlight = false;
            this.ledThree.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.ledThree.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ledThree.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ledThree.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.ledThree.IsGlass = true;
            this.ledThree.LedName = "Sent Peer-to-Peer Request";
            this.ledThree.LedState = Terrarium.Forms.LedStates.Waiting;
            this.ledThree.Location = new System.Drawing.Point(100, 30);
            this.ledThree.Name = "ledThree";
            this.ledThree.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.ledThree.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ledThree.Size = new System.Drawing.Size(32, 32);
            this.ledThree.TabIndex = 18;
            this.ledThree.TabStop = false;
            this.ledThree.UseStyles = false;
            this.ledThree.UseVisualStyleBackColor = false;
            // 
            // ledTwo
            // 
            this.ledTwo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ledTwo.BackColor = System.Drawing.Color.Transparent;
            this.ledTwo.BorderColor = System.Drawing.Color.Black;
            this.ledTwo.Depth = 4;
            this.ledTwo.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ledTwo.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ledTwo.Highlight = false;
            this.ledTwo.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.ledTwo.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ledTwo.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ledTwo.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.ledTwo.IsGlass = true;
            this.ledTwo.LedName = "Peer-to-Peer Discovery Web Service";
            this.ledTwo.LedState = Terrarium.Forms.LedStates.Waiting;
            this.ledTwo.Location = new System.Drawing.Point(62, 30);
            this.ledTwo.Name = "ledTwo";
            this.ledTwo.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.ledTwo.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ledTwo.Size = new System.Drawing.Size(32, 32);
            this.ledTwo.TabIndex = 17;
            this.ledTwo.TabStop = false;
            this.ledTwo.UseStyles = false;
            this.ledTwo.UseVisualStyleBackColor = false;
            // 
            // ledOne
            // 
            this.ledOne.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ledOne.BackColor = System.Drawing.Color.Transparent;
            this.ledOne.BorderColor = System.Drawing.Color.Black;
            this.ledOne.Depth = 4;
            this.ledOne.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ledOne.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ledOne.Highlight = false;
            this.ledOne.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.ledOne.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ledOne.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ledOne.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.ledOne.IsGlass = true;
            this.ledOne.LedName = "Reporting Web Service";
            this.ledOne.LedState = Terrarium.Forms.LedStates.Waiting;
            this.ledOne.Location = new System.Drawing.Point(24, 30);
            this.ledOne.Name = "ledOne";
            this.ledOne.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.ledOne.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ledOne.Size = new System.Drawing.Size(32, 32);
            this.ledOne.TabIndex = 16;
            this.ledOne.TabStop = false;
            this.ledOne.UseStyles = false;
            this.ledOne.UseVisualStyleBackColor = false;
            // 
            // glassPanel1
            // 
            this.glassPanel1.Borders = ((Terrarium.Glass.GlassBorders)(((Terrarium.Glass.GlassBorders.Left | Terrarium.Glass.GlassBorders.Right)
                        | Terrarium.Glass.GlassBorders.Bottom)));
            this.glassPanel1.Controls.Add(this.glassLabel1);
            this.glassPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.glassPanel1.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.glassPanel1.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.glassPanel1.IsGlass = true;
            this.glassPanel1.IsSunk = false;
            this.glassPanel1.Location = new System.Drawing.Point(0, 0);
            this.glassPanel1.Name = "glassPanel1";
            this.glassPanel1.Size = new System.Drawing.Size(200, 24);
            this.glassPanel1.TabIndex = 1;
            this.glassPanel1.UseStyles = true;
            // 
            // glassLabel1
            // 
            this.glassLabel1.BackColor = System.Drawing.Color.Transparent;
            this.glassLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glassLabel1.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.glassLabel1.ForeColor = System.Drawing.Color.White;
            this.glassLabel1.Location = new System.Drawing.Point(0, 0);
            this.glassLabel1.Name = "glassLabel1";
            this.glassLabel1.NoWrap = false;
            this.glassLabel1.Size = new System.Drawing.Size(200, 24);
            this.glassLabel1.TabIndex = 0;
            this.glassLabel1.Text = " Status";
            this.glassLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DeveloperPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Navy;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Name = "DeveloperPanel";
            this.Size = new System.Drawing.Size(200, 512);
            this.panel3.ResumeLayout(false);
            this.glassPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.navigatePictureBox)).EndInit();
            this.glassPanel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.glassPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private Terrarium.Glass.GlassPanel glassPanel2;
        private System.Windows.Forms.PictureBox navigatePictureBox;
        private Terrarium.Glass.GlassPanel glassPanel3;
        private Terrarium.Glass.GlassLabel glassLabel3;
        private Terrarium.Glass.GlassLabel webRootLabel;
        private Terrarium.Glass.GlassLabel failedReceivesCountLabel;
        private Terrarium.Glass.GlassLabel glassLabel11;
        private Terrarium.Glass.GlassLabel failedSendsCountLabel;
        private Terrarium.Glass.GlassLabel glassLabel5;
        private Terrarium.Glass.GlassLabel teleportationsCountLabel;
        private Terrarium.Glass.GlassLabel glassLabel13;
        private Terrarium.Glass.GlassLabel maximumAnimalCountLabel;
        private Terrarium.Glass.GlassLabel glassLabel15;
        private Terrarium.Glass.GlassLabel animalCountLabel;
        private Terrarium.Glass.GlassLabel glassLabel9;
        private Terrarium.Glass.GlassLabel peerCountLabel;
        private Terrarium.Glass.GlassLabel glassLabel7;
        private Terrarium.Glass.GlassLabel gameModeLabel;
        private Terrarium.Glass.GlassLabel glassLabel4;
        private TerrariumLed ledFour;
        private TerrariumLed ledThree;
        private TerrariumLed ledTwo;
        private TerrariumLed ledOne;
        private Terrarium.Glass.GlassPanel glassPanel1;
        private Terrarium.Glass.GlassLabel glassLabel1;
	}
}
