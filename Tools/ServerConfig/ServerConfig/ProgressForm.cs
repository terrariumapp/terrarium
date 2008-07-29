//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace ServerConfig
{
	/// <summary>
	/// Summary description for ProgressForm.
	/// </summary>
	public class ProgressForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label currentStepLabel;
		private System.Windows.Forms.Label label1;

		public ProgressForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
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
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.label1 = new System.Windows.Forms.Label();
			this.currentStepLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(16, 80);
			this.progressBar1.Maximum = 5;
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(296, 16);
			this.progressBar1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(296, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Terrarium Server is now installing...";
			// 
			// currentStepLabel
			// 
			this.currentStepLabel.Location = new System.Drawing.Point(24, 48);
			this.currentStepLabel.Name = "currentStepLabel";
			this.currentStepLabel.Size = new System.Drawing.Size(288, 16);
			this.currentStepLabel.TabIndex = 2;
			this.currentStepLabel.Text = "label2";
			// 
			// ProgressForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(328, 110);
			this.ControlBox = false;
			this.Controls.Add(this.currentStepLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.progressBar1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "ProgressForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Load += new System.EventHandler(this.ProgressForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated (e);
			this.Left = this.Owner.Left + (this.Owner.Width/2-this.Width/2);
			this.Top = this.Owner.Top + (this.Owner.Height/2-this.Height/2);

		}

		public string CurrentStepDescription
		{
			get
			{
				return this.currentStepLabel.Text;
			}
			set
			{
				this.currentStepLabel.Text = value;
			}
		}

		public int ProgressValue
		{
			get
			{
				return this.progressBar1.Value;
			}
			set
			{
				this.progressBar1.Value = value;
			}
		}

		private void ProgressForm_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
