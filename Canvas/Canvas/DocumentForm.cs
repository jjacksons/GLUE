using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Canvas
{
	public partial class DocumentForm : Form, ICanvasOwner, IEditToolOwner
	{
		CanvasCtrl m_canvas;
		DataModel m_data;

		MenuItemManager m_menuItems = new MenuItemManager();
		
		string m_filename = string.Empty;
		public DocumentForm(string filename)
		{
			InitializeComponent();

			Text = "<New Document>";
			m_data = new DataModel();
			if (filename.Length > 0 && File.Exists(filename) && m_data.Load(filename))
			{
				Text = filename;
				m_filename = filename;
			}

			m_canvas = new CanvasCtrl(this, m_data);
			m_canvas.Dock = DockStyle.Fill;
			Controls.Add(m_canvas);
			m_canvas.SetCenter(new UnitPoint(0, 0));
			m_canvas.RunningSnaps = new Type[] 
				{
				typeof(VertextSnapPoint),
				typeof(MidpointSnapPoint),
				typeof(IntersectSnapPoint),
				typeof(QuadrantSnapPoint),
				typeof(CenterSnapPoint),
				typeof(DivisionSnapPoint),
				};

			m_canvas.AddQuickSnapType(Keys.N, typeof(NearestSnapPoint));
			m_canvas.AddQuickSnapType(Keys.M, typeof(MidpointSnapPoint));
			m_canvas.AddQuickSnapType(Keys.I, typeof(IntersectSnapPoint));
			m_canvas.AddQuickSnapType(Keys.V, typeof(VertextSnapPoint));
			m_canvas.AddQuickSnapType(Keys.P, typeof(PerpendicularSnapPoint));
			m_canvas.AddQuickSnapType(Keys.Q, typeof(QuadrantSnapPoint));
			m_canvas.AddQuickSnapType(Keys.C, typeof(CenterSnapPoint));
			m_canvas.AddQuickSnapType(Keys.T, typeof(TangentSnapPoint));
			m_canvas.AddQuickSnapType(Keys.D, typeof(DivisionSnapPoint));

			m_canvas.KeyDown += new KeyEventHandler(OnCanvasKeyDown);
			SetupMenuItems();
            SetupGLMItems();
			SetupDrawTools();
			SetupLayerToolstrip();
			SetupEditTools();
            SetupModuleItems();
			UpdateLayerUI();

			MenuStrip menuitem = new MenuStrip();
			menuitem.Items.Add(m_menuItems.GetMenuStrip("edit"));
			menuitem.Items.Add(m_menuItems.GetMenuStrip("draw"));
            menuitem.Items.Add(m_menuItems.GetMenuStrip("GLM"));
			menuitem.Visible = false;
			Controls.Add(menuitem);
			this.MainMenuStrip = menuitem;
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			//m_canvas.SetCenter(m_data.CenterPoint);
		}
        static bool shownames = false;
        void SetupGLMItems()
        {
            MenuItem mmitem = m_menuItems.GetItem("Export GLM");
            mmitem.Text = "Export GLM";
            mmitem.ToolTipText = "Export GLM";
            mmitem.Click += new EventHandler(OnGLMExport);
            

            mmitem = m_menuItems.GetItem("Import GLM");
            mmitem.Text = "Import GLM";
            mmitem.ToolTipText = "Import GLM";
            mmitem.Click += new EventHandler(OnGLMImport);

            mmitem = m_menuItems.GetItem("Run Simulation");
            mmitem.Text = "Run Simulation";
            mmitem.ToolTipText = "Run Simulation (F5)";
            mmitem.Click += new EventHandler(runSimulation);
            mmitem.SingleKey = Keys.F5;
            mmitem.Tag = "Run Simulation";

            ToolStripMenuItem menu = m_menuItems.GetMenuStrip("GLM");
            menu.MergeAction = System.Windows.Forms.MergeAction.Insert;
            menu.MergeIndex = 3;
            menu.Text = "&GLM";
            menu.DropDownItems.Add(m_menuItems.GetItem("Import GLM").CreateMenuItem());
            menu.DropDownItems.Add(m_menuItems.GetItem("Export GLM").CreateMenuItem());
            menu.DropDownItems.Add(m_menuItems.GetItem("Run Simulation").CreateMenuItem());
        }
		void SetupMenuItems()
		{
			MenuItem mmitem = m_menuItems.GetItem("Undo");
			mmitem.Text = "Undo";
			mmitem.Image = MenuImages16x16.Image(MenuImages16x16.eIndexes.Undo);
			mmitem.ToolTipText = "Undo (Ctrl-Z)";
			mmitem.Click += new EventHandler(OnUndo);
			mmitem.ShortcutKeys = Shortcut.CtrlZ;

			mmitem = m_menuItems.GetItem("Redo");
			mmitem.Text = "Redo";
			mmitem.ToolTipText = "Undo (Ctrl-Y)";
			mmitem.Image = MenuImages16x16.Image(MenuImages16x16.eIndexes.Redo);
			mmitem.Click += new EventHandler(OnRedo);
			mmitem.ShortcutKeys = Shortcut.CtrlY;

			mmitem = m_menuItems.GetItem("Select");
			mmitem.Text = "Select";
			mmitem.ToolTipText = "Select (Esc)";
			mmitem.Image = DrawToolsImages16x16.Image(DrawToolsImages16x16.eIndexes.Select);
			mmitem.Click += new EventHandler(OnToolSelect);
			mmitem.ShortcutKeyDisplayString = "Esc";
			mmitem.SingleKey = Keys.Escape;
			mmitem.Tag = "select";

			mmitem = m_menuItems.GetItem("Pan");
			mmitem.Text = "Pan";
			mmitem.ToolTipText = "Pan (P)";
			mmitem.Image = DrawToolsImages16x16.Image(DrawToolsImages16x16.eIndexes.Pan);
			mmitem.Click += new EventHandler(OnToolSelect);
			mmitem.ShortcutKeyDisplayString = "P";
			mmitem.SingleKey = Keys.P;
			mmitem.Tag = "pan";

			mmitem = m_menuItems.GetItem("Move");
			mmitem.Text = "Move";
			mmitem.ToolTipText = "Move (M)";
			mmitem.Image = DrawToolsImages16x16.Image(DrawToolsImages16x16.eIndexes.Move);
			mmitem.Click += new EventHandler(OnToolSelect);
			mmitem.ShortcutKeyDisplayString = "M";
			mmitem.SingleKey = Keys.M;
			mmitem.Tag = "move";

			ToolStrip strip = m_menuItems.GetStrip("edit");
			strip.Items.Add(m_menuItems.GetItem("Select").CreateButton());
			strip.Items.Add(m_menuItems.GetItem("Pan").CreateButton());
			strip.Items.Add(m_menuItems.GetItem("Move").CreateButton());
			strip.Items.Add(new ToolStripSeparator());
			strip.Items.Add(m_menuItems.GetItem("Undo").CreateButton());
			strip.Items.Add(m_menuItems.GetItem("Redo").CreateButton());

			ToolStripMenuItem menu = m_menuItems.GetMenuStrip("edit");
			menu.MergeAction = System.Windows.Forms.MergeAction.Insert;
			menu.MergeIndex = 1;
			menu.Text = "&Edit";
			menu.DropDownItems.Add(m_menuItems.GetItem("Undo").CreateMenuItem());
			menu.DropDownItems.Add(m_menuItems.GetItem("Redo").CreateMenuItem());
			menu.DropDownItems.Add(new ToolStripSeparator());
			menu.DropDownItems.Add(m_menuItems.GetItem("Select").CreateMenuItem());
			menu.DropDownItems.Add(m_menuItems.GetItem("Pan").CreateMenuItem());
			menu.DropDownItems.Add(m_menuItems.GetItem("Move").CreateMenuItem());
		}
		void SetupDrawTools()
		{
			MenuItem mmitem = m_menuItems.GetItem("Lines");
			mmitem.Text = "Lines";
			mmitem.ToolTipText = "Lines (L)";
			mmitem.Image = DrawToolsImages16x16.Image(DrawToolsImages16x16.eIndexes.Line);
			mmitem.Click += new EventHandler(OnToolSelect);
			mmitem.SingleKey = Keys.L;
			mmitem.ShortcutKeyDisplayString = "L";
			mmitem.Tag = "lines";
			m_data.AddDrawTool(mmitem.Tag.ToString(), new DrawTools.LineEdit(false));

			mmitem = m_menuItems.GetItem("Line");
			mmitem.Text = "Line";
			mmitem.ToolTipText = "Single line (S)";
			mmitem.Image = DrawToolsImages16x16.Image(DrawToolsImages16x16.eIndexes.Line);
			mmitem.Click += new EventHandler(OnToolSelect);
			mmitem.SingleKey = Keys.S;
			mmitem.ShortcutKeyDisplayString = "S";
			mmitem.Tag = "singleline";
			m_data.AddDrawTool(mmitem.Tag.ToString(), new DrawTools.LineEdit(true));

			mmitem = m_menuItems.GetItem("Circle2P");
			mmitem.Text = "Circle 2P";
			mmitem.ToolTipText = "Circle 2 point";
			mmitem.Image = DrawToolsImages16x16.Image(DrawToolsImages16x16.eIndexes.Circle2P);
			mmitem.Click += new EventHandler(OnToolSelect);
			mmitem.Tag = "circle2P";
			m_data.AddDrawTool(mmitem.Tag.ToString(), new DrawTools.Circle(DrawTools.Arc.eArcType.type2point));

			mmitem = m_menuItems.GetItem("CircleCR");
			mmitem.Text = "Circle CR";
			mmitem.ToolTipText = "Circle Center-Radius";
			mmitem.Image = DrawToolsImages16x16.Image(DrawToolsImages16x16.eIndexes.CircleCR);
			mmitem.Click += new EventHandler(OnToolSelect);
			mmitem.SingleKey = Keys.C;
			mmitem.ShortcutKeyDisplayString = "C";
			mmitem.Tag = "circleCR";
			m_data.AddDrawTool(mmitem.Tag.ToString(), new DrawTools.Circle(DrawTools.Arc.eArcType.typeCenterRadius));

			mmitem = m_menuItems.GetItem("Arc2P");
			mmitem.Text = "Arc 2P";
			mmitem.ToolTipText = "Arc 2 point";
			mmitem.Image = DrawToolsImages16x16.Image(DrawToolsImages16x16.eIndexes.Arc2P);
			mmitem.Click += new EventHandler(OnToolSelect);
			mmitem.Tag = "arc2P";
			m_data.AddDrawTool(mmitem.Tag.ToString(), new DrawTools.Arc(DrawTools.Arc.eArcType.type2point));

			mmitem = m_menuItems.GetItem("Arc3P132");
			mmitem.Text = "Arc 3P";
			mmitem.ToolTipText = "Arc 3 point (Start / End / Include)";
			mmitem.Image = DrawToolsImages16x16.Image(DrawToolsImages16x16.eIndexes.Arc3P132);
			mmitem.Click += new EventHandler(OnToolSelect);
			mmitem.Tag = "arc3P132";
			m_data.AddDrawTool(mmitem.Tag.ToString(), new DrawTools.Arc3Point(DrawTools.Arc3Point.eArcType.kArc3P132));

			mmitem = m_menuItems.GetItem("Arc3P123");
			mmitem.Text = "Arc 3P";
			mmitem.ToolTipText = "Arc 3 point (Start / Include / End)";
			mmitem.Image = DrawToolsImages16x16.Image(DrawToolsImages16x16.eIndexes.Arc3P123);
			mmitem.Click += new EventHandler(OnToolSelect);
			mmitem.Tag = "arc3P123";
			m_data.AddDrawTool(mmitem.Tag.ToString(), new DrawTools.Arc3Point(DrawTools.Arc3Point.eArcType.kArc3P123));

			mmitem = m_menuItems.GetItem("ArcCR");
			mmitem.Text = "Arc CR";
			mmitem.ToolTipText = "Arc Center-Radius";
			mmitem.Image = DrawToolsImages16x16.Image(DrawToolsImages16x16.eIndexes.ArcCR);
			mmitem.Click += new EventHandler(OnToolSelect);
			mmitem.SingleKey = Keys.A;
			mmitem.ShortcutKeyDisplayString = "A";
			mmitem.Tag = "arcCR";
			m_data.AddDrawTool(mmitem.Tag.ToString(), new DrawTools.Arc(DrawTools.Arc.eArcType.typeCenterRadius));

			ToolStrip strip = m_menuItems.GetStrip("draw");
			strip.Items.Add(m_menuItems.GetItem("Lines").CreateButton());
			strip.Items.Add(m_menuItems.GetItem("Circle2P").CreateButton());
			strip.Items.Add(m_menuItems.GetItem("CircleCR").CreateButton());
			strip.Items.Add(m_menuItems.GetItem("Arc2P").CreateButton());
			strip.Items.Add(m_menuItems.GetItem("ArcCR").CreateButton());
			strip.Items.Add(m_menuItems.GetItem("Arc3P132").CreateButton());
			strip.Items.Add(m_menuItems.GetItem("Arc3P123").CreateButton());

			ToolStripMenuItem menu = m_menuItems.GetMenuStrip("draw");
			menu.MergeAction = System.Windows.Forms.MergeAction.Insert;
			menu.MergeIndex = 2;
			menu.Text = "Draw &Tools";
			menu.DropDownItems.Add(m_menuItems.GetItem("Lines").CreateMenuItem());
			menu.DropDownItems.Add(m_menuItems.GetItem("Line").CreateMenuItem());
			menu.DropDownItems.Add(m_menuItems.GetItem("Circle2P").CreateMenuItem());
			menu.DropDownItems.Add(m_menuItems.GetItem("CircleCR").CreateMenuItem());
			menu.DropDownItems.Add(m_menuItems.GetItem("Arc2P").CreateMenuItem());
			menu.DropDownItems.Add(m_menuItems.GetItem("ArcCR").CreateMenuItem());
			menu.DropDownItems.Add(m_menuItems.GetItem("Arc3P132").CreateMenuItem());
			menu.DropDownItems.Add(m_menuItems.GetItem("Arc3P123").CreateMenuItem());
		}
		void SetupEditTools()
		{
			MenuItem item = m_menuItems.GetItem("Meet2Lines");
			item.Text = "Meet 2 Lines";
			item.ToolTipText = "Meet 2 Lines";
			item.Image = EditToolsImages16x16.Image(EditToolsImages16x16.eIndexes.Meet2Lines);
			item.Click += new EventHandler(OnEditToolSelect);
			item.Tag = "meet2lines";
			m_data.AddEditTool(item.Tag.ToString(), new EditTools.LinesMeetEditTool(this));

			item = m_menuItems.GetItem("ShrinkExtend");
			item.Text = "Shrink or Extend";
			item.ToolTipText = "Shrink or Extend";
			item.Image = EditToolsImages16x16.Image(EditToolsImages16x16.eIndexes.LineSrhinkExtend);
			item.Click += new EventHandler(OnEditToolSelect);
			item.Tag = "shrinkextend";
			m_data.AddEditTool(item.Tag.ToString(), new EditTools.LineShrinkExtendEditTool(this));

			ToolStrip strip = m_menuItems.GetStrip("modify");
			strip.Items.Add(m_menuItems.GetItem("Meet2Lines").CreateButton());
			strip.Items.Add(m_menuItems.GetItem("ShrinkExtend").CreateButton());
			m_toolHint = string.Empty;
		}
        void SetupModuleItems()
        {
            ToolStripMenuItem item = m_menuItems.GetMenuStrip("powerflow");
            item.ToolTipText = "Powerflow Module";
            item.Image = ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow);

            item.DropDownItems.Add("node", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow),new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("node", new ModuleItems.powerflow.node());
            item.DropDownItems.Add("line_configuration", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow),new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("line_configuration", new ModuleItems.powerflow.line_configuration());
            item.DropDownItems.Add("line_spacing", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow), new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("line_spacing", new ModuleItems.powerflow.line_spacing());
            item.DropDownItems.Add("overhead_line", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow), new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("overhead_line", new ModuleItems.powerflow.overhead_line());
            item.DropDownItems.Add("overhead_line_conductor", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow), new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("overhead_line_conductor", new ModuleItems.powerflow.overhead_line_conductor());
            item.DropDownItems.Add("underground_line", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow), new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("underground_line", new ModuleItems.powerflow.underground_line());
            item.DropDownItems.Add("underground_line_conductor", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow), new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("underground_line_conductor", new ModuleItems.powerflow.underground_line_conductor());
            item.DropDownItems.Add("Triplex line", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Triplex Line Configuration", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Triplex Conductor", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("transformer", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow), new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("transformer", new ModuleItems.powerflow.transformer());
            item.DropDownItems.Add("Transformer Configuration", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("load", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow), new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("load", new ModuleItems.powerflow.load());
            item.DropDownItems.Add("meter", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow), new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("meter", new ModuleItems.powerflow.meter());
            item.DropDownItems.Add("Triplex Node", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Triplex Meter", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("regulator", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow), new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("regulator", new ModuleItems.powerflow.regulator());
            item.DropDownItems.Add("Regulator Configuration", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Capacitor", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("fuse", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow), new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("fuse", new ModuleItems.powerflow.fuse());
            item.DropDownItems.Add("switch", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow), new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("switch", new ModuleItems.powerflow.Switch());
            item.DropDownItems.Add("Recloser", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Relay", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Substation", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Parametric Load", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Volt-VAr Control", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Volt Dump", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Current Dump", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Bill Dump", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Fault Check", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Frequency Generator", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Motor", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Restoration", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Series Reactor", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Sectionalizer", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Power Metrics", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            item.DropDownItems.Add("Emissions", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.powerflow));
            
            item = m_menuItems.GetMenuStrip("generator");
            item.ToolTipText = "Generators Module";
            item.Image = ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.generators);
            item.Tag = "generator";

            item.DropDownItems.Add("solar", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.generators), new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("solar", new ModuleItems.generator.solar());
            item.DropDownItems.Add("inverter", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.generators), new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("inverter", new ModuleItems.generator.inverter());

            item = m_menuItems.GetMenuStrip("tape");
            item.ToolTipText = "Tape Module";
            item.Image = ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.tape);
            item.Tag = "Tape";

            item.DropDownItems.Add("player", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.tape), new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("player", new ModuleItems.tape.player());
            item.DropDownItems.Add("recorder", ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.tape), new EventHandler(OnModuleSelect));
            m_data.AddDrawTool("recorder", new ModuleItems.tape.recorder());

            MenuItem mmitem = m_menuItems.GetItem("clock");
            mmitem.Text = "clock";
            mmitem.ToolTipText = "Gridlab Simulation Clock";
            mmitem.Image = ModuleItemsImages16x16.Image(ModuleItemsImages16x16.eIndexes.clock);
            mmitem.Click += new EventHandler(OnModuleSelect);
            m_data.AddDrawTool("clock", new ModuleItems.clock());


            ToolStrip strip = m_menuItems.GetStrip("modules");
            strip.Items.Add(m_menuItems.GetItem("clock").CreateButton());
            strip.Items.Add(m_menuItems.GetMenuStrip("powerflow"));
            strip.Items.Add(m_menuItems.GetMenuStrip("generator"));
            strip.Items.Add(m_menuItems.GetMenuStrip("tape"));
            m_toolHint = string.Empty;
        }


		ToolStripStatusLabel m_mousePosLabel = new ToolStripStatusLabel();
		ToolStripStatusLabel m_snapInfoLabel = new ToolStripStatusLabel();
		ToolStripStatusLabel m_drawInfoLabel = new ToolStripStatusLabel();
		ToolStripComboBox m_layerCombo = new ToolStripComboBox();

		void SetupLayerToolstrip()
		{
			StatusStrip status = m_menuItems.GetStatusStrip("status");
			m_mousePosLabel.AutoSize = true;
			m_mousePosLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right;
			m_mousePosLabel.Size = new System.Drawing.Size(110, 17);
			status.Items.Add(m_mousePosLabel);

			m_snapInfoLabel.AutoSize = true;
			m_snapInfoLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right;
			m_snapInfoLabel.Size = new System.Drawing.Size(200, 17);
			status.Items.Add(m_snapInfoLabel);

			//m_drawInfoLabel.AutoSize = true;
			m_drawInfoLabel.Spring = true;
			m_drawInfoLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right;
			m_drawInfoLabel.TextAlign = ContentAlignment.MiddleLeft;
			m_drawInfoLabel.Size = new System.Drawing.Size(200, 17);
			status.Items.Add(m_drawInfoLabel);

			ToolStrip strip = m_menuItems.GetStrip("layer");
			strip.Items.Add(new ToolStripLabel("Active Layer"));

			m_layerCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			int index = 1;
			foreach (DrawingLayer layer in m_data.Layers)
			{
				string name = string.Format("({0}) - {1}", index, layer.Name);

				MenuItem mmitem = m_menuItems.GetItem(name);
				mmitem.Text = name;
				mmitem.Image = DrawToolsImages16x16.Image(DrawToolsImages16x16.eIndexes.ArcCR);
				mmitem.Click += new EventHandler(OnLayerSelect);
				mmitem.SingleKey = Keys.D0 + index;
				mmitem.Tag = new CommonTools.NameObject<DrawingLayer>(mmitem.Text, layer);

				m_layerCombo.Items.Add(new CommonTools.NameObject<DrawingLayer>(mmitem.Text, layer));
				m_layerCombo.SelectedIndexChanged += mmitem.Click;
				
				index++;
			}
			strip.Items.Add(m_layerCombo);
		}
		public ToolStrip GetToolStrip(string id)
		{
			return m_menuItems.GetStrip(id);
		}
		public void Save()
		{
			UpdateData();
			if (m_filename.Length == 0)
				SaveAs();
			else
				m_data.Save(m_filename);
		}
		public void SaveAs()
		{
			UpdateData();
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Filter = "GLUE XML files (*.gxml)|*.gxml";
			dlg.OverwritePrompt = true;
			if (m_filename.Length > 0)
				dlg.FileName = m_filename;
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				m_filename = dlg.FileName;
				m_data.Save(m_filename);
				Text = m_filename;
			}
		}
		public CanvasCtrl Canvas
		{
			get { return m_canvas ;}
		}
		public DataModel Model
		{
			get { return m_data; }
		}
		void UpdateData()
		{
			// update any additional properties of data which is not part of the interface
			m_data.CenterPoint = m_canvas.GetCenter();
		}
        void OnModuleSelect(object sender, System.EventArgs e) { m_canvas.CommandSelectDrawTool(((ToolStripItem)sender).Text); }
        void runSimulation(object sender, System.EventArgs e) { runglm(); }
        public void runglm()
        {
                MainWin temps = (MainWin)this.Parent.Parent;
                export("temp.glm");
                temps.StartProcess("cmd.exe", "gridlabd -v temp.glm");
                
        }

		void OnToolSelect(object sender, System.EventArgs e)
		{
			string toolid = string.Empty;
			bool fromKeyboard = false;
			if (sender is MenuItem) // from keyboard
			{
				toolid = ((MenuItem)sender).Tag.ToString();
				fromKeyboard = true;
			}
			if (sender is ToolStripItem) // from menu or toolbar
			{
				toolid = ((ToolStripItem)sender).Tag.ToString();
			}
			if (toolid == "select")
			{
				m_canvas.CommandEscape();
				return;
			}
			if (toolid == "pan")
			{
				m_canvas.CommandPan();
				return;
			}
			if (toolid == "move")
			{
				// if from keyboard then handle immediately, if from mouse click then only switch mode
				m_canvas.CommandMove(fromKeyboard);
				return;
			}
			m_canvas.CommandSelectDrawTool(toolid);
		}
		void OnEditToolSelect(object sender, System.EventArgs e)
		{
			string toolid = string.Empty;
			//bool fromKeyboard = false;
			if (sender is MenuItem) // from keyboard
			{
				toolid = ((MenuItem)sender).Tag.ToString();
				//fromKeyboard = true;
			}
			if (sender is ToolStripItem) // from menu or toolbar
			{
				toolid = ((ToolStripItem)sender).Tag.ToString();
			}
			m_canvas.CommandEdit(toolid);
		}
		void UpdateLayerUI()
		{
			CommonTools.NameObject<DrawingLayer> selitem = m_layerCombo.SelectedItem as CommonTools.NameObject<DrawingLayer>;
			if (selitem == null || selitem.Object != m_data.ActiveLayer)
			{
				foreach (CommonTools.NameObject<DrawingLayer> obj in m_layerCombo.Items)
				{
					if (obj.Object == m_data.ActiveLayer)
						m_layerCombo.SelectedItem = obj;
				}
			}
		}
        private void OnGLMExport(object sender, EventArgs e)
        {

            UpdateData();
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "GLM File (*.glm)|*.glm";
            dlg.OverwritePrompt = true;
            if (m_filename.Length > 0)
                dlg.FileName = m_filename;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                export(dlg.FileName);
                m_filename = dlg.FileName;
            }


        }
        private void export(String filename)
        {

            if (!isDirty()) return;
            String export = "";
            foreach (object o in m_canvas.Model.ActiveLayer.Objects)
            {
                if (o.GetType().ToString().IndexOf("Module") > 0)
                {
                    ModuleItems.Module temp = o as ModuleItems.Module;
                    export = export + temp.toGLM();
                }
                export = export + System.Environment.NewLine;
            }
            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            file.WriteLine(export);
            file.Close();
        }
        private void OnGLMImport(object sender, EventArgs e)
        {
            importFile(String.Empty);
        }
        private void importFile(String filename)
        {

            String path = "";
            bool multifile = false;
            bool multiimport = false;
            if (filename.Trim().Length == 0)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Gridlabd GLM files (*.glm)|*.glm";
                DialogResult results = dlg.ShowDialog(this);
                if (results != DialogResult.OK) return;
                filename = dlg.FileName;
                path = filename.Remove(filename.IndexOf(dlg.SafeFileName));
            }

           
            if (this.isDirty())
            {
                DialogResult result = MessageBox.Show(this, "This file is already edited. Do you want to merge?", Program.AppName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel) return;
                if (result == DialogResult.No)
                {
                    this.Save();
                    this.Close();
                }
            }

            int FileLineCount = 0;
            string line;
            List<ModuleItems.Module> objects = new List<ModuleItems.Module>();
            ModuleItems.Module temp = null;
            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            String prevline = "";

            while ((line = file.ReadLine()) != null)
            {

                line = Regex.Replace(line, "\t", "");
                line = Regex.Replace(line, "//.*", "").Trim();
                FileLineCount++;
                if (line.Trim() == string.Empty) continue;
                prevline = line.Trim();
                if (line.Trim() == "{") line = prevline + " {";
                if (line.IndexOf("#") >= 0 && !multifile)
                {
                    DialogResult result = MessageBox.Show(this, "This file used external files. Do you want to import?", Program.AppName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    multifile = true;
                    if (result == DialogResult.Cancel) return;
                    multiimport = result == System.Windows.Forms.DialogResult.Yes;
                }
                if (line.IndexOf("#include") >= 0 && multiimport) importFile(path + Regex.Replace(line, ".*\"(.*)\"", "$1"));
                if (line.IndexOf("object") >= 0)
                {
                    String type = Regex.Replace(line, ".*object (.*){", "$1").Trim();

                    if (type.IndexOf(":") >= 0) type = Regex.Replace(line, ".*object (.*):.*{", "$1").Trim();
                    temp = (ModuleItems.Module)Model.CreateObject(type, new UnitPoint(), null);
                    if (type.IndexOf(":") >= 0) temp.Properties.Add(new ModuleItems.Property("name", Regex.Replace(line, ".*object .*:(.*){", "$1").Trim(), ""));
                    continue;
                }
                if ((line.Trim() == "}" || line.Trim() == "};") && temp != null)
                {
                    objects.Add(temp);
                    temp = null;
                    continue;
                }
                if (temp != null)
                {
                    line = Regex.Replace(line, "(.*) (.*) .*;", "$1 $2;").Trim();
                    foreach (ModuleItems.Property p in temp.Properties) if (p.name == Regex.Replace(line, "(.*) .*", "$1").Trim()) p.value = Regex.Replace(line, ".* (.*);", "$1").Trim();
                    continue;
                }
                if (line.IndexOf("module") >= 0)
                {
                    //todo adda a module object
                }

            }
            file.Close();
            SetHint("Completed glm file import. Processing...");
            UnitPoint start = new UnitPoint(0, 0);
            while (objects.Count > 0)
            {

                ModuleItems.Module m = objects[0];
                objects.Remove(m);
                m.FromPoint = m.StartPoint = m.ToPoint = m.EndPoint = start;
                if (m.tofrom)
                {
                    m.FromPoint = new UnitPoint(start.X + 1,start.Y + 1);
                    m.EndPoint = new UnitPoint(start.X + 1, start.Y);
                    m.ToPoint = new UnitPoint(start.X , start.Y - 1);
                }
                if (m.child)
                {
                    m.FromPoint = new UnitPoint(start.X + 1, start.Y + 1);
                    m.EndPoint = new UnitPoint(start.X + 1, start.Y);
                }
                Model.AddObject(Model.ActiveLayer, m);

                Trace(m,objects);
                start.X++;
                start.X++;
                start.X++;
                if (start.X > 50)
                {
                    start.X = 0;
                    start.Y += 3;
                }
            }


            Invalidate();
            SetHint("");
        }
        private void Trace(ModuleItems.Module m, List<ModuleItems.Module> l)
        {
            CanvasWrapper c = new CanvasWrapper(m_canvas);
            List<ModuleItems.Module> toremove = new List<ModuleItems.Module>();
            foreach (ModuleItems.Property p in m.Properties)
                if (p.name == "name")
                    foreach (ModuleItems.Module n in l)
                        foreach (ModuleItems.Property q in n.Properties)
                            if ((q.name == "from" && q.value.ToString() == p.value.ToString()) || (q.name == "parent" && q.value.ToString() == p.value.ToString())) toremove.Add(n);
            foreach (ModuleItems.Property p in m.Properties)
                if (p.name == "to")
                    foreach (ModuleItems.Module n in l)
                        foreach (ModuleItems.Property q in n.Properties)
                            if ((q.name == "name" && q.value.ToString() == p.value.ToString())) toremove.Add(n);
            
            while (toremove.Count > 0)
            {
                int spacer = toremove.Count - 1;
                while(m_canvas.Model.GetHitObjects(c, new UnitPoint(m.ToPoint.X + spacer * 2, m.ToPoint.Y - 1)).Count > 0) spacer++;
                toremove[0].FromPoint = toremove[0].StartPoint = toremove[0].ToPoint = toremove[0].EndPoint = m.ToPoint;
                if (toremove[0].tofrom)
                {
                    toremove[0].StartPoint = new UnitPoint(m.ToPoint.X + spacer*2, m.ToPoint.Y-1);
                    toremove[0].EndPoint = new UnitPoint(m.ToPoint.X + 1 + spacer * 2, m.ToPoint.Y - 1);
                    toremove[0].ToPoint = new UnitPoint(m.ToPoint.X + spacer * 2, m.ToPoint.Y - 2);
                }
                if (toremove[0].child)
                {
                    toremove[0].StartPoint = new UnitPoint(m.ToPoint.X + spacer * 2, m.ToPoint.Y - 1);
                    toremove[0].EndPoint = toremove[0].ToPoint = new UnitPoint(m.ToPoint.X + 1 + spacer * 2, m.ToPoint.Y - 1);
                }
                Model.AddObject(Model.ActiveLayer, toremove[0]);
                l.Remove(toremove[0]);
                Trace(toremove[0],l);
                toremove.Remove(toremove[0]);
            }

        }



		void OnLayerSelect(object sender, System.EventArgs e)
		{
			CommonTools.NameObject<DrawingLayer> obj = null;
			if (sender is ToolStripComboBox)
				obj = ((ToolStripComboBox)sender).SelectedItem as CommonTools.NameObject<DrawingLayer>;
			if (sender is MenuItem)
				obj = ((MenuItem)sender).Tag as CommonTools.NameObject<DrawingLayer>;
			if (obj == null)
				return;
			m_data.ActiveLayer = obj.Object as DrawingLayer;
			m_canvas.DoInvalidate(true);
			UpdateLayerUI();
		}
		void OnUndo(object sender, System.EventArgs e)
		{
			if (m_data.DoUndo())
				m_canvas.DoInvalidate(true);
		}
		void OnRedo(object sender, System.EventArgs e)
		{
			if (m_data.DoRedo())
				m_canvas.DoInvalidate(true);
		}
		public void UpdateUI()
		{
			m_menuItems.GetItem("Undo").Enabled = m_data.CanUndo();
			m_menuItems.GetItem("Redo").Enabled = m_data.CanRedo();
			m_menuItems.GetItem("Move").Enabled = m_data.SelectedCount > 0;
		}
		void OnCanvasKeyDown(object sender, KeyEventArgs e)
		{
			if (Control.ModifierKeys != Keys.None)
				return;

			MenuItem item = m_menuItems.FindFromSingleKey(e.KeyCode);
			if (item != null && item.Click != null)
			{
				item.Click(item, null);
				e.Handled = true;
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (m_data.IsDirty)
			{
				string s = "Save Changes to " + Path.GetFileName(m_filename) + "?";
				DialogResult result = MessageBox.Show(this, s, Program.AppName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if (result == DialogResult.Cancel)
				{
					e.Cancel = true;
					return;
				}
				if (result == DialogResult.Yes)
					Save();
			}
			m_menuItems.DisableAll();
			base.OnFormClosing(e);
		}
		#region ICanvasOwner Members
		public void SetPositionInfo(UnitPoint unitpos)
		{
			m_mousePosLabel.Text = unitpos.PosAsString();
			string s = string.Empty;
			if (m_data.SelectedCount == 1 || m_canvas.NewObject != null)
			{
				IDrawObject obj = m_data.GetFirstSelected();
				if (obj == null)
					obj = m_canvas.NewObject;
				if (obj != null)
					s = obj.GetInfoAsString();
			}
			if (m_toolHint.Length > 0)
				s = m_toolHint;
			if (s != m_drawInfoLabel.Text)
				m_drawInfoLabel.Text = s;
		}
		public void SetSnapInfo(ISnapPoint snap)
		{
			m_snapHint = string.Empty;
			if (snap != null)
				m_snapHint = string.Format("Snap@{0}, {1}", snap.SnapPoint.PosAsString(), snap.GetType());
			m_snapInfoLabel.Text = m_snapHint;
		}
		#endregion
		#region IEditToolOwner
		public void SetHint(string text)
		{
			m_toolHint = text;
			m_drawInfoLabel.Text = m_toolHint;
			//SetHint();
		}
        public bool isDirty()
        {
            return m_data.IsDirty; 
        }
		#endregion
		string m_toolHint = string.Empty;
		string m_snapHint = string.Empty;
		/*
		void SetHint()
		{
			if (m_toolHint.Length > 0)
				m_snapInfoLabel.Text = m_toolHint;
			else
				m_snapInfoLabel.Text = m_snapHint;
		}
		*/
	}
}