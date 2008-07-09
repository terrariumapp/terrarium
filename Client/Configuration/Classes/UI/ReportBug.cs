//------------------------------------------------------------------------------ 
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                           
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Windows.Forms;

using Terrarium.Configuration;
using Terrarium.Tools;

using Terrarium.Services.Watson;

namespace Terrarium.Forms
{
    /// <summary>
    ///  A dialog used to report a user submitted bug.  This is similar
    ///  to the Watson dialog, but doesn't contain exception information.
    /// </summary>
    public class ReportBug : System.Windows.Forms.Form
    {
        #region Designer Generated Fields
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label EmailLabel;
        private System.Windows.Forms.Button Send;
        private System.Windows.Forms.Button DontSend;
        private System.Windows.Forms.TextBox Information;
        private System.Windows.Forms.TextBox Email;
        #endregion

        /// <summary>
        ///  The dataset holding the error information to send to the Watson
        ///  Service.
        /// </summary>
        private DataSet errorInformation;

        /// <summary>
        ///  If the text changes then a click on the Send button will submit
        ///  the text.  Otherwise it won't
        /// </summary>
        private bool textChanged = false;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        ///  Constructor used to create a new User bug reporting form.
        /// </summary>
        public ReportBug()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

    #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.Send = new System.Windows.Forms.Button();
			this.DontSend = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.Information = new System.Windows.Forms.TextBox();
			this.Email = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.EmailLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// Send
			// 
			this.Send.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Send.Location = new System.Drawing.Point(112, 352);
			this.Send.Name = "Send";
			this.Send.Size = new System.Drawing.Size(75, 32);
			this.Send.TabIndex = 0;
			this.Send.Text = "Send";
			this.Send.Click += new System.EventHandler(this.Send_Click);
			// 
			// DontSend
			// 
			this.DontSend.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.DontSend.Location = new System.Drawing.Point(208, 352);
			this.DontSend.Name = "DontSend";
			this.DontSend.Size = new System.Drawing.Size(75, 32);
			this.DontSend.TabIndex = 1;
			this.DontSend.Text = "Don\'t Send";
			this.DontSend.Click += new System.EventHandler(this.DontSend_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(24, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(376, 32);
			this.label1.TabIndex = 2;
			this.label1.Text = "The version of the game you\'re running, and the operating system you\'re running w" +
				"ill automatically be included.";
			// 
			// Information
			// 
			this.Information.Location = new System.Drawing.Point(24, 128);
			this.Information.Multiline = true;
			this.Information.Name = "Information";
			this.Information.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.Information.Size = new System.Drawing.Size(368, 216);
			this.Information.TabIndex = 6;
			this.Information.Text = "[Enter steps to reproduce the problem]\r\n\r\n[Enter Expected Behavior Here]\r\n\r\n[Ente" +
				"r actual behavior here]";
			this.Information.TextChanged += new System.EventHandler(this.Information_TextChanged);
			// 
			// Email
			// 
			this.Email.Location = new System.Drawing.Point(24, 80);
			this.Email.Name = "Email";
			this.Email.Size = new System.Drawing.Size(376, 20);
			this.Email.TabIndex = 4;
			this.Email.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 112);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(344, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Enter the suggestion or bug description here:";
			// 
			// EmailLabel
			// 
			this.EmailLabel.Location = new System.Drawing.Point(24, 64);
			this.EmailLabel.Name = "EmailLabel";
			this.EmailLabel.Size = new System.Drawing.Size(376, 16);
			this.EmailLabel.TabIndex = 3;
			this.EmailLabel.Text = "Email (not required):";
			// 
			// ReportBug
			// 
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(5, 13);
            // this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(408, 390);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.EmailLabel);
			this.Controls.Add(this.Email);
			this.Controls.Add(this.Information);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.DontSend);
			this.Controls.Add(this.Send);
			this.Name = "ReportBug";
			this.Text = "Report a Bug or Suggestion";
			this.Load += new System.EventHandler(this.Form_Load);
			this.ResumeLayout(false);

		}
    #endregion

        /// <summary>
        ///  Send event handler.  This method sends the user bug dataset
        ///  to the Watson Web Service.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Null</param>
        private void Send_Click(object sender, System.EventArgs e)
        {
            if (!textChanged)
            {
                return;
            }
    
            try 
            {
                using (WatsonService service = new WatsonService())
                {
                    errorInformation.Tables["Watson"].Rows[0]["UserComment"] = this.Information.Text;
                    errorInformation.Tables["Watson"].Rows[0]["UserEmail"] = this.Email.Text;
                    service.Url = GameConfig.WebRoot + "/watson/watson.asmx";
                    service.Timeout = 20000;
                    service.ReportError(errorInformation);
                }

                MessageBox.Show(this, "Thank you! Your bug/suggestion has been sent to the Terrarium development team successfully.", "Bug Sent");
                this.Close();
            }
            catch (Exception exception)
            {
                // Catch all exceptions because we don't want the user to get into an infinite loop
                // and if the website is down, we'll just throw away the data
                ErrorLog.LogHandledException(exception);
                MessageBox.Show(this, "Sorry! There was a problem sending your bug: " + exception.Message, "Problem sending bug.");
            }
        }

        /// <summary>
        ///  Create a default dataset on form load initialized to type "UserBug"
        ///  with no previous tracings.
        /// </summary>
        /// <param name="sender">Form</param>
        /// <param name="e">Null</param>
        private void Form_Load(object sender, System.EventArgs e)
        {
            errorInformation = ErrorLog.CreateErrorLogDataSet("userbug", "");
        }

        /// <summary>
        ///  DontSend event handler used to close the form down without sending
        ///  anything to the server.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Null</param>
        private void DontSend_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        ///  Information event handler used to determine if the text is changed.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">Null</param>
        private void Information_TextChanged(object sender, System.EventArgs e)
        {
            textChanged = true;
        }
    }
}