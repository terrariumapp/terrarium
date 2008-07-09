//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace Terrarium.Forms
{
    partial class TickerHistoryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.glassPanel2 = new Terrarium.Glass.GlassPanel();
            this.glassLabel1 = new Terrarium.Glass.GlassLabel();
            this.historyListBox = new System.Windows.Forms.ListBox();
            this.glassPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // glassPanel2
            // 
            this.glassPanel2.Borders = ((Terrarium.Glass.GlassBorders)((((Terrarium.Glass.GlassBorders.Left | Terrarium.Glass.GlassBorders.Top)
                        | Terrarium.Glass.GlassBorders.Right)
                        | Terrarium.Glass.GlassBorders.Bottom)));
            this.glassPanel2.Controls.Add(this.glassLabel1);
            this.glassPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.glassPanel2.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.glassPanel2.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.glassPanel2.IsGlass = true;
            this.glassPanel2.IsSunk = false;
            this.glassPanel2.Location = new System.Drawing.Point(0, 0);
            this.glassPanel2.Name = "glassPanel2";
            this.glassPanel2.Size = new System.Drawing.Size(292, 20);
            this.glassPanel2.TabIndex = 1;
            this.glassPanel2.UseStyles = true;
            // 
            // glassLabel1
            // 
            this.glassLabel1.BackColor = System.Drawing.Color.Transparent;
            this.glassLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glassLabel1.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.glassLabel1.ForeColor = System.Drawing.Color.White;
            this.glassLabel1.Location = new System.Drawing.Point(0, 0);
            this.glassLabel1.Name = "glassLabel1";
            this.glassLabel1.NoWrap = false;
            this.glassLabel1.Size = new System.Drawing.Size(292, 20);
            this.glassLabel1.TabIndex = 2;
            this.glassLabel1.Text = "  Message History";
            this.glassLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // historyListBox
            // 
            this.historyListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.historyListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.historyListBox.FormattingEnabled = true;
            this.historyListBox.HorizontalScrollbar = true;
            this.historyListBox.Location = new System.Drawing.Point(12, 26);
            this.historyListBox.Name = "historyListBox";
            this.historyListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.historyListBox.Size = new System.Drawing.Size(268, 223);
            this.historyListBox.TabIndex = 2;
            // 
            // TickerHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 256);
            this.Controls.Add(this.historyListBox);
            this.Controls.Add(this.glassPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TickerHistoryForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TickerHistoryForm";
            this.glassPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Terrarium.Glass.GlassPanel glassPanel2;
        private Terrarium.Glass.GlassLabel glassLabel1;
        private System.Windows.Forms.ListBox historyListBox;
    }
}