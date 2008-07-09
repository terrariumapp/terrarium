//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Windows.Forms;

using OrganismBase;
using Terrarium.Game;
using Terrarium.Renderer;
using Terrarium.Forms;

namespace Terrarium.Client 
{
    internal class PropertySheet : Terrarium.Forms.TerrariumForm
    {
        private System.Windows.Forms.ComboBox comboBox;
        private Terrarium.Glass.GlassLabel glassLabel1;
        private GenericTypeDescriptor selectedObject;

        private string selectedGuid = null;
        private string currentGuid = null;
        Boolean formClosed = false;
        private System.Windows.Forms.PropertyGrid propertyGrid;
		private Terrarium.Glass.GlassButton closeButton;

        private System.ComponentModel.Container components = null;

        internal PropertySheet()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
			this.titleBar.ShowMaximizeButton = false;
			this.titleBar.ShowMinimizeButton = false;
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
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.closeButton = new Terrarium.Glass.GlassButton();
            this.glassLabel1 = new Terrarium.Glass.GlassLabel();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleBar
            // 
            this.titleBar.Size = new System.Drawing.Size(392, 32);
            this.titleBar.Title = "Creature Details";
            this.titleBar.CloseClicked += new System.EventHandler(this.CloseForm_Click);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.closeButton);
            this.bottomPanel.Gradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bottomPanel.Gradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.bottomPanel.Location = new System.Drawing.Point(0, 376);
            this.bottomPanel.Size = new System.Drawing.Size(392, 40);
            // 
            // comboBox
            // 
            this.comboBox.BackColor = System.Drawing.SystemColors.Window;
            this.comboBox.DisplayMember = "Text";
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox.ForeColor = System.Drawing.Color.Black;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(114, 75);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(270, 20);
            this.comboBox.TabIndex = 3;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.BackColor = System.Drawing.Color.Gray;
            this.propertyGrid.CommandsBackColor = System.Drawing.Color.Gray;
            this.propertyGrid.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyGrid.HelpBackColor = System.Drawing.Color.Gray;
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.LineColor = System.Drawing.Color.Silver;
            this.propertyGrid.Location = new System.Drawing.Point(8, 104);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(376, 266);
            this.propertyGrid.TabIndex = 10;
            this.propertyGrid.ToolbarVisible = false;
            this.propertyGrid.ViewBackColor = System.Drawing.Color.Black;
            this.propertyGrid.ViewForeColor = System.Drawing.Color.White;
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.BorderColor = System.Drawing.Color.Black;
            this.closeButton.Depth = 4;
            this.closeButton.DisabledGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.closeButton.DisabledGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.closeButton.Highlight = false;
            this.closeButton.HighlightGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.closeButton.HighlightGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.closeButton.HoverGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.closeButton.HoverGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.closeButton.IsGlass = true;
            this.closeButton.Location = new System.Drawing.Point(305, 2);
            this.closeButton.Name = "closeButton";
            this.closeButton.NormalGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.closeButton.NormalGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.closeButton.PressedGradient.Bottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.closeButton.PressedGradient.Top = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.closeButton.Size = new System.Drawing.Size(75, 36);
            this.closeButton.TabIndex = 0;
            this.closeButton.TabStop = false;
            this.closeButton.Text = "Close";
            this.closeButton.UseStyles = true;
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.CloseForm_Click);
            // 
            // glassLabel1
            // 
            this.glassLabel1.BackColor = System.Drawing.Color.Transparent;
            this.glassLabel1.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.glassLabel1.ForeColor = System.Drawing.Color.White;
            this.glassLabel1.Location = new System.Drawing.Point(12, 78);
            this.glassLabel1.Name = "glassLabel1";
            this.glassLabel1.NoWrap = false;
            this.glassLabel1.Size = new System.Drawing.Size(93, 17);
            this.glassLabel1.TabIndex = 15;
            this.glassLabel1.Text = "Select Organism:";
            // 
            // PropertySheet
            // 
            this.ClientSize = new System.Drawing.Size(392, 416);
            this.Controls.Add(this.glassLabel1);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.comboBox);
            this.Description = "View the properties of the selected organisms";
            this.Name = "PropertySheet";
            this.Title = "Organism Properties";
            this.Closed += new System.EventHandler(this.PropertySheet_Closed);
            this.Load += new System.EventHandler(this.PropertySheet_Load);
            this.Controls.SetChildIndex(this.bottomPanel, 0);
            this.Controls.SetChildIndex(this.titleBar, 0);
            this.Controls.SetChildIndex(this.comboBox, 0);
            this.Controls.SetChildIndex(this.propertyGrid, 0);
            this.Controls.SetChildIndex(this.glassLabel1, 0);
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        internal void RefreshGrid()
        {
            if (formClosed)
            {
                return;
            }

            if (GameEngine.Current != null && GameEngine.Current.CurrentVector != null)
            {
                ArrayList list = new ArrayList();
                ArrayList shownList = new ArrayList();
            
                WorldState worldState = GameEngine.Current.CurrentVector.State;
                
                if (comboBox.SelectedItem == null)
                {
                    selectedGuid = null;
                }
                else
                {
                    selectedGuid = ((OrganismComboItem) comboBox.SelectedItem).ID;
                }
                
                if (currentGuid != selectedGuid)
                {
                    currentGuid = selectedGuid;
                    selectedGuid = null;
                    if (currentGuid != null)
                    {
                        OrganismState o = worldState.GetOrganismState(currentGuid);
                        if (o != null)
                        {
                            selectedObject = new GenericTypeDescriptor(o);
                            propertyGrid.SelectedObject = selectedObject;
                        }
                        else
                        {
                            UnselectObject(currentGuid);
                        }
                    }
                    else
                    {
                        propertyGrid.SelectedObject = null;
                    }
                }
                else
                {
                    if (currentGuid == null)
                    {
                        propertyGrid.SelectedObject = null;
                    }
                    else
                    {
                        OrganismState o = worldState.GetOrganismState(currentGuid);
                        if (o != null)
                        {
                            selectedObject.SetObject(o);
                            propertyGrid.Refresh();
                        }
                        else
                        {
                            UnselectObject(currentGuid);
                        }
                    }
                }
            }
        }

        internal void SelectObject(OrganismState state)
        {
            if (formClosed)
            {
                return;
            }
                
            comboBox.Items.Add(new OrganismComboItem(state));
            if (comboBox.Items.Count == 1)
            {
                comboBox.SelectedIndex = 0;
            }
            RefreshGrid();
        }

        internal void UnselectObject(OrganismState state)
        {
            if (formClosed)
            {
                return;
            }

            UnselectObject(state.ID);
        }
        
        internal void UnselectObject(string id)
        {
            bool removedSelected = false;
        
            foreach (OrganismComboItem o in comboBox.Items)
            {
                if (o.ID == id)
                {
                    if (o == comboBox.SelectedItem)
                    {
                        removedSelected = true;
                    }
                    comboBox.Items.Remove(o);
                    break;
                }
            }
            
            if (comboBox.Items.Count > 0 && removedSelected)
            {
                comboBox.SelectedIndex = 0;
            }
            RefreshGrid();
        }

        private void showOrganism_CheckChanged(object sender, System.EventArgs e)
        {
            RefreshGrid();
        }

        private void PropertySheet_Closed(object sender, System.EventArgs e)
        {
            formClosed = true;
        }

        private void PropertySheet_Load(object sender, System.EventArgs e)
        {
            if (GameEngine.Current != null && GameEngine.Current.CurrentVector != null)
            {
                WorldState worldState = GameEngine.Current.CurrentVector.State;
                foreach (OrganismState state in worldState.Organisms)
                {
                    if (state.RenderInfo != null && ((TerrariumSprite) state.RenderInfo).Selected)
                    {
                        comboBox.Items.Add(new OrganismComboItem(state));
                    }
                }
            }
            
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

		private void CloseForm_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}
    }
    
    internal class OrganismComboItem
    {
        private string text;
        private string id;
        private string name;
        
        public OrganismComboItem(OrganismState o)
        {
            this.id = o.ID;
            this.name = ((Species) o.Species).Name;
            this.text = this.name + " - " + this.id;
        }
        
        public string Text
        {
            get 
            {
                return text; 
            }
        }
        
        public string ID
        {
            get 
            {
                return id;
            }
        }
        
        public string Name 
        {
            get 
            {
                return name;
            }
        }
    }
}