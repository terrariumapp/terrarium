//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------


namespace Terrarium.Forms
{
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.ComponentModel;
	using System.ComponentModel.Design;
	using System.Drawing.Design;

	/// <summary>
	/// 
	/// </summary>
	public class StatusBar : UserControl
	{
		private System.Windows.Forms.Panel ledPanel;
		private Terrarium.Forms.TerrariumLed ledFour;
		private Terrarium.Forms.TerrariumLed ledThree;
		private Terrarium.Forms.TerrariumLed ledTwo;
		private Terrarium.Forms.TerrariumLed ledOne;
		
		/// <summary>
		///  The current game mode, Ecosystem or Terrarium
		/// </summary>
		private GameModes	mode;
		private string		modeText;
		private string		webRoot;
		private string		animalText;
		private string		peerText;
		private string		networkText;

		private TerrariumLed[]		leds = new Terrarium.Forms.TerrariumLed[4];
		private Terrarium.Metal.MetalPanel statusPanel;
		private Terrarium.Metal.MetalPanel statisticsPanel;
		private Terrarium.Metal.MetalPanel modePanel;
		private System.Windows.Forms.PictureBox modeImage;
		private Terrarium.Metal.MetalLabel modeLabel;
		private System.Windows.Forms.PictureBox pictureBox1;
		private Terrarium.Metal.MetalLabel statisticsLabel;

		/// <summary>
		///  The current game mode
		/// </summary>
		private ScreenSaverMode screenSaverMode;

		/// <summary>
		/// 
		/// </summary>
		public StatusBar()
		{
			InitializeComponent();

			// Hook up the Leds
			leds[0] = this.ledOne;
			leds[1] = this.ledTwo;
			leds[2] = this.ledThree;
			leds[3] = this.ledFour;
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatusBar));
			this.statusPanel = new Terrarium.Metal.MetalPanel();
			this.statisticsPanel = new Terrarium.Metal.MetalPanel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.statisticsLabel = new Terrarium.Metal.MetalLabel();
			this.modePanel = new Terrarium.Metal.MetalPanel();
			this.modeLabel = new Terrarium.Metal.MetalLabel();
			this.modeImage = new System.Windows.Forms.PictureBox();
			this.ledPanel = new System.Windows.Forms.Panel();
			this.ledFour = new Terrarium.Forms.TerrariumLed();
			this.ledThree = new Terrarium.Forms.TerrariumLed();
			this.ledTwo = new Terrarium.Forms.TerrariumLed();
			this.ledOne = new Terrarium.Forms.TerrariumLed();
			this.statusPanel.SuspendLayout();
			this.statisticsPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.modePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.modeImage)).BeginInit();
			this.ledPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusPanel
			// 
			this.statusPanel.Borders = ((Terrarium.Metal.MetalBorders)(((Terrarium.Metal.MetalBorders.Left | Terrarium.Metal.MetalBorders.Right)
						| Terrarium.Metal.MetalBorders.Bottom)));
			this.statusPanel.Controls.Add(this.statisticsPanel);
			this.statusPanel.Controls.Add(this.modePanel);
			this.statusPanel.Controls.Add(this.ledPanel);
			this.statusPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.statusPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.statusPanel.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.statusPanel.Location = new System.Drawing.Point(0, 0);
			this.statusPanel.Name = "statusPanel";
			this.statusPanel.Size = new System.Drawing.Size(800, 16);
			this.statusPanel.Sunk = false;
			this.statusPanel.TabIndex = 5;
			this.statusPanel.UseStyles = true;
			// 
			// statisticsPanel
			// 
			this.statisticsPanel.Borders = ((Terrarium.Metal.MetalBorders)(((Terrarium.Metal.MetalBorders.Left | Terrarium.Metal.MetalBorders.Right)
						| Terrarium.Metal.MetalBorders.Bottom)));
			this.statisticsPanel.Controls.Add(this.pictureBox1);
			this.statisticsPanel.Controls.Add(this.statisticsLabel);
			this.statisticsPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.statisticsPanel.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.statisticsPanel.Location = new System.Drawing.Point(480, 0);
			this.statisticsPanel.Name = "statisticsPanel";
			this.statisticsPanel.Size = new System.Drawing.Size(296, 16);
			this.statisticsPanel.Sunk = true;
			this.statisticsPanel.TabIndex = 8;
			this.statisticsPanel.UseStyles = true;
			this.statisticsPanel.Resize += new System.EventHandler(this.statisticsPanel_Resize);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(4, 2);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(12, 12);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 10;
			this.pictureBox1.TabStop = false;
			// 
			// statisticsLabel
			// 
			this.statisticsLabel.BackColor = System.Drawing.Color.Transparent;
			this.statisticsLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statisticsLabel.ForeColor = System.Drawing.Color.White;
			this.statisticsLabel.Location = new System.Drawing.Point(24, 0);
			this.statisticsLabel.Name = "statisticsLabel";
			this.statisticsLabel.NoWrap = true;
			this.statisticsLabel.Size = new System.Drawing.Size(264, 16);
			this.statisticsLabel.TabIndex = 3;
			this.statisticsLabel.Text = "terrarium system statistics text";
			this.statisticsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// modePanel
			// 
			this.modePanel.Borders = ((Terrarium.Metal.MetalBorders)(((Terrarium.Metal.MetalBorders.Left | Terrarium.Metal.MetalBorders.Right)
						| Terrarium.Metal.MetalBorders.Bottom)));
			this.modePanel.Controls.Add(this.modeLabel);
			this.modePanel.Controls.Add(this.modeImage);
			this.modePanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.modePanel.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.modePanel.Location = new System.Drawing.Point(24, 0);
			this.modePanel.Name = "modePanel";
			this.modePanel.Size = new System.Drawing.Size(296, 16);
			this.modePanel.Sunk = true;
			this.modePanel.TabIndex = 7;
			this.modePanel.UseStyles = true;
			this.modePanel.Resize += new System.EventHandler(this.modePanel_Resize);
			// 
			// modeLabel
			// 
			this.modeLabel.BackColor = System.Drawing.Color.Transparent;
			this.modeLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
			this.modeLabel.ForeColor = System.Drawing.Color.White;
			this.modeLabel.Location = new System.Drawing.Point(24, 0);
			this.modeLabel.Name = "modeLabel";
			this.modeLabel.NoWrap = true;
			this.modeLabel.Size = new System.Drawing.Size(264, 16);
			this.modeLabel.TabIndex = 10;
			this.modeLabel.Text = "Game Mode Stuff";
			this.modeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// modeImage
			// 
			this.modeImage.BackColor = System.Drawing.Color.Transparent;
			this.modeImage.Image = ((System.Drawing.Image)(resources.GetObject("modeImage.Image")));
			this.modeImage.Location = new System.Drawing.Point(4, 2);
			this.modeImage.Name = "modeImage";
			this.modeImage.Size = new System.Drawing.Size(12, 12);
			this.modeImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.modeImage.TabIndex = 9;
			this.modeImage.TabStop = false;
			// 
			// ledPanel
			// 
			this.ledPanel.BackColor = System.Drawing.Color.Transparent;
			this.ledPanel.Controls.Add(this.ledFour);
			this.ledPanel.Controls.Add(this.ledThree);
			this.ledPanel.Controls.Add(this.ledTwo);
			this.ledPanel.Controls.Add(this.ledOne);
			this.ledPanel.Location = new System.Drawing.Point(360, 0);
			this.ledPanel.Name = "ledPanel";
			this.ledPanel.Size = new System.Drawing.Size(88, 16);
			this.ledPanel.TabIndex = 2;
			// 
			// ledFour
			// 
			this.ledFour.BackColor = System.Drawing.Color.Transparent;
			this.ledFour.BorderColor = System.Drawing.Color.Black;
			this.ledFour.Depth = 3;
			this.ledFour.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(96)))), ((int)(((byte)(0)))));
			this.ledFour.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.ledFour.Highlight = false;
			this.ledFour.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.ledFour.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.ledFour.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.ledFour.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.ledFour.IsGlass = true;
			this.ledFour.LedName = "Received Peer-to-Peer Request";
			this.ledFour.LedState = Terrarium.Forms.LedStates.Idle;
			this.ledFour.Location = new System.Drawing.Point(72, 0);
			this.ledFour.Name = "ledFour";
			this.ledFour.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.ledFour.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.ledFour.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.ledFour.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.ledFour.Size = new System.Drawing.Size(16, 16);
			this.ledFour.TabIndex = 11;
			this.ledFour.TabStop = false;
			this.ledFour.UseStyles = false;
			this.ledFour.UseVisualStyleBackColor = false;
			// 
			// ledThree
			// 
			this.ledThree.BackColor = System.Drawing.Color.Transparent;
			this.ledThree.BorderColor = System.Drawing.Color.Black;
			this.ledThree.Depth = 3;
			this.ledThree.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(96)))), ((int)(((byte)(0)))));
			this.ledThree.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.ledThree.Highlight = false;
			this.ledThree.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.ledThree.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.ledThree.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.ledThree.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.ledThree.IsGlass = true;
			this.ledThree.LedName = "Sent Peer-to-Peer Request";
			this.ledThree.LedState = Terrarium.Forms.LedStates.Idle;
			this.ledThree.Location = new System.Drawing.Point(48, 0);
			this.ledThree.Name = "ledThree";
			this.ledThree.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.ledThree.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.ledThree.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.ledThree.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.ledThree.Size = new System.Drawing.Size(16, 16);
			this.ledThree.TabIndex = 10;
			this.ledThree.TabStop = false;
			this.ledThree.UseStyles = false;
			this.ledThree.UseVisualStyleBackColor = false;
			// 
			// ledTwo
			// 
			this.ledTwo.BackColor = System.Drawing.Color.Transparent;
			this.ledTwo.BorderColor = System.Drawing.Color.Black;
			this.ledTwo.Depth = 3;
			this.ledTwo.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(96)))), ((int)(((byte)(0)))));
			this.ledTwo.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.ledTwo.Highlight = false;
			this.ledTwo.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.ledTwo.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.ledTwo.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.ledTwo.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.ledTwo.IsGlass = true;
			this.ledTwo.LedName = "Peer-to-Peer Discovery Web Service";
			this.ledTwo.LedState = Terrarium.Forms.LedStates.Idle;
			this.ledTwo.Location = new System.Drawing.Point(24, 0);
			this.ledTwo.Name = "ledTwo";
			this.ledTwo.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.ledTwo.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.ledTwo.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.ledTwo.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.ledTwo.Size = new System.Drawing.Size(16, 16);
			this.ledTwo.TabIndex = 9;
			this.ledTwo.TabStop = false;
			this.ledTwo.UseStyles = false;
			this.ledTwo.UseVisualStyleBackColor = false;
			// 
			// ledOne
			// 
			this.ledOne.BackColor = System.Drawing.Color.Transparent;
			this.ledOne.BorderColor = System.Drawing.Color.Black;
			this.ledOne.Depth = 3;
			this.ledOne.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
			this.ledOne.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.ledOne.Highlight = false;
			this.ledOne.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.ledOne.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.ledOne.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.ledOne.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.ledOne.IsGlass = true;
			this.ledOne.LedName = "Reporting Web Service";
			this.ledOne.LedState = Terrarium.Forms.LedStates.Failed;
			this.ledOne.Location = new System.Drawing.Point(0, 0);
			this.ledOne.Name = "ledOne";
			this.ledOne.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.ledOne.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.ledOne.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.ledOne.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.ledOne.Size = new System.Drawing.Size(16, 16);
			this.ledOne.TabIndex = 8;
			this.ledOne.TabStop = false;
			this.ledOne.UseStyles = false;
			this.ledOne.UseVisualStyleBackColor = false;
			// 
			// StatusBar
			// 
			this.Controls.Add(this.statusPanel);
			this.Name = "StatusBar";
			this.Size = new System.Drawing.Size(800, 16);
			this.Resize += new System.EventHandler(this.StatusBar_Resize);
			this.statusPanel.ResumeLayout(false);
			this.statisticsPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.modePanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.modeImage)).EndInit();
			this.ledPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		private void StatusBar_Resize(object sender, System.EventArgs e)
		{
			int totalWidth = this.Width;
			int usedWidth = ledPanel.Width + 96;
			int availableWidth = totalWidth - usedWidth;

			modePanel.Width = availableWidth / 2;
			statisticsPanel.Width = modePanel.Width;

			statisticsPanel.Left = totalWidth - (statisticsPanel.Width + 24);

			ledPanel.Left = totalWidth / 2 - ledPanel.Width / 2;

		}

		private void modePanel_Resize(object sender, System.EventArgs e)
		{
			this.modeLabel.Width = this.modePanel.Width - this.modeLabel.Left - 16;
		}

		private void statisticsPanel_Resize(object sender, System.EventArgs e)
		{
			this.statisticsLabel.Width = this.statisticsPanel.Width - this.statisticsLabel.Left - 16;
		}

		/// <summary>
		///  Number of creatures in the current terrarium.
		/// </summary>
		public string AnimalCount
		{
			set
			{
				this.animalText = "Animals: " + value;
				this.statisticsLabel.Text = this.animalText + "   " + this.peerText + "   " + this.networkText;
			}
		}

		/// <summary>
		///  Number of creatures in the current terrarium.
		/// </summary>
		public string NetworkCount
		{
			set
			{
				this.networkText = "Teleport: " + value;
				this.statisticsLabel.Text = this.animalText + "   " + this.peerText + "   " + this.networkText;
			}
		}

		/// <summary>
		///  Current game running mode.  Used to disable features.
		/// </summary>
		public ScreenSaverMode Mode
		{
			get 
			{
				return screenSaverMode;
			}
        
			set 
			{
				screenSaverMode = value;
			}
		}

		/// <summary>
		///  Current game mode.  Either Terrarium or EcoSystem.
		/// </summary>
		public GameModes GameMode 
		{
			get
			{
				return mode;
			}
        
			set
			{
				mode = value;
        // TODO: Need to implement
//				this.modePictureBox.Image = images[(int)mode];
//				this.modePictureBox.Invalidate();
			}
		}

		/// <summary>
		///  Text to display next to the mode images.
		/// </summary>
		[Browsable(false)]
		public string ModeText 
		{
			get 
			{
				return this.modeText;
			}
        
			set
			{
				this.modeText = value;
				this.modeLabel.Text = this.modeText + ":  " + this.webRoot;
			}
		}

		/// <summary>
		///  Allows the application to set the WebRoot
		///  used in the Help link.
		/// </summary>
		[Browsable(false)]
		public string WebRoot
		{
			get 
			{
				return this.webRoot;
			}
        
			set
			{
				this.webRoot = value;
				this.modeLabel.Text = this.modeText + ":  " + this.webRoot;
			}
		}

		/// <summary>
		///  Count of the number of peers in the current Terrarium
		///  channel or Ecosystem.
		/// </summary>
		[Browsable(false)]
		public int PeerCount 
		{
			set
			{
				this.peerText = "Peers: " + value.ToString();
				this.statisticsLabel.Text = this.animalText + "   " + this.peerText + "   " + this.networkText;
			}
		}

		/// <summary>
		///  Provides quick array based access to fall 5 
		///  status LEDs within the TerrariumTopPanel.
		/// </summary>
		[Browsable(false)]
		public TerrariumLed [] Leds
		{
			get
			{
				return leds;
			}
		}
	}
}
