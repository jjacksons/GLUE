using System.Collections.Generic;
using System.Windows.Forms;
namespace Canvas
{
	partial class MainWin
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
        public void updateProperties(List<ModuleItems.Property> list)
        {
            if (list.Count == 0) return;
            int i = 0;
            foreach (ModuleItems.Property prop in list)
            {
                Label temp = new Label();
                temp.Text = prop.name;
                temp.Top = PropertiesPanel.Top + (DefaultLabel.Top - PropertiesPanel.Top) +26 *i;
                temp.Left = DefaultLabel.Left;

                TextBox temp2 = new TextBox();
                temp2.Text = prop.value.ToString();
                temp2.Top = PropertiesPanel.Top + (DefaultTextbox.Top - PropertiesPanel.Top) +26* i;
                temp.Left = DefaultTextbox.Left;
            }
        }
		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.m_mainMenu = new System.Windows.Forms.MenuStrip();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gLMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sINCALToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gISToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_windowMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_helpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.m_aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.layersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PropertiesPanel = new System.Windows.Forms.Panel();
            this.DefaultLabel = new System.Windows.Forms.Label();
            this.DefaultTextbox = new System.Windows.Forms.TextBox();
            this.Close_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.m_mainMenu.SuspendLayout();
            this.PropertiesPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // m_mainMenu
            // 
            this.m_mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.m_windowMenu,
            this.m_helpMenu});
            this.m_mainMenu.Location = new System.Drawing.Point(0, 0);
            this.m_mainMenu.MdiWindowListItem = this.m_windowMenu;
            this.m_mainMenu.Name = "m_mainMenu";
            this.m_mainMenu.Size = new System.Drawing.Size(803, 24);
            this.m_mainMenu.TabIndex = 0;
            this.m_mainMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.m_mainMenu_ItemClicked);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gLMToolStripMenuItem,
            this.sINCALToolStripMenuItem,
            this.lIDToolStripMenuItem,
            this.gISToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.importToolStripMenuItem.Text = "Import";
            // 
            // gLMToolStripMenuItem
            // 
            this.gLMToolStripMenuItem.Name = "gLMToolStripMenuItem";
            this.gLMToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.gLMToolStripMenuItem.Text = "GLM";
            this.gLMToolStripMenuItem.Click += new System.EventHandler(this.OnGLMImport);
            // 
            // sINCALToolStripMenuItem
            // 
            this.sINCALToolStripMenuItem.Name = "sINCALToolStripMenuItem";
            this.sINCALToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.sINCALToolStripMenuItem.Text = "SINCAL";
            // 
            // lIDToolStripMenuItem
            // 
            this.lIDToolStripMenuItem.Name = "lIDToolStripMenuItem";
            this.lIDToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.lIDToolStripMenuItem.Text = "LID";
            // 
            // gISToolStripMenuItem
            // 
            this.gISToolStripMenuItem.Name = "gISToolStripMenuItem";
            this.gISToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.gISToolStripMenuItem.Text = "GIS";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(12, 20);
            // 
            // m_windowMenu
            // 
            this.m_windowMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem});
            this.m_windowMenu.Name = "m_windowMenu";
            this.m_windowMenu.Size = new System.Drawing.Size(63, 20);
            this.m_windowMenu.Text = "&Window";
            this.m_windowMenu.DropDownOpening += new System.EventHandler(this.OnUpdateMenuUI);
            this.m_windowMenu.Click += new System.EventHandler(this.m_windowMenu_Click);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // m_helpMenu
            // 
            this.m_helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_aboutMenuItem});
            this.m_helpMenu.Name = "m_helpMenu";
            this.m_helpMenu.Size = new System.Drawing.Size(44, 20);
            this.m_helpMenu.Text = "&Help";
            // 
            // m_aboutMenuItem
            // 
            this.m_aboutMenuItem.Name = "m_aboutMenuItem";
            this.m_aboutMenuItem.Size = new System.Drawing.Size(107, 22);
            this.m_aboutMenuItem.Text = "&About";
            this.m_aboutMenuItem.Click += new System.EventHandler(this.OnAbout);
            // 
            // optionsToolStripMenuItem1
            // 
            this.optionsToolStripMenuItem1.Name = "optionsToolStripMenuItem1";
            this.optionsToolStripMenuItem1.Size = new System.Drawing.Size(127, 22);
            this.optionsToolStripMenuItem1.Tag = "Grid";
            this.optionsToolStripMenuItem1.Text = "&Grid";
            this.optionsToolStripMenuItem1.Click += new System.EventHandler(this.OnOptions);
            // 
            // layersToolStripMenuItem
            // 
            this.layersToolStripMenuItem.Name = "layersToolStripMenuItem";
            this.layersToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.layersToolStripMenuItem.Tag = "Layers";
            this.layersToolStripMenuItem.Text = "Layers";
            this.layersToolStripMenuItem.Click += new System.EventHandler(this.OnOptions);
            // 
            // PropertiesPanel
            // 
            this.PropertiesPanel.AutoScroll = true;
            this.PropertiesPanel.Controls.Add(this.DefaultLabel);
            this.PropertiesPanel.Controls.Add(this.textBox1);
            this.PropertiesPanel.Controls.Add(this.DefaultTextbox);
            this.PropertiesPanel.Controls.Add(this.Close_button);
            this.PropertiesPanel.Controls.Add(this.label1);
            this.PropertiesPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.PropertiesPanel.Location = new System.Drawing.Point(603, 24);
            this.PropertiesPanel.Name = "PropertiesPanel";
            this.PropertiesPanel.Size = new System.Drawing.Size(200, 485);
            this.PropertiesPanel.TabIndex = 6;
            // 
            // DefaultLabel
            // 
            this.DefaultLabel.AutoSize = true;
            this.DefaultLabel.Location = new System.Drawing.Point(3, 28);
            this.DefaultLabel.Name = "DefaultLabel";
            this.DefaultLabel.Size = new System.Drawing.Size(44, 13);
            this.DefaultLabel.TabIndex = 3;
            this.DefaultLabel.Text = "Name 1";
            this.DefaultLabel.Visible = false;
            // 
            // DefaultTextbox
            // 
            this.DefaultTextbox.Location = new System.Drawing.Point(85, 25);
            this.DefaultTextbox.Name = "DefaultTextbox";
            this.DefaultTextbox.Size = new System.Drawing.Size(112, 20);
            this.DefaultTextbox.TabIndex = 2;
            this.DefaultTextbox.Visible = false;
            // 
            // Close_button
            // 
            this.Close_button.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Close_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Close_button.Location = new System.Drawing.Point(177, 0);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(23, 22);
            this.Close_button.TabIndex = 0;
            this.Close_button.Text = "X";
            this.Close_button.UseVisualStyleBackColor = false;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 22);
            this.label1.TabIndex = 1;
            this.label1.Text = "Properties";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(85, 51);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(112, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Visible = false;
            // 
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 509);
            this.Controls.Add(this.PropertiesPanel);
            this.Controls.Add(this.m_mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.m_mainMenu;
            this.Name = "MainWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainWin";
            this.m_mainMenu.ResumeLayout(false);
            this.m_mainMenu.PerformLayout();
            this.PropertiesPanel.ResumeLayout(false);
            this.PropertiesPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip m_mainMenu;
        private System.Windows.Forms.ToolStripMenuItem m_windowMenu;
		private System.Windows.Forms.ToolStripMenuItem m_helpMenu;
		private System.Windows.Forms.ToolStripMenuItem m_aboutMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem layersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gLMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sINCALToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gISToolStripMenuItem;
        private ToolStripPanel BottomToolStripPanel;
        private ToolStripPanel TopToolStripPanel;
        private ToolStripPanel RightToolStripPanel;
        private ToolStripPanel LeftToolStripPanel;
        private Panel PropertiesPanel;
        private Label DefaultLabel;
        private TextBox DefaultTextbox;
        private Button Close_button;
        private Label label1;
        private ToolStripMenuItem propertiesToolStripMenuItem;
        private TextBox textBox1;
	}
}