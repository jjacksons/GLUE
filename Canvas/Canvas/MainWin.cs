using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Canvas
{
    public partial class MainWin : Form
    {
        MenuItemManager m_menuItems;
        public MainWin()
        {
            UnitPoint p = HitUtil.CenterPointFrom3Points(new UnitPoint(0, 2), new UnitPoint(1.4142136f, 1.4142136f), new UnitPoint(2, 0));

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
                ToolStripManager.RevertMerge(m_menuItems.GetStrip("modules"));
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
            dlg.Filter = "GLUE XML files (*.gxml)|*.gxml";
            if (dlg.ShowDialog(this) == DialogResult.OK)
                OpenDocument(dlg.FileName);
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
        private String prevtext = " ";


        
        public void WriteOutput(string output, Color color)
        {
            if (string.IsNullOrEmpty(lastInput) == false &&
                (output == lastInput || output.Replace("\r\n", "") == lastInput))
                return;

            Invoke((Action)(() =>
            {
                //  Write the output.
                ConsoleText.SelectionColor = color;
                ConsoleText.SelectedText += output;
                inputStart = ConsoleText.SelectionStart;
            }));
        }
        private string lastInput;
        int inputStart = -1;
        private ProcessInterface.ProcessInterface processInterace = new ProcessInterface.ProcessInterface();
        private List<ProcessInterface.KeyMapping> keyMappings = new List<ProcessInterface.KeyMapping>();
        public delegate void ConsoleEventHanlder(object sender, ConsoleEventArgs args);
        public event ConsoleEventHanlder OnConsoleInput;
        public event ConsoleEventHanlder OnConsoleOutput;
        public void StartProcess(string fileName, string arguments)
        {

            //  Start the process.
            processInterace.StartProcess(fileName, arguments);
            processInterace.OnProcessOutput += new ProcessInterface.ProcessEventHanlder(processInterace_OnProcessOutput);
            processInterace.OnProcessError += new ProcessInterface.ProcessEventHanlder(processInterace_OnProcessError);
            processInterace.OnProcessInput += new ProcessInterface.ProcessEventHanlder(processInterace_OnProcessInput);
            processInterace.OnProcessExit += new ProcessInterface.ProcessEventHanlder(processInterace_OnProcessExit);
            //  If we enable input, make the control not read only.
                ConsoleText.ReadOnly = false;
                ConsolePanel.Visible = true;
        }
        void processInterace_OnProcessOutput(object sender, ProcessInterface.ProcessEventArgs args)
        {
            //  Write the output, in white
            WriteOutput(args.Content, Color.White);

            //  Fire the output event.
            FireConsoleOutputEvent(args.Content);
        }
        void processInterace_OnProcessError(object sender, ProcessInterface.ProcessEventArgs args)
        {
            //  Write the output, in red
            WriteOutput(args.Content, Color.Red);

            //  Fire the output event.
            FireConsoleOutputEvent(args.Content);
        }
        void processInterace_OnProcessInput(object sender, ProcessInterface.ProcessEventArgs args)
        {
            throw new NotImplementedException();
        }
        void processInterace_OnProcessExit(object sender, ProcessInterface.ProcessEventArgs args)
        {

            //  Read only again.
            Invoke((Action)(() =>
            {
                ConsoleText.ReadOnly = true;
            }));
        }
        public void StopProcess()
        {
            //  Stop the interface.
            processInterace.StopProcess();
            ConsoleText.ReadOnly = true;
            ConsolePanel.Visible = false;
        }

        private void FireConsoleOutputEvent(string content)
        {
            //  Get the event.
            var theEvent = OnConsoleOutput;
            if (theEvent != null)
                theEvent(this, new ConsoleEventArgs(content));
        }
        private void FireConsoleInputEvent(string content)
        {
            //  Get the event.
            var theEvent = OnConsoleInput;
            if (theEvent != null)
                theEvent(this, new ConsoleEventArgs(content));
        }
        public class ConsoleEventArgs : EventArgs
        {
            public ConsoleEventArgs() { }
            public ConsoleEventArgs(string content){Content = content;}
            public string Content { get; private set; }
        }
        public void WriteInput(string input, Color color, bool echo)
        {
            Invoke((Action)(() =>
            {
                //  Are we echoing?
                if (echo)
                {
                    ConsoleText.SelectionColor = color;
                    ConsoleText.SelectedText += input;
                    inputStart = ConsoleText.SelectionStart;
                }

                lastInput = input;

                //  Write the input.
                processInterace.WriteInput(input);

                //  Fire the event.
                FireConsoleInputEvent(input);
            }));
        }

        void ConsoleText_KeyDown(object sender, KeyEventArgs e)
        {

            //  If we're at the input point and it's backspace, bail.
            if ((ConsoleText.SelectionStart <= inputStart) && e.KeyCode == Keys.Back) e.SuppressKeyPress = true;

            //  Are we in the read-only zone?
            if (ConsoleText.SelectionStart < inputStart)
            {
                //  Allow arrows and Ctrl-C.
                if (!(e.KeyCode == Keys.Left ||
                    e.KeyCode == Keys.Right ||
                    e.KeyCode == Keys.Up ||
                    e.KeyCode == Keys.Down ||
                    (e.KeyCode == Keys.C && e.Control)))
                {
                    e.SuppressKeyPress = true;
                }
            }

            //  Is it the return key?
            if (e.KeyCode == Keys.Return)
            {
                //  Get the input.
                if (inputStart < 0) return;
                string input = ConsoleText.Text.Substring(inputStart, (ConsoleText.SelectionStart) - inputStart);

                //  Write the input (without echoing).
                WriteInput(input, Color.White, false);
            }
        }

      

        public void updateProperties(List<ModuleItems.Property> list, List<ModuleItems.Property> defaults)
        {
            for (int j = PropertiesPanel.Controls.Count - 1; j > 3; j--) PropertiesPanel.Controls.RemoveAt(j);
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
                this.toolTip1.SetToolTip(temp2, prop.value.ToString() + prop.unit);
                temp2.TextChanged += new System.EventHandler(updateProperty);


                PropertiesPanel.Controls.Add(temp);
                PropertiesPanel.Controls.Add(temp2);

            }
            if (PropertiesPanel.VerticalScroll.Visible) PropertiesPanel.Width = 200 + System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;



            this.ResizeRedraw = true;
        }
        private void updateProperty(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            this.m_activeDocument.Canvas.updateActiveProperty(new ModuleItems.Property(t.Tag.ToString(), t.Text, ""), this.m_activeDocument.Canvas);

        }


        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConsolePanel.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConsolePanel.Visible = false;
            StopProcess();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FindPanel.Visible = false;
        }
        public void showFind()
        {
            FindPanel.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Trim() == String.Empty) return;
            if (textBox3.Text.Trim() == "*" ||textBox3.Text.Trim() == "?") textBox3.Text = ".";
            if (checkBox1.Checked)
            {
                foreach (IDrawObject o in m_activeDocument.Model.ActiveLayer.Objects)
                {
                    if (o is ModuleItems.Module)
                    {
                        ModuleItems.Module temp = o as ModuleItems.Module;
                        foreach (ModuleItems.Property p in temp.Properties) if (Regex.IsMatch(p.name, textBox3.Text) && Regex.IsMatch(p.value.ToString(), ".*" + textBox1.Text + ".*")) { m_activeDocument.Model.AddSelectedObject(o); }
                    }
                }
            }
            else
            {
                List<ModuleItems.Module> found = new List<ModuleItems.Module>();
                foreach (IDrawObject o in m_activeDocument.Model.ActiveLayer.Objects)
                {
                    if (o is ModuleItems.Module)
                    {
                        ModuleItems.Module temp = o as ModuleItems.Module;
                        foreach (ModuleItems.Property p in temp.Properties)
                        {
                            if (checkBox2.Checked)
                            {
                                try
                                {
                                    if (Regex.IsMatch(p.name, textBox3.Text) && Regex.IsMatch(p.value.ToString(), ".*" + textBox1.Text + ".*")) found.Add(temp);
                                }
                                catch { 
                                    m_activeDocument.SetHint("Invalid Search Syntax");
                                    this.Invalidate(true);
                                    return;
                                }
                            }
                            else
                            {
                                if (p.name == textBox3.Text && p.value.ToString() == textBox1.Text) found.Add(temp);
                            }
                        }
                    }
                    

                }
                if (found.Count == 0)
                {
                    m_activeDocument.Model.ClearSelectedObjects();
                    m_activeDocument.SetHint("Nothing found");
                    return;
                }
                if (m_activeDocument.Model.SelectedCount == 0)
                {
                    m_activeDocument.Model.ClearSelectedObjects();
                    m_activeDocument.Model.AddSelectedObject(found[0]);
                    
                    m_activeDocument.Canvas.HandleSelection(new List<IDrawObject>{ m_activeDocument.Model.GetFirstSelected()});
                    this.Invalidate(true);
                    return;
                }
                for (int i = 0; i < found.Count; i++)
                {
                    foreach (IDrawObject d in m_activeDocument.Model.SelectedObjects) if (d == found[i])
                        {
                            if (i == found.Count - 1) i = -1;
                            m_activeDocument.Model.ClearSelectedObjects();
                            m_activeDocument.Model.AddSelectedObject(found[i + 1]);
                            m_activeDocument.Canvas.HandleSelection(new List<IDrawObject> { m_activeDocument.Model.GetFirstSelected() });
                            this.Invalidate(true);
                            return;
                        }
                }
                m_activeDocument.Model.ClearSelectedObjects();
                m_activeDocument.Model.AddSelectedObject(found[0]);
                
            }
            this.Invalidate(true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Trim() == String.Empty) return;
            if (textBox3.Text.Trim() == "*" || textBox3.Text.Trim() == "?") textBox3.Text = ".";
            int count = 0;
            if (checkBox1.Checked)
            {
                m_activeDocument.Model.ClearSelectedObjects();
                foreach (IDrawObject o in m_activeDocument.Model.ActiveLayer.Objects)
                {
                    if (o is ModuleItems.Module)
                    {
                        ModuleItems.Module temp = o as ModuleItems.Module;
                        foreach (ModuleItems.Property p in temp.Properties) if (Regex.IsMatch(p.name, textBox3.Text) && Regex.IsMatch(p.value.ToString(), ".*" + textBox1.Text + ".*")) {
                            p.value = Regex.Replace(p.value.ToString(), textBox1.Text, textBox2.Text); 
                            count++;
                            m_activeDocument.Model.AddSelectedObject(o);
                            
                        }
                    }
                }
                if (count > 0) m_activeDocument.SetHint(count+" Replacements Made"); else m_activeDocument.SetHint("Nothing found");
            }
            else
            {
                List<ModuleItems.Module> found = new List<ModuleItems.Module>();
                foreach (IDrawObject o in m_activeDocument.Model.ActiveLayer.Objects)
                {
                    if (o is ModuleItems.Module)
                    {
                        ModuleItems.Module temp = o as ModuleItems.Module;
                        foreach (ModuleItems.Property p in temp.Properties)
                        {
                            if (checkBox2.Checked)
                            {
                                try
                                {
                                    if (Regex.IsMatch(p.name, textBox3.Text) && Regex.IsMatch(p.value.ToString(), ".*" + textBox1.Text + ".*")) found.Add(temp);
                                }
                                catch
                                {
                                    m_activeDocument.SetHint("Invalid Search Syntax");
                                    this.Invalidate(true);
                                    return;
                                }
                            }
                            else
                            {
                                if (p.name == textBox3.Text && p.value.ToString() == textBox1.Text) found.Add(temp);
                            }
                        }
                    }


                }
                if (found.Count == 0)
                {
                    m_activeDocument.Model.ClearSelectedObjects();
                    m_activeDocument.SetHint("Nothing found");
                    return;
                }
                if (m_activeDocument.Model.SelectedCount == 0)
                {
                    
                    m_activeDocument.Model.ClearSelectedObjects();
                    foreach(ModuleItems.Property p in found[0].Properties)if(Regex.IsMatch(p.name, textBox3.Text) && Regex.IsMatch(p.value.ToString(), ".*" + textBox1.Text + ".*"))p.value = Regex.Replace(p.value.ToString(), textBox1.Text, textBox2.Text);
                    m_activeDocument.Model.AddSelectedObject(found[0]);
                    m_activeDocument.Canvas.HandleSelection(new List<IDrawObject> { m_activeDocument.Model.GetFirstSelected() });
                    this.Invalidate(true);
                    return;
                }
                for (int i = 0; i < found.Count; i++)
                {
                    foreach (IDrawObject d in m_activeDocument.Model.SelectedObjects) if (d == found[i])
                        {
                            if (i == found.Count - 1) i = -1;
                            m_activeDocument.Model.ClearSelectedObjects();
                            foreach (ModuleItems.Property p in found[0].Properties) if (Regex.IsMatch(p.name, textBox3.Text) && Regex.IsMatch(p.value.ToString(), ".*" + textBox1.Text + ".*")) p.value = Regex.Replace(p.value.ToString(), textBox1.Text, textBox2.Text);
                    
                            m_activeDocument.Model.AddSelectedObject(found[i + 1]);
                            m_activeDocument.Canvas.HandleSelection(new List<IDrawObject> { m_activeDocument.Model.GetFirstSelected() });
                            this.Invalidate(true);
                            return;
                        }
                }
                m_activeDocument.Model.ClearSelectedObjects();
                m_activeDocument.Model.AddSelectedObject(found[0]);

            }
            this.Invalidate(true);
        }
    }
}