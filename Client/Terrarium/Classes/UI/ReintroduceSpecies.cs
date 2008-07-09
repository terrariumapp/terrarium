//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web.Services.Protocols;
using System.Windows.Forms;

using OrganismBase;

using Terrarium.Configuration;
using Terrarium.Game;

using Terrarium.Services.Species;
using Terrarium.Forms;

namespace Terrarium.Client 
{
    internal class ReintroduceSpecies : TerrariumForm
    {
        // For use in caching
        private static DataSet dataSet = null;
        private static DateTime cacheDate = DateTime.Now;
        
        private static string refreshExplanation = "To introduce creatures from the Server please download the Server List.  You can only download a new list once every 30 minutes.\n\n";
        
        private System.Windows.Forms.DataGrid dataGrid1;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn3;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn4;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn5;

        private System.ComponentModel.Container components = null;
        private bool reintroduce;
        private OpenFileDialog openFileDialog1;
        WebClientAsyncResult pendingAsyncResult;
        SpeciesService service;
        private Terrarium.Glass.GlassLabel retrievingData;
		private Terrarium.Glass.GlassLabel RefreshTime;
		private Terrarium.Glass.GlassButton okButton;
		private Terrarium.Glass.GlassButton cancelButton;
		private Terrarium.Glass.GlassButton serverListButton;
		private Terrarium.Glass.GlassButton browseButton;
        bool connectionCancelled = false;

        internal ReintroduceSpecies(bool reintroduce)
        {
            this.reintroduce = reintroduce;

            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog1.Filter = ".NET Terrarium Assemblies (*.dll)|*.dll|All Files (*.*)|*.*";
            this.openFileDialog1.Title = "Choose the Assembly where your animal is located";
            this.openFileDialog1.DefaultExt = ".dll";

            InitializeComponent();

			this.titleBar.ShowMaximizeButton = false;
			this.titleBar.ShowMinimizeButton = false;

            this.Description = refreshExplanation;
            
            if (dataSet == null)
            {
                this.serverListButton.Enabled = true;
                this.RefreshTime.Text = "";
            }
            else
            {
                this.serverListButton.Enabled = false;
                
                if (cacheDate > DateTime.Now.AddMinutes(-30))
                {
                    this.serverListButton.Enabled = false;
                    this.RefreshTime.ForeColor = Color.Green;
                }
                else
                {
                    this.serverListButton.Enabled = true;
                    this.RefreshTime.ForeColor = Color.Yellow;
                }
                
                this.RefreshTime.Text = "Cached for: " + ((int) (DateTime.Now - cacheDate).TotalMinutes).ToString() + " mins";
            }

            if (reintroduce)
            {
                this.browseButton.Visible = false;
            }
        }

        protected override void Dispose(bool dispose)
        {
            if (dispose)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(dispose);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn4 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn5 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.retrievingData = new Terrarium.Glass.GlassLabel();
			this.RefreshTime = new Terrarium.Glass.GlassLabel();
			this.okButton = new Terrarium.Glass.GlassButton();
			this.cancelButton = new Terrarium.Glass.GlassButton();
			this.serverListButton = new Terrarium.Glass.GlassButton();
			this.browseButton = new Terrarium.Glass.GlassButton();
			this.bottomPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.SuspendLayout();
			// 
			// titleBar
			// 
			this.titleBar.Size = new System.Drawing.Size(456, 32);
			this.titleBar.Title = "Introduce Creature";
			this.titleBar.CloseClicked += new System.EventHandler(this.Cancel_Click);
			// 
			// bottomPanel
			// 
			this.bottomPanel.Controls.Add(this.browseButton);
			this.bottomPanel.Controls.Add(this.serverListButton);
			this.bottomPanel.Controls.Add(this.cancelButton);
			this.bottomPanel.Controls.Add(this.okButton);
			this.bottomPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.bottomPanel.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.bottomPanel.Location = new System.Drawing.Point(0, 352);
			this.bottomPanel.Size = new System.Drawing.Size(456, 40);
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
			this.dataGrid1.Location = new System.Drawing.Point(8, 71);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.ReadOnly = true;
			this.dataGrid1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.dataGrid1.Size = new System.Drawing.Size(440, 272);
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
            this.dataGridTextBoxColumn1,
            this.dataGridTextBoxColumn2,
            this.dataGridTextBoxColumn3,
            this.dataGridTextBoxColumn4,
            this.dataGridTextBoxColumn5});
			this.dataGridTableStyle1.HeaderBackColor = System.Drawing.Color.Gray;
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.Color.White;
			this.dataGridTableStyle1.MappingName = "Table";
			// 
			// dataGridTextBoxColumn1
			// 
			this.dataGridTextBoxColumn1.Format = "";
			this.dataGridTextBoxColumn1.FormatInfo = null;
			this.dataGridTextBoxColumn1.HeaderText = "Species Name";
			this.dataGridTextBoxColumn1.MappingName = "Name";
			this.dataGridTextBoxColumn1.ReadOnly = true;
			this.dataGridTextBoxColumn1.Width = 150;
			// 
			// dataGridTextBoxColumn2
			// 
			this.dataGridTextBoxColumn2.Format = "";
			this.dataGridTextBoxColumn2.FormatInfo = null;
			this.dataGridTextBoxColumn2.HeaderText = "Type";
			this.dataGridTextBoxColumn2.MappingName = "Type";
			this.dataGridTextBoxColumn2.ReadOnly = true;
			this.dataGridTextBoxColumn2.Width = 75;
			// 
			// dataGridTextBoxColumn3
			// 
			this.dataGridTextBoxColumn3.Format = "";
			this.dataGridTextBoxColumn3.FormatInfo = null;
			this.dataGridTextBoxColumn3.HeaderText = "Author";
			this.dataGridTextBoxColumn3.MappingName = "Author";
			this.dataGridTextBoxColumn3.ReadOnly = true;
			this.dataGridTextBoxColumn3.Width = 75;
			// 
			// dataGridTextBoxColumn4
			// 
			this.dataGridTextBoxColumn4.Format = "d";
			this.dataGridTextBoxColumn4.FormatInfo = null;
			this.dataGridTextBoxColumn4.HeaderText = "Date Introduced";
			this.dataGridTextBoxColumn4.MappingName = "DateAdded";
			this.dataGridTextBoxColumn4.ReadOnly = true;
			this.dataGridTextBoxColumn4.Width = 90;
			// 
			// dataGridTextBoxColumn5
			// 
			this.dataGridTextBoxColumn5.Format = "d";
			this.dataGridTextBoxColumn5.FormatInfo = null;
			this.dataGridTextBoxColumn5.HeaderText = "Last Reintroduction";
			this.dataGridTextBoxColumn5.MappingName = "LastReintroduction";
			this.dataGridTextBoxColumn5.ReadOnly = true;
			this.dataGridTextBoxColumn5.Width = 125;
			// 
			// retrievingData
			// 
			this.retrievingData.BackColor = System.Drawing.Color.Transparent;
			this.retrievingData.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.retrievingData.ForeColor = System.Drawing.Color.White;
			this.retrievingData.Location = new System.Drawing.Point(72, 195);
			this.retrievingData.Name = "retrievingData";
			this.retrievingData.NoWrap = false;
			this.retrievingData.Size = new System.Drawing.Size(321, 19);
			this.retrievingData.TabIndex = 5;
			this.retrievingData.Text = "retrieving Species List From Server...";
			this.retrievingData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.retrievingData.Visible = false;
			// 
			// RefreshTime
			// 
			this.RefreshTime.BackColor = System.Drawing.Color.Transparent;
			this.RefreshTime.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RefreshTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.RefreshTime.Location = new System.Drawing.Point(456, 152);
			this.RefreshTime.Name = "RefreshTime";
			this.RefreshTime.NoWrap = false;
			this.RefreshTime.Size = new System.Drawing.Size(88, 32);
			this.RefreshTime.TabIndex = 10;
			this.RefreshTime.Text = "label1";
			this.RefreshTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// okButton
			// 
			this.okButton.BackColor = System.Drawing.Color.Transparent;
			this.okButton.BorderColor = System.Drawing.Color.Black;
			this.okButton.Depth = 3;
			this.okButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.okButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.okButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.okButton.ForeColor = System.Drawing.Color.White;
			this.okButton.Highlight = false;
			this.okButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.okButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.okButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.okButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.okButton.IsGlass = true;
			this.okButton.Location = new System.Drawing.Point(288, 2);
			this.okButton.Name = "okButton";
			this.okButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.okButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.okButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.okButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.okButton.Size = new System.Drawing.Size(75, 36);
			this.okButton.TabIndex = 16;
			this.okButton.TabStop = false;
			this.okButton.Text = "OK";
			this.okButton.UseStyles = true;
			this.okButton.UseVisualStyleBackColor = false;
			this.okButton.Click += new System.EventHandler(this.OK_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.BackColor = System.Drawing.Color.Transparent;
			this.cancelButton.BorderColor = System.Drawing.Color.Black;
			this.cancelButton.Depth = 3;
			this.cancelButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.cancelButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.cancelButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancelButton.ForeColor = System.Drawing.Color.White;
			this.cancelButton.Highlight = false;
			this.cancelButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.cancelButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.cancelButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.cancelButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.cancelButton.IsGlass = true;
			this.cancelButton.Location = new System.Drawing.Point(369, 2);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.cancelButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.cancelButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.cancelButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.cancelButton.Size = new System.Drawing.Size(75, 36);
			this.cancelButton.TabIndex = 17;
			this.cancelButton.TabStop = false;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseStyles = true;
			this.cancelButton.UseVisualStyleBackColor = false;
			this.cancelButton.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// serverListButton
			// 
			this.serverListButton.BackColor = System.Drawing.Color.Transparent;
			this.serverListButton.BorderColor = System.Drawing.Color.Black;
			this.serverListButton.Depth = 3;
			this.serverListButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.serverListButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.serverListButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.serverListButton.ForeColor = System.Drawing.Color.White;
			this.serverListButton.Highlight = false;
			this.serverListButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.serverListButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.serverListButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.serverListButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.serverListButton.IsGlass = true;
			this.serverListButton.Location = new System.Drawing.Point(12, 2);
			this.serverListButton.Name = "serverListButton";
			this.serverListButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.serverListButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.serverListButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.serverListButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.serverListButton.Size = new System.Drawing.Size(100, 36);
			this.serverListButton.TabIndex = 18;
			this.serverListButton.TabStop = false;
			this.serverListButton.Text = "Server List";
			this.serverListButton.UseStyles = true;
			this.serverListButton.UseVisualStyleBackColor = false;
			this.serverListButton.Click += new System.EventHandler(this.ServerList_Click);
			// 
			// browseButton
			// 
			this.browseButton.BackColor = System.Drawing.Color.Transparent;
			this.browseButton.BorderColor = System.Drawing.Color.Black;
			this.browseButton.Depth = 3;
			this.browseButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.browseButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.browseButton.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.browseButton.ForeColor = System.Drawing.Color.White;
			this.browseButton.Highlight = false;
			this.browseButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.browseButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.browseButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.browseButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.browseButton.IsGlass = true;
			this.browseButton.Location = new System.Drawing.Point(118, 2);
			this.browseButton.Name = "browseButton";
			this.browseButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.browseButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
			this.browseButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
			this.browseButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.browseButton.Size = new System.Drawing.Size(80, 36);
			this.browseButton.TabIndex = 0;
			this.browseButton.TabStop = false;
			this.browseButton.Text = "Browse...";
			this.browseButton.UseStyles = true;
			this.browseButton.UseVisualStyleBackColor = false;
			this.browseButton.Click += new System.EventHandler(this.Browse_Click);
			// 
			// ReintroduceSpecies
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(456, 392);
			this.Controls.Add(this.RefreshTime);
			this.Controls.Add(this.retrievingData);
			this.Controls.Add(this.dataGrid1);
			this.Name = "ReintroduceSpecies";
			this.Title = "Reintroduce Species";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ReintroduceSpecies_Paint);
			this.Controls.SetChildIndex(this.bottomPanel, 0);
			this.Controls.SetChildIndex(this.titleBar, 0);
			this.Controls.SetChildIndex(this.dataGrid1, 0);
			this.Controls.SetChildIndex(this.retrievingData, 0);
			this.Controls.SetChildIndex(this.RefreshTime, 0);
			this.bottomPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.ResumeLayout(false);

		}
        #endregion

        private void OK_Click(object sender, System.EventArgs e)
        {
            if (pendingAsyncResult != null)
            {
                connectionCancelled = true;
                pendingAsyncResult.Abort();
                pendingAsyncResult = null;
            }

            if (GameEngine.Current == null || dataGrid1.DataSource == null ||
                this.BindingContext[dataGrid1.DataSource, "Table"] == null ||
                this.BindingContext[dataGrid1.DataSource, "Table"].Count == 0)
            {
                this.Hide();
                return;
            }

            DataRowView drv = (DataRowView)(this.BindingContext[dataGrid1.DataSource,"Table"].Current);
            SpeciesService service = new SpeciesService();
            service.Url = GameConfig.WebRoot + "/Species/AddSpecies.asmx";
            service.Timeout = 60000;

            byte [] speciesAssemblyBytes = null;

            try
            {
                if (reintroduce)
                {
                    speciesAssemblyBytes = service.ReintroduceSpecies((string)drv["Name"], Assembly.GetExecutingAssembly().GetName().Version.ToString(), GameEngine.Current.CurrentVector.State.StateGuid);
                }
                else
                {
                    speciesAssemblyBytes = service.GetSpeciesAssembly((string)drv["Name"], Assembly.GetExecutingAssembly().GetName().Version.ToString());
                }
            }
            catch(WebException)
            {
                MessageBox.Show(this, "The connection to the server timed out.  Please try again later.");
            }

            if (speciesAssemblyBytes == null)
            {
                MessageBox.Show("Error retrieving species from server.");
            }
            else
            {
                dataSet.Tables["Table"].Rows.Remove(drv.Row);

                // Save it to a temp file
                string tempFile = PrivateAssemblyCache.GetSafeTempFileName();
                using (Stream fileStream = File.OpenWrite(tempFile))
                {
                    fileStream.Write(speciesAssemblyBytes, 0, (int) speciesAssemblyBytes.Length);
                    fileStream.Close();
                }                   

                try
                {
                    GameEngine.Current.AddNewOrganism(tempFile, Point.Empty, reintroduce);
                    File.Delete(tempFile);
                }
                catch (TargetInvocationException exception)
                {
                    Exception innerException = exception;
                    while (innerException.InnerException != null)
                    {
                        innerException = innerException.InnerException;
                    }

                    MessageBox.Show(innerException.Message, "Error Loading Assembly", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                catch (GameEngineException exception)
                {
                    MessageBox.Show(exception.Message, "Error Loading Assembly", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                this.Hide();
            }
        }

        private void Cancel_Click(object sender, System.EventArgs e)
        {
            if (pendingAsyncResult != null)
            {
                connectionCancelled = true;
                pendingAsyncResult.Abort();
                pendingAsyncResult = null;
            }

            this.Hide();
        }
        
        private void ServerList_Click(object sender, EventArgs e)
        {
            this.retrievingData.Visible = true;
        
            service = new SpeciesService();
            service.Timeout = 10000;
            service.Url = GameConfig.WebRoot + "/Species/AddSpecies.asmx";

            try
            {
                if (reintroduce)
                {
                    pendingAsyncResult = (WebClientAsyncResult) service.BeginGetExtinctSpecies(Assembly.GetExecutingAssembly().GetName().Version.ToString(), "", new AsyncCallback(ExtinctSpeciesCallback), null);
                }
                else
                {
                    pendingAsyncResult = (WebClientAsyncResult) service.BeginGetAllSpecies(Assembly.GetExecutingAssembly().GetName().Version.ToString(), "", new AsyncCallback(AllSpeciesCallback), null);
                }
            }
            catch(WebException)
            {
                MessageBox.Show(this, "The connection to the server timed out.  Please try again later.");
            }
        }

        void ExtinctSpeciesCallback(IAsyncResult asyncResult)
        {
            try
            {
                pendingAsyncResult = null;
                dataSet = service.EndGetExtinctSpecies(asyncResult);
                cacheDate = DateTime.Now;
                retrievingData.Visible = false;
            }
            catch
            {
                if (!connectionCancelled)
                {
                    retrievingData.Text = "There was a problem getting species list from server.";
                }
                return;
            }

            this.Invalidate();
        }

        void AllSpeciesCallback(IAsyncResult asyncResult)
        {
            try
            {
                pendingAsyncResult = null;
                dataSet = service.EndGetAllSpecies(asyncResult);
                cacheDate = DateTime.Now;
                retrievingData.Visible = false;
            }
            catch
            {
                if (!connectionCancelled)
                {
                    retrievingData.Text = "There was a problem getting species list from server.";
                }
                return;
            }

            this.Invalidate();
        }

        private void Browse_Click(object sender, System.EventArgs e)
        {
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
                        if (newSpecies != null)
                        {
                            string warnings = newSpecies.GetAttributeWarnings();
                            if (warnings.Length != 0)
                            {
                                MessageBox.Show("Your organism was introduced, but there were some warnings:\r\n" + warnings, "Organism Assembly Warnings");
                            }
                        }
                    }
                }
                catch (TargetInvocationException exception)
                {
                    Exception innerException = exception;
                    while (innerException.InnerException != null)
                    {
                        innerException = innerException.InnerException;
                    }

                    MessageBox.Show(innerException.Message, "Error Loading Assembly", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                catch (GameEngineException exception)
                {
                    MessageBox.Show(exception.Message, "Error Loading Assembly", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                catch (IOException exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                this.Hide();
            }
        }

        private void ReintroduceSpecies_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (dataSet != null)
            {
                dataSet.Tables[0].DefaultView.AllowNew = false;
                dataSet.Tables[0].DefaultView.AllowEdit = false;
                dataGrid1.DataSource = dataSet;
                dataGrid1.DataMember = "Table";
            }
        }
    }
}