//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

using Terrarium.Metal;
using Terrarium.Forms;

namespace Terrarium.Tools
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private bool		dirty		= false;
		private string		fileName	= "";

		#region Designer Fields
		private System.Windows.Forms.Panel propertyGridPanel;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel previewPanel;
		private System.Windows.Forms.MenuItem menuItem1;
		private Terrarium.Metal.MetalPanel bottomPanel;
		private Terrarium.Metal.MetalPanel middlePanel;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private Terrarium.Metal.MetalButton normalButton;
		private Terrarium.Metal.MetalButton highlightButton;
		private Terrarium.Metal.MetalButton metalButton2;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		#endregion
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.MenuItem fileNewMenu;
		private System.Windows.Forms.MenuItem fileOpenMenu;
		private System.Windows.Forms.MenuItem fileSaveMenu;
		private System.Windows.Forms.MenuItem fileSaveAsMenu;
		private System.Windows.Forms.MenuItem fileExitMenu;
		private Terrarium.Forms.TitleBar titleBar;
		private Terrarium.Forms.StatusBar statusBar;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.RefreshPropertyGrid();

			string initialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.Personal );
			if( Directory.Exists( initialDirectory ) )
			{
				initialDirectory += "\\Terrarium";

				if ( false == Directory.Exists( initialDirectory ) )
				{
					Directory.CreateDirectory( initialDirectory );
				}
			}
			else
			{
				initialDirectory = Environment.CurrentDirectory;
			}
			this.openFileDialog.InitialDirectory = initialDirectory;
			this.saveFileDialog.InitialDirectory = initialDirectory;

			this.statusBar.Leds[0].LedState = LedStates.Idle;
			this.statusBar.Leds[1].LedState = LedStates.Idle;
			this.statusBar.Leds[2].LedState = LedStates.Waiting;
			this.statusBar.Leds[3].LedState = LedStates.Failed;

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.propertyGridPanel = new System.Windows.Forms.Panel();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.fileNewMenu = new System.Windows.Forms.MenuItem();
			this.fileOpenMenu = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.fileSaveMenu = new System.Windows.Forms.MenuItem();
			this.fileSaveAsMenu = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.fileExitMenu = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.previewPanel = new System.Windows.Forms.Panel();
			this.statusBar = new Terrarium.Forms.StatusBar();
			this.titleBar = new Terrarium.Forms.TitleBar();
			this.middlePanel = new Terrarium.Metal.MetalPanel();
			this.metalButton2 = new Terrarium.Metal.MetalButton();
			this.highlightButton = new Terrarium.Metal.MetalButton();
			this.normalButton = new Terrarium.Metal.MetalButton();
			this.bottomPanel = new Terrarium.Metal.MetalPanel();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.propertyGridPanel.SuspendLayout();
			this.previewPanel.SuspendLayout();
			this.middlePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// propertyGridPanel
			// 
			this.propertyGridPanel.Controls.Add(this.propertyGrid1);
			this.propertyGridPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.propertyGridPanel.Location = new System.Drawing.Point(376, 0);
			this.propertyGridPanel.Name = "propertyGridPanel";
			this.propertyGridPanel.Size = new System.Drawing.Size(256, 361);
			this.propertyGridPanel.TabIndex = 0;
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.CommandsForeColor = System.Drawing.Color.White;
			this.propertyGrid1.CommandsVisibleIfAvailable = true;
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.propertyGrid1.HelpVisible = false;
			this.propertyGrid1.LargeButtons = false;
			this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.SelectedObject = true;
			this.propertyGrid1.Size = new System.Drawing.Size(256, 361);
			this.propertyGrid1.TabIndex = 0;
			this.propertyGrid1.Text = "propertyGrid1";
			this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;
			this.propertyGrid1.Click += new System.EventHandler(this.propertyGrid1_Click);
			this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem3});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.fileNewMenu,
																					  this.fileOpenMenu,
																					  this.menuItem10,
																					  this.fileSaveMenu,
																					  this.fileSaveAsMenu,
																					  this.menuItem9,
																					  this.fileExitMenu});
			this.menuItem1.Text = "&File";
			// 
			// fileNewMenu
			// 
			this.fileNewMenu.Index = 0;
			this.fileNewMenu.Text = "&New";
			this.fileNewMenu.Click += new System.EventHandler(this.fileNewMenu_Click);
			// 
			// fileOpenMenu
			// 
			this.fileOpenMenu.Index = 1;
			this.fileOpenMenu.Text = "&Open...";
			this.fileOpenMenu.Click += new System.EventHandler(this.fileOpenMenu_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 2;
			this.menuItem10.Text = "-";
			// 
			// fileSaveMenu
			// 
			this.fileSaveMenu.Index = 3;
			this.fileSaveMenu.Text = "&Save";
			this.fileSaveMenu.Click += new System.EventHandler(this.fileSaveMenu_Click);
			// 
			// fileSaveAsMenu
			// 
			this.fileSaveAsMenu.Index = 4;
			this.fileSaveAsMenu.Text = "Save &As...";
			this.fileSaveAsMenu.Click += new System.EventHandler(this.fileSaveAsMenu_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 5;
			this.menuItem9.Text = "-";
			// 
			// fileExitMenu
			// 
			this.fileExitMenu.Index = 6;
			this.fileExitMenu.Text = "E&xit";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem4});
			this.menuItem3.Text = "&Help";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 0;
			this.menuItem4.Text = "&About Metal Style Editor...";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter1.Location = new System.Drawing.Point(373, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 361);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// previewPanel
			// 
			this.previewPanel.Controls.Add(this.statusBar);
			this.previewPanel.Controls.Add(this.titleBar);
			this.previewPanel.Controls.Add(this.middlePanel);
			this.previewPanel.Controls.Add(this.bottomPanel);
			this.previewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.previewPanel.Location = new System.Drawing.Point(0, 0);
			this.previewPanel.Name = "previewPanel";
			this.previewPanel.Size = new System.Drawing.Size(373, 361);
			this.previewPanel.TabIndex = 2;
			// 
			// statusBar
			// 
			this.statusBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.statusBar.GameMode = Terrarium.Forms.GameModes.Ecosystem;
			this.statusBar.Location = new System.Drawing.Point(0, 24);
			this.statusBar.Mode = Terrarium.Forms.ScreenSaverMode.ShowSettingsModeless;
			this.statusBar.ModeText = null;
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(373, 16);
			this.statusBar.TabIndex = 4;
			this.statusBar.WebRoot = null;
			// 
			// titleBar
			// 
			this.titleBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.titleBar.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.titleBar.ForeColor = System.Drawing.Color.White;
			this.titleBar.Image = ((System.Drawing.Image)(resources.GetObject("titleBar.Image")));
			this.titleBar.Location = new System.Drawing.Point(0, 0);
			this.titleBar.Name = "titleBar";
			this.titleBar.Size = new System.Drawing.Size(373, 24);
			this.titleBar.TabIndex = 3;
			this.titleBar.Title = "Form Title";
			// 
			// middlePanel
			// 
			this.middlePanel.Borders = Terrarium.Metal.MetalBorders.LeftAndRight;
			this.middlePanel.Controls.Add(this.metalButton2);
			this.middlePanel.Controls.Add(this.highlightButton);
			this.middlePanel.Controls.Add(this.normalButton);
			this.middlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.middlePanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.middlePanel.Gradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.middlePanel.Location = new System.Drawing.Point(0, 0);
			this.middlePanel.Name = "middlePanel";
			this.middlePanel.Size = new System.Drawing.Size(373, 337);
			this.middlePanel.Sunk = true;
			this.middlePanel.TabIndex = 2;
			this.middlePanel.UseStyles = true;
			// 
			// metalButton2
			// 
			this.metalButton2.Borders = Terrarium.Metal.MetalBorders.All;
			this.metalButton2.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(64)), ((System.Byte)(64)));
			this.metalButton2.DisabledGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(64)), ((System.Byte)(64)));
			this.metalButton2.Enabled = false;
			this.metalButton2.Highlight = false;
			this.metalButton2.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.metalButton2.HighlightGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(192)));
			this.metalButton2.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(64)), ((System.Byte)(0)));
			this.metalButton2.HoverGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(216)), ((System.Byte)(0)));
			this.metalButton2.Location = new System.Drawing.Point(144, 192);
			this.metalButton2.Name = "metalButton2";
			this.metalButton2.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.metalButton2.NormalGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.metalButton2.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(216)), ((System.Byte)(0)));
			this.metalButton2.PressedGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(64)), ((System.Byte)(0)));
			this.metalButton2.Size = new System.Drawing.Size(96, 32);
			this.metalButton2.TabIndex = 2;
			this.metalButton2.TabStop = false;
			this.metalButton2.Text = "Disabled";
			// 
			// highlightButton
			// 
			this.highlightButton.Borders = Terrarium.Metal.MetalBorders.All;
			this.highlightButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(64)), ((System.Byte)(64)));
			this.highlightButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(64)), ((System.Byte)(64)));
			this.highlightButton.Highlight = true;
			this.highlightButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.highlightButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(192)));
			this.highlightButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(64)), ((System.Byte)(0)));
			this.highlightButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(216)), ((System.Byte)(0)));
			this.highlightButton.Location = new System.Drawing.Point(144, 144);
			this.highlightButton.Name = "highlightButton";
			this.highlightButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.highlightButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.highlightButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(216)), ((System.Byte)(0)));
			this.highlightButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(64)), ((System.Byte)(0)));
			this.highlightButton.Size = new System.Drawing.Size(96, 32);
			this.highlightButton.TabIndex = 1;
			this.highlightButton.TabStop = false;
			this.highlightButton.Text = "Highlight";
			// 
			// normalButton
			// 
			this.normalButton.Borders = Terrarium.Metal.MetalBorders.All;
			this.normalButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(64)), ((System.Byte)(64)));
			this.normalButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(64)), ((System.Byte)(64)), ((System.Byte)(64)));
			this.normalButton.Highlight = false;
			this.normalButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.normalButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(192)));
			this.normalButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(64)), ((System.Byte)(0)));
			this.normalButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(216)), ((System.Byte)(0)));
			this.normalButton.Location = new System.Drawing.Point(144, 96);
			this.normalButton.Name = "normalButton";
			this.normalButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.normalButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.normalButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(216)), ((System.Byte)(0)));
			this.normalButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(64)), ((System.Byte)(0)));
			this.normalButton.Size = new System.Drawing.Size(96, 32);
			this.normalButton.TabIndex = 0;
			this.normalButton.TabStop = false;
			this.normalButton.Text = "Normal";
			// 
			// bottomPanel
			// 
			this.bottomPanel.Borders = Terrarium.Metal.MetalBorders.All;
			this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.bottomPanel.Gradient.Top = System.Drawing.Color.FromArgb(((System.Byte)(96)), ((System.Byte)(96)), ((System.Byte)(96)));
			this.bottomPanel.Location = new System.Drawing.Point(0, 337);
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Size = new System.Drawing.Size(373, 24);
			this.bottomPanel.Sunk = false;
			this.bottomPanel.TabIndex = 1;
			this.bottomPanel.UseStyles = true;
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "style";
			this.openFileDialog.Filter = "Metal Style|*.style|All files|*.*";
			this.openFileDialog.Title = "Select a Style To Open";
			this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "style";
			this.saveFileDialog.Filter = "Metal Style|*.style|All files|*.*";
			this.saveFileDialog.Title = "Select a File Name to Save";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 11);
			this.ClientSize = new System.Drawing.Size(632, 361);
			this.Controls.Add(this.previewPanel);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.propertyGridPanel);
			this.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Menu = this.mainMenu1;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.Text = "Terrarium Metal Style Editor";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.SystemColorsChanged += new System.EventHandler(this.MainForm_SystemColorsChanged);
			this.propertyGridPanel.ResumeLayout(false);
			this.previewPanel.ResumeLayout(false);
			this.middlePanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			this.Invalidate(true);
		}

		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			this.dirty = true;
			this.titleBar.Title = MetalStyleManager.Active.Name;
			this.Invalidate(true);
		}

		private void openFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
		
		}

		private bool SaveStyle( bool saveAs )
		{
			if ( saveAs == true || this.fileName == null || this.fileName.Length == 0 )
			{
				DialogResult result = this.saveFileDialog.ShowDialog();
				if ( result == DialogResult.Cancel )
					return false;
				else
					this.fileName = this.saveFileDialog.FileName;
			}
			if ( this.fileName == null || this.fileName.Length == 0 )
				return false;

			XmlSerializer serializer = new XmlSerializer( typeof(MetalStyle) );

			Stream stream = File.Open( this.fileName, FileMode.Create, FileAccess.Write, FileShare.Read );
			serializer.Serialize( stream, MetalStyleManager.Active );
			stream.Flush();
			stream.Close();

			this.dirty = false;

			return true;
		}

		private void fileOpenMenu_Click(object sender, System.EventArgs e)
		{
			if ( this.dirty == true )
			{
				DialogResult result = MessageBox.Show( "Would you like to save your existing style?", "Save Style", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
				if ( result == DialogResult.Cancel )
					return;
				if ( result == DialogResult.Yes )
				{
					SaveStyle( false );
				}
			}

			this.openFileDialog.FileName = "";

			if ( DialogResult.Cancel != this.openFileDialog.ShowDialog() )
			{
				this.fileName = this.openFileDialog.FileName;
			}

			this.RefreshPropertyGrid();

		}

		private void RefreshPropertyGrid()
		{
			Terrarium.Metal.MetalStyleManager.Active = Terrarium.Metal.MetalStyleManager.LoadStyle( this.fileName );
			this.propertyGrid1.SelectedObject = Terrarium.Metal.MetalStyleManager.Active;
			//this.propertyGrid1.ExpandAllGridItems();

			this.titleBar.Title = Terrarium.Metal.MetalStyleManager.Active.Name;

			this.Invalidate(true);
		}

		private void fileSaveAsMenu_Click(object sender, System.EventArgs e)
		{
			this.SaveStyle( true );
		}

		private void fileNewMenu_Click(object sender, System.EventArgs e)
		{
			if ( this.dirty == true )
			{
				DialogResult result = MessageBox.Show( "Would you like to save your existing style?", "Save Style", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
				if ( result == DialogResult.Cancel )
					return;
				if ( result == DialogResult.Yes )
				{
					SaveStyle( false );
				}
			}

			MetalStyleManager.Active = new MetalStyle();
			this.fileName = "";
			this.RefreshPropertyGrid();

		}

		private void fileSaveMenu_Click(object sender, System.EventArgs e)
		{
			this.SaveStyle( false );
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
		
		}

		private void propertyGrid1_Click(object sender, System.EventArgs e)
		{
		
		}

		private void MainForm_SystemColorsChanged(object sender, System.EventArgs e)
		{
			MetalStyleManager.RefreshSystemStyle();
			this.Invalidate(true);
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			string version = typeof(Terrarium.Forms.TitleBar).Assembly.GetName().Version.ToString();
			MessageBox.Show( "Terrarium Metal Style Editor\r\nVersion " + version + ".", "About Style Editor...", MessageBoxButtons.OK, MessageBoxIcon.Information );
		}

	}
}
