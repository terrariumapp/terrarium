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

	using Terrarium.Glass;

	/// <summary>
	/// 
	/// </summary>
	public class GlassBottomPanel : UserControl
	{

		/// <summary>
		///  The current game mode used to enable some features
		///  and disable others as well as set up initial tab
		///  display.
		/// </summary>
		private GameModes mode;
		private GlassPanel GlassPanel1;
		private ToolTip controlsToolTip;
        private IContainer components;
		private Panel panel1;
		private Panel controlsMasterPanel;
		private GlassButton loadTerrariumButton;
		private GlassButton newTerrariumButton;
		private GlassButton joinEcosystemButton;
		private Panel panel2;
		private Panel ecosystemControlsPanel;
		private GlassButton reintroduceAnimalButton;
		private GlassButton introduceAnimalEcosystemButton;
		private Panel terrariumControlsPanel;
		private GlassButton pauseButton;
		private GlassButton addAnimalButton;
		private GlassButton introduceAnimalTerrariumButton;
        private ComboBox addAnimalComboBox;
		private Panel buttonsPanel;
		private GlassButton traceButton;
		private GlassButton statisticsButton;
		private GlassButton detailsButton;
        private GlassButton settingsButton;
        private TickerBar tickerBar;
        private GlassButton developerPanelButton;
		/// <summary>
		///  The current screen saver mode used to enable some
		///  features and disable others.
		/// </summary>
		private ScreenSaverMode screenSaverMode;

		#region Terrarium Bottom Panel Public Control Fields

        /// <summary>
        ///  Access to the SettingsButton
        /// </summary>
        [Browsable(false)]
        public GlassButton DeveloperButton
        {
            get
            {
                return this.developerPanelButton;
            }
        }

		/// <summary>
		///  Access to the SettingsButton
		/// </summary>
		[Browsable(false)]
		public GlassButton SettingsButton
		{
			get
			{
				return this.settingsButton;
			}
		}

		/// <summary>
		///  Access to the DetailsButton
		/// </summary>
		[Browsable(false)]
		public GlassButton DetailsButton
		{
			get
			{
				return this.detailsButton;
			}
		}
		
		/// <summary>
		///  Access to the StatisticsButton
		/// </summary>
		[Browsable(false)]
		public GlassButton StatisticsButton
		{
			get
			{
				return this.statisticsButton;
			}
		}

		/// <summary>
		///  Access to the TraceButton
		/// </summary>
		[Browsable(false)]
		public GlassButton TraceButton
		{
			get
			{
				return this.traceButton;
			}
		}

		/// <summary>
		///  Access to the AddAnimalComboBox
		/// </summary>
		[Browsable(false)]
		public ComboBox AddAnimalComboBox
		{
			get
			{
				return addAnimalComboBox;
			}
		}

		/// <summary>
		///  Access to the AddAnimalButton
		/// </summary>
		[Browsable(false)]
		public GlassButton AddAnimalButton
		{
			get
			{
				return addAnimalButton;
			}
		}

		/// <summary>
		///  Access to the IntroduceAnimalEcosystemButton
		/// </summary>
		[Browsable(false)]
		public GlassButton IntroduceAnimalEcosystemButton
		{
			get
			{
				return introduceAnimalEcosystemButton;
			}
		}

		/// <summary>
		///  Access to the IntroduceAnimalTerrariumButton
		/// </summary>
		[Browsable(false)]
		public GlassButton IntroduceAnimalTerrariumButton
		{
			get
			{
				return introduceAnimalTerrariumButton;
			}
		}

		/// <summary>
		///  Access to the JoinEcosystemButton
		/// </summary>
		[Browsable(false)]
		public GlassButton JoinEcosystemButton
		{
			get
			{
				return joinEcosystemButton;
			}
		}

		/// <summary>
		///  Access to the LoadTerrariumButton
		/// </summary>
		[Browsable(false)]
		public GlassButton LoadTerrariumButton
		{
			get
			{
				return loadTerrariumButton;
			}
		}

		/// <summary>
		///  Access to the NewTerrariumButton
		/// </summary>
		[Browsable(false)]
		public GlassButton NewTerrariumButton
		{
			get
			{
				return newTerrariumButton;
			}
		}

		/// <summary>
		///  Access to the PauseButton
		/// </summary>
		[Browsable(false)]
		public GlassButton PauseButton
		{
			get
			{
				return pauseButton;
			}
		}

		/// <summary>
		///  Access to the ReintroduceAnimalButton
		/// </summary>
		[Browsable(false)]
		public GlassButton ReintroduceAnimalButton
		{
			get
			{
				return reintroduceAnimalButton;
			}
		}

		#endregion

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
				if (mode == GameModes.Ecosystem)
				{
					this.ecosystemControlsPanel.Visible = true;
					this.terrariumControlsPanel.Visible = false;
				}
				else
				{
					this.terrariumControlsPanel.Visible = true;
					this.ecosystemControlsPanel.Visible = false;
				}

				int panelWidth = 0;
				foreach (Control childControl in this.GlassPanel1.Controls)
				{
					if (childControl.Visible == true)
						panelWidth += childControl.Width;
				}
				this.GlassPanel1.Width = panelWidth;

				pauseButton.Enabled = (mode == GameModes.Terrarium);
				addAnimalButton.Enabled = (mode == GameModes.Terrarium);
				addAnimalComboBox.Enabled = (mode == GameModes.Terrarium);
				introduceAnimalTerrariumButton.Enabled = (mode == GameModes.Terrarium);
				joinEcosystemButton.Enabled = (mode == GameModes.Terrarium);

				introduceAnimalEcosystemButton.Enabled = (mode == GameModes.Ecosystem);
				reintroduceAnimalButton.Enabled = (mode == GameModes.Ecosystem);
			}
		}

		/// <summary>
		/// The messaging ticker
		/// </summary>
		public TickerBar Ticker
		{
			get
			{
				return this.tickerBar;
			}
		}

		#region Event Handlers
		private void InvalidateViewportRect()
		{
			//viewPortSizeOnMap = new Size((viewPortSize.Width * this.navigatePictureBox.Width) / landSize.Width,
			//    (viewPortSize.Height * this.navigatePictureBox.Height) / landSize.Height);
		}

		/// <summary>
		///  Make sure we invalidate the viewport rectangle if the size of the
		///  picture box changes.  In the current implementation this won't
		///  happen, but this will help enable a skinning mode.
		/// </summary>
		/// <param name="sender">PictureBox</param>
		/// <param name="e">Empty event arguments</param>
		private void navigatePictureBox_Resize(object sender, System.EventArgs e)
		{
			InvalidateViewportRect();
		}
		  
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);

			//int totalWidth = this.Width;
			//int subPanelWidth = (totalWidth-miniMapWidth) / 2;
			//int buttonsPanelWidth = buttonsPanel.Width;


			//controlsMasterPanel.Left = subPanelWidth/2 - controlsMasterPanel.Width/2;

			//traceTextBox.Left = (subPanelWidth/2 - traceTextBox.Width/2) + subPanelWidth + miniMapWidth;

			//buttonsPanel.Left = 16; //totalWidth/2 - buttonsPanelWidth/2;

			//tickerBar.Width = totalWidth - buttonsPanel.Width - 48;

			//tickerBar.Left = buttonsPanel.Left + buttonsPanel.Width + 16;

		}

		/// <summary>
		/// 
		/// </summary>
		public GlassBottomPanel()
		{
			InitializeComponent();

			controlsToolTip.SetToolTip(this.settingsButton, "View and change current game settings.");
			controlsToolTip.SetToolTip(this.detailsButton, "View details about the selected organism.");
			controlsToolTip.SetToolTip(this.statisticsButton, "Open the statistics window for your local Ecosystem.");
			controlsToolTip.SetToolTip(this.traceButton, "Open the diagnostic trace window.");

			controlsToolTip.SetToolTip(this.joinEcosystemButton, "Switch to Ecosystem mode.");
			controlsToolTip.SetToolTip(this.newTerrariumButton, "Switch to Terrarium mode and create a new Terrarium.");
			controlsToolTip.SetToolTip(this.loadTerrariumButton, "Switch to Terrarium mode and load an existing Terrarium.");

            controlsToolTip.SetToolTip(this.developerPanelButton, "Show or Hide the Developer Panel.");

		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlassBottomPanel));
            this.GlassPanel1 = new Terrarium.Glass.GlassPanel();
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.traceButton = new Terrarium.Glass.GlassButton();
            this.statisticsButton = new Terrarium.Glass.GlassButton();
            this.detailsButton = new Terrarium.Glass.GlassButton();
            this.settingsButton = new Terrarium.Glass.GlassButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.controlsMasterPanel = new System.Windows.Forms.Panel();
            this.loadTerrariumButton = new Terrarium.Glass.GlassButton();
            this.newTerrariumButton = new Terrarium.Glass.GlassButton();
            this.joinEcosystemButton = new Terrarium.Glass.GlassButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ecosystemControlsPanel = new System.Windows.Forms.Panel();
            this.reintroduceAnimalButton = new Terrarium.Glass.GlassButton();
            this.introduceAnimalEcosystemButton = new Terrarium.Glass.GlassButton();
            this.terrariumControlsPanel = new System.Windows.Forms.Panel();
            this.pauseButton = new Terrarium.Glass.GlassButton();
            this.addAnimalButton = new Terrarium.Glass.GlassButton();
            this.introduceAnimalTerrariumButton = new Terrarium.Glass.GlassButton();
            this.addAnimalComboBox = new System.Windows.Forms.ComboBox();
            this.controlsToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tickerBar = new Terrarium.Forms.TickerBar();
            this.developerPanelButton = new Terrarium.Glass.GlassButton();
            this.GlassPanel1.SuspendLayout();
            this.buttonsPanel.SuspendLayout();
            this.controlsMasterPanel.SuspendLayout();
            this.ecosystemControlsPanel.SuspendLayout();
            this.terrariumControlsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // GlassPanel1
            // 
            this.GlassPanel1.Borders = ((Terrarium.Glass.GlassBorders)((Terrarium.Glass.GlassBorders.Top | Terrarium.Glass.GlassBorders.Bottom)));
            this.GlassPanel1.Controls.Add(this.buttonsPanel);
            this.GlassPanel1.Controls.Add(this.panel1);
            this.GlassPanel1.Controls.Add(this.controlsMasterPanel);
            this.GlassPanel1.Controls.Add(this.panel2);
            this.GlassPanel1.Controls.Add(this.ecosystemControlsPanel);
            this.GlassPanel1.Controls.Add(this.terrariumControlsPanel);
            this.GlassPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.GlassPanel1.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.GlassPanel1.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.GlassPanel1.IsGlass = true;
            this.GlassPanel1.IsSunk = false;
            this.GlassPanel1.Location = new System.Drawing.Point(286, 0);
            this.GlassPanel1.Name = "GlassPanel1";
            this.GlassPanel1.Size = new System.Drawing.Size(702, 48);
            this.GlassPanel1.TabIndex = 3;
            this.GlassPanel1.UseStyles = true;
            // 
            // buttonsPanel
            // 
            this.buttonsPanel.BackColor = System.Drawing.Color.Transparent;
            this.buttonsPanel.Controls.Add(this.developerPanelButton);
            this.buttonsPanel.Controls.Add(this.traceButton);
            this.buttonsPanel.Controls.Add(this.statisticsButton);
            this.buttonsPanel.Controls.Add(this.detailsButton);
            this.buttonsPanel.Controls.Add(this.settingsButton);
            this.buttonsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonsPanel.Location = new System.Drawing.Point(1, 0);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(225, 48);
            this.buttonsPanel.TabIndex = 33;
            // 
            // traceButton
            // 
            this.traceButton.BackColor = System.Drawing.Color.Transparent;
            this.traceButton.BorderColor = System.Drawing.Color.Black;
            this.traceButton.Depth = 4;
            this.traceButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.traceButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.traceButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.traceButton.ForeColor = System.Drawing.Color.White;
            this.traceButton.Highlight = false;
            this.traceButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.traceButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.traceButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.traceButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.traceButton.Image = ((System.Drawing.Image)(resources.GetObject("traceButton.Image")));
            this.traceButton.IsGlass = true;
            this.traceButton.Location = new System.Drawing.Point(178, 2);
            this.traceButton.Name = "traceButton";
            this.traceButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.traceButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.traceButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.traceButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.traceButton.Size = new System.Drawing.Size(44, 44);
            this.traceButton.TabIndex = 11;
            this.traceButton.TabStop = false;
            this.traceButton.UseStyles = true;
            this.traceButton.UseVisualStyleBackColor = false;
            // 
            // statisticsButton
            // 
            this.statisticsButton.BackColor = System.Drawing.Color.Transparent;
            this.statisticsButton.BorderColor = System.Drawing.Color.Black;
            this.statisticsButton.Depth = 4;
            this.statisticsButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.statisticsButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.statisticsButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statisticsButton.ForeColor = System.Drawing.Color.White;
            this.statisticsButton.Highlight = false;
            this.statisticsButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.statisticsButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.statisticsButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.statisticsButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.statisticsButton.Image = ((System.Drawing.Image)(resources.GetObject("statisticsButton.Image")));
            this.statisticsButton.IsGlass = true;
            this.statisticsButton.Location = new System.Drawing.Point(134, 2);
            this.statisticsButton.Name = "statisticsButton";
            this.statisticsButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.statisticsButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.statisticsButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.statisticsButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.statisticsButton.Size = new System.Drawing.Size(44, 44);
            this.statisticsButton.TabIndex = 10;
            this.statisticsButton.TabStop = false;
            this.statisticsButton.UseStyles = true;
            this.statisticsButton.UseVisualStyleBackColor = false;
            // 
            // detailsButton
            // 
            this.detailsButton.BackColor = System.Drawing.Color.Transparent;
            this.detailsButton.BorderColor = System.Drawing.Color.Black;
            this.detailsButton.Depth = 4;
            this.detailsButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.detailsButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.detailsButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.detailsButton.ForeColor = System.Drawing.Color.White;
            this.detailsButton.Highlight = false;
            this.detailsButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.detailsButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.detailsButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.detailsButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.detailsButton.Image = ((System.Drawing.Image)(resources.GetObject("detailsButton.Image")));
            this.detailsButton.IsGlass = true;
            this.detailsButton.Location = new System.Drawing.Point(90, 2);
            this.detailsButton.Name = "detailsButton";
            this.detailsButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.detailsButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.detailsButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.detailsButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.detailsButton.Size = new System.Drawing.Size(44, 44);
            this.detailsButton.TabIndex = 9;
            this.detailsButton.TabStop = false;
            this.detailsButton.UseStyles = true;
            this.detailsButton.UseVisualStyleBackColor = false;
            // 
            // settingsButton
            // 
            this.settingsButton.BackColor = System.Drawing.Color.Transparent;
            this.settingsButton.BorderColor = System.Drawing.Color.Black;
            this.settingsButton.Depth = 4;
            this.settingsButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.settingsButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.settingsButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsButton.ForeColor = System.Drawing.Color.White;
            this.settingsButton.Highlight = false;
            this.settingsButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.settingsButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.settingsButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.settingsButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.settingsButton.Image = ((System.Drawing.Image)(resources.GetObject("settingsButton.Image")));
            this.settingsButton.IsGlass = true;
            this.settingsButton.Location = new System.Drawing.Point(46, 2);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.settingsButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.settingsButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.settingsButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.settingsButton.Size = new System.Drawing.Size(44, 44);
            this.settingsButton.TabIndex = 8;
            this.settingsButton.TabStop = false;
            this.settingsButton.UseStyles = true;
            this.settingsButton.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(226, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(8, 48);
            this.panel1.TabIndex = 32;
            // 
            // controlsMasterPanel
            // 
            this.controlsMasterPanel.BackColor = System.Drawing.Color.Transparent;
            this.controlsMasterPanel.Controls.Add(this.loadTerrariumButton);
            this.controlsMasterPanel.Controls.Add(this.newTerrariumButton);
            this.controlsMasterPanel.Controls.Add(this.joinEcosystemButton);
            this.controlsMasterPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.controlsMasterPanel.Location = new System.Drawing.Point(234, 0);
            this.controlsMasterPanel.Name = "controlsMasterPanel";
            this.controlsMasterPanel.Size = new System.Drawing.Size(135, 48);
            this.controlsMasterPanel.TabIndex = 31;
            // 
            // loadTerrariumButton
            // 
            this.loadTerrariumButton.BackColor = System.Drawing.Color.Transparent;
            this.loadTerrariumButton.BorderColor = System.Drawing.Color.Black;
            this.loadTerrariumButton.Depth = 4;
            this.loadTerrariumButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.loadTerrariumButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.loadTerrariumButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadTerrariumButton.ForeColor = System.Drawing.Color.White;
            this.loadTerrariumButton.Highlight = false;
            this.loadTerrariumButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.loadTerrariumButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.loadTerrariumButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.loadTerrariumButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.loadTerrariumButton.Image = ((System.Drawing.Image)(resources.GetObject("loadTerrariumButton.Image")));
            this.loadTerrariumButton.IsGlass = true;
            this.loadTerrariumButton.Location = new System.Drawing.Point(88, 2);
            this.loadTerrariumButton.Name = "loadTerrariumButton";
            this.loadTerrariumButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.loadTerrariumButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.loadTerrariumButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.loadTerrariumButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.loadTerrariumButton.Size = new System.Drawing.Size(44, 44);
            this.loadTerrariumButton.TabIndex = 20;
            this.loadTerrariumButton.TabStop = false;
            this.loadTerrariumButton.UseStyles = true;
            this.loadTerrariumButton.UseVisualStyleBackColor = false;
            // 
            // newTerrariumButton
            // 
            this.newTerrariumButton.BackColor = System.Drawing.Color.Transparent;
            this.newTerrariumButton.BorderColor = System.Drawing.Color.Black;
            this.newTerrariumButton.Depth = 4;
            this.newTerrariumButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.newTerrariumButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.newTerrariumButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newTerrariumButton.ForeColor = System.Drawing.Color.White;
            this.newTerrariumButton.Highlight = false;
            this.newTerrariumButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.newTerrariumButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.newTerrariumButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.newTerrariumButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.newTerrariumButton.Image = ((System.Drawing.Image)(resources.GetObject("newTerrariumButton.Image")));
            this.newTerrariumButton.IsGlass = true;
            this.newTerrariumButton.Location = new System.Drawing.Point(44, 2);
            this.newTerrariumButton.Name = "newTerrariumButton";
            this.newTerrariumButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.newTerrariumButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.newTerrariumButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.newTerrariumButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.newTerrariumButton.Size = new System.Drawing.Size(44, 44);
            this.newTerrariumButton.TabIndex = 19;
            this.newTerrariumButton.TabStop = false;
            this.newTerrariumButton.UseStyles = true;
            this.newTerrariumButton.UseVisualStyleBackColor = false;
            // 
            // joinEcosystemButton
            // 
            this.joinEcosystemButton.BackColor = System.Drawing.Color.Transparent;
            this.joinEcosystemButton.BorderColor = System.Drawing.Color.Black;
            this.joinEcosystemButton.Depth = 4;
            this.joinEcosystemButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.joinEcosystemButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.joinEcosystemButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.joinEcosystemButton.ForeColor = System.Drawing.Color.White;
            this.joinEcosystemButton.Highlight = false;
            this.joinEcosystemButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.joinEcosystemButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.joinEcosystemButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.joinEcosystemButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.joinEcosystemButton.Image = ((System.Drawing.Image)(resources.GetObject("joinEcosystemButton.Image")));
            this.joinEcosystemButton.IsGlass = true;
            this.joinEcosystemButton.Location = new System.Drawing.Point(0, 2);
            this.joinEcosystemButton.Name = "joinEcosystemButton";
            this.joinEcosystemButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.joinEcosystemButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.joinEcosystemButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.joinEcosystemButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.joinEcosystemButton.Size = new System.Drawing.Size(44, 44);
            this.joinEcosystemButton.TabIndex = 18;
            this.joinEcosystemButton.TabStop = false;
            this.joinEcosystemButton.UseStyles = true;
            this.joinEcosystemButton.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(369, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(8, 48);
            this.panel2.TabIndex = 30;
            // 
            // ecosystemControlsPanel
            // 
            this.ecosystemControlsPanel.BackColor = System.Drawing.Color.Transparent;
            this.ecosystemControlsPanel.Controls.Add(this.reintroduceAnimalButton);
            this.ecosystemControlsPanel.Controls.Add(this.introduceAnimalEcosystemButton);
            this.ecosystemControlsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ecosystemControlsPanel.Location = new System.Drawing.Point(377, 0);
            this.ecosystemControlsPanel.Name = "ecosystemControlsPanel";
            this.ecosystemControlsPanel.Size = new System.Drawing.Size(88, 48);
            this.ecosystemControlsPanel.TabIndex = 29;
            // 
            // reintroduceAnimalButton
            // 
            this.reintroduceAnimalButton.BackColor = System.Drawing.Color.Transparent;
            this.reintroduceAnimalButton.BorderColor = System.Drawing.Color.Black;
            this.reintroduceAnimalButton.Depth = 4;
            this.reintroduceAnimalButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.reintroduceAnimalButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.reintroduceAnimalButton.Enabled = false;
            this.reintroduceAnimalButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reintroduceAnimalButton.ForeColor = System.Drawing.Color.White;
            this.reintroduceAnimalButton.Highlight = false;
            this.reintroduceAnimalButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.reintroduceAnimalButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.reintroduceAnimalButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.reintroduceAnimalButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.reintroduceAnimalButton.Image = ((System.Drawing.Image)(resources.GetObject("reintroduceAnimalButton.Image")));
            this.reintroduceAnimalButton.IsGlass = true;
            this.reintroduceAnimalButton.Location = new System.Drawing.Point(44, 2);
            this.reintroduceAnimalButton.Name = "reintroduceAnimalButton";
            this.reintroduceAnimalButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.reintroduceAnimalButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.reintroduceAnimalButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.reintroduceAnimalButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.reintroduceAnimalButton.Size = new System.Drawing.Size(44, 44);
            this.reintroduceAnimalButton.TabIndex = 12;
            this.reintroduceAnimalButton.TabStop = false;
            this.reintroduceAnimalButton.UseStyles = true;
            this.reintroduceAnimalButton.UseVisualStyleBackColor = false;
            // 
            // introduceAnimalEcosystemButton
            // 
            this.introduceAnimalEcosystemButton.BackColor = System.Drawing.Color.Transparent;
            this.introduceAnimalEcosystemButton.BorderColor = System.Drawing.Color.Black;
            this.introduceAnimalEcosystemButton.Depth = 4;
            this.introduceAnimalEcosystemButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.introduceAnimalEcosystemButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.introduceAnimalEcosystemButton.Enabled = false;
            this.introduceAnimalEcosystemButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.introduceAnimalEcosystemButton.ForeColor = System.Drawing.Color.White;
            this.introduceAnimalEcosystemButton.Highlight = false;
            this.introduceAnimalEcosystemButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.introduceAnimalEcosystemButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.introduceAnimalEcosystemButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.introduceAnimalEcosystemButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.introduceAnimalEcosystemButton.Image = ((System.Drawing.Image)(resources.GetObject("introduceAnimalEcosystemButton.Image")));
            this.introduceAnimalEcosystemButton.IsGlass = true;
            this.introduceAnimalEcosystemButton.Location = new System.Drawing.Point(0, 2);
            this.introduceAnimalEcosystemButton.Name = "introduceAnimalEcosystemButton";
            this.introduceAnimalEcosystemButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.introduceAnimalEcosystemButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.introduceAnimalEcosystemButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.introduceAnimalEcosystemButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.introduceAnimalEcosystemButton.Size = new System.Drawing.Size(44, 44);
            this.introduceAnimalEcosystemButton.TabIndex = 11;
            this.introduceAnimalEcosystemButton.TabStop = false;
            this.introduceAnimalEcosystemButton.UseStyles = true;
            this.introduceAnimalEcosystemButton.UseVisualStyleBackColor = false;
            // 
            // terrariumControlsPanel
            // 
            this.terrariumControlsPanel.BackColor = System.Drawing.Color.Transparent;
            this.terrariumControlsPanel.Controls.Add(this.pauseButton);
            this.terrariumControlsPanel.Controls.Add(this.addAnimalButton);
            this.terrariumControlsPanel.Controls.Add(this.introduceAnimalTerrariumButton);
            this.terrariumControlsPanel.Controls.Add(this.addAnimalComboBox);
            this.terrariumControlsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.terrariumControlsPanel.Location = new System.Drawing.Point(465, 0);
            this.terrariumControlsPanel.Name = "terrariumControlsPanel";
            this.terrariumControlsPanel.Size = new System.Drawing.Size(237, 48);
            this.terrariumControlsPanel.TabIndex = 27;
            // 
            // pauseButton
            // 
            this.pauseButton.BackColor = System.Drawing.Color.Transparent;
            this.pauseButton.BorderColor = System.Drawing.Color.Black;
            this.pauseButton.Depth = 4;
            this.pauseButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pauseButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pauseButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pauseButton.ForeColor = System.Drawing.Color.White;
            this.pauseButton.Highlight = false;
            this.pauseButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.pauseButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pauseButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.pauseButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.pauseButton.Image = ((System.Drawing.Image)(resources.GetObject("pauseButton.Image")));
            this.pauseButton.IsGlass = true;
            this.pauseButton.Location = new System.Drawing.Point(190, 2);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pauseButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.pauseButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.pauseButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.pauseButton.Size = new System.Drawing.Size(44, 44);
            this.pauseButton.TabIndex = 22;
            this.pauseButton.TabStop = false;
            this.pauseButton.UseStyles = true;
            this.pauseButton.UseVisualStyleBackColor = false;
            // 
            // addAnimalButton
            // 
            this.addAnimalButton.BackColor = System.Drawing.Color.Transparent;
            this.addAnimalButton.BorderColor = System.Drawing.Color.Black;
            this.addAnimalButton.Depth = 4;
            this.addAnimalButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.addAnimalButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.addAnimalButton.Enabled = false;
            this.addAnimalButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addAnimalButton.ForeColor = System.Drawing.Color.White;
            this.addAnimalButton.Highlight = false;
            this.addAnimalButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.addAnimalButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.addAnimalButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.addAnimalButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.addAnimalButton.Image = ((System.Drawing.Image)(resources.GetObject("addAnimalButton.Image")));
            this.addAnimalButton.IsGlass = true;
            this.addAnimalButton.Location = new System.Drawing.Point(146, 2);
            this.addAnimalButton.Name = "addAnimalButton";
            this.addAnimalButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.addAnimalButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.addAnimalButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.addAnimalButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.addAnimalButton.Size = new System.Drawing.Size(44, 44);
            this.addAnimalButton.TabIndex = 18;
            this.addAnimalButton.TabStop = false;
            this.addAnimalButton.UseStyles = true;
            this.addAnimalButton.UseVisualStyleBackColor = false;
            // 
            // introduceAnimalTerrariumButton
            // 
            this.introduceAnimalTerrariumButton.BackColor = System.Drawing.Color.Transparent;
            this.introduceAnimalTerrariumButton.BorderColor = System.Drawing.Color.Black;
            this.introduceAnimalTerrariumButton.Depth = 4;
            this.introduceAnimalTerrariumButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.introduceAnimalTerrariumButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.introduceAnimalTerrariumButton.Enabled = false;
            this.introduceAnimalTerrariumButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.introduceAnimalTerrariumButton.ForeColor = System.Drawing.Color.White;
            this.introduceAnimalTerrariumButton.Highlight = false;
            this.introduceAnimalTerrariumButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.introduceAnimalTerrariumButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.introduceAnimalTerrariumButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.introduceAnimalTerrariumButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.introduceAnimalTerrariumButton.Image = ((System.Drawing.Image)(resources.GetObject("introduceAnimalTerrariumButton.Image")));
            this.introduceAnimalTerrariumButton.IsGlass = true;
            this.introduceAnimalTerrariumButton.Location = new System.Drawing.Point(0, 2);
            this.introduceAnimalTerrariumButton.Name = "introduceAnimalTerrariumButton";
            this.introduceAnimalTerrariumButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.introduceAnimalTerrariumButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.introduceAnimalTerrariumButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.introduceAnimalTerrariumButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.introduceAnimalTerrariumButton.Size = new System.Drawing.Size(44, 44);
            this.introduceAnimalTerrariumButton.TabIndex = 17;
            this.introduceAnimalTerrariumButton.TabStop = false;
            this.introduceAnimalTerrariumButton.UseStyles = true;
            this.introduceAnimalTerrariumButton.UseVisualStyleBackColor = false;
            // 
            // addAnimalComboBox
            // 
            this.addAnimalComboBox.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addAnimalComboBox.FormattingEnabled = true;
            this.addAnimalComboBox.Location = new System.Drawing.Point(47, 14);
            this.addAnimalComboBox.Name = "addAnimalComboBox";
            this.addAnimalComboBox.Size = new System.Drawing.Size(96, 20);
            this.addAnimalComboBox.TabIndex = 16;
            // 
            // controlsToolTip
            // 
            this.controlsToolTip.AutoPopDelay = 5000;
            this.controlsToolTip.InitialDelay = 1000;
            this.controlsToolTip.IsBalloon = true;
            this.controlsToolTip.ReshowDelay = 100;
            // 
            // tickerBar
            // 
            this.tickerBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tickerBar.Location = new System.Drawing.Point(0, 0);
            this.tickerBar.MessageLife = 2500;
            this.tickerBar.Name = "tickerBar";
            this.tickerBar.Size = new System.Drawing.Size(286, 48);
            this.tickerBar.TabIndex = 6;
            // 
            // developerPanelButton
            // 
            this.developerPanelButton.BackColor = System.Drawing.Color.Transparent;
            this.developerPanelButton.BorderColor = System.Drawing.Color.Black;
            this.developerPanelButton.Depth = 4;
            this.developerPanelButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.developerPanelButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.developerPanelButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.developerPanelButton.ForeColor = System.Drawing.Color.White;
            this.developerPanelButton.Highlight = false;
            this.developerPanelButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.developerPanelButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.developerPanelButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.developerPanelButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.developerPanelButton.Image = ((System.Drawing.Image)(resources.GetObject("developerPanelButton.Image")));
            this.developerPanelButton.IsGlass = true;
            this.developerPanelButton.Location = new System.Drawing.Point(2, 2);
            this.developerPanelButton.Name = "developerPanelButton";
            this.developerPanelButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.developerPanelButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.developerPanelButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.developerPanelButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.developerPanelButton.Size = new System.Drawing.Size(44, 44);
            this.developerPanelButton.TabIndex = 12;
            this.developerPanelButton.TabStop = false;
            this.developerPanelButton.UseStyles = true;
            this.developerPanelButton.UseVisualStyleBackColor = false;
            // 
            // GlassBottomPanel
            // 
            this.Controls.Add(this.tickerBar);
            this.Controls.Add(this.GlassPanel1);
            this.Name = "GlassBottomPanel";
            this.Size = new System.Drawing.Size(988, 48);
            this.GlassPanel1.ResumeLayout(false);
            this.buttonsPanel.ResumeLayout(false);
            this.controlsMasterPanel.ResumeLayout(false);
            this.ecosystemControlsPanel.ResumeLayout(false);
            this.terrariumControlsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
	}
}
