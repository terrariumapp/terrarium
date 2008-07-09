//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terrarium.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TickerHistoryForm : Form
    {
        private TickerBar tickerBar = null;

        private delegate void AddMessageHandler(string message);

        /// <summary>
        /// 
        /// </summary>
        public TickerHistoryForm(TickerBar tickerBar)
        {
            this.tickerBar = tickerBar;

            InitializeComponent();
            this.BackColor = Glass.GlassStyleManager.Active.DialogColor;
            this.historyListBox.BackColor = this.BackColor;
            this.historyListBox.Font = Glass.GlassStyleManager.Active.Font;
            this.historyListBox.ForeColor = Glass.GlassStyleManager.Active.ForeColor;
            this.historyListBox.MouseLeave += new EventHandler(historyListBox_MouseLeave);
        }

        void historyListBox_MouseLeave(object sender, EventArgs e)
        {
            this.Capture = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void AddHistoryItem(TickerMessage message)
        {
            if (message != null && message.Text != null && message.Text.Length > 0)
            {
                this.historyListBox.Invoke(new AddMessageHandler(this.AddMessage), message.Text);
            }
        }

        private void AddMessage(string message)
        {
            this.historyListBox.Items.Insert(0, DateTime.Now.ToLongTimeString() + "\t" + message);

            if (this.historyListBox.Items.Count > 250)
                this.historyListBox.Items.RemoveAt(this.historyListBox.Items.Count - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnActivated(EventArgs e)
        {
            this.Capture = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.Capture == true)
            {
                if (e.X < 0 ||
                    e.X > this.Width ||
                    e.Y > this.Height + this.tickerBar.Height ||
                    e.Y < 0)
                {
                    this.Capture = false;
                    this.Hide();
                }
            }
        }
    }
}