//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

using OrganismBase;
using Terrarium.Game;
using Terrarium.Renderer;
using Terrarium.Forms;

namespace Terrarium.Client 
{
    internal class TraceWindow : Terrarium.Forms.TerrariumForm
	{
        private System.Windows.Forms.TextBox traceTextBox;
        private System.Windows.Forms.CheckBox showSystemTraces;
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.CheckBox showTimings;
        private System.Windows.Forms.CheckBox showTraces;
        object lastTraceSender = null;
		private Terrarium.Glass.GlassLabel GlassLabel1;
		private Terrarium.Glass.GlassLabel GlassLabel2;
		private Terrarium.Glass.GlassLabel GlassLabel3;
        private Terrarium.Glass.GlassButton closeButton;
        private Terrarium.Glass.GlassLabel glassLabel4;
        private Terrarium.Glass.GlassLabel glassLabel5;
        private Terrarium.Glass.GlassLabel glassLabel6;
        private ResizeBar resizeBar1;
        bool skipReportingOnce = false; // When you turn off tracing, we skip reporting once since it will report
        // timings from the last tick when tracing was on, and these will be way off

        internal TraceWindow()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.bottomPanel.Controls.Remove(this.resizeBar1);
            this.bottomPanel.Controls.Add(this.resizeBar1);

			this.titleBar.ShowMaximizeButton = false;
			this.titleBar.ShowMinimizeButton = false;

			this.BackColor = Terrarium.Glass.GlassStyleManager.Active.DialogColor;

        }

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
        private void InitializeComponent()
        {
            this.showTimings = new System.Windows.Forms.CheckBox();
            this.showSystemTraces = new System.Windows.Forms.CheckBox();
            this.traceTextBox = new System.Windows.Forms.TextBox();
            this.showTraces = new System.Windows.Forms.CheckBox();
            this.GlassLabel1 = new Terrarium.Glass.GlassLabel();
            this.GlassLabel2 = new Terrarium.Glass.GlassLabel();
            this.GlassLabel3 = new Terrarium.Glass.GlassLabel();
            this.closeButton = new Terrarium.Glass.GlassButton();
            this.glassLabel4 = new Terrarium.Glass.GlassLabel();
            this.glassLabel5 = new Terrarium.Glass.GlassLabel();
            this.glassLabel6 = new Terrarium.Glass.GlassLabel();
            this.resizeBar1 = new Terrarium.Forms.ResizeBar();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleBar
            // 
            this.titleBar.Size = new System.Drawing.Size(488, 32);
            this.titleBar.Title = "Trace Window";
            this.titleBar.CloseClicked += new System.EventHandler(this.CloseForm_Click);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.resizeBar1);
            this.bottomPanel.Controls.Add(this.closeButton);
            this.bottomPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bottomPanel.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.bottomPanel.Location = new System.Drawing.Point(0, 352);
            this.bottomPanel.Size = new System.Drawing.Size(488, 40);
            // 
            // showTimings
            // 
            this.showTimings.BackColor = System.Drawing.Color.White;
            this.showTimings.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showTimings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showTimings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showTimings.Location = new System.Drawing.Point(136, 80);
            this.showTimings.Name = "showTimings";
            this.showTimings.Size = new System.Drawing.Size(13, 13);
            this.showTimings.TabIndex = 1;
            this.showTimings.UseVisualStyleBackColor = false;
            // 
            // showSystemTraces
            // 
            this.showSystemTraces.BackColor = System.Drawing.Color.White;
            this.showSystemTraces.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showSystemTraces.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showSystemTraces.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showSystemTraces.Location = new System.Drawing.Point(16, 80);
            this.showSystemTraces.Name = "showSystemTraces";
            this.showSystemTraces.Size = new System.Drawing.Size(13, 13);
            this.showSystemTraces.TabIndex = 1;
            this.showSystemTraces.UseVisualStyleBackColor = false;
            // 
            // traceTextBox
            // 
            this.traceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.traceTextBox.BackColor = System.Drawing.Color.Black;
            this.traceTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.traceTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.traceTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.traceTextBox.Location = new System.Drawing.Point(11, 104);
            this.traceTextBox.Multiline = true;
            this.traceTextBox.Name = "traceTextBox";
            this.traceTextBox.ReadOnly = true;
            this.traceTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.traceTextBox.Size = new System.Drawing.Size(469, 242);
            this.traceTextBox.TabIndex = 0;
            this.traceTextBox.WordWrap = false;
            // 
            // showTraces
            // 
            this.showTraces.BackColor = System.Drawing.Color.White;
            this.showTraces.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showTraces.Checked = true;
            this.showTraces.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showTraces.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showTraces.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showTraces.Location = new System.Drawing.Point(256, 80);
            this.showTraces.Name = "showTraces";
            this.showTraces.Size = new System.Drawing.Size(13, 13);
            this.showTraces.TabIndex = 2;
            this.showTraces.UseVisualStyleBackColor = false;
            this.showTraces.CheckedChanged += new System.EventHandler(this.showTraces_CheckedChanged);
            // 
            // GlassLabel1
            // 
            this.GlassLabel1.AutoSize = true;
            this.GlassLabel1.BackColor = System.Drawing.Color.Transparent;
            this.GlassLabel1.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.GlassLabel1.ForeColor = System.Drawing.Color.White;
            this.GlassLabel1.Location = new System.Drawing.Point(32, 56);
            this.GlassLabel1.Name = "GlassLabel1";
            this.GlassLabel1.NoWrap = false;
            this.GlassLabel1.Size = new System.Drawing.Size(80, 14);
            this.GlassLabel1.TabIndex = 2;
            this.GlassLabel1.Text = "System Events";
            // 
            // GlassLabel2
            // 
            this.GlassLabel2.AutoSize = true;
            this.GlassLabel2.BackColor = System.Drawing.Color.Transparent;
            this.GlassLabel2.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.GlassLabel2.ForeColor = System.Drawing.Color.White;
            this.GlassLabel2.Location = new System.Drawing.Point(152, 56);
            this.GlassLabel2.Name = "GlassLabel2";
            this.GlassLabel2.NoWrap = false;
            this.GlassLabel2.Size = new System.Drawing.Size(83, 14);
            this.GlassLabel2.TabIndex = 3;
            this.GlassLabel2.Text = "Engine Timings";
            // 
            // GlassLabel3
            // 
            this.GlassLabel3.AutoSize = true;
            this.GlassLabel3.BackColor = System.Drawing.Color.Transparent;
            this.GlassLabel3.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.GlassLabel3.ForeColor = System.Drawing.Color.White;
            this.GlassLabel3.Location = new System.Drawing.Point(272, 56);
            this.GlassLabel3.Name = "GlassLabel3";
            this.GlassLabel3.NoWrap = false;
            this.GlassLabel3.Size = new System.Drawing.Size(86, 14);
            this.GlassLabel3.TabIndex = 4;
            this.GlassLabel3.Text = "Creature Traces";
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
            this.closeButton.Location = new System.Drawing.Point(387, 2);
            this.closeButton.Name = "closeButton";
            this.closeButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.closeButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.closeButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.closeButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.closeButton.Size = new System.Drawing.Size(75, 36);
            this.closeButton.TabIndex = 16;
            this.closeButton.TabStop = false;
            this.closeButton.Text = "Close";
            this.closeButton.UseStyles = true;
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.CloseForm_Click);
            // 
            // glassLabel4
            // 
            this.glassLabel4.BackColor = System.Drawing.Color.Transparent;
            this.glassLabel4.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.glassLabel4.ForeColor = System.Drawing.Color.White;
            this.glassLabel4.Location = new System.Drawing.Point(35, 80);
            this.glassLabel4.Name = "glassLabel4";
            this.glassLabel4.NoWrap = false;
            this.glassLabel4.Size = new System.Drawing.Size(83, 17);
            this.glassLabel4.TabIndex = 15;
            this.glassLabel4.Text = "System Events";
            // 
            // glassLabel5
            // 
            this.glassLabel5.BackColor = System.Drawing.Color.Transparent;
            this.glassLabel5.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.glassLabel5.ForeColor = System.Drawing.Color.White;
            this.glassLabel5.Location = new System.Drawing.Point(155, 80);
            this.glassLabel5.Name = "glassLabel5";
            this.glassLabel5.NoWrap = false;
            this.glassLabel5.Size = new System.Drawing.Size(83, 17);
            this.glassLabel5.TabIndex = 16;
            this.glassLabel5.Text = "Engine Timings";
            // 
            // glassLabel6
            // 
            this.glassLabel6.BackColor = System.Drawing.Color.Transparent;
            this.glassLabel6.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.glassLabel6.ForeColor = System.Drawing.Color.White;
            this.glassLabel6.Location = new System.Drawing.Point(275, 80);
            this.glassLabel6.Name = "glassLabel6";
            this.glassLabel6.NoWrap = false;
            this.glassLabel6.Size = new System.Drawing.Size(108, 17);
            this.glassLabel6.TabIndex = 17;
            this.glassLabel6.Text = "Creature Traces";
            // 
            // resizeBar1
            // 
            this.resizeBar1.BackColor = System.Drawing.Color.Black;
            this.resizeBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.resizeBar1.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resizeBar1.ForeColor = System.Drawing.Color.White;
            this.resizeBar1.Location = new System.Drawing.Point(468, 0);
            this.resizeBar1.Name = "resizeBar1";
            this.resizeBar1.Size = new System.Drawing.Size(20, 40);
            this.resizeBar1.TabIndex = 17;
            // 
            // TraceWindow
            // 
            this.BackColor = System.Drawing.Color.Peru;
            this.ClientSize = new System.Drawing.Size(488, 392);
            this.Controls.Add(this.glassLabel6);
            this.Controls.Add(this.glassLabel5);
            this.Controls.Add(this.glassLabel4);
            this.Controls.Add(this.traceTextBox);
            this.Controls.Add(this.showTraces);
            this.Controls.Add(this.showTimings);
            this.Controls.Add(this.showSystemTraces);
            this.Description = "Use this to view organism trace messages and various engine trace messages";
            this.Name = "TraceWindow";
            this.Title = "Trace Window";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form_Closing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.bottomPanel, 0);
            this.Controls.SetChildIndex(this.titleBar, 0);
            this.Controls.SetChildIndex(this.showSystemTraces, 0);
            this.Controls.SetChildIndex(this.showTimings, 0);
            this.Controls.SetChildIndex(this.showTraces, 0);
            this.Controls.SetChildIndex(this.traceTextBox, 0);
            this.Controls.SetChildIndex(this.glassLabel4, 0);
            this.Controls.SetChildIndex(this.glassLabel5, 0);
            this.Controls.SetChildIndex(this.glassLabel6, 0);
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion

        internal void DebugWriteLine(string value)
        {
            if (this.showSystemTraces.Checked == false && value != null && value[0] == '#')
            {
                return;
            }

            if (this.showTimings.Checked == false && value != null && value[0] == '$')
            {
                return;
            }

            if (traceTextBox.Text.Length > 10000)
            {
                traceTextBox.Text = value + "\r\n" + traceTextBox.Text.Substring(0, 10000) + "...";
            }
            else
            {
                traceTextBox.Text = value + "\r\n" + traceTextBox.Text;
            }
        }

        internal void TickEnded()
        {
            lastTraceSender = null;
            if (!showTraces.Checked) 
            {
                ShowOrganismTimings();
            }
        }

        private void Form_Load(object sender, System.EventArgs e)
        {
            // When the trace window loads, we hook up all selected organisms to it
            if (showTraces.Checked) 
            {
                HookupAllSelectedOrganisms(true);
            }
        }

        private void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // When the trace window closes, we unhook all selected organisms
            if (showTraces.Checked) 
            {
                HookupAllSelectedOrganisms(false);
            }
        }

        internal void SelectOrganism(OrganismState state)
        {
            if (showTraces.Checked)
            {
                AddTrace(state);
            }
        }

        internal void UnselectOrganism(OrganismState state)
        {
            if (showTraces.Checked)
            {
                RemoveTrace(state);
            }
        }

        private void AddTrace(OrganismState state)
        {
            Organism organism = GameEngine.Current.Scheduler.GetOrganism(state.ID);
            if (organism != null)
            {
                organism.Trace += new TraceEventHandler(TraceEvent);
            }
        }

        private void RemoveTrace(OrganismState state)
        {
            Organism organism = GameEngine.Current.Scheduler.GetOrganism(state.ID);
            if (organism != null)
            {
                organism.Trace -= new TraceEventHandler(TraceEvent);
            }
        }

        // Hooks up or removes selected organisms from tracing at startup or shutdown
        private void HookupAllSelectedOrganisms(bool acceptTraces) 
        {
            if (GameEngine.Current != null)
            {
                if (acceptTraces == false)
                {
                    skipReportingOnce = true;
                }
                else
                {
                    skipReportingOnce = false;
                }

                GameEngine.Current.Scheduler.PenalizeForTime = !acceptTraces;

                WorldState worldState = GameEngine.Current.CurrentVector.State;
                foreach (OrganismState state in worldState.Organisms)
                {
                    if (state.RenderInfo != null && ((TerrariumSprite) state.RenderInfo).Selected)
                    {
                        if (acceptTraces)
                        {
                            AddTrace(state);
                        }
                        else
                        {
                            RemoveTrace(state);
                        }
                    }
                }
            }
        }

        private void ShowOrganismTimings()
        {
            if (skipReportingOnce)
            {
                skipReportingOnce = false;
            }
            else
            {
                WorldState worldState = GameEngine.Current.CurrentVector.State;
                foreach (OrganismState state in worldState.Organisms)
                {
                    if (state.RenderInfo != null && ((TerrariumSprite) state.RenderInfo).Selected)
                    {
                        DebugWriteLine("---- Timings From: " + state.ID + "\r\n      " +
                            GameEngine.Current.Scheduler.GetOrganismTimingReport(state.ID));
                    }
                }
            }
        }

        private void TraceEvent(object sender, params object [] items)
        {
            if (sender != lastTraceSender)
            {
                lastTraceSender = sender;
                DebugWriteLine("---- Traces From: " + ((Organism) sender).ID + "Tick " + GameEngine.Current.CurrentVector.State.TickNumber);
            }

            StringBuilder builder = new StringBuilder();
            foreach (object item in items)
            {
                builder.Append(item.ToString() + "-");
            }

            DebugWriteLine(builder.ToString());
        }

        private void showTraces_CheckedChanged(object sender, System.EventArgs e)
        {
            HookupAllSelectedOrganisms(showTraces.Checked);     
        }

		private void CloseForm_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}
    }
}