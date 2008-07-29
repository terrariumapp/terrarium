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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(StatusBar));
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
			this.modePanel.SuspendLayout();
			this.ledPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusPanel
			// 
			this.statusPanel.Borders = ((Terrarium.Metal.MetalBorders)((Terrarium.Metal.MetalBorders.LeftAndRight | Terrarium.Metal.MetalBorders.Bottom)));
			this.statusPanel.Controls.Add(this.statisticsPanel);
			this.statusPanel.Controls.Add(this.modePanel);
			this.statusPanel.Controls.Add(this.ledPanel);
			this.statusPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.statusPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.statusPanel.Gradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.statusPanel.Location = new System.Drawing.Point(0, 0);
			this.statusPanel.Name = "statusPanel";
			this.statusPanel.Size = new System.Drawing.Size(800, 16);
			this.statusPanel.Sunk = false;
			this.statusPanel.TabIndex = 5;
			this.statusPanel.UseStyles = true;
			// 
			// statisticsPanel
			// 
			this.statisticsPanel.Borders = ((Terrarium.Metal.MetalBorders)((Terrarium.Metal.MetalBorders.LeftAndRight | Terrarium.Metal.MetalBorders.Bottom)));
			this.statisticsPanel.Controls.Add(this.pictureBox1);
			this.statisticsPanel.Controls.Add(this.statisticsLabel);
			this.statisticsPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.statisticsPanel.Gradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
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
			this.statisticsLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
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
			this.modePanel.Borders = ((Terrarium.Metal.MetalBorders)((Terrarium.Metal.MetalBorders.LeftAndRight | Terrarium.Metal.MetalBorders.Bottom)));
			this.modePanel.Controls.Add(this.modeLabel);
			this.modePanel.Controls.Add(this.modeImage);
			this.modePanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.modePanel.Gradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
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
			this.ledFour.Borders = ((Terrarium.Metal.MetalBorders)((Terrarium.Metal.MetalBorders.LeftAndRight | Terrarium.Metal.MetalBorders.Bottom)));
			this.ledFour.Gradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(96)), ((System.Byte)(0)));
			this.ledFour.Gradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(255)), ((System.Byte)(64)));
			this.ledFour.LedName = "Received Peer-to-Peer Request";
			this.ledFour.LedState = Terrarium.Forms.LedStates.Idle;
			this.ledFour.Location = new System.Drawing.Point(72, 0);
			this.ledFour.Name = "ledFour";
			this.ledFour.Size = new System.Drawing.Size(16, 16);
			this.ledFour.Sunk = false;
			this.ledFour.TabIndex = 11;
			this.ledFour.UseStyles = false;
			// 
			// ledThree
			// 
			this.ledThree.Borders = ((Terrarium.Metal.MetalBorders)((Terrarium.Metal.MetalBorders.LeftAndRight | Terrarium.Metal.MetalBorders.Bottom)));
			this.ledThree.Gradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(96)), ((System.Byte)(0)));
			this.ledThree.Gradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(255)), ((System.Byte)(64)));
			this.ledThree.LedName = "Sent Peer-to-Peer Request";
			this.ledThree.LedState = Terrarium.Forms.LedStates.Idle;
			this.ledThree.Location = new System.Drawing.Point(48, 0);
			this.ledThree.Name = "ledThree";
			this.ledThree.Size = new System.Drawing.Size(16, 16);
			this.ledThree.Sunk = false;
			this.ledThree.TabIndex = 10;
			this.ledThree.UseStyles = false;
			// 
			// ledTwo
			// 
			this.ledTwo.Borders = ((Terrarium.Metal.MetalBorders)((Terrarium.Metal.MetalBorders.LeftAndRight | Terrarium.Metal.MetalBorders.Bottom)));
			this.ledTwo.Gradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(96)), ((System.Byte)(0)));
			this.ledTwo.Gradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(255)), ((System.Byte)(64)));
			this.ledTwo.LedName = "Peer-to-Peer Discovery Web Service";
			this.ledTwo.LedState = Terrarium.Forms.LedStates.Idle;
			this.ledTwo.Location = new System.Drawing.Point(24, 0);
			this.ledTwo.Name = "ledTwo";
			this.ledTwo.Size = new System.Drawing.Size(16, 16);
			this.ledTwo.Sunk = false;
			this.ledTwo.TabIndex = 9;
			this.ledTwo.UseStyles = false;
			// 
			// ledOne
			// 
			this.ledOne.Borders = ((Terrarium.Metal.MetalBorders)((Terrarium.Metal.MetalBorders.LeftAndRight | Terrarium.Metal.MetalBorders.Bottom)));
			this.ledOne.Gradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(96)), ((System.Byte)(0)));
			this.ledOne.Gradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(255)), ((System.Byte)(64)));
			this.ledOne.LedName = "Reporting Web Service";
			this.ledOne.LedState = Terrarium.Forms.LedStates.Idle;
			this.ledOne.Location = new System.Drawing.Point(0, 0);
			this.ledOne.Name = "ledOne";
			this.ledOne.Size = new System.Drawing.Size(16, 16);
			this.ledOne.Sunk = false;
			this.ledOne.TabIndex = 8;
			this.ledOne.UseStyles = false;
			// 
			// StatusBar
			// 
			this.Controls.Add(this.statusPanel);
			this.Name = "StatusBar";
			this.Size = new System.Drawing.Size(800, 16);
			this.Resize += new System.EventHandler(this.StatusBar_Resize);
			this.statusPanel.ResumeLayout(false);
			this.statisticsPanel.ResumeLayout(false);
			this.modePanel.ResumeLayout(false);
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
