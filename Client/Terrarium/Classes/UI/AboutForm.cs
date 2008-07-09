//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using System.Drawing;
using Terrarium.Glass;

namespace Terrarium.Client
{
	/// <summary>
	/// 
	/// </summary>
	public class AboutForm : System.Windows.Forms.Form
	{
		private Label lastUpdateLabel;
		private Label updatesEnabledLabel;
        private Label clientVersionLabel;
        private GlassLabel glassLabel4;
        private GlassButton closeButton;
        private GlassLabel GlassLabel3;
        private GlassLabel GlassLabel2;
        private GlassLabel GlassLabel1;
        private Label nextUpdateLabel;
		/// <summary>
		/// Default constructor
		/// </summary>
		public AboutForm()
		{
			InitializeComponent();

			this.clientVersionLabel.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

			this.BackColor = GlassStyleManager.Active.DialogColor;
		}

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.lastUpdateLabel = new System.Windows.Forms.Label();
            this.updatesEnabledLabel = new System.Windows.Forms.Label();
            this.clientVersionLabel = new System.Windows.Forms.Label();
            this.nextUpdateLabel = new System.Windows.Forms.Label();
            this.glassLabel4 = new Terrarium.Glass.GlassLabel();
            this.closeButton = new Terrarium.Glass.GlassButton();
            this.GlassLabel3 = new Terrarium.Glass.GlassLabel();
            this.GlassLabel2 = new Terrarium.Glass.GlassLabel();
            this.GlassLabel1 = new Terrarium.Glass.GlassLabel();
            this.SuspendLayout();
            // 
            // lastUpdateLabel
            // 
            this.lastUpdateLabel.BackColor = System.Drawing.Color.Transparent;
            this.lastUpdateLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastUpdateLabel.ForeColor = System.Drawing.Color.White;
            this.lastUpdateLabel.Location = new System.Drawing.Point(137, 224);
            this.lastUpdateLabel.Name = "lastUpdateLabel";
            this.lastUpdateLabel.Size = new System.Drawing.Size(233, 16);
            this.lastUpdateLabel.TabIndex = 13;
            this.lastUpdateLabel.Text = "label1";
            this.lastUpdateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // updatesEnabledLabel
            // 
            this.updatesEnabledLabel.BackColor = System.Drawing.Color.Transparent;
            this.updatesEnabledLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updatesEnabledLabel.ForeColor = System.Drawing.Color.White;
            this.updatesEnabledLabel.Location = new System.Drawing.Point(137, 208);
            this.updatesEnabledLabel.Name = "updatesEnabledLabel";
            this.updatesEnabledLabel.Size = new System.Drawing.Size(128, 16);
            this.updatesEnabledLabel.TabIndex = 12;
            this.updatesEnabledLabel.Text = "label1";
            this.updatesEnabledLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // clientVersionLabel
            // 
            this.clientVersionLabel.BackColor = System.Drawing.Color.Transparent;
            this.clientVersionLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clientVersionLabel.ForeColor = System.Drawing.Color.White;
            this.clientVersionLabel.Location = new System.Drawing.Point(137, 192);
            this.clientVersionLabel.Name = "clientVersionLabel";
            this.clientVersionLabel.Size = new System.Drawing.Size(120, 16);
            this.clientVersionLabel.TabIndex = 11;
            this.clientVersionLabel.Text = "label1";
            this.clientVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nextUpdateLabel
            // 
            this.nextUpdateLabel.BackColor = System.Drawing.Color.Transparent;
            this.nextUpdateLabel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextUpdateLabel.ForeColor = System.Drawing.Color.White;
            this.nextUpdateLabel.Location = new System.Drawing.Point(137, 240);
            this.nextUpdateLabel.Name = "nextUpdateLabel";
            this.nextUpdateLabel.Size = new System.Drawing.Size(233, 16);
            this.nextUpdateLabel.TabIndex = 16;
            this.nextUpdateLabel.Text = "label1";
            this.nextUpdateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // glassLabel4
            // 
            this.glassLabel4.BackColor = System.Drawing.Color.Transparent;
            this.glassLabel4.Font = new System.Drawing.Font("Verdana", 6.75F);
            this.glassLabel4.ForeColor = System.Drawing.Color.White;
            this.glassLabel4.Location = new System.Drawing.Point(12, 240);
            this.glassLabel4.Name = "glassLabel4";
            this.glassLabel4.NoWrap = false;
            this.glassLabel4.Size = new System.Drawing.Size(121, 16);
            this.glassLabel4.TabIndex = 15;
            this.glassLabel4.Text = "Next Update Check:";
            this.glassLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.BorderColor = System.Drawing.Color.Black;
            this.closeButton.Depth = 4;
            this.closeButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.closeButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.closeButton.Highlight = false;
            this.closeButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.closeButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.closeButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(96)))), ((int)(((byte)(0)))));
            this.closeButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.closeButton.IsGlass = true;
            this.closeButton.Location = new System.Drawing.Point(393, 224);
            this.closeButton.Name = "closeButton";
            this.closeButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.closeButton.NormalGradient.Top = System.Drawing.Color.Silver;
            this.closeButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.closeButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.closeButton.Size = new System.Drawing.Size(75, 36);
            this.closeButton.TabIndex = 14;
            this.closeButton.TabStop = false;
            this.closeButton.Text = "Close";
            this.closeButton.UseStyles = false;
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // GlassLabel3
            // 
            this.GlassLabel3.BackColor = System.Drawing.Color.Transparent;
            this.GlassLabel3.Font = new System.Drawing.Font("Verdana", 6.75F);
            this.GlassLabel3.ForeColor = System.Drawing.Color.White;
            this.GlassLabel3.Location = new System.Drawing.Point(12, 224);
            this.GlassLabel3.Name = "GlassLabel3";
            this.GlassLabel3.NoWrap = false;
            this.GlassLabel3.Size = new System.Drawing.Size(121, 16);
            this.GlassLabel3.TabIndex = 10;
            this.GlassLabel3.Text = "Last Update Check:";
            this.GlassLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GlassLabel2
            // 
            this.GlassLabel2.BackColor = System.Drawing.Color.Transparent;
            this.GlassLabel2.Font = new System.Drawing.Font("Verdana", 6.75F);
            this.GlassLabel2.ForeColor = System.Drawing.Color.White;
            this.GlassLabel2.Location = new System.Drawing.Point(12, 208);
            this.GlassLabel2.Name = "GlassLabel2";
            this.GlassLabel2.NoWrap = false;
            this.GlassLabel2.Size = new System.Drawing.Size(121, 16);
            this.GlassLabel2.TabIndex = 9;
            this.GlassLabel2.Text = "Auto Update:";
            this.GlassLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GlassLabel1
            // 
            this.GlassLabel1.BackColor = System.Drawing.Color.Transparent;
            this.GlassLabel1.Font = new System.Drawing.Font("Verdana", 6.75F);
            this.GlassLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.GlassLabel1.Location = new System.Drawing.Point(12, 192);
            this.GlassLabel1.Name = "GlassLabel1";
            this.GlassLabel1.NoWrap = false;
            this.GlassLabel1.Size = new System.Drawing.Size(121, 16);
            this.GlassLabel1.TabIndex = 8;
            this.GlassLabel1.Text = "Client Version:";
            this.GlassLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AboutForm
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(480, 272);
            this.Controls.Add(this.nextUpdateLabel);
            this.Controls.Add(this.glassLabel4);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.lastUpdateLabel);
            this.Controls.Add(this.updatesEnabledLabel);
            this.Controls.Add(this.clientVersionLabel);
            this.Controls.Add(this.GlassLabel3);
            this.Controls.Add(this.GlassLabel2);
            this.Controls.Add(this.GlassLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "~";
            this.ResumeLayout(false);

        }

        private void closeButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}