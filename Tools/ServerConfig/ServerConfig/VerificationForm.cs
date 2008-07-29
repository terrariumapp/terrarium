//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace ServerConfig
{
	/// <summary>
	/// Summary description for ProgressForm.
	/// </summary>
	public class VerificationForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.LinkLabel serverUrlLink;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Panel messagePanel;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public VerificationForm(ArrayList messageList)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.pictureBox1.Image = SystemIcons.Information.ToBitmap();

			if (messageList.Count > 0)
			{
				foreach(string message in messageList)
				{
					this.listBox1.Items.Add(message);
				}

				this.messagePanel.Visible = true;
				this.Height = 344;
			}
			else
			{
				this.messagePanel.Visible = false;
				this.Height = 184;
			}
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated (e);
			this.Left = this.Owner.Left + (this.Owner.Width/2-this.Width/2);
			this.Top = this.Owner.Top + (this.Owner.Height/2-this.Height/2);

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(VerificationForm));
			this.serverUrlLink = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.closeButton = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.messagePanel = new System.Windows.Forms.Panel();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.messagePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// serverUrlLink
			// 
			this.serverUrlLink.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
			this.serverUrlLink.Location = new System.Drawing.Point(16, 80);
			this.serverUrlLink.Name = "serverUrlLink";
			this.serverUrlLink.Size = new System.Drawing.Size(328, 16);
			this.serverUrlLink.TabIndex = 0;
			this.serverUrlLink.TabStop = true;
			this.serverUrlLink.Text = "http://localhost/TerrariumTest/default.aspx";
			this.serverUrlLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.serverUrlLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.serverUrlLink_LinkClicked);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(56, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(296, 32);
			this.label1.TabIndex = 1;
			this.label1.Text = "Your Terrarium server is now installed.  To verify that it is running properly, c" +
				"lick the following link.";
			// 
			// closeButton
			// 
			this.closeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.closeButton.Location = new System.Drawing.Point(140, 280);
			this.closeButton.Name = "closeButton";
			this.closeButton.TabIndex = 11;
			this.closeButton.Text = "Close";
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Location = new System.Drawing.Point(16, 32);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.TabIndex = 12;
			this.pictureBox1.TabStop = false;
			// 
			// messagePanel
			// 
			this.messagePanel.Controls.Add(this.listBox1);
			this.messagePanel.Controls.Add(this.label2);
			this.messagePanel.Location = new System.Drawing.Point(8, 112);
			this.messagePanel.Name = "messagePanel";
			this.messagePanel.Size = new System.Drawing.Size(344, 152);
			this.messagePanel.TabIndex = 15;
			// 
			// listBox1
			// 
			this.listBox1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.listBox1.HorizontalScrollbar = true;
			this.listBox1.ItemHeight = 15;
			this.listBox1.Location = new System.Drawing.Point(8, 28);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(328, 109);
			this.listBox1.TabIndex = 16;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(296, 12);
			this.label2.TabIndex = 15;
			this.label2.Text = "There were some errors installing the server.";
			// 
			// VerificationForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(362, 320);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.messagePanel);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.serverUrlLink);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "VerificationForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Installation Complete";
			this.Load += new System.EventHandler(this.ProgressForm_Load);
			this.messagePanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void ProgressForm_Load(object sender, System.EventArgs e)
		{
		
		}

		private void closeButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
			Application.Exit();
		}

		private void serverUrlLink_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(this.serverUrlLink.Text);
		}

		public string ServerUrl
		{
			get
			{
				return this.serverUrlLink.Text;
			}
			set
			{
				this.serverUrlLink.Text = value;
			}
		}
	}
}
