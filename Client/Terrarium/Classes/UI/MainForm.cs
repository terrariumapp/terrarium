//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------


using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Data;

using Microsoft.Win32;

using Terrarium.Client.SplashScreen;
using Terrarium.Configuration;
using Terrarium.Forms;
using Terrarium.Game;
using Terrarium.Hosting;
using Terrarium.Glass;
using Terrarium.PeerToPeer;
using Terrarium.Renderer;
//using Terrarium.Renderer.DirectX;
using DxVBLib;
using Terrarium.Tools;
using Terrarium.Services.Species;

using OrganismBase;


namespace Terrarium.Client
{
	/// <summary>
	///  <para>
	///   The Primary Terrarium application UI.  This class is responsible for
	///   providing the link between user input and the various UI controls and
	///   primary game engine.  This class also launches the GameEngine dependent
	///   on the load method and user input.
	///  </para>
	///  <para>
	///   The MainForm can be launched with a bunch of extra properties that control
	///   how the application initializes in the beginning.  It is also capable of
	///   processing most Screen Saver related command line parameters.
	///  </para>
	/// </summary>
	internal class MainForm : System.Windows.Forms.Form
	{
		// Need to have a variable to hold the form instance or it will be
		// garbage collected and go away.  It is private since nothing should be
		// referencing the main form
		private static MainForm mainForm;

		//get the screen size and calculate the viewport location
		private Size screenSize;
		private Rectangle virtualScreen;
		private Rectangle viewPort;
		private Rectangle controlStripTop;
		private Rectangle controlStripBottom;
		private Rectangle screenRectangle;

		private static TerrariumTraceListener traceListener;

		private const string ecosystemStateFileName = "\\Ecosystem.bin";

		private static string[] commandLineArgs;

		private bool alreadyRunningTimer = false;
		private const Boolean traceEnabled = true;
		private bool runningBlockedVersion = false;

		// Mutex to detect multiple versions of the game
        private static string appMutexName = "{2AED158D-74C9-4297-B83B-13B87FCA6BFC}";
        private static Mutex appMutex;

		private Random random = new Random(Environment.TickCount);

		private WorldVector oldVector;      // current state - 1
		private WorldVector newVector;      // current state

		private int frameNumber = 0;

		// Command line arguments / Screensaver support
		private Boolean startAtStartup;
		private ScreenSaverMode screenSaverMode = ScreenSaverMode.NoScreenSaver;
		private Int32 hwndScreenSaverParent = 0;
		private int turnOffDirectXCounter = 0;
		private Boolean turnOffDirectX = false;
		private Boolean noDirectX = false;
		private string gamePath = null;
		private bool firstActivate;

		// Window Resizing and Fullscreen support
		private bool fullScreen = false;
		private bool showUI = true;
		private Point originalLocation;
		private Size originalSize;

		private static bool maliciousOrganism = false;
		private static bool relaunch = false;
		private static bool performBlacklistCheck = false;
		private static bool blacklistCheckOnRestart = false;

        // New command line option holders
        private static bool wasRelaunched = false;
        private static bool skipSplashScreen = false;
        private static Rectangle windowRectangle;
        private static FormWindowState windowState = FormWindowState.Normal;

        // Screen messages
		private const string emptyEcosystemMessage = "Waiting for animals to be teleported from other peers running Terrarium...";
		private const string emptyEcosystemServerDownMessage = "The Terrarium server is experiencing temporary difficulties.  This is probably why you aren't receiving any animals.";
		private const string emptyTerrariumMessage = "Introduce animals into your terrarium by clicking on the 'Introduce Animal' button below.";

        private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.NotifyIcon taskBar;

		private System.Windows.Forms.Timer screenSaverTimer = null;

        // Used to force a save when restarting.  Used by auto-updates
        private bool forceSave = false;

		#region Designer Generated Fields
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Timer timer1;
		private OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItemText;
		private System.Windows.Forms.MenuItem menuItemDamage;
		private System.Windows.Forms.MenuItem menuItemEnergy;
		private System.Windows.Forms.MenuItem menuItemAbout;
        private System.Windows.Forms.MenuItem menuItemClearBadPeers;
		#endregion
		#region Engine Timing Fields
		Int64 totalPaintFrameTime = 0;
		TimeMonitor frameRateTimer = new TimeMonitor();

		Int64 totalProcessTime = 0;
		double[] processTimes = new double[10];

		Int64 totalPaintTime = 0;
		double[] paintTimes = new double[10];

		TimeMonitor outsideTimer = new TimeMonitor();
		double totalOutsideTime = 0;
		double[] outsideTimes = new double[10];
		#endregion
		#region Secondary Window support (Trace, Reporting, Property Sheet)
		private PropertySheet propertySheet;
		private TraceWindow traceWindow;
        private string engineStateText;        
        private TerrariumDirectDrawGameView tddGameView;
        private Terrarium.Forms.Classes.Controls.DeveloperPanel developerPanel;
        private GlassBottomPanel bottomPanel;
        private ResizeBar resizeBar;
        private GlassTitleBar titleBar;
        private Panel bottomContainerPanel;
		private ReportStats reportWindow;
		#endregion
		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItemClearBadPeers = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.menuItemText = new System.Windows.Forms.MenuItem();
            this.menuItemDamage = new System.Windows.Forms.MenuItem();
            this.menuItemEnergy = new System.Windows.Forms.MenuItem();
            this.tddGameView = new Terrarium.Renderer.TerrariumDirectDrawGameView();
            this.developerPanel = new Terrarium.Forms.Classes.Controls.DeveloperPanel();
            this.bottomPanel = new Terrarium.Forms.GlassBottomPanel();
            this.resizeBar = new Terrarium.Forms.ResizeBar();
            this.titleBar = new Terrarium.Forms.GlassTitleBar();
            this.bottomContainerPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.tddGameView)).BeginInit();
            this.bottomContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 40;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "dll";
            this.openFileDialog1.Filter = ".NET Terrarium Assemblies (*.dll)|*.dll|All Files (*.*)|*.*";
            this.openFileDialog1.Title = "Choose the Assembly where your animal is located";
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemClearBadPeers,
            this.menuItem1,
            this.menuItemAbout});
            // 
            // menuItemClearBadPeers
            // 
            this.menuItemClearBadPeers.Index = 0;
            this.menuItemClearBadPeers.Text = "Reset Bad Peer List";
            this.menuItemClearBadPeers.Click += new System.EventHandler(this.menuItemClearBadPeers_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "-";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 2;
            this.menuItemAbout.Text = "About Terrarium...";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // menuItemText
            // 
            this.menuItemText.Index = -1;
            this.menuItemText.Text = "";
            // 
            // menuItemDamage
            // 
            this.menuItemDamage.Index = -1;
            this.menuItemDamage.Text = "";
            // 
            // menuItemEnergy
            // 
            this.menuItemEnergy.Index = -1;
            this.menuItemEnergy.Text = "";
            // 
            // tddGameView
            // 
            this.tddGameView.BackColor = System.Drawing.Color.Black;
            this.tddGameView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tddGameView.DrawBackgroundGrid = false;
            this.tddGameView.DrawBoundingBox = false;
            this.tddGameView.DrawCursor = true;
            this.tddGameView.DrawDestinationLines = false;
            this.tddGameView.DrawScreen = true;
            this.tddGameView.DrawText = true;
            this.tddGameView.Location = new System.Drawing.Point(200, 32);
            this.tddGameView.Name = "tddGameView";
            this.tddGameView.Size = new System.Drawing.Size(619, 453);
            this.tddGameView.TabIndex = 21;
            this.tddGameView.TabStop = false;
            this.tddGameView.TerrariumMessage = null;
            this.tddGameView.OrganismClicked += new Terrarium.Renderer.OrganismClickedEventHandler(this.Organism_Clicked);
            // 
            // developerPanel
            // 
            this.developerPanel.AnimalCount = 0;
            this.developerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.developerPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.developerPanel.FailedReceives = 0;
            this.developerPanel.FailedSends = 0;
            this.developerPanel.GameModeText = "XXXXX";
            this.developerPanel.LandSize = new System.Drawing.Size(1600, 1600);
            this.developerPanel.Location = new System.Drawing.Point(0, 32);
            this.developerPanel.MapLocation = new System.Drawing.Point(0, 0);
            this.developerPanel.MaximumAnimalCount = 0;
            this.developerPanel.Name = "developerPanel";
            this.developerPanel.PeerCount = 0;
            this.developerPanel.PlantCount = 0;
            this.developerPanel.Size = new System.Drawing.Size(200, 453);
            this.developerPanel.TabIndex = 20;
            this.developerPanel.Teleportations = 0;
            this.developerPanel.WebRoot = "XXXXX";
            // 
            // bottomPanel
            // 
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomPanel.GameMode = Terrarium.Forms.GameModes.Ecosystem;
            this.bottomPanel.Location = new System.Drawing.Point(0, 0);
            this.bottomPanel.Mode = Terrarium.Forms.ScreenSaverMode.ShowSettingsModeless;
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(799, 48);
            this.bottomPanel.TabIndex = 19;

            // 
            // resizeBar
            // 
            this.resizeBar.BackColor = System.Drawing.Color.Transparent;
            this.resizeBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.resizeBar.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resizeBar.ForeColor = System.Drawing.Color.White;
            this.resizeBar.Location = new System.Drawing.Point(799, 0);
            this.resizeBar.Name = "resizeBar";
            this.resizeBar.Size = new System.Drawing.Size(20, 48);
            this.resizeBar.TabIndex = 18;
            // 
            // titleBar
            // 
            this.titleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleBar.ForeColor = System.Drawing.Color.White;
            this.titleBar.Image = ((System.Drawing.Image)(resources.GetObject("titleBar.Image")));
            this.titleBar.Location = new System.Drawing.Point(0, 0);
            this.titleBar.Name = "titleBar";
            this.titleBar.Size = new System.Drawing.Size(819, 32);
            this.titleBar.TabIndex = 3;
            this.titleBar.Title = "Terrarium";
            this.titleBar.DoubleClick += new System.EventHandler(this.titleBar_DoubleClick);
            this.titleBar.CloseClicked += new System.EventHandler(this.Close_Click);
            this.titleBar.MinimizeClicked += new System.EventHandler(this.titleBar_MinimizeClicked);
            this.titleBar.MaximizeClicked += new System.EventHandler(this.titleBar_MaximizeClicked);

            // 
            // bottomContainerPanel
            // 
            this.bottomContainerPanel.Controls.Add(this.bottomPanel);
            this.bottomContainerPanel.Controls.Add(this.resizeBar);
            this.bottomContainerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomContainerPanel.Location = new System.Drawing.Point(0, 485);
            this.bottomContainerPanel.Name = "bottomContainerPanel";
            this.bottomContainerPanel.Size = new System.Drawing.Size(819, 48);
            this.bottomContainerPanel.TabIndex = 22;
            // 
            // MainForm
            // 
            this.BackColor = System.Drawing.Color.Magenta;
            this.ClientSize = new System.Drawing.Size(819, 533);
            this.ContextMenu = this.contextMenu1;
            this.Controls.Add(this.tddGameView);
            this.Controls.Add(this.developerPanel);
            this.Controls.Add(this.titleBar);
            this.Controls.Add(this.bottomContainerPanel);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(320, 240);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Terrarium";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form_Closing);
            this.SystemColorsChanged += new System.EventHandler(this.MainForm_SystemColorsChanged);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tddGameView)).EndInit();
            this.bottomContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		// Main entry point for the game
		[STAThread()]
		internal static int Main(string[] args)
		{
#if DEBUG_CLICKONCE
			MessageBox.Show("Click OK to continue", "Click-Once Debugging", MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

			// Turn on XP Themes...
			Application.EnableVisualStyles();
			Application.DoEvents();

            Trace.WriteLine("");
            Trace.WriteLine("*****");
            Trace.WriteLine("Environment.CommandLine: " + Environment.CommandLine);
            Trace.WriteLine("GameConfig.RelaunchCommandLine: " + GameConfig.RelaunchCommandLine);
            Trace.WriteLine("MainForm.SpecialUserAppDataPath: " + MainForm.SpecialUserAppDataPath);
            Trace.WriteLine("*****");
            Trace.WriteLine("");


            // Let's see if we've been relaunched.  If so, these command line args trump the default one.
			if (GameConfig.RelaunchCommandLine != null && GameConfig.RelaunchCommandLine.Length > 0)
			{
                wasRelaunched = true;
				args = GameConfig.RelaunchCommandLine.Split('|');
			}

			commandLineArgs = args;

			// Parse the command line arguments
			ScreenSaverMode mode = ScreenSaverMode.NoScreenSaver;
			Int32 hwnd = 0;
			Boolean start = true;
			string gamePath = "";
			Boolean enableLogging = false;
			Boolean noDirectX = false;

			try
			{
				if (args.Length > 0)
				{
                    string commandName = "";

                    for( int i = 0; i < args.Length; ++i)
                    {
                        if (args[i].Length == 0)
                            continue;

                        if (args[i][0] == '/' || args[i][0] == '-')
                        {
                            commandName = args[i].Substring(1).ToLower(CultureInfo.InvariantCulture);;

                            switch(commandName)
                            {
                                case "c":
                                    // Show the Settings dialog box, modal to the foreground window
                                    mode = ScreenSaverMode.ShowSettingsModal;
                                    break;
                                case "p":
                                    // Preview Screen Saver as child of window <HWND>
                                    mode = ScreenSaverMode.Preview;
                                    i++;
                                    hwnd = Convert.ToInt32(args[i]);
                                    break;
                                case "s":
                                    mode = ScreenSaverMode.Run;
                                    break;
                                case "nostart":
                                    start = false;
                                    break;
                                case "nodirectx":
                                    noDirectX = true;
                                    break;
                                case "loadterrarium":
                                    mode = ScreenSaverMode.RunLoadTerrarium;
                                    i++;
                                    gamePath = args[i];
                                    break;
                                case "newterrarium":
                                    mode = ScreenSaverMode.RunNewTerrarium;
                                    i++;
                                    gamePath = args[i];
                                    break;
                                case "enablelogging":
                                    enableLogging = true;
                                    break;
                                case "blacklistcheck":
                                    performBlacklistCheck = true;
                                    break;
                                case "skipsplashScreen":
                                    skipSplashScreen = true;
                                    break;
                                case "windowrectangle":
                                    i++;
                                    string[] rectangleValues = args[i].Split(',');
                                    if (rectangleValues.Length != 4)
                                        continue;
                                    windowRectangle = new Rectangle(Convert.ToInt32(rectangleValues[0]), Convert.ToInt32(rectangleValues[1]), Convert.ToInt32(rectangleValues[2]), Convert.ToInt32(rectangleValues[3]));
                                    break;
                                case "windowstate":
                                    i++;
                                    windowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), args[i]);
                                    break;
                            }
                        }
                    }
				}
			}
			catch (Exception e)
			{
				ErrorLog.LogHandledException(e);
				MessageBox.Show("Error reading command line arguments: " + e.ToString(), "CommandLine Error");
                GameConfig.RelaunchCommandLine = "";
                return 0;
			}

			// Clear out any relaunch commands
			GameConfig.RelaunchCommandLine = "";

			// Catch toplevel exceptions and assertions so we can stop the timer
			// and report them properly
			Debug.Listeners.Clear();
			traceListener = new TerrariumTraceListener(enableLogging);
			Debug.Listeners.Add(traceListener);
			
			// Create/Open a global named mutex that indicates if the app is already running
			// if we can get the mutex, then we are the only version running
            appMutex = new System.Threading.Mutex(false, appMutexName);
			if (!appMutex.WaitOne(new TimeSpan(1), false))
			{
                //// Don't show a messagebox because it locks the exe and dll's and prevents the
                //// app from being upgraded
                //if (mode == ScreenSaverMode.Run)
                //{
                //    Application.Run(new SimpleScreenSaver());
                //}
                appMutex.Close();
				return 0;
			}

			// Make sure we can access our config file
			try
			{
				GameConfig.CheckConfigFile();
			}
			catch (Exception e)
			{
				ErrorLog.LogHandledException(e);
				MessageBox.Show("Error loading '" + GameConfig.UserSettingsLocation + "': " + e.Message + "\r\n\r\nPlease correct the problem and restart Terrarium.");
				return 0;
			}

			// early check for an enabled security system
			// if it's not enabled, bail out without running code
			if (!SecurityUtils.SecurityEnabled)
			{
				MessageBox.Show("Terrarium will not run if security is disabled.\nPlease use \"caspol -s on\" to enable",
					"Terrarium Security Check");
				return 0;
			}

			// check for globally disabled strong name verification or selective terrarium verification
			if (SecurityUtils.VerificationDisabled)
			{
				MessageBox.Show("Terrarium will not run if strong name verification is disabled.\nPlease use \"sn.exe -Vx\" to enable it",
					"Terrarium Security Check");
				return 0;
			}

			// Disable directX if the user is running in too low of a resolution
			if (Screen.PrimaryScreen.Bounds.Width < 800 ||
				Screen.PrimaryScreen.Bounds.Height < 600)
			{
				noDirectX = true;
			}

			FileInfo info = new FileInfo(Assembly.GetEntryAssembly().Location);
			if (info.Extension == ".scr" && mode == ScreenSaverMode.NoScreenSaver)
			{
				mode = ScreenSaverMode.ShowSettingsModeless;
			}

			// Don't run if we're being asked to preview or change settings
			if (mode == ScreenSaverMode.Run || mode == ScreenSaverMode.NoScreenSaver ||
				mode == ScreenSaverMode.RunLoadTerrarium || mode == ScreenSaverMode.RunNewTerrarium)
			{
                if (skipSplashScreen == false)
                {
#if !DEVELOPER
            
				    SplashWindow.Current.Image = new Bitmap( typeof(MainForm), "splashscreen.png" );
				    SplashWindow.Current.ShowShadow = true;
				    SplashWindow.Current.MinimumDuration = 3000;
				    SplashWindow.Current.Show();
#endif
                }
                
                mainForm = new MainForm(start, mode, gamePath, hwnd, noDirectX);

				System.Windows.Forms.Application.Run(mainForm);
			}

			// When we get to here, the app is shutting down because the Application.Run command has returned
			appMutex.ReleaseMutex();
			appMutex.Close();

			if (relaunch)
			{
				Debug.WriteLine("Relaunching...");

				// We separate commandline arguments with "|" since this is an invalid character for a file name
				string configLine = "";
				switch (mainForm.screenSaverMode)
				{
					case ScreenSaverMode.RunLoadTerrarium:
						configLine += "|/loadterrarium|" + mainForm.gamePath;
						break;

					case ScreenSaverMode.RunNewTerrarium:
						configLine += "|/newterrarium|" + mainForm.gamePath;
						break;

					case ScreenSaverMode.Run:
                        configLine += "|/s";
						break;
				}

                if (blacklistCheckOnRestart)
                {
                    configLine += "|/blacklistcheck" + configLine;
                }

                // Let's not display the splash screen on a restart
                configLine += "|/skipSplashScreen";

                // Add the current window size, position, and state
                configLine += "|/windowRectangle|" + mainForm.Location.X.ToString() + "," + mainForm.Location.X.ToString() + "," + mainForm.Size.Width.ToString() + "," + mainForm.Size.Height.ToString();

                if (mainForm.Visible == false && mainForm.taskBar.Visible == true)
                {
                    windowState = FormWindowState.Minimized;
                }
                else
                {
                    windowState = mainForm.WindowState;
                }

                configLine += "|windowState|" + windowState.ToString();

                GameConfig.RelaunchCommandLine = configLine;
                Application.Restart();
			}

			Debug.Listeners.Clear();
			if (maliciousOrganism)
			{
				Environment.Exit(0);
			}
			return 0;
		}

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name == "Game, Version=2.0.50418.4, Culture=neutral, PublicKeyToken=be981b9cca9a8595")
            {
                return typeof(PrivateAssemblyCache).Assembly;
            }
            else
            {
                return Assembly.Load(args.Name);
            }
        }

		internal MainForm()
			: this(false, ScreenSaverMode.NoScreenSaver, null, 0, true)
		{
		}

		internal MainForm(Boolean start, ScreenSaverMode mode, string gamePath, Int32 hwndParent, Boolean noDirectX)
		{
			firstActivate = true;
			screenSaverMode = mode;
			this.gamePath = gamePath;
			hwndScreenSaverParent = hwndParent;
			startAtStartup = start;
			this.noDirectX = noDirectX;

			// Check to see if this version has been blocked by the server (because of a security issue)
			// if it has, then don't actually start the game and just wait for
			// an updated version to get downloaded.
			if (GameConfig.BlockedVersion.Length != 0)
			{
				Version v = null;
				try
				{
					v = new Version(GameConfig.BlockedVersion);
				}
				catch (Exception e)
				{
					ErrorLog.LogHandledException(e);
					GameConfig.BlockedVersion = "";
				}

				if (v != null && v >= Assembly.GetExecutingAssembly().GetName().Version)
				{
					// They are running a blocked version
					runningBlockedVersion = true;
					startAtStartup = false;
				}
			}

			// Load the Glass Style
			GlassStyleManager.Refresh();
			GlassStyleManager.SetStyle(GameConfig.StyleName);


			InitializeComponent();

			if (this.screenSaverMode == ScreenSaverMode.Run)
			{
				this.ShowInTaskbar = false;
			}

			InitializeScreen();

            this.titleBar.ShowBugButton = false;

			// Post set-up of variables that aren't designed

            // Reset the Resize bar so it can detect the MainForm as the root parent
            this.bottomContainerPanel.Controls.Remove(this.resizeBar);
            this.bottomContainerPanel.Controls.Add(this.resizeBar);

            this.BackColor = GlassStyleManager.Active.DialogColor;

			this.developerPanel.WebRoot = GameConfig.WebRoot;

			this.developerPanel.ViewPortSize = this.tddGameView.Size;
			
			this.bottomPanel.Location = new Point(controlStripBottom.Left, controlStripBottom.Top);
			this.bottomPanel.AddAnimalButton.Click += new EventHandler(this.AddNewAnimal_Click);
			this.bottomPanel.AddAnimalComboBox.DropDown += new EventHandler(this.AddAnimalComboBox_DropDown);
			this.bottomPanel.PauseButton.Click += new EventHandler(this.Pause_Click);
			this.bottomPanel.IntroduceAnimalEcosystemButton.Click += new EventHandler(this.introduceAnimalEcosystem_Click);
			this.bottomPanel.IntroduceAnimalTerrariumButton.Click += new EventHandler(this.introduceAnimalTerrarium_Click);
			this.bottomPanel.ReintroduceAnimalButton.Click += new EventHandler(this.reintroduceAnimal_Click);
			this.bottomPanel.NewTerrariumButton.Click += new EventHandler(this.newTerrarium_Click);
			this.bottomPanel.LoadTerrariumButton.Click += new EventHandler(this.loadTerrarium_Click);
			this.developerPanel.NavigatePictureBox.MouseDown += new MouseEventHandler(this.navigatePictureBox_MouseDown);
			this.bottomPanel.JoinEcosystemButton.Click += new EventHandler(loadEcosystem_Click);

			this.bottomPanel.SettingsButton.Click += new EventHandler(SettingsButton_Click);
			this.bottomPanel.DetailsButton.Click += new EventHandler(DetailsButton_Click);
			this.bottomPanel.StatisticsButton.Click += new EventHandler(StatisticsButton_Click);
			this.bottomPanel.TraceButton.Click += new EventHandler(TraceButton_Click);
            this.bottomPanel.DeveloperButton.Click += new EventHandler(DeveloperButton_Click);
			traceListener.TimerToStop = this.timer1;
			SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(PowerModeChanged);

			this.taskBar = new NotifyIcon();
			this.taskBar.DoubleClick += new EventHandler(taskBar_DoubleClick);
			MenuItem showMenu = new MenuItem("Show", new EventHandler(this.taskBar_DoubleClick));
			showMenu.DefaultItem = true;
			MenuItem settingsMenu = new MenuItem("Settings...", new EventHandler(this.SettingsButton_Click));
			MenuItem exitMenu = new MenuItem("Exit", new EventHandler(this.Close_Click));
			this.taskBar.ContextMenu = new ContextMenu(new MenuItem[] { showMenu, settingsMenu, new MenuItem("-"), exitMenu });

			this.taskBar.Icon = this.Icon;
			this.taskBar.Visible = false;
			this.taskBar.Text = "Terrarium";

			// Set up the game view
			if (!this.DesignMode)
			{
				if (!tddGameView.InitializeDirectDraw(false))
				{
					noDirectX = true;
				}
				else
				{
					try
					{
						tddGameView.DrawBackgroundGrid = GameConfig.BackgroundGrid;
						tddGameView.AddBackgroundSlide();
						tddGameView.AddComplexSpriteSurface("cursor", 1, 9);
						tddGameView.AddComplexSpriteSurface("teleporter", 16, 1);
						tddGameView.AddComplexSizedSpriteSurface("plant", 1, 1);
					}
					catch { }
				}

			}

			if (screenSaverMode == ScreenSaverMode.Run)
			{
				// Start the scrolling timer
				this.screenSaverTimer = new System.Windows.Forms.Timer();
				this.screenSaverTimer.Interval = 30000;
				this.screenSaverTimer.Tick += new EventHandler(screenSaverTimer_Tick);
				this.screenSaverTimer.Start();
			}
		}

        void DeveloperButton_Click(object sender, EventArgs e)
        {
            this.developerPanel.Visible = !this.developerPanel.Visible;
            this.OnSizeChanged(new EventArgs());
        }

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				// The Network runs on another thread that doesn't die during Application.Exit()
				// which only shuts down the UI thread.  If we shut-down the network explicitly
				// during form disposal then the application exits properly and doesn't leave an
				// instance in memory.
				try
				{
					if (GameEngine.Current != null)
					{
						GameEngine.Current.StopGame(false);
					}
				}
				catch
				{
				}
			}
		}

		private void InitializeScreen()
		{
			if (screenSaverMode == ScreenSaverMode.Run)
			{
				screenSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
				this.ClientSize = screenSize;
				this.Location = new Point(0, 0);
			}
			else
			{
				screenSize = new Size(800, 600);
				this.ClientSize = screenSize;
				this.Text = ".NET Terrarium";
				this.StartPosition = FormStartPosition.CenterScreen;
			}

			virtualScreen = new Rectangle((screenSize.Width - EngineSettings.MonitorModeWidth) / 2,
				(screenSize.Height - EngineSettings.MonitorModeHeight) / 2,
				EngineSettings.MonitorModeWidth,
				EngineSettings.MonitorModeHeight);

			viewPort = new Rectangle(this.tddGameView.Left,
				this.tddGameView.Right,
				this.tddGameView.Width,
				this.tddGameView.Height);

			controlStripTop = new Rectangle(virtualScreen.X,
				virtualScreen.Y,
				virtualScreen.Width,
				50);

			controlStripBottom = new Rectangle(virtualScreen.X,
				virtualScreen.Y + controlStripTop.Height + viewPort.Height,
				virtualScreen.Width,
				100);

			screenRectangle = Screen.FromControl(this).Bounds;
		}

		private void InitializeGraphics(int xPixels, int yPixels)
		{
			if (noDirectX)
			{
				return;
			}

			RECT worldSize = tddGameView.CreateWorld(xPixels, yPixels);
			this.developerPanel.LandSize = new Size(tddGameView.ActualSize.Right, tddGameView.ActualSize.Bottom);
			this.developerPanel.MiniMap = tddGameView.MiniMap;

			this.developerPanel.GenerateMiniMap(worldSize.Right - worldSize.Left, worldSize.Bottom - worldSize.Top);
		}

		// Try to load a game
		private void LoadGame()
		{
			try
			{
				if (screenSaverMode != ScreenSaverMode.Run)
				{
					if (GameConfig.WebRoot.Length == 0)
					{
						ShowPropertiesDialog("Server");
					}

                    //if (GameConfig.UserEmail.Length == 0)
                    //{
                    //    ShowPropertiesDialog("Registration");
                    //}
				}

				this.Cursor = Cursors.WaitCursor;

				if (performBlacklistCheck)
				{
					// We have been launched and asked to check for blacklisted species before
					// we start up.  Call the server and ask.
					SpeciesService service = new SpeciesService();
					service.Url = GameConfig.WebRoot + "/Species/AddSpecies.asmx";
					service.Timeout = 60000;
					string[] blacklist = null;
					try
					{
						blacklist = service.GetBlacklistedSpecies();

						if (blacklist != null)
						{
							PrivateAssemblyCache pac = new PrivateAssemblyCache(SpecialUserAppDataPath, ecosystemStateFileName, false, false);
							pac.BlacklistAssemblies(blacklist);
						}
					}
					catch (Exception e)
					{
						Debug.WriteLine("Problem blacklisting animals on startup");
						ErrorLog.LogHandledException(e);
					}

					engineStateText += "Terrarium had to sanitize your ecosystem because it contained some bad animals.\r\n";
				}

				switch (screenSaverMode)
				{
					case ScreenSaverMode.NoScreenSaver: // run ecosystem
						GameEngine.LoadEcosystemGame(SpecialUserAppDataPath, ecosystemStateFileName, developerPanel.Leds);
						break;
					case ScreenSaverMode.Run:  // run the screensaver
						GameEngine.LoadEcosystemGame(SpecialUserAppDataPath, ecosystemStateFileName, developerPanel.Leds);
						break;
                    case ScreenSaverMode.RunLoadTerrarium:
                        GameEngine.LoadTerrariumGame(Path.GetDirectoryName(gamePath), gamePath, developerPanel.Leds);
                        break;
                    case ScreenSaverMode.RunNewTerrarium:
                        GameEngine.LoadTerrariumGame(Path.GetDirectoryName(gamePath), gamePath, developerPanel.Leds);
                        break;
                }
			}
			catch (SocketException e)
			{
				ErrorLog.LogHandledException(e);
				MessageBox.Show("A version of the Terrarium is already running.  If you are connected via Terminal Server you can shut down any instances of Terrarium by using the Task Manager.");
				Application.Exit();
			}
			catch (Exception e)
			{
				if (screenSaverMode == ScreenSaverMode.NoScreenSaver || screenSaverMode == ScreenSaverMode.Run)
				{
					// Only assert in ecosystem mode because it should never fail
					Debug.Assert(false, "Problem loading ecosystem: " + e.ToString());
				}

				MessageBox.Show(this, "Can't load game: " + e.Message, "Error Loading Game", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				CloseTerrarium(false);
				return;
			}

			NewGameLoaded();
		}

		// This is the timer that runs the game
		private void timer1_Tick(object sender, System.EventArgs e)
		{
			// Things like messageboxes and such will allow the timer to run while there is already one running
			// this causes all kinds of issues
			if (alreadyRunningTimer)
			{
				Debug.WriteLine("Timer called reentrantly...");
				return;
			}

			try
			{
				alreadyRunningTimer = true;

				DoTick();
			}
			finally
			{
				alreadyRunningTimer = false;
			}
		}

		// NEVER pop up UI in this function, or in any function that it calls.  If you do, and the
		// function pumps messages, we could shut the game down without DoTick expecting it.
		private void DoTick()
		{
			if (GameEngine.Current == null)
			{
				return;
			}

			double outsideTime = 0;
			if (outsideTimer.IsStarted)
			{
				outsideTime = (double)outsideTimer.EndGetMicroseconds() / (double)1000;
			}

			totalOutsideTime += outsideTime;
			if (frameNumber == 0)
			{
				outsideTimes[9] = outsideTime;
				ReportTimes();
			}
			else
			{
				outsideTimes[frameNumber - 1] = outsideTime;
			}

			// Shutdown if our appdomain is getting too big
			if (GameEngine.Current.Pac.PacOrganismCount > 600 ||
				GameEngine.Current.Pac.PacSize > 104857600)
			{
				LoadEcosystem(null);
			}

			// Paint the screen
			if (this.Visible == true && !this.resizeBar.Dragging && !noDirectX && !turnOffDirectX)
			{
				TimeMonitor startRefreshTime = new TimeMonitor();
				startRefreshTime.Start();

				TimeMonitor paintFrame = new TimeMonitor();
				paintFrame.Start();
				if (oldVector != null)
				{
					PaintFrame(oldVector, newVector, frameNumber, false);
				}
				totalPaintFrameTime += paintFrame.EndGetMicroseconds();

				try
				{
					tddGameView.DrawScreen = GameConfig.DrawScreen;
					tddGameView.DrawBoundingBox = GameConfig.BoundingBoxes;

					// NOTE: Removed since we change how we render background grids
					//		 we only need to set it once and the view will handle it
					//		 from then on
					//tddGameView.DrawBackgroundGrid = GameConfig.BackgroundGrid;
					tddGameView.DrawDestinationLines = GameConfig.DestinationLines;
					tddGameView.RenderFrame();
					// Successful frame, reset our counter
					// This prevents us from turning off DirectX rendering when we
					// get a hiccup like an out of memory exception.
					turnOffDirectXCounter = 0;
					this.developerPanel.MapLocation = new Point(tddGameView.ViewSize.Left, tddGameView.ViewSize.Top);
				}
				catch (Exception exc)
				{
					Trace.WriteLine(exc.ToString());
					this.turnOffDirectXCounter++;
					if (turnOffDirectXCounter > 10)
					{
						Trace.WriteLine("Turning off DirectX");
						turnOffDirectX = true;
					}
				}

				Int64 refreshTime = startRefreshTime.EndGetMicroseconds();
				paintTimes[frameNumber] = (double)refreshTime / (double)1000;
				totalPaintTime += refreshTime;
			}

			// Process the turn
			TimeMonitor startProcessTime = new TimeMonitor();
			startProcessTime.Start();
			Boolean turnFinished = false;
			try
			{
				turnFinished = GameEngine.Current.ProcessTurn();
			}
			catch (InvalidPeerException ipe)
			{
				timer1.Enabled = false;
				MessageBox.Show(this, ipe.Message, "Can't run EcoSystem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				CloseTerrarium(false);
				return;
			}
			catch (MaliciousOrganismException)
			{
				timer1.Enabled = false;
				relaunch = true;
				maliciousOrganism = true;
				this.Close();
				return;
			}
			catch (ShutdownFailureException)
			{
				// This version of the game has been marked as bad
				timer1.Enabled = false;
				GameConfig.BlockedVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
				runningBlockedVersion = true;
				relaunch = true;
				this.Close();
				return;
			}
			catch (OrganismBlacklistException)
			{
				// Reload the ecosystem but blacklist on startup
				blacklistCheckOnRestart = true;
				LoadEcosystem(null);
				return;
			}
			catch (StateTimedOutException)
			{
				// our state has been timed out -- need to create a new ecosystem
				timer1.Enabled = false;
				MessageBox.Show(this, "Your animals have all died since you haven't run Terrarium for a long time.  Starting fresh.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				CloseTerrarium(false);
				LoadEcosystem(SpecialUserAppDataPath + ecosystemStateFileName);
				return;
			}
			catch (StateCorruptedException corruptedException)
			{
				// our state is corrupted, probably because the app was killed and we're reporting duplicate data -- need to create a new ecosystem
				timer1.Enabled = false;
				MessageBox.Show(this, "Your part of the ecosystem is messed up. Starting fresh. [Last Reported Tick = " + corruptedException.LastReportedTick + "]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				CloseTerrarium(false);
				LoadEcosystem(SpecialUserAppDataPath + ecosystemStateFileName);
				return;
			}

			Int64 processTime = startProcessTime.EndGetMicroseconds();
			processTimes[frameNumber] = (double)processTime / (double)1000;
			totalProcessTime += processTime;

			frameNumber++;
			if (turnFinished)
			{
				if (frameNumber != 10)
				{
					throw new ApplicationException("Frame numbers messed up.");
				}

				frameNumber = 0;

				if (propertySheet != null)
				{
					propertySheet.RefreshGrid();
				}
			}

			outsideTimer.Start();
		}

		// In 10 frames we animate the world from oldState to newState
		// Frame 0 paints the world exactly as it is in oldState
		internal void PaintFrame(WorldVector oldVector, WorldVector newVector, int frameNumber, Boolean erase)
		{
			if (frameNumber == 0)
			{
				if (newVector == null)
				{
					tddGameView.UpdateWorld(oldVector);
				}
				else
				{
					tddGameView.UpdateWorld(newVector);
				}
			}
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if (turnOffDirectX)
			{
				turnOffDirectX = false;
			}

			if (firstActivate)
			{
				firstActivate = false;
				SplashWindow.Current.Hide(this);

				if (startAtStartup)
				{
					LoadGame();
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs me)
		{
			base.OnMouseMove(me);
			if (turnOffDirectX)
			{
				turnOffDirectX = false;
			}
		}

		private void Form_KeyUp(object sender, KeyEventArgs ke)
		{
			if (screenSaverMode == ScreenSaverMode.Run)
			{
				ke.Handled = true;
				this.Close();
			}
		}

		private void MiniMap_Updated(object sender, MiniMapUpdatedEventArgs e)
		{
			this.developerPanel.MiniMap = e.MiniMap;
		}

		private void Organism_Clicked(object sender, OrganismClickedEventArgs e)
		{
			if (((TerrariumSprite)e.OrganismState.RenderInfo).Selected == true)
			{
				if (traceWindow != null)
				{
					traceWindow.SelectOrganism(e.OrganismState);
				}

				if (propertySheet != null)
				{
					propertySheet.SelectObject(e.OrganismState);
				}
			}
			else
			{
				if (traceWindow != null)
				{
					traceWindow.UnselectOrganism(e.OrganismState);
				}

				if (propertySheet != null)
				{
					propertySheet.UnselectObject(e.OrganismState);
				}
			}
		}

		private void menuItemClearBadPeers_Click(object sender, System.EventArgs e)
		{
			GameEngine.Current.NetworkEngine.PeerManager.ClearBadPeers();
			MessageBox.Show(this, "Cleared the list of peers that were thought to be invalid.  Everyone is valid again.");
		}

		private void menuItemPeerProperties_Click(object sender, System.EventArgs e)
		{
			ShowPropertiesDialog(null);
		}

		private void ShowPropertiesDialog(string panel)
		{
			PeerProperties peerForm = new PeerProperties();
			if (panel != null)
			{
				peerForm.SwitchPanel(panel);
			}

			peerForm.ShowDialog(this);

			tddGameView.DrawBackgroundGrid = GameConfig.BackgroundGrid;

			// Web Root might have changed.
			if (panel == "Server")
			{
				this.developerPanel.WebRoot = GameConfig.WebRoot;
			}
			this.Invalidate(true);
		}

		private void menuItemAbout_Click(object sender, System.EventArgs e)
		{
			using (AboutForm aboutForm = new AboutForm())
			{
				aboutForm.ShowDialog(this);
			}
		}

		private void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!CloseTerrarium())
			{
				e.Cancel = true;
			}
			else
			{
				// Set this to true, so that we don't fire any pending timer messages
				alreadyRunningTimer = true;

				this.bottomPanel.Ticker.Stop();
				this.taskBar.Visible = false;
			}
		}

		private void Close_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void newTerrarium_Click(object sender, System.EventArgs e)
		{
			if (screenSaverMode == ScreenSaverMode.Run)
			{
				MessageBox.Show(
					this,
					"This isn't permitted while running in screensaver mode because it could allow someone to get access to your machine when it is locked.  Try closing and starting the Terrarium normally to do this.",
					"Operation Not Permitted",
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
				return;
			}

			NewTerrarium();
		}

		// We can't use Application.UserAppDataPath because we are launched by the stub, whose version doesn't
		// change, so change the version to be the version of this assembly
		// We also treat build version numbers as non-breaking so they aren't included in the path
		internal static string SpecialUserAppDataPath
		{
			get
			{

                string path;

                //if (UpdateManager.IsDeployed)
                //{
                //    path = Application.UserAppDataPath;
                //}
                //else
                //{
                    path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    path = Path.Combine(path, "Terrarium");
                    path = Path.Combine(path, PrivateAssemblyCache.VersionedDirectoryPreamble);
                //}
                         
				if (GameConfig.WebRoot == null || GameConfig.WebRoot.Length == 0)
				{
					GameConfig.WebRoot = "http://www.terrariumgame.net/terrarium";
				}

                Uri serverUrl = new Uri(GameConfig.WebRoot);

                string segments = serverUrl.AbsolutePath.Replace('/', '\\');
                segments = segments.TrimStart('\\');
                segments = segments.TrimEnd('\\');

                path = Path.Combine(path, serverUrl.Host);
                path = Path.Combine(path, segments);

                //if (GameConfig.WebRoot.Substring(0, 5).ToLower(CultureInfo.InvariantCulture) == "http:")
                //{
                //    path = Path.Combine(path, HttpUtility.UrlEncode(GameConfig.WebRoot.Substring(5)));
                //}
                //else
                //{
                //    path = Path.Combine(path, HttpUtility.UrlEncode(GameConfig.WebRoot));
                //}

				// Get rid of all % strings because they can sometimes get unencoded and cause us to load files
				// improperly 
				path = path.Replace("%", "_");

				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}

				return path;
			}
		}

		private void loadEcosystem_Click(object sender, System.EventArgs args)
		{
			LoadEcosystem(null);
		}

		private void LoadEcosystem(string pathToDelete)
		{
			if (CloseTerrarium())
			{
				this.Hide();

				// Delete the old ecosystem if asked to
				if (pathToDelete != null)
				{
					File.Delete(pathToDelete);
				}

                // We want to run in "normal" or Ecosystem mode
                this.screenSaverMode = ScreenSaverMode.NoScreenSaver;

				relaunch = true;
				gamePath = "";

                this.Close();
			}
		}

		private void NewTerrarium()
		{
			SaveFileDialog save = new SaveFileDialog();
			save.Filter = ".NET Terrariums (*.ter)|*.ter|All Files (*.*)|*.*";
			save.Title = "New Terrarium";
			save.DefaultExt = "Ter";
			DialogResult result = save.ShowDialog();
			if (result == DialogResult.OK)
			{
				// Clear out the file and directory
				try
				{
					if (File.Exists(save.FileName))
					{
						File.Delete(save.FileName);
					}

					string pacPath = PrivateAssemblyCache.GetBaseAssemblyDirectory(
						Path.GetDirectoryName(save.FileName), Path.GetFileName(save.FileName));

					if (Directory.Exists(pacPath))
					{
						Directory.Delete(pacPath, true);
					}
				}
				catch (Exception exception)
				{
					ErrorLog.LogHandledException(exception);
					MessageBox.Show("Problem creating new Terrarium: " + exception.Message);
					return;
				}

				if (CloseTerrarium())
				{
					this.Hide();
					FileInfo fileInfo = new FileInfo(save.FileName);
					relaunch = true;
					gamePath = save.FileName;
					screenSaverMode = ScreenSaverMode.RunLoadTerrarium;
					this.Close();
				}
			}
		}

		private void loadTerrarium_Click(object sender, System.EventArgs args)
		{
			if (screenSaverMode == ScreenSaverMode.Run)
			{
				MessageBox.Show(
					this,
					"This isn't permitted while running in screensaver mode because it could allow someone to get access to your machine when it is locked.  Try closing and starting the Terrarium normally to do this.",
					"Operation Not Permitted",
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
				return;
			}

			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = ".NET Terrariums (*.ter)|*.ter|All Files (*.*)|*.*";
			dialog.Title = "Locate your Terrarium";
			dialog.DefaultExt = ".ter";
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				if (CloseTerrarium())
				{
					try
					{
						this.Hide();
						FileInfo fileInfo = new FileInfo(dialog.FileName);
						relaunch = true;
						gamePath = dialog.FileName;
						screenSaverMode = ScreenSaverMode.RunLoadTerrarium;
						this.Close();
					}
					catch (Exception e)
					{
						MessageBox.Show(this, e.Message, "Error Loading Terrarium", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
		}

		// Sets up the form after a new game has been loaded
		private void NewGameLoaded()
		{
			if (GameEngine.Current.Pac.LastRun.Length != 0)
			{
				engineStateText += "Terrarium automatically shutdown and restarted (without saving)because the creature '" + PrivateAssemblyCache.GetAssemblyShortName(GameEngine.Current.Pac.LastRun) + "' hung your machine and Terrarium needed to remove this offensive animal.";
				GameEngine.Current.Pac.LastRun = "";
			}

			if (GameEngine.Current.EcosystemMode == true)
			{
				this.developerPanel.GameModeText = "Ecosystem";
				developerPanel.WebRoot = GameConfig.WebRoot;
				bottomPanel.GameMode = GameModes.Ecosystem;
				bottomPanel.Mode = screenSaverMode;
			}
			else
			{
				this.developerPanel.GameModeText = "Terrarium";
                this.bottomPanel.GameMode = GameModes.Terrarium;

				if (GameEngine.Current.FileName.Length == 0)
				{
					developerPanel.WebRoot = "[New Terrarium]";
				}
				else
				{
					developerPanel.WebRoot = GameEngine.Current.FileName;
				}
			}

			GameEngine.Current.WorldVectorChanged += new WorldVectorChangedEventHandler(WorldVectorChanged);
			GameEngine.Current.EngineStateChanged += new EngineStateChangedEventHandler(EngineStateChanged);

			oldVector = null;
			newVector = null;
			frameNumber = 0;

			InitializeGraphics(GameEngine.Current.WorldWidth, GameEngine.Current.WorldHeight);

			timer1.Enabled = true;

			if (screenSaverMode == ScreenSaverMode.Run)
			{
				Block.BlockScreens(Screen.FromControl(this));

				this.ShowUI = false;
				this.Fullscreen = true;
				this.ContextMenu = null;
				this.tddGameView.DrawText = true;
				Cursor.Hide();
				Cursor.Position = new Point(this.Width / 2, this.Height / 2);
				this.tddGameView.DrawCursor = false;
			}
			else
			{
				this.Cursor = Cursors.Default;

                if (wasRelaunched)
                {
                    this.SuspendLayout();

                    if (windowState == FormWindowState.Maximized)
                    {
                        this.WindowState = FormWindowState.Maximized;
                    }
                    else if (windowState == FormWindowState.Minimized)
                    {
                        this.Hide();
                        this.taskBar.Visible = true;
                    }
                    else
                    {
                        this.WindowState = FormWindowState.Normal;
                    }

                    this.Location = new Point(windowRectangle.Left, windowRectangle.Top);
                    this.Size = new Size(windowRectangle.Width, windowRectangle.Height);
                    
                    this.ResumeLayout();
                }
                else
                {
                    if (GameConfig.StartFullscreen == true)
                        this.Fullscreen = true;
                }
			}

			this.ConfigureTickerBar();
		}

		// Returns true if succeeded, false otherwise
		private Boolean CloseTerrarium()
		{
			return CloseTerrarium(true);
		}

		private Boolean CloseTerrarium(bool saveExisting)
		{
			if (maliciousOrganism)
			{
				return true;
			}

			timer1.Enabled = false;
			
			this.bottomPanel.Ticker.Stop();

			if (tddGameView != null)
			{
				tddGameView.Paused = true;
			}

			if (!saveExisting)
			{
				GameEngine.Current.StopGame(false);
			}
			else if (GameEngine.Current != null)
			{
				// If we are shutting down because they are running a blocked version, we always save
				if (GameEngine.Current.EcosystemMode == true || runningBlockedVersion)
				{
					// Always save Ecosystem mode games
					GameEngine.Current.StopGame(true);
				}
				else
				{
                    if (this.forceSave == true)
                    {
                        GameEngine.Current.StopGame(true);
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show(this, "Save existing Terrarium?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.Cancel)
                        {
                            timer1.Enabled = true;
                            if (tddGameView != null)
                            {
                                tddGameView.Paused = false;
                            }
                            return false;
                        }
                        else if (result == DialogResult.No)
                        {
                            GameEngine.Current.StopGame(false);
                        }
                        else
                        {
                            if (GameEngine.Current.FileName.Length == 0)
                            {
                                SaveFileDialog save = new SaveFileDialog();
                                save.Filter = ".NET Terrariums (*.ter)|*.ter|All Files (*.*)|*.*";
                                save.Title = "Save";
                                save.DefaultExt = "Ter";
                                result = save.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    GameEngine.Current.FileName = save.FileName;
                                    GameEngine.Current.StopGame(true);
                                }
                                else
                                {
                                    // Cancelled out
                                    timer1.Enabled = true;
                                    if (tddGameView != null)
                                    {
                                        tddGameView.Paused = false;
                                    }
                                    return false;
                                }
                            }
                            else
                            {
                                GameEngine.Current.StopGame(true);
                            }
                        }
                    }
				}
			}

			if (tddGameView != null)
			{
				tddGameView.Refresh();
			}

			return true;
		}

		private void AddNewAnimal_Click(object sender, System.EventArgs e)
		{
			try
			{
				OrganismAssemblyInfo oas = (OrganismAssemblyInfo)this.bottomPanel.AddAnimalComboBox.SelectedItem;
				if (oas != null)
				{
					//Assembly assembly = Assembly.Load(((OrganismAssemblyInfo) this.bottomPanel.AddAnimalComboBox.SelectedItem).FullName);
					Assembly assembly = GameEngine.Current.Pac.LoadOrganismAssembly(((OrganismAssemblyInfo)this.bottomPanel.AddAnimalComboBox.SelectedItem).FullName);
					GameEngine.Current.AddNewOrganism(Species.GetSpeciesFromAssembly(assembly), Point.Empty);
				}
			}
			catch (TargetInvocationException exception)
			{
				Exception innerException = exception;
				while (innerException.InnerException != null)
				{
					innerException = innerException.InnerException;
				}

				MessageBox.Show(this, innerException.Message, "Error Loading Species", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			catch (Exception exception)
			{
				MessageBox.Show(this, exception.Message, "Error Loading Species", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void showProperties_Click(object sender, System.EventArgs e)
		{
			DisplayPropertySheet();
		}

		private void introduceAnimalTerrarium_Click(object sender, System.EventArgs e)
		{
			if (screenSaverMode == ScreenSaverMode.Run)
			{
				MessageBox.Show(
					this,
					"This isn't permitted while running in screensaver mode because it could allow someone to get access to your machine when it is locked.  Try closing and starting the Terrarium normally to do this.",
					"Operation Not Permitted",
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
				return;
			}

			this.Cursor = Cursors.WaitCursor;
			ReintroduceSpecies species = new ReintroduceSpecies(false);
			species.ShowDialog();
			this.Cursor = Cursors.Default;
		}

		private void introduceAnimalEcosystem_Click(object sender, System.EventArgs e)
		{
			if (screenSaverMode == ScreenSaverMode.Run)
			{
				MessageBox.Show(
					this,
					"This isn't permitted while running in screensaver mode because it could allow someone to get access to your machine when it is locked.  Try closing and starting the Terrarium normally to do this.",
					"Operation Not Permitted",
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
				return;
			}

			String assemblyName;
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				assemblyName = openFileDialog1.FileName;

				Species newSpecies = null;
				try
				{
					if (GameEngine.Current != null)
					{
						newSpecies = GameEngine.Current.AddNewOrganism(assemblyName, Point.Empty, false);
					}
				}
				catch (TargetInvocationException exception)
				{
					Exception innerException = exception;
					while (innerException.InnerException != null)
					{
						innerException = innerException.InnerException;
					}

					MessageBox.Show(this, innerException.Message, "Error Loading Assembly", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				catch (GameEngineException exception)
				{
					MessageBox.Show(this, exception.Message, "Error Loading Assembly", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				catch (IOException exception)
				{
					MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
		}

		private void reintroduceAnimal_Click(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			ReintroduceSpecies species = new ReintroduceSpecies(true);
			species.ShowDialog();
			this.Cursor = Cursors.Default;
		}

		private void Pause_Click(object sender, System.EventArgs e)
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Terrarium.Forms.GlassBottomPanel));

			if (timer1.Enabled)
			{
                this.bottomPanel.PauseButton.Image = ((System.Drawing.Image)(resources.GetObject("playButton.Image")));
				if (tddGameView != null)
				{
					tddGameView.Paused = true;
				}
			}
			else
			{
                this.bottomPanel.PauseButton.Image = ((System.Drawing.Image)(resources.GetObject("pauseButton.Image")));
			}

			timer1.Enabled = !timer1.Enabled;
		}

		// Called by the game engine every time a tick creates a new state for the world
		private void WorldVectorChanged(object sender, WorldVectorChangedEventArgs e)
		{
			oldVector = newVector;
			newVector = e.NewVector;

			if (tddGameView != null)
			{
				if (e.NewVector.State.Organisms.Count < 1)
				{
					if (GameEngine.Current != null && GameEngine.Current.EcosystemMode)
					{
						if (developerPanel.Leds[(int)LedIndicators.DiscoveryWebService].LedState == LedStates.Failed ||
							developerPanel.Leds[(int)LedIndicators.ReportWebService].LedState == LedStates.Failed)
						{
							tddGameView.TerrariumMessage = emptyEcosystemServerDownMessage;
						}
						else
						{
							tddGameView.TerrariumMessage = emptyEcosystemMessage;
						}
					}
					else
					{
						tddGameView.TerrariumMessage = emptyTerrariumMessage;
					}
				}
				else
				{
					// Only clear our messages, not those set by someone else
					if (tddGameView.TerrariumMessage == emptyEcosystemMessage || tddGameView.TerrariumMessage == emptyTerrariumMessage)
					{
						tddGameView.TerrariumMessage = null;
					}
				}

				// If the network has a problem, always override the message with this
				if (GameEngine.Current != null && GameEngine.Current.NetworkEngine != null)
				{
					if (GameEngine.Current.NetworkEngine.NetworkStatusMessage.Length != 0)
					{
						tddGameView.TerrariumMessage = GameEngine.Current.NetworkEngine.NetworkStatusMessage;
					}
					else
					{
						if (tddGameView.TerrariumMessage == NetworkEngine.NetworkBehindNatMessage)
						{
							tddGameView.TerrariumMessage = null;
						}
					}
				}
			}

			// Tell the tracewindow that we've got a new tick
			if (traceWindow != null)
			{
				traceWindow.TickEnded();
			}

			this.developerPanel.AnimalCount = GameEngine.Current.AnimalCount;
			this.developerPanel.MaximumAnimalCount = GameEngine.Current.MaxAnimals;
			this.developerPanel.PeerCount = GameEngine.Current.PeerCount;

			if (GameEngine.Current.EcosystemMode == true)
			{
				//this.taskBar.Text = GameConfig.WebRoot;
			}
			else
			{
				//this.taskBar.Text = GameEngine.Current.FileName;
			}

			this.taskBar.Text = "Terrarium";

			this.taskBar.Text += "\r\nPopulation: " + GameEngine.Current.AnimalCount + "/" + GameEngine.Current.MaxAnimals + "\r\nPeers: " + GameEngine.Current.PeerCount;

			try
			{
                if (GameEngine.Current.IsNetworkEnabled == true)
                {
                    developerPanel.Teleportations = GameEngine.Current.NetworkEngine.Teleportations;
                    developerPanel.FailedSends = GameEngine.Current.NetworkEngine.FailedTeleportationSends;
                    developerPanel.FailedReceives = GameEngine.Current.NetworkEngine.FailedTeleportationReceives;
                }
			}
			catch { }

			if (engineStateText != null && engineStateText.Length != 0)
			{
				this.bottomPanel.Ticker.Messages.Enqueue(engineStateText);
				engineStateText = "";
			}
		}

		private string CreateHTTPSURL(string url)
		{
			return ConvertToHTTPS(url);
		}

		public static string ConvertToHTTPS(string url)
		{
			url = url.ToLower(CultureInfo.InvariantCulture);
			if (url.StartsWith("https"))
			{
				return url;
			}
			else
			{
				return url.Insert(4, "s");
			}
		}

		// Report timing information to the trace window
		private void ReportTimes()
		{
			Int64 frameRate = 0;
			if (frameRateTimer.IsStarted)
			{
				frameRate = frameRateTimer.EndGetMicroseconds();
			}

			if (traceEnabled && traceWindow != null)
			{
				traceWindow.DebugWriteLine(string.Format(
					"$Frame Timings for 10 frames (mSec)\r\n" +
					"   All    - Total:{40, 6:##0.00}[{30, 6:##0.00},{31, 6:##0.00},{32, 6:##0.00},{33, 6:##0.00},{34, 6:##0.00},{35, 6:##0.00},{36,6:##0.00},{37, 6:##0.00},{38, 6:##0.00},{39, 6:##0.00}\r\n" +
					"   Outside- Total:{41, 6:##0.00}[{20, 6:##0.00},{21, 6:##0.00},{22, 6:##0.00},{23, 6:##0.00},{24, 6:##0.00},{25, 6:##0.00},{26,6:##0.00},{27, 6:##0.00},{28, 6:##0.00},{29, 6:##0.00}\r\n" +
					"   Process- Total:{42, 6:##0.00}[{0, 6:##0.00},{1, 6:##0.00},{2, 6:##0.00},{3, 6:##0.00},{4, 6:##0.00},{5, 6:##0.00},{6,6:##0.00},{7, 6:##0.00},{8, 6:##0.00},{9, 6:##0.00}\r\n" +
					"   Paint  - Total:{43, 6:##0.00}[{10, 6:##0.00},{11, 6:##0.00},{12, 6:##0.00},{13, 6:##0.00},{14, 6:##0.00},{15, 6:##0.00},{16,6:##0.00},{17, 6:##0.00},{18, 6:##0.00},{19, 6:##0.00}",
					processTimes[0], processTimes[1], processTimes[2], processTimes[3], processTimes[4], processTimes[5], processTimes[6], processTimes[7], processTimes[8], processTimes[9],
					paintTimes[0], paintTimes[1], paintTimes[2], paintTimes[3], paintTimes[4], paintTimes[5], paintTimes[6], paintTimes[7], paintTimes[8], paintTimes[9],
					outsideTimes[0], outsideTimes[1], outsideTimes[2], outsideTimes[3], outsideTimes[4], outsideTimes[5], outsideTimes[6], outsideTimes[7], outsideTimes[8], outsideTimes[9],
					outsideTimes[0] + processTimes[0] + paintTimes[0], outsideTimes[1] + processTimes[1] + paintTimes[1],
					outsideTimes[2] + processTimes[2] + paintTimes[2], outsideTimes[3] + processTimes[3] + paintTimes[3],
					outsideTimes[4] + processTimes[4] + paintTimes[4], outsideTimes[5] + processTimes[5] + paintTimes[5],
					outsideTimes[6] + processTimes[6] + paintTimes[6], outsideTimes[7] + processTimes[7] + paintTimes[7],
					outsideTimes[8] + processTimes[8] + paintTimes[8], outsideTimes[9] + processTimes[9] + paintTimes[9],
					(double)frameRate / (double)1000,
					totalOutsideTime,
					(double)totalProcessTime / (double)1000,
					(double)totalPaintTime / (double)1000));
#if TRACE
				if (tddGameView != null)
				{
					tddGameView.Profiler.ClearProfiler();
				}
#endif
			}

			totalProcessTime = 0;
			totalPaintTime = 0;
			totalPaintFrameTime = 0;
			totalOutsideTime = 0;

			frameRateTimer.Start();
		}

		internal void DeveloperTrace(string msg)
		{
			if (traceWindow != null)
			{
				traceWindow.DebugWriteLine(msg);
			}
		}

		//Handle state changes from the GameEngine
		private void EngineStateChanged(object sender, EngineStateChangedEventArgs ev)
		{
			switch (ev.StateChange)
			{
				case EngineStateChangeType.AnimalTeleported:
					//this.topPanel.GameEventText = ev.ShortDescription;
					engineStateText += ev.LongDescription + "\r\n";
					break;

				case EngineStateChangeType.Other:
					engineStateText += ev.LongDescription + "\r\n";
					break;

				case EngineStateChangeType.DeveloperInformation:
					if (traceEnabled && traceWindow != null)
					{
						traceWindow.DebugWriteLine(ev.ShortDescription);
					}
					break;
			}
		}

		private void AddAnimalComboBox_DropDown(object sender, EventArgs e)
		{
			this.bottomPanel.AddAnimalComboBox.Items.Clear();
			if (GameEngine.Current != null)
			{
				this.bottomPanel.AddAnimalComboBox.Items.AddRange(GameEngine.Current.Pac.GetAssemblies());
			}
		}

		private void navigatePictureBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (tddGameView != null)
			{
				tddGameView.CenterTo((e.X * this.developerPanel.LandSize.Width) / this.developerPanel.NavigatePictureBox.Width,
					(e.Y * this.developerPanel.LandSize.Height) / this.developerPanel.NavigatePictureBox.Height);
			}
		}

		internal void TemporarilySuspendBlacklist()
		{
			GameEngine engine = GameEngine.Current;
			if (engine != null)
			{
				engine.Scheduler.TemporarilySuspendBlacklisting();
			}
		}

		internal bool SuspendBlacklisting
		{
			set
			{
				GameEngine engine = GameEngine.Current;
				if (engine != null)
				{
					engine.Scheduler.SuspendBlacklisting = value;
				}
			}
		}

		internal void PowerModeChanged(object sender, PowerModeChangedEventArgs e)
		{
			switch (e.Mode)
			{
				case PowerModes.Resume:
					SuspendBlacklisting = false;
					Debug.WriteLine("Temporarily suspending blacklists because machine is resuming.");
					TemporarilySuspendBlacklist();
					break;
				case PowerModes.Suspend:
					Debug.WriteLine("Suspend blacklists because of computer powersave mode change.");
					SuspendBlacklisting = true;
					break;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(this.BackColor);
		}

		/// <summary>
		/// Do nothing when painting the background
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

		//-----------------------------------------------------
		//
		// Window Resizing and Pseudo-Fullscreen Support
		//
		//-----------------------------------------------------
		protected override void OnSizeChanged(EventArgs args)
		{
			base.OnSizeChanged(args);
			if (this.Visible == false || this.resizeBar.Dragging)
				return;

			try
			{
				if (this.tddGameView != null && this.Visible == true)
				{
					Debug.WriteLine(this.tddGameView.Size);
					this.developerPanel.ViewPortSize = this.tddGameView.Size;
					this.tddGameView.ResizeViewer();
				}
			}
			catch { }
		}

		private void titleBar_MaximizeClicked(object sender, System.EventArgs e)
		{
			this.Fullscreen = !this.Fullscreen;
		}

		private void titleBar_MinimizeClicked(object sender, System.EventArgs e)
		{
			this.Hide();
			this.taskBar.Visible = true;
		}

		/// <summary>
		/// Run in fullscreen?
		/// </summary>
		public bool Fullscreen
		{
			get
			{
				return this.fullScreen;
			}

			set
			{
				if (this.fullScreen == value)
					return;

				this.tddGameView.DrawScreen = false;

				this.fullScreen = value;

				if (this.fullScreen == true)
				{
					this.originalLocation = this.Location;
					this.originalSize = this.ClientSize;

					if (this.screenSaverMode == ScreenSaverMode.Run && GameConfig.ScreenSaverSpanMonitors == true)
					{
						int left = 99999, top = 99999, right = -99999, bottom = -99999;

						foreach (Screen screen in Screen.AllScreens)
						{
							if (screen.Bounds.Left < left)
								left = screen.Bounds.Left;
							if (screen.Bounds.Top < top)
								top = screen.Bounds.Top;
							if (screen.Bounds.Right > right)
								right = screen.Bounds.Right;
							if (screen.Bounds.Bottom > bottom)
								bottom = screen.Bounds.Right;
						}

						this.Location = new Point(left, top);
						this.Size = new Size(right - left, bottom - top);
						this.TopMost = true;
					}
					else
					{
						this.WindowState = FormWindowState.Maximized;
					}

				}
				else
				{
					this.ShowUI = true;

					this.WindowState = FormWindowState.Normal;

					this.ClientSize = this.originalSize;

					this.Location = this.originalLocation;
				}

                this.resizeBar.Visible = !this.fullScreen;

				this.Invalidate(true);

				this.OnSizeChanged(new EventArgs());

				this.tddGameView.DrawScreen = true;
			}
		}

		/// <summary>
		/// Show the User Interface controls?
		/// </summary>
		public bool ShowUI
		{
			get
			{
				return this.showUI;
			}

			set
			{
				if (this.showUI == value)
					return;

				this.showUI = value;

				this.Visible = false;

				this.titleBar.Visible = this.showUI;
                this.developerPanel.Visible = this.showUI;
                this.bottomContainerPanel.Visible = this.showUI;
                //this.bottomPanel.Visible = this.showUI;
                //this.resizeBar.Visible = this.showUI;

				this.Visible = true;

				this.OnSizeChanged(new EventArgs());
			}
		}

		private void SettingsButton_Click(object sender, EventArgs e)
		{
			this.ShowPropertiesDialog(null);
		}

		private void DetailsButton_Click(object sender, EventArgs e)
		{
			DisplayPropertySheet();
		}

		private void StatisticsButton_Click(object sender, EventArgs e)
		{
			DisplayReportWindow();
		}

		private void TraceButton_Click(object sender, EventArgs e)
		{
			DisplayTraceForm();
		}

		private void screenSaverTimer_Tick(object sender, EventArgs e)
		{
			int xOffset = random.Next(this.developerPanel.LandSize.Width);
			int yOffset = random.Next(this.developerPanel.LandSize.Height);
			this.tddGameView.CenterTo(xOffset, yOffset);
		}

		private void MainForm_SystemColorsChanged(object sender, System.EventArgs e)
		{
			this.Invalidate(true);
		}

		private void bottomPanel_ExpandedChanged(object sender, System.EventArgs e)
		{
			this.OnSizeChanged(new EventArgs());
		}

		private void MainForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Right)
			{
				this.tddGameView.ScrollRight(16);
			}
			else if (e.KeyCode == Keys.Left)
			{
				this.tddGameView.ScrollLeft(16);
			}
			else if (e.KeyCode == Keys.Up)
			{
				this.tddGameView.ScrollUp(16);
			}
			else if (e.KeyCode == Keys.Down)
			{
				this.tddGameView.ScrollDown(16);
			}

			if (this.screenSaverMode != ScreenSaverMode.Run)
			{
				if (e.KeyCode == Keys.F1)
				{
					this.SettingsButton_Click(this, new EventArgs());
				}
				else if (e.KeyCode == Keys.F2)
				{
					DisplayPropertySheet();
				}
				else if (e.KeyCode == Keys.F3)
				{
					DisplayReportWindow();
				}
				else if (e.KeyCode == Keys.F4)
				{
					DisplayTraceForm();
				}
				else if (e.KeyCode == Keys.F5)
				{
					GameConfig.BoundingBoxes = !GameConfig.BoundingBoxes;
				}
				else if (e.KeyCode == Keys.F6)
				{
					GameConfig.DestinationLines = !GameConfig.DestinationLines;
				}

				if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.Enter)
				{
					this.ShowUI = !this.ShowUI;
					if (this.ShowUI == false)
					{
						this.Fullscreen = true;
					}
					else
					{
						this.Fullscreen = false;
					}
				}
			}
		}

		protected void ConfigureTickerBar()
		{
			this.bottomPanel.Ticker.Stop();

            this.bottomPanel.Ticker.Messages.Enqueue("Welcome to Terrarium!", Color.White, null);
			this.bottomPanel.Ticker.Messages.Enqueue("You are running version " + Assembly.GetExecutingAssembly().GetName().Version.ToString(), Color.White,null);
            //if (UpdateManager.IsDeployed == true)
            //    this.bottomPanel.Ticker.Messages.Enqueue("Auto-updates are enabled", Color.White, null);
            //else
            //    this.bottomPanel.Ticker.Messages.Enqueue("Auto-updates are not enabled", Color.Yellow, null);
            			
			this.bottomPanel.Ticker.Start();
		}

		private void titleBar_DoubleClick(object sender, System.EventArgs e)
		{
			this.Fullscreen = !this.Fullscreen;
		}

		private void DisplayReportWindow()
		{
			if (reportWindow == null || !reportWindow.IsHandleCreated)
			{
				reportWindow = new ReportStats();
				AddOwnedForm(reportWindow);

				reportWindow.StartPosition = FormStartPosition.CenterParent;
			}

			if (!reportWindow.Visible)
			{
				reportWindow.Show();
			}
		}

		private void DisplayTraceForm()
		{
			if (traceWindow == null || !traceWindow.IsHandleCreated)
			{
				traceWindow = new TraceWindow();
				AddOwnedForm(traceWindow);

				traceWindow.StartPosition = FormStartPosition.CenterParent;
			}

			if (!traceWindow.Visible)
			{
				traceWindow.Show();
			}
		}

		private void DisplayPropertySheet()
		{
			if (propertySheet == null || !propertySheet.IsHandleCreated)
			{
				propertySheet = new PropertySheet();
				AddOwnedForm(propertySheet);

				propertySheet.StartPosition = FormStartPosition.CenterParent;
			}

			if (!propertySheet.Visible)
			{
				propertySheet.Show();
			}
		}

		private void taskBar_DoubleClick(object sender, EventArgs e)
		{
            this.Show();
			this.taskBar.Visible = false;
		}
	}
}