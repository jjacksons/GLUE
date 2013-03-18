using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.generator
{
    class inverter : Module
    {

        public inverter() { setupProperties(new List<Property>()); child = true; }
        public inverter(inverter old) { m_p1 = old.FromPoint; m_p2 = old.StartPoint; m_p3 = old.EndPoint; m_p4 = old.ToPoint; setupProperties(old.Properties); child = true; }
        private void setupProperties(List<Property> prop)
        {

            Properties = new List<Property>(9);
            DefaultProperties = new List<Property>(9);
            DefaultProperties.Add(new Property("name", "", ""));
            DefaultProperties.Add(new Property("phases", "", ""));
            DefaultProperties.Add(new Property("parent", "", ""));
            DefaultProperties.Add(new Property("inverter_type", "", ""));
            DefaultProperties.Add(new Property("generator_status", "", ""));
            DefaultProperties.Add(new Property("generator_mode", "", ""));
            DefaultProperties.Add(new Property("V_In", "", ""));
            DefaultProperties.Add(new Property("I_In", "", ""));
            DefaultProperties.Add(new Property("VA_In", "", ""));
            DefaultProperties.Add(new Property("Vdc", "", ""));
            DefaultProperties.Add(new Property("power_factor", "", ""));
            DefaultProperties.Add(new Property("Rated_kV", "", ""));
            DefaultProperties.Add(new Property("efficiency", "", ""));

            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("inverter");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("inverter@{0},{1} - L={2:f4}<{3:f4}",
                StartPoint.PosAsString(),
                EndPoint.PosAsString(),
                HitUtil.Distance(StartPoint, EndPoint),
                HitUtil.RadiansToDegrees(HitUtil.LineAngleR(StartPoint, EndPoint, 0)));
        }
        public override IDrawObject Clone()
        {
            return new inverter(this);
        }
        
        public override string Id
        {
            get { return "inverter"; }
        }
        public override string toGLM()
        {
            String s = "object inverter {" + System.Environment.NewLine;
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
                canvas.DrawLine(canvas, pen, m_p2, new UnitPoint(m_p2.X + 0.2,m_p2.Y));
                canvas.DrawLine(canvas, pen, m_p3, new UnitPoint(m_p2.X + 0.8, m_p2.Y));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.2, m_p2.Y-0.3), new UnitPoint(m_p2.X + 0.8, m_p2.Y+0.3));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.25, m_p2.Y+0.20), new UnitPoint(m_p2.X + 0.55, m_p2.Y+0.20));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.25, m_p2.Y + 0.25), new UnitPoint(m_p2.X + 0.35, m_p2.Y + 0.25));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.45, m_p2.Y + 0.25), new UnitPoint(m_p2.X + 0.55, m_p2.Y + 0.25));
                canvas.DrawArc(canvas, pen, new UnitPoint(m_p2.X + 0.70, m_p2.Y - 0.2), (float)0.05, 0, 180);
                canvas.DrawArc(canvas, pen, new UnitPoint(m_p2.X + 0.60, m_p2.Y - 0.2), (float)0.05, 0, -180);
                canvas.Graphics.DrawRectangle(pen, canvas.ToScreen(new UnitPoint(m_p2.X + 0.2,1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y + 0.3)).Y, canvas.ToScreen(new UnitPoint(m_p2.X + 0.8, 11)).X - canvas.ToScreen(new UnitPoint(m_p2.X + 0.2,1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.3)).Y- canvas.ToScreen(new UnitPoint(1, m_p2.Y + 0.3)).Y);

            }
            else
            {
                canvas.DrawLine(canvas, pen, m_p2, new UnitPoint(m_p2.X , m_p2.Y- 0.2));
                canvas.DrawLine(canvas, pen, m_p3, new UnitPoint(m_p2.X , m_p2.Y- 0.8));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X - 0.3, m_p2.Y - 0.8), new UnitPoint(m_p2.X + 0.3, m_p2.Y - 0.2));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X - 0.25, m_p2.Y - 0.30), new UnitPoint(m_p2.X + 0.05, m_p2.Y- 0.30));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X - 0.25, m_p2.Y - 0.25), new UnitPoint(m_p2.X - 0.15, m_p2.Y - 0.25));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X - 0.05, m_p2.Y - 0.25), new UnitPoint(m_p2.X + 0.05, m_p2.Y - 0.25));
                canvas.DrawArc(canvas, pen, new UnitPoint(m_p2.X + 0.20, m_p2.Y - 0.7), (float)0.05, 0, 180);
                canvas.DrawArc(canvas, pen, new UnitPoint(m_p2.X + 0.10, m_p2.Y - 0.7), (float)0.05, 0, -180);
                canvas.Graphics.DrawRectangle(pen, canvas.ToScreen(new UnitPoint(m_p2.X - 0.3, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.2)).Y, canvas.ToScreen(new UnitPoint(m_p2.X + 0.3, 11)).X - canvas.ToScreen(new UnitPoint(m_p2.X - 0.3, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.8)).Y - canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.2)).Y);

            }
        }
    }
}
