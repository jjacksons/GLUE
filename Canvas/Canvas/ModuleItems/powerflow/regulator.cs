using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.powerflow
{
    class regulator : Module
    {

        public regulator() { setupProperties(new List<Property>()); tofrom = true; }
        public regulator(regulator old) { m_p1 = old.FromPoint; m_p2 = old.StartPoint; m_p3 = old.EndPoint; m_p4 = old.ToPoint; setupProperties(old.Properties); tofrom = true; }
        private void setupProperties(List<Property> prop)
        {

            Properties = new List<Property>(9);
            DefaultProperties = new List<Property>(9);
            DefaultProperties.Add(new Property("name", "", ""));
            DefaultProperties.Add(new Property("phases", "", ""));
            DefaultProperties.Add(new Property("from", "", ""));
            DefaultProperties.Add(new Property("to", "", ""));
            DefaultProperties.Add(new Property("tap_A", "", "Kelvin"));
            DefaultProperties.Add(new Property("tap_B", "", "sec"));
            DefaultProperties.Add(new Property("tap_C", "", "C"));
            DefaultProperties.Add(new Property("sense_node", "", ""));
            DefaultProperties.Add(new Property("configuration", "", ""));
            DefaultProperties.Add(new Property("tap_A_change_count", "", ""));
            DefaultProperties.Add(new Property("tap_B_change_count", "", "Celsius"));
            DefaultProperties.Add(new Property("tap_C_change_count", "", ""));
            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("regulator");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("regulator@{0},{1} - T={2}, F={3}",
                StartPoint.PosAsString(),
                EndPoint.PosAsString(),
                ToPoint.PosAsString(),
                FromPoint.PosAsString());
        }
        public override IDrawObject Clone()
        {
            return new regulator(this);
        }
        
        public override string Id
        {
            get { return "regulator"; }
        }
        public override string toGLM()
        {
            String s = "object regulator {" + System.Environment.NewLine;
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
                if (m_p3.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p3);
                if (m_p4.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p4);
            }
            canvas.DrawLine(canvas, pen, m_p1, m_p2);
            if (m_p3 != m_p4) canvas.DrawLine(canvas, pen, m_p3, m_p4);
            pen = new Pen(pen.Color, (float)3);
            if (horizontal)
            {
                canvas.DrawLine(canvas, pen, m_p2, new UnitPoint(m_p2.X+0.2,m_p2.Y));
                canvas.DrawLine(canvas, pen, m_p3, new UnitPoint(m_p2.X + 0.8, m_p2.Y));
                canvas.DrawArc(canvas, pen, new UnitPoint(m_p2.X + 0.61, m_p2.Y), (float)0.19, 0, 360);
                canvas.DrawArc(canvas, pen, new UnitPoint(m_p2.X + 0.39, m_p2.Y), (float)0.19, 0, 360);
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.3, m_p2.Y-0.3), new UnitPoint(m_p2.X+0.71, m_p2.Y+0.31));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.7, m_p2.Y + 0.2), new UnitPoint(m_p2.X + 0.7, m_p2.Y + 0.3));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.6, m_p2.Y + 0.3), new UnitPoint(m_p2.X + 0.7, m_p2.Y + 0.3));
            }
            else
            {
                canvas.DrawLine(canvas, pen, m_p2, new UnitPoint(m_p2.X, m_p2.Y - 0.2));
                canvas.DrawLine(canvas, pen, m_p3, new UnitPoint(m_p2.X, m_p2.Y - 0.8));
                canvas.DrawArc(canvas, pen, new UnitPoint(m_p2.X, m_p2.Y - 0.61), (float)0.19, 0, 360);
                canvas.DrawArc(canvas, pen, new UnitPoint(m_p2.X, m_p2.Y - 0.39), (float)0.19, 0, 360);
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X - 0.3, m_p2.Y - 0.3), new UnitPoint(m_p2.X + 0.31, m_p2.Y - 0.71));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.2, m_p2.Y - 0.7), new UnitPoint(m_p2.X + 0.3, m_p2.Y - 0.7));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.3, m_p2.Y - 0.6), new UnitPoint(m_p2.X + 0.3, m_p2.Y - 0.7));
                
            }
        }
    }
}
