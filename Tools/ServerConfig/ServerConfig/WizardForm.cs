//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace ServerConfig
{
	/// <summary>
	/// Summary description for WizardForm.
	/// </summary>
	public class WizardForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox terrariumPasswordTextBox;
		private System.Windows.Forms.TextBox databasePasswordTextBox;
		private System.Windows.Forms.TextBox databaseUsernameTextBox;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox databaseNameTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox databaseServerTextBox;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TextBox webPathText;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.TextBox webNameText;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.CheckBox useIntegratedSecurityCheckBox;
		private System.Windows.Forms.Panel headerPanel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// If true, we don't prompt on close
		/// </summary>
		private bool installed = false;

		public WizardForm()
		{

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Let's assume that they are running this from the default location and try to set
			// some defaults

			DirectoryInfo parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
			string fullPath = Path.Combine(parentDirectory.FullName,"Website");
			if (Directory.Exists(fullPath))
				this.webPathText.Text = fullPath;

			this.useIntegratedSecurityCheckBox.Checked = true;

			this.databaseServerTextBox.Text = Environment.MachineName;

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WizardForm));
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.useIntegratedSecurityCheckBox = new System.Windows.Forms.CheckBox();
			this.terrariumPasswordTextBox = new System.Windows.Forms.TextBox();
			this.databasePasswordTextBox = new System.Windows.Forms.TextBox();
			this.databaseUsernameTextBox = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.databaseNameTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.databaseServerTextBox = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.browseButton = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.webNameText = new System.Windows.Forms.TextBox();
			this.webPathText = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.headerPanel = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.descriptionLabel = new System.Windows.Forms.Label();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.headerPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.useIntegratedSecurityCheckBox);
			this.groupBox2.Controls.Add(this.terrariumPasswordTextBox);
			this.groupBox2.Controls.Add(this.databasePasswordTextBox);
			this.groupBox2.Controls.Add(this.databaseUsernameTextBox);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.databaseNameTextBox);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.databaseServerTextBox);
			this.groupBox2.Location = new System.Drawing.Point(8, 88);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(488, 224);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Database Settings";
			// 
			// useIntegratedSecurityCheckBox
			// 
			this.useIntegratedSecurityCheckBox.Location = new System.Drawing.Point(136, 64);
			this.useIntegratedSecurityCheckBox.Name = "useIntegratedSecurityCheckBox";
			this.useIntegratedSecurityCheckBox.Size = new System.Drawing.Size(240, 24);
			this.useIntegratedSecurityCheckBox.TabIndex = 1;
			this.useIntegratedSecurityCheckBox.Text = "Use Integrated Security";
			this.useIntegratedSecurityCheckBox.CheckedChanged += new System.EventHandler(this.useIntegratedSecurityCheckBox_CheckedChanged);
			// 
			// terrariumPasswordTextBox
			// 
			this.terrariumPasswordTextBox.Location = new System.Drawing.Point(136, 184);
			this.terrariumPasswordTextBox.Name = "terrariumPasswordTextBox";
			this.terrariumPasswordTextBox.PasswordChar = '*';
			this.terrariumPasswordTextBox.Size = new System.Drawing.Size(336, 21);
			this.terrariumPasswordTextBox.TabIndex = 5;
			this.terrariumPasswordTextBox.Text = "";
			this.terrariumPasswordTextBox.TextChanged += new System.EventHandler(this.terrariumTextBox_TextChanged);
			this.terrariumPasswordTextBox.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// databasePasswordTextBox
			// 
			this.databasePasswordTextBox.Location = new System.Drawing.Point(136, 120);
			this.databasePasswordTextBox.Name = "databasePasswordTextBox";
			this.databasePasswordTextBox.PasswordChar = '*';
			this.databasePasswordTextBox.Size = new System.Drawing.Size(336, 21);
			this.databasePasswordTextBox.TabIndex = 3;
			this.databasePasswordTextBox.Text = "";
			this.databasePasswordTextBox.TextChanged += new System.EventHandler(this.terrariumTextBox_TextChanged);
			// 
			// databaseUsernameTextBox
			// 
			this.databaseUsernameTextBox.Location = new System.Drawing.Point(136, 88);
			this.databaseUsernameTextBox.Name = "databaseUsernameTextBox";
			this.databaseUsernameTextBox.Size = new System.Drawing.Size(336, 21);
			this.databaseUsernameTextBox.TabIndex = 2;
			this.databaseUsernameTextBox.Text = "";
			this.databaseUsernameTextBox.TextChanged += new System.EventHandler(this.terrariumTextBox_TextChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(16, 88);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(58, 17);
			this.label7.TabIndex = 16;
			this.label7.Text = "Username:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(16, 120);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(55, 17);
			this.label6.TabIndex = 15;
			this.label6.Text = "Password:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(16, 192);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(108, 17);
			this.label5.TabIndex = 14;
			this.label5.Text = "Terrarium Password:";
			// 
			// databaseNameTextBox
			// 
			this.databaseNameTextBox.Location = new System.Drawing.Point(136, 152);
			this.databaseNameTextBox.Name = "databaseNameTextBox";
			this.databaseNameTextBox.Size = new System.Drawing.Size(336, 21);
			this.databaseNameTextBox.TabIndex = 4;
			this.databaseNameTextBox.Text = "Terrarium";
			this.databaseNameTextBox.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 160);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(87, 17);
			this.label3.TabIndex = 12;
			this.label3.Text = "Database Name:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(16, 32);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 17);
			this.label4.TabIndex = 11;
			this.label4.Text = "Server:";
			// 
			// databaseServerTextBox
			// 
			this.databaseServerTextBox.Location = new System.Drawing.Point(136, 32);
			this.databaseServerTextBox.Name = "databaseServerTextBox";
			this.databaseServerTextBox.Size = new System.Drawing.Size(336, 21);
			this.databaseServerTextBox.TabIndex = 0;
			this.databaseServerTextBox.Text = "localhost";
			this.databaseServerTextBox.TextChanged += new System.EventHandler(this.terrariumTextBox_TextChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.browseButton);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Controls.Add(this.webNameText);
			this.groupBox3.Controls.Add(this.webPathText);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Location = new System.Drawing.Point(8, 336);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(488, 104);
			this.groupBox3.TabIndex = 8;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Web Settings";
			// 
			// browseButton
			// 
			this.browseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.browseButton.Location = new System.Drawing.Point(416, 64);
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size(56, 23);
			this.browseButton.TabIndex = 8;
			this.browseButton.Text = "Browse";
			this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(16, 32);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(63, 17);
			this.label8.TabIndex = 7;
			this.label8.Text = "Web Name:";
			// 
			// webNameText
			// 
			this.webNameText.Location = new System.Drawing.Point(136, 32);
			this.webNameText.Name = "webNameText";
			this.webNameText.Size = new System.Drawing.Size(336, 21);
			this.webNameText.TabIndex = 6;
			this.webNameText.Text = "Terrarium";
			this.webNameText.TextChanged += new System.EventHandler(this.terrariumTextBox_TextChanged);
			// 
			// webPathText
			// 
			this.webPathText.Location = new System.Drawing.Point(136, 64);
			this.webPathText.Name = "webPathText";
			this.webPathText.Size = new System.Drawing.Size(272, 21);
			this.webPathText.TabIndex = 7;
			this.webPathText.Text = "..\\Website";
			this.webPathText.TextChanged += new System.EventHandler(this.terrariumTextBox_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 17);
			this.label2.TabIndex = 6;
			this.label2.Text = "Web Path:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			// 
			// headerPanel
			// 
			this.headerPanel.BackColor = System.Drawing.Color.White;
			this.headerPanel.Controls.Add(this.pictureBox1);
			this.headerPanel.Controls.Add(this.descriptionLabel);
			this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.headerPanel.Location = new System.Drawing.Point(0, 0);
			this.headerPanel.Name = "headerPanel";
			this.headerPanel.Size = new System.Drawing.Size(504, 72);
			this.headerPanel.TabIndex = 9;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(16, 16);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			// 
			// descriptionLabel
			// 
			this.descriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.descriptionLabel.BackColor = System.Drawing.Color.Transparent;
			this.descriptionLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.descriptionLabel.Location = new System.Drawing.Point(64, 16);
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Size = new System.Drawing.Size(424, 48);
			this.descriptionLabel.TabIndex = 4;
			this.descriptionLabel.Text = "To create a Terrarium server, please fill in the fields below and click the \"OK\" " +
				"button.  This utility will then create the database and populate it with default" +
				" data, as well as create the virtual directory and ASP.Net application.";
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Location = new System.Drawing.Point(416, 448);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 10;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.okButton.Location = new System.Drawing.Point(328, 448);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 9;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// WizardForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(504, 478);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.headerPanel);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WizardForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Terrarium Server Configuration";
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.headerPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing (e);
			if (installed == false)
			{
				if (MessageBox.Show("Are you sure you wish to cancel?", "Terrarium Server Utility",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
				{
					e.Cancel = true;
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);

			Pen darkPen = new Pen(SystemColors.ControlDark, 1.0f);
			Pen lightPen = new Pen(SystemColors.ControlLight, 1.0f);

			e.Graphics.DrawLine(darkPen, 0, this.headerPanel.Height, this.Width,this.headerPanel.Height);
			e.Graphics.DrawLine(lightPen, 0, this.headerPanel.Height+1, this.Width,this.headerPanel.Height+1);

			lightPen.Dispose();
			darkPen.Dispose();
		}


		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void browseButton_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			dialog.Description = "Please select the location of the Terrarium server files.";
			dialog.SelectedPath = this.webPathText.Text;
			dialog.ShowNewFolderButton = false;
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				this.webPathText.Text = dialog.SelectedPath;
			}
		}

		private void textBox_Leave(object sender, System.EventArgs e)
		{
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			ArrayList messageList = new ArrayList();

			this.Enabled = false;
			this.Cursor = Cursors.WaitCursor;

			ProgressForm progressForm = new ProgressForm();
			progressForm.Owner = this;
			progressForm.Show();

			Application.DoEvents();

			string createConnectionString = null;

			if (this.useIntegratedSecurityCheckBox.Checked == true)
				createConnectionString = "Server=" + this.databaseServerTextBox.Text + ";Integrated Security=True;";
			else
				createConnectionString = "Server=" + this.databaseServerTextBox.Text + ";UID=" + this.databaseUsernameTextBox.Text + ";PWD=" + this.databasePasswordTextBox.Text + ";";
	
			progressForm.CurrentStepDescription = "Creating the database.";
			progressForm.ProgressValue = 1;
			Application.DoEvents();

			try
			{
				ServerSetupHelper.InstallDatabase(createConnectionString, this.databaseNameTextBox.Text,this.terrariumPasswordTextBox.Text);
			}
			catch( Exception ex)
			{
				messageList.Add(ex.Message);
			}

			string webConnectionString = "Server=" + this.databaseServerTextBox.Text + ";Database=" + this.databaseNameTextBox.Text + ";UID=TerrariumUser;PWD=" + this.terrariumPasswordTextBox.Text + ";";

			progressForm.CurrentStepDescription = "Preparing to create the website.";
			progressForm.ProgressValue = 2;
			Application.DoEvents();

			try
			{
				ServerSetupHelper.InstallWebConfig(this.webPathText.Text, webConnectionString);
			}
			catch(Exception ex)
			{
				messageList.Add(ex.Message);
			}

			progressForm.CurrentStepDescription = "Creating the website.";
			progressForm.ProgressValue = 3;
			Application.DoEvents();
			
			try
			{
				ServerSetupHelper.InstallWebRoot("localhost",this.webPathText.Text,this.webNameText.Text);
			}
			catch(Exception ex)
			{
				messageList.Add(ex.Message);
			}

			progressForm.CurrentStepDescription = "Setting up permissions.";
			progressForm.ProgressValue = 4;
			Application.DoEvents();

			try
			{
				ServerSetupHelper.InstallACLs(this.webPathText.Text);
			}
			catch(Exception ex)
			{
				messageList.Add(ex.Message);
			}

			Application.DoEvents();

			progressForm.CurrentStepDescription = "Installing performance counters and event logs.";
			progressForm.ProgressValue = 5;
			Application.DoEvents();

			try
			{
				//Install the performance counters and the event logs
				System.Configuration.Install.AssemblyInstaller installer = new System.Configuration.Install.AssemblyInstaller("InstallerItems.dll", null);
				System.Collections.Hashtable dictionary = new Hashtable();
				installer.Install(dictionary);
				installer.Commit(dictionary);
				installer.Dispose();
			}
			catch(Exception ex)
			{
				messageList.Add(ex.Message);
			}

			this.Cursor = Cursors.Default;

			progressForm.Close();
			progressForm.Dispose();

			installed = true;

			VerificationForm verification = new VerificationForm(messageList);
			verification.ServerUrl = "http://" + System.Environment.MachineName + "/" + this.webNameText.Text + "/default.aspx";
			verification.Owner = this;
			verification.ShowDialog();
			
			this.Close();
		}

		private void useIntegratedSecurityCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			this.databaseUsernameTextBox.Enabled = !this.useIntegratedSecurityCheckBox.Checked;
			this.databasePasswordTextBox.Enabled = !this.useIntegratedSecurityCheckBox.Checked;
		}

		private void terrariumTextBox_TextChanged(object sender, System.EventArgs e)
		{
			if (this.databaseServerTextBox.Text.Length > 0 &&
				this.databaseNameTextBox.Text.Length > 0 &&
				(this.useIntegratedSecurityCheckBox.Checked == true || this.databaseUsernameTextBox.Text.Length > 0 && this.databasePasswordTextBox.Text.Length > 0) &&
				this.webNameText.Text.Length > 0 &&
				this.webPathText.Text.Length > 0 &&
				this.terrariumPasswordTextBox.Text.Length > 0)
			{
				this.okButton.Enabled = true;
			}
			else
			{
				this.okButton.Enabled = false;
			}		
		}

		public string DatabaseServer
		{
			get
			{
				return this.databaseServerTextBox.Text;
			}
			set
			{
				this.databaseServerTextBox.Text = value;
			}
		}

		public string DatabaseUsername
		{
			get
			{
				return this.databaseUsernameTextBox.Text;
			}
			set
			{
				this.databaseUsernameTextBox.Text = value;
			}
		}

		public string DatabasePassword
		{
			get
			{
				return this.databasePasswordTextBox.Text;
			}
			set
			{
				this.databasePasswordTextBox.Text = value;
			}
		}

		public string DatabaseName
		{
			get
			{
				return this.databaseNameTextBox.Text;
			}
			set
			{
				this.databaseNameTextBox.Text = value;
			}
		}

		public string TerrariumPassword
		{
			get
			{
				return this.terrariumPasswordTextBox.Text;
			}
			set
			{
				this.terrariumPasswordTextBox.Text = value;
			}
		}

		public string WebName
		{
			get
			{
				return this.webNameText.Text;
			}
			set
			{
				this.webNameText.Text = value;
			}
		}

		public string WebPath
		{
			get
			{
				return this.webPathText.Text;
			}
			set
			{
				this.webPathText.Text = value;
			}
		}

		public bool UseIntegratedSecurity
		{
			get
			{
				return this.useIntegratedSecurityCheckBox.Checked;
			}
			set
			{
				this.useIntegratedSecurityCheckBox.Checked = value;
			}
		}
	}
}
