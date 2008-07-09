//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Windows.Forms;

using Terrarium.Game;
using Terrarium.Forms;

namespace Terrarium.Client 
{
    /// <summary>
    ///  Reports local terrarium statistics to the user.
    /// </summary>
    public class ReportStats : Terrarium.Forms.TerrariumForm	
    {
    #region Designer Generated Fields

        private System.Windows.Forms.Timer timerAutoRefresh;
		private Terrarium.Glass.GlassButton closeButton;
		private Terrarium.Glass.GlassButton refreshButton;
        private System.Windows.Forms.DataGrid dataGrid1;

		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn3;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn4;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn5;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn6;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn7;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn8;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn9;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn10;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn11;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn12;
        private ResizeBar resizeBar1;

        private System.ComponentModel.IContainer components;
    #endregion
    #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerAutoRefresh = new System.Windows.Forms.Timer(this.components);
            this.closeButton = new Terrarium.Glass.GlassButton();
            this.refreshButton = new Terrarium.Glass.GlassButton();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn4 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn5 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn10 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn6 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn9 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn7 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn8 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn12 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn11 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.resizeBar1 = new Terrarium.Forms.ResizeBar();
            this.bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // titleBar
            // 
            this.titleBar.Size = new System.Drawing.Size(434, 32);
            this.titleBar.Title = "Local Statistics";
            this.titleBar.CloseClicked += new System.EventHandler(this.CloseForm_Click);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.resizeBar1);
            this.bottomPanel.Controls.Add(this.closeButton);
            this.bottomPanel.Controls.Add(this.refreshButton);
            this.bottomPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bottomPanel.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.bottomPanel.Location = new System.Drawing.Point(0, 328);
            this.bottomPanel.Size = new System.Drawing.Size(434, 40);
            // 
            // timerAutoRefresh
            // 
            this.timerAutoRefresh.Enabled = true;
            this.timerAutoRefresh.Interval = 10000;
            this.timerAutoRefresh.Tick += new System.EventHandler(this.timerAutoRefresh_Tick);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.BorderColor = System.Drawing.Color.Black;
            this.closeButton.Depth = 4;
            this.closeButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.closeButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.closeButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.ForeColor = System.Drawing.Color.White;
            this.closeButton.Highlight = false;
            this.closeButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.closeButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.closeButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.closeButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.closeButton.IsGlass = true;
            this.closeButton.Location = new System.Drawing.Point(338, 2);
            this.closeButton.Name = "closeButton";
            this.closeButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.closeButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.closeButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.closeButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.closeButton.Size = new System.Drawing.Size(75, 36);
            this.closeButton.TabIndex = 11;
            this.closeButton.TabStop = false;
            this.closeButton.Text = "Close";
            this.closeButton.UseStyles = true;
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.CloseForm_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshButton.BackColor = System.Drawing.Color.Transparent;
            this.refreshButton.BorderColor = System.Drawing.Color.Black;
            this.refreshButton.Depth = 4;
            this.refreshButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.refreshButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.refreshButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshButton.ForeColor = System.Drawing.Color.White;
            this.refreshButton.Highlight = false;
            this.refreshButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.refreshButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.refreshButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.refreshButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.refreshButton.IsGlass = true;
            this.refreshButton.Location = new System.Drawing.Point(257, 2);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.refreshButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.refreshButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.refreshButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.refreshButton.Size = new System.Drawing.Size(75, 36);
            this.refreshButton.TabIndex = 10;
            this.refreshButton.TabStop = false;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseStyles = true;
            this.refreshButton.UseVisualStyleBackColor = false;
            this.refreshButton.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // dataGrid1
            // 
            this.dataGrid1.AlternatingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid1.BackgroundColor = System.Drawing.Color.Gray;
            this.dataGrid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dataGrid1.CaptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dataGrid1.CaptionForeColor = System.Drawing.Color.White;
            this.dataGrid1.CaptionVisible = false;
            this.dataGrid1.DataMember = "";
            this.dataGrid1.FlatMode = true;
            this.dataGrid1.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGrid1.ForeColor = System.Drawing.Color.Black;
            this.dataGrid1.GridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGrid1.HeaderBackColor = System.Drawing.Color.Gray;
            this.dataGrid1.HeaderForeColor = System.Drawing.Color.White;
            this.dataGrid1.Location = new System.Drawing.Point(12, 81);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGrid1.Size = new System.Drawing.Size(410, 232);
            this.dataGrid1.TabIndex = 8;
            this.dataGrid1.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.AlternatingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
            this.dataGridTableStyle1.BackColor = System.Drawing.Color.White;
            this.dataGridTableStyle1.DataGrid = this.dataGrid1;
            this.dataGridTableStyle1.ForeColor = System.Drawing.Color.Black;
            this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dataGridTextBoxColumn2,
            this.dataGridTextBoxColumn3,
            this.dataGridTextBoxColumn4,
            this.dataGridTextBoxColumn5,
            this.dataGridTextBoxColumn10,
            this.dataGridTextBoxColumn6,
            this.dataGridTextBoxColumn9,
            this.dataGridTextBoxColumn7,
            this.dataGridTextBoxColumn8,
            this.dataGridTextBoxColumn1,
            this.dataGridTextBoxColumn12,
            this.dataGridTextBoxColumn11});
            this.dataGridTableStyle1.HeaderBackColor = System.Drawing.Color.Gray;
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.Color.White;
            this.dataGridTableStyle1.MappingName = "History";
            // 
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            this.dataGridTextBoxColumn2.HeaderText = "Species";
            this.dataGridTextBoxColumn2.MappingName = "SpeciesName";
            this.dataGridTextBoxColumn2.ReadOnly = true;
            this.dataGridTextBoxColumn2.Width = 125;
            // 
            // dataGridTextBoxColumn3
            // 
            this.dataGridTextBoxColumn3.Format = "d";
            this.dataGridTextBoxColumn3.FormatInfo = null;
            this.dataGridTextBoxColumn3.HeaderText = "Population";
            this.dataGridTextBoxColumn3.MappingName = "Population";
            this.dataGridTextBoxColumn3.ReadOnly = true;
            this.dataGridTextBoxColumn3.Width = 75;
            // 
            // dataGridTextBoxColumn4
            // 
            this.dataGridTextBoxColumn4.Format = "d";
            this.dataGridTextBoxColumn4.FormatInfo = null;
            this.dataGridTextBoxColumn4.HeaderText = "Births";
            this.dataGridTextBoxColumn4.MappingName = "BirthCount";
            this.dataGridTextBoxColumn4.ReadOnly = true;
            this.dataGridTextBoxColumn4.Width = 50;
            // 
            // dataGridTextBoxColumn5
            // 
            this.dataGridTextBoxColumn5.Format = "d";
            this.dataGridTextBoxColumn5.FormatInfo = null;
            this.dataGridTextBoxColumn5.HeaderText = "Starved";
            this.dataGridTextBoxColumn5.MappingName = "StarvedCount";
            this.dataGridTextBoxColumn5.ReadOnly = true;
            this.dataGridTextBoxColumn5.Width = 75;
            // 
            // dataGridTextBoxColumn10
            // 
            this.dataGridTextBoxColumn10.Format = "d";
            this.dataGridTextBoxColumn10.FormatInfo = null;
            this.dataGridTextBoxColumn10.HeaderText = "Old Age";
            this.dataGridTextBoxColumn10.MappingName = "OldAgeCount";
            this.dataGridTextBoxColumn10.ReadOnly = true;
            this.dataGridTextBoxColumn10.Width = 75;
            // 
            // dataGridTextBoxColumn6
            // 
            this.dataGridTextBoxColumn6.Format = "d";
            this.dataGridTextBoxColumn6.FormatInfo = null;
            this.dataGridTextBoxColumn6.HeaderText = "Killed";
            this.dataGridTextBoxColumn6.MappingName = "KilledCount";
            this.dataGridTextBoxColumn6.ReadOnly = true;
            this.dataGridTextBoxColumn6.Width = 50;
            // 
            // dataGridTextBoxColumn9
            // 
            this.dataGridTextBoxColumn9.Format = "d";
            this.dataGridTextBoxColumn9.FormatInfo = null;
            this.dataGridTextBoxColumn9.HeaderText = "Sick";
            this.dataGridTextBoxColumn9.MappingName = "SickCount";
            this.dataGridTextBoxColumn9.ReadOnly = true;
            this.dataGridTextBoxColumn9.Width = 50;
            // 
            // dataGridTextBoxColumn7
            // 
            this.dataGridTextBoxColumn7.Format = "d";
            this.dataGridTextBoxColumn7.FormatInfo = null;
            this.dataGridTextBoxColumn7.HeaderText = "Errors";
            this.dataGridTextBoxColumn7.MappingName = "ErrorCount";
            this.dataGridTextBoxColumn7.ReadOnly = true;
            this.dataGridTextBoxColumn7.Width = 50;
            // 
            // dataGridTextBoxColumn8
            // 
            this.dataGridTextBoxColumn8.Format = "d";
            this.dataGridTextBoxColumn8.FormatInfo = null;
            this.dataGridTextBoxColumn8.HeaderText = "Timeouts";
            this.dataGridTextBoxColumn8.MappingName = "TimeoutCount";
            this.dataGridTextBoxColumn8.ReadOnly = true;
            this.dataGridTextBoxColumn8.Width = 50;
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "d";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "Security Violations";
            this.dataGridTextBoxColumn1.MappingName = "SecurityViolationCount";
            this.dataGridTextBoxColumn1.ReadOnly = true;
            this.dataGridTextBoxColumn1.Width = 75;
            // 
            // dataGridTextBoxColumn12
            // 
            this.dataGridTextBoxColumn12.Format = "d";
            this.dataGridTextBoxColumn12.FormatInfo = null;
            this.dataGridTextBoxColumn12.HeaderText = "Teleported Here";
            this.dataGridTextBoxColumn12.MappingName = "TeleportedToCount";
            this.dataGridTextBoxColumn12.ReadOnly = true;
            this.dataGridTextBoxColumn12.Width = 75;
            // 
            // dataGridTextBoxColumn11
            // 
            this.dataGridTextBoxColumn11.Format = "d";
            this.dataGridTextBoxColumn11.FormatInfo = null;
            this.dataGridTextBoxColumn11.HeaderText = "Teleported Away";
            this.dataGridTextBoxColumn11.MappingName = "TeleportedFromCount";
            this.dataGridTextBoxColumn11.ReadOnly = true;
            this.dataGridTextBoxColumn11.Width = 75;
            // 
            // resizeBar1
            // 
            this.resizeBar1.BackColor = System.Drawing.Color.Black;
            this.resizeBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.resizeBar1.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resizeBar1.ForeColor = System.Drawing.Color.White;
            this.resizeBar1.Location = new System.Drawing.Point(414, 0);
            this.resizeBar1.Name = "resizeBar1";
            this.resizeBar1.Size = new System.Drawing.Size(20, 40);
            this.resizeBar1.TabIndex = 12;
            // 
            // ReportStats
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(434, 368);
            this.Controls.Add(this.dataGrid1);
            this.Description = "Every statistic except population is only counted from the start of this terrariu" +
                "m session.  Population carries over between sessions.";
            this.Name = "ReportStats";
            this.Title = "Population Statistics";
            this.Controls.SetChildIndex(this.dataGrid1, 0);
            this.Controls.SetChildIndex(this.titleBar, 0);
            this.Controls.SetChildIndex(this.bottomPanel, 0);
            this.bottomPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.ResumeLayout(false);

		}
    #endregion

        /// <summary>
        ///  Creates a new ReportStats form and enables designer support.
        /// </summary>
        public ReportStats()
        {
            InitializeComponent();

            this.bottomPanel.Controls.Remove(this.resizeBar1);
            this.bottomPanel.Controls.Add(this.resizeBar1);

            // Initially refreshes form data.
            Refresh_Click(this.refreshButton, null);
			this.timerAutoRefresh.Enabled = true;

			this.titleBar.ShowMaximizeButton = false;
			this.titleBar.ShowMinimizeButton = false;
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

//        /// <summary>
//        ///  Enables refresh timers depending on the state of the check box.
//        /// </summary>
//        /// <param name="sender">CheckBox</param>
//        /// <param name="e">Null</param>
//        private void checkBoxAutoRefresh_Click(object sender, System.EventArgs e)
//        {
//            timerAutoRefresh.Enabled = checkBoxAutoRefresh.Checked;
//        }
    
        /// <summary>
        ///  Updates the data on the form whenever the timer ticks.
        /// </summary>
        /// <param name="sender">Timer</param>
        /// <param name="e">Null</param>
        private void timerAutoRefresh_Tick(object sender, System.EventArgs e)
        {
            Refresh_Click(this.refreshButton, null);
        }

        /// <summary>
        ///  Updates form data as a result of the user clicking the refresh button.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Null</param>
        private void Refresh_Click(object sender, System.EventArgs e)
        {
            if (GameEngine.Current != null)
            {
                DataSet data = GameEngine.Current.PopulationData.GetCurrentReportingStats(GameEngine.Current.CurrentVector.State.StateGuid, GameEngine.Current.CurrentVector.State.TickNumber);
                dataGrid1.DataSource = data;
                dataGrid1.DataMember = "History";
                this.Invalidate();
            }
        }

        /// <summary>
        ///  Closes the form as a result of the close button.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Null</param>
        private void CloseForm_Click(object sender, System.EventArgs e)
        {
            this.Hide();
        }

    }
}