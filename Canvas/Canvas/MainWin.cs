using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Canvas
{
	public partial class MainWin : Form
	{
		MenuItemManager m_menuItems;
		public MainWin()
		{
			UnitPoint p = HitUtil.CenterPointFrom3Points(new UnitPoint(0,2), new UnitPoint(1.4142136f, 1.4142136f), new UnitPoint(2,0));

			InitializeComponent();
			Text = Program.AppName;
			string[] args = Environment.GetCommandLineArgs();
			if (args.Length == 2) // assume it points to a file
				OpenDocument(args[1]);
			else
				OpenDocument(string.Empty);
			
			m_menuItems = new MenuItemManager(this);
			m_menuItems.SetupStripPanels();
			SetupToolbars();
			
			Application.Idle += new EventHandler(OnIdle);
		}
		void SetupToolbars()
		{
			MenuItem mmitem = m_menuItems.GetItem("New");
			mmitem.Text = "&New";
			mmitem.Image = MenuImages16x16.Image(MenuImages16x16.eIndexes.NewDocument);
			mmitem.Click += new EventHandler(OnFileNew);
			mmitem.ToolTipText = "New document";

			mmitem = m_menuItems.GetItem("Open");
			mmitem.Text = "&Open";
			mmitem.Image = MenuImages16x16.Image(MenuImages16x16.eIndexes.OpenDocument);
			mmitem.Click += new EventHandler(OnFileOpen);
			mmitem.ToolTipText = "Open document";

			mmitem = m_menuItems.GetItem("Save");
			mmitem.Text = "&Save";
			mmitem.Image = MenuImages16x16.Image(MenuImages16x16.eIndexes.SaveDocument);
			mmitem.Click += new EventHandler(OnFileSave);
			mmitem.ToolTipText = "Save document";

			mmitem = m_menuItems.GetItem("SaveAs");
			mmitem.Text = "Save &As";
			mmitem.Click += new EventHandler(OnFileSaveAs);

			mmitem = m_menuItems.GetItem("Exit");
			mmitem.Text = "E&xit";
			mmitem.Click += new EventHandler(OnFileExit);

			ToolStrip strip = m_menuItems.GetStrip("file");
			strip.Items.Add(m_menuItems.GetItem("New").CreateButton());
			strip.Items.Add(m_menuItems.GetItem("Open").CreateButton());
			strip.Items.Add(m_menuItems.GetItem("Save").CreateButton());

			ToolStripMenuItem menuitem = m_menuItems.GetMenuStrip("file");
			menuitem.Text = "&File";
			menuitem.DropDownItems.Add(m_menuItems.GetItem("New").CreateMenuItem());
			menuitem.DropDownItems.Add(m_menuItems.GetItem("Open").CreateMenuItem());
			menuitem.DropDownItems.Add(m_menuItems.GetItem("Save").CreateMenuItem());
			menuitem.DropDownItems.Add(m_menuItems.GetItem("SaveAs").CreateMenuItem());
			menuitem.DropDownItems.Add(new ToolStripSeparator());
			menuitem.DropDownItems.Add(m_menuItems.GetItem("Exit").CreateMenuItem());
			m_mainMenu.Items.Insert(0, menuitem);

			ToolStripPanel panel = m_menuItems.GetStripPanel(DockStyle.Top);

			panel.Join(m_menuItems.GetStrip("layer"));
			panel.Join(m_menuItems.GetStrip("draw"));
			panel.Join(m_menuItems.GetStrip("edit"));
			panel.Join(m_menuItems.GetStrip("file"));
			panel.Join(m_mainMenu);

			panel = m_menuItems.GetStripPanel(DockStyle.Left);
			panel.Join(m_menuItems.GetStrip("modify"));

            panel = m_menuItems.GetStripPanel(DockStyle.Left);
            panel.Join(m_menuItems.GetStrip("modules"));

			panel = m_menuItems.GetStripPanel(DockStyle.Bottom);
			panel.Join(m_menuItems.GetStatusStrip("status"));
		}
		void OnIdle(object sender, EventArgs e)
		{
			m_activeDocument = this.ActiveMdiChild as DocumentForm;
			if (m_activeDocument != null)
				m_activeDocument.UpdateUI();

		}
		protected DocumentForm m_activeDocument = null;
		protected override void OnMdiChildActivate(EventArgs e)
		{
			DocumentForm olddocument = m_activeDocument;
			base.OnMdiChildActivate(e);
			m_activeDocument = this.ActiveMdiChild as DocumentForm;
			foreach (Control ctrl in Controls)
			{
				if (ctrl is ToolStripPanel)
					((ToolStripPanel)ctrl).SuspendLayout();
			}
			if (m_activeDocument != null)
			{
				ToolStripManager.RevertMerge(m_menuItems.GetStrip("edit"));
				ToolStripManager.RevertMerge(m_menuItems.GetStrip("draw"));
				ToolStripManager.RevertMerge(m_menuItems.GetStrip("layer"));
				ToolStripManager.RevertMerge(m_menuItems.GetStrip("status"));
				ToolStripManager.RevertMerge(m_menuItems.GetStrip("modify"));
				ToolStripManager.Merge(m_activeDocument.GetToolStrip("draw"), m_menuItems.GetStrip("draw"));
				ToolStripManager.Merge(m_activeDocument.GetToolStrip("edit"), m_menuItems.GetStrip("edit"));
				ToolStripManager.Merge(m_activeDocument.GetToolStrip("layer"), m_menuItems.GetStrip("layer"));
				ToolStripManager.Merge(m_activeDocument.GetToolStrip("status"), m_menuItems.GetStrip("status"));
				ToolStripManager.Merge(m_activeDocument.GetToolStrip("modify"), m_menuItems.GetStrip("modify"));
                ToolStripManager.Merge(m_activeDocument.GetToolStrip("modules"), m_menuItems.GetStrip("modules"));
			}
			foreach (Control ctrl in Controls)
			{
				if (ctrl is ToolStripPanel)
					((ToolStripPanel)ctrl).ResumeLayout();
			}
		}
		private void OnFileOpen(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Cad XML files (*.cadxml)|*.cadxml";
			if (dlg.ShowDialog(this) == DialogResult.OK)
				OpenDocument(dlg.FileName);
		}
        private void OnGLMImport(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Gridlabd GLM files (*.glm)|*.glm";
            DialogResult results = dlg.ShowDialog(this);
            if (results != DialogResult.OK) return;
            string filename = dlg.SafeFileName.Replace(".glm", "");
            if (this.m_activeDocument == null) OpenDocument(string.Empty);
            if (this.m_activeDocument.isDirty())
            {
                DialogResult result = MessageBox.Show(this, "This file is already edited. Do you want to merge?", Program.AppName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel) return;
                if (result == DialogResult.No)
                {
                    this.m_activeDocument.Save();
                    this.m_activeDocument.Close();
                    OpenDocument(string.Empty);
                }
            }
            int FileLineCount = 0;
            string line;
            List<ModuleItems.Module> objects = new List<ModuleItems.Module>();
            Type typ = null;
            ModuleItems.Module temp = null;
            System.IO.StreamReader file = new System.IO.StreamReader(dlg.FileName);
            while ((line = file.ReadLine()) != null)
            {

                line = Regex.Replace(line, "\t", "");
                line = Regex.Replace(line,"//.*","");
                FileLineCount++;
                if (line.Trim() == string.Empty) continue;
                if (line.IndexOf("object") >= 0)
                {
                    String type = Regex.Replace(line, ".*object (.*){", "$1").Trim();
                    
                    if (type.IndexOf(":") >= 0) type = Regex.Replace(line, ".*object (.*):.*{", "$1").Trim();
                    temp = (ModuleItems.Module)m_activeDocument.Model.CreateObject(type, new UnitPoint(),null);
                    if (type.IndexOf(":") >= 0) temp.Properties.Add(new ModuleItems.Property("name",Regex.Replace(line, ".*object .*:(.*){", "$1").Trim(),""));
                    continue;
                }
                if ((line.Trim() == "}" || line.Trim() == "};")&& temp != null)
                {
                    objects.Add(temp);
                    temp = null;
                    typ = null;
                    continue;
                }
                if (temp != null)
                {
                    foreach (ModuleItems.Property p in temp.Properties) if (p.name == Regex.Replace(line, "(.*) .*", "$1").Trim()) p.value = Regex.Replace(line, ".* (.*);", "$1").Trim();
                }
                              
            }
            m_activeDocument.SetHint("Completed glm file import. Processing...");
            //connect
            UnitPoint start = new UnitPoint(0,0);
            UnitPoint end = new UnitPoint(1,0);
            UnitPoint from = new UnitPoint(1,1);
            UnitPoint to = new UnitPoint(0,-1);
            foreach (ModuleItems.Module m in objects)
            {
                if (m.tofrom || m.child)
                {
                    m.to_connections = new ModuleItems.powerflow.node();
                    m.from_connections = new ModuleItems.powerflow.node();
                    foreach (ModuleItems.Property p in m.Properties)
                    {
                        if (p.name == "to")
                            foreach (ModuleItems.Module n in objects)
                                foreach (ModuleItems.Property q in m.Properties)
                                    if (q.name == "name" && q.value == p.value) m.to_connections = n;
                        if (p.name == "from")
                            foreach (ModuleItems.Module n in objects)
                                foreach (ModuleItems.Property q in m.Properties)
                                    if (q.name == "name" && q.value == p.value) m.from_connections = n;
                        if (p.name == "parent")
                            foreach (ModuleItems.Module n in objects)
                                foreach (ModuleItems.Property q in m.Properties)
                                    if (q.name == "name" && q.value == p.value) m.from_connections = n;
                    }
                }
                m.FromPoint = m.StartPoint = start;
                m.EndPoint = m.ToPoint = end;
                if(m.tofrom){
                    m.FromPoint = from;
                    m.ToPoint = to;
                }
                if(m.child)m.FromPoint = from;
                start.X += 2;
                end.X += 2;
                from.X += 2;
                to.X += 2;
                if (start.X > 20)
                {
                    start.X = to.X = 0;
                    from.X = end.X = 1;
                    start.Y += 2;
                    end.Y += 2;
                    to.Y += 2;
                    from.Y += 2;
                }

    
                        
            }
            foreach (ModuleItems.Module m in objects)
            {
                if (m.tofrom)
                {
                    m.Move(new UnitPoint(m.FromPoint.X - m.from_connections.ToPoint.X, m.FromPoint.Y - m.from_connections.ToPoint.Y));
                    m.to_connections.Move(new UnitPoint(m.to_connections.FromPoint.X - m.ToPoint.X, m.to_connections.FromPoint.Y - m.ToPoint.Y));
                }
                if (m.child)m.Move(new UnitPoint(m.FromPoint.X - m.from_connections.ToPoint.X, m.FromPoint.Y - m.from_connections.ToPoint.Y));
            }
            foreach (ModuleItems.Module m in objects) m_activeDocument.Model.AddObject(m_activeDocument.Model.ActiveLayer, m);
            file.Close();
        }

		private void OnFileSave(object sender, EventArgs e)
		{
			DocumentForm doc = this.ActiveMdiChild as DocumentForm;
			if (doc != null)
				doc.Save();
		}
		private void OnFileNew(object sender, EventArgs e)
		{
			OpenDocument(string.Empty);
		}
		void OpenDocument(string filename)
		{
			DocumentForm f = new DocumentForm(filename);
			f.MdiParent = this;
			f.WindowState = FormWindowState.Maximized;
			f.Show();
		}

		private void OnFileSaveAs(object sender, EventArgs e)
		{
			DocumentForm doc = this.ActiveMdiChild as DocumentForm;
			if (doc != null)
				doc.SaveAs();
		}
		private void OnFileExit(object sender, EventArgs e)
		{
			Close();
		}
		private void OnUpdateMenuUI(object sender, EventArgs e)
		{


		}

		private void OnAbout(object sender, EventArgs e)
		{
			About dlg = new About();
			dlg.ShowDialog(this);
		}

		private void OnOptions(object sender, EventArgs e)
		{
			DocumentForm doc = this.ActiveMdiChild as DocumentForm;
			if (doc == null)
				return;

			Options.OptionsDlg dlg = new Canvas.Options.OptionsDlg();
			dlg.Config.Grid.CopyFromLayer(doc.Model.GridLayer as GridLayer);
			dlg.Config.Background.CopyFromLayer(doc.Model.BackgroundLayer as BackgroundLayer);
			foreach (DrawingLayer layer in doc.Model.Layers)
				dlg.Config.Layers.Add(new Options.OptionsLayer(layer));
			
			ToolStripItem item = sender as ToolStripItem;
			dlg.SelectPage(item.Tag);

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				dlg.Config.Grid.CopyToLayer((GridLayer)doc.Model.GridLayer);
				dlg.Config.Background.CopyToLayer((BackgroundLayer)doc.Model.BackgroundLayer);
				foreach (Options.OptionsLayer optionslayer in dlg.Config.Layers)
				{
					DrawingLayer layer = (DrawingLayer)doc.Model.GetLayer(optionslayer.Layer.Id);
					if (layer != null)
						optionslayer.CopyToLayer(layer);
					else
					{
						// delete layer
					}
				}

				doc.Canvas.DoInvalidate(true);
			}
		}

        private void m_mainMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void gLMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.m_activeDocument.isDirty())
            {
                string filename = "";
                DialogResult result = MessageBox.Show(this, "This file is already edited. Do you want to merge?", Program.AppName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel) return;
                if (result == DialogResult.No)
                {
                    this.m_activeDocument.Save();
                    this.m_activeDocument.Close();
                    this.m_activeDocument = new DocumentForm(filename);
                }
            }
        }

        private void m_windowMenu_Click(object sender, EventArgs e)
        {

        }

        private void Close_button_Click(object sender, EventArgs e)
        {
            PropertiesPanel.Visible = false;
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PropertiesPanel.Visible = true;

        }
        public void updateProperties(List<ModuleItems.Property> list, List<ModuleItems.Property> defaults)
        {
            for (int j = PropertiesPanel.Controls.Count-1; j > 3; j--) PropertiesPanel.Controls.RemoveAt(j);
            PropertiesPanel.Width = 200;
            if (list.Count == 0) return;
            int i = 0;

            foreach (ModuleItems.Property prop in list)
            {
                Label temp = new Label();
                temp.Text = prop.name;
                temp.TabIndex = i;
                temp.Size = DefaultLabel.Size;
                temp.Location = DefaultLabel.Location;
                temp.Top += i * 26;
                this.toolTip1.SetToolTip(temp, prop.name);

                TextBox temp2 = new TextBox();
                temp2.Font = new Font(temp2.Font, FontStyle.Bold);
                foreach (ModuleItems.Property p in defaults) if (prop.name == p.name && prop.value == p.value) temp2.Font = new Font(temp2.Font, FontStyle.Regular);
                temp2.Text = prop.value.ToString();
                temp2.Size = DefaultTextbox.Size;
                temp2.TabIndex = i + 1;
                temp2.Location = DefaultTextbox.Location;
                temp2.Top += i++ * 26;
                temp2.Tag = prop.name;
                this.toolTip1.SetToolTip(temp2, prop.value.ToString());
                temp2.TextChanged += new System.EventHandler(updateProperty);
                

                PropertiesPanel.Controls.Add(temp);
                PropertiesPanel.Controls.Add(temp2);
               
            }
            if (PropertiesPanel.VerticalScroll.Visible)PropertiesPanel.Width = 200 + System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
            
                

            this.ResizeRedraw = true;
        }
        private void updateProperty(object sender, EventArgs e){
            TextBox t = (TextBox)sender;
            this.m_activeDocument.Canvas.updateActiveProperty(new ModuleItems.Property(t.Tag.ToString(),t.Text,""), this.m_activeDocument.Canvas);
            
        }


	}
}