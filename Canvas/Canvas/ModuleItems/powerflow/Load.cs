using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.powerflow
{
    class load : Module
    {

        public load() { setupProperties(new List<Property>()); this.child = true; }
        public load(load old) { m_p1 = old.FromPoint; m_p2 = old.StartPoint; m_p3 = old.EndPoint; m_p4 = old.ToPoint; setupProperties(old.Properties); this.child = true; }
        private void setupProperties(List<Property> prop)
        {
            
            Properties = new List<Property>();
            DefaultProperties = new List<Property>(6);
            DefaultProperties.Add(new Property("name", "", ""));
            DefaultProperties.Add(new Property("phases", "", ""));
            DefaultProperties.Add(new Property("nominal_voltage", "", " V"));
            DefaultProperties.Add(new Property("load_class", "", ""));
            DefaultProperties.Add(new Property("constant_power_A", "", "VA"));
            DefaultProperties.Add(new Property("constant_power_B", "", "VA"));
            DefaultProperties.Add(new Property("constant_power_C", "", "VA"));
            DefaultProperties.Add(new Property("constant_current_A", "", "A"));
            DefaultProperties.Add(new Property("constant_current_B", "", "A"));
            DefaultProperties.Add(new Property("constant_current_C", "", "A"));
            DefaultProperties.Add(new Property("constant_impedance_A", "", "OHM"));
            DefaultProperties.Add(new Property("constant_impedance_B", "", "OHM"));
            DefaultProperties.Add(new Property("constant_impedance_C", "", "OHM"));

            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("load");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("load@{0}",
                StartPoint.PosAsString());
        }
        public override IDrawObject Clone()
        {
            return new load(this);
        }

        public override string Id
        {
            get { return "load"; }
        }
        public override string toGLM()
        {
            String s = "object load {" + System.Environment.NewLine;
            foreach (Property p in Properties)
                foreach (Property q in DefaultProperties)
                    if (p.name == q.name && p.value.ToString() != q.value.ToString()) s = s + "    " + p.name + " " + p.value.ToString() + ";" + System.Environment.NewLine;
            s = s + "}" + System.Environment.NewLine;
            return s;
        }
        public override void Draw(ICanvas canvas, RectangleF unitrect)
        {
            Color color = Color;
            Pen pen = canvas.CreatePen(color, Width);
            if (Highlighted || Selected)
            {
                pen = Canvas.DrawTools.DrawUtils.SelectedPen;
                if (m_p1.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p1);
                if (m_p2.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p2);
                //if ((m_p3.X != m_p2.X + 1 || m_p3.Y != m_p2.Y) && this.horizontal) m_p3 = new UnitPoint(m_p2.X + 1, m_p2.Y);
                //if (m_p3.Y != m_p2.Y + 1 && !this.horizontal) m_p3 = new UnitPoint(m_p2.X, m_p2.Y + 1);
                if (m_p3.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p3);
            }
            canvas.DrawLine(canvas, pen, m_p1, m_p2);
            pen = new Pen(pen.Color, (float)3);
            if (horizontal)
            {
                canvas.DrawLine(canvas, pen, m_p2, new UnitPoint(m_p2.X + 0.2, m_p2.Y));
                canvas.Graphics.DrawRectangle(pen, canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y + 0.1)).Y, canvas.ToScreen(m_p3).X - canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.1)).Y - canvas.ToScreen(new UnitPoint(1, m_p2.Y + 0.1)).Y);
            }
            else
            {
                canvas.DrawLine(canvas, pen, m_p2, new UnitPoint(m_p2.X , m_p2.Y- 0.2));
                canvas.Graphics.DrawRectangle(pen, canvas.ToScreen(new UnitPoint(m_p2.X - 0.1, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.2)).Y, canvas.ToScreen(new UnitPoint(m_p2.X + 0.1, 1)).X - canvas.ToScreen(new UnitPoint(m_p2.X - 0.1, m_p2.Y + 0.2)).X, canvas.ToScreen(m_p3).Y - canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.2)).Y);

            }

        }
    }
}
