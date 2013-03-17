using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.generators
{

    class Solar : Module
    {

        public Solar() { setupProperties(new List<Property>()); tofrom = true; }
        public Solar(Solar old) { m_p1 = old.FromPoint; m_p2 = old.StartPoint; m_p3 = old.EndPoint; m_p4 = old.ToPoint; setupProperties(old.Properties); tofrom = true; }
        private void setupProperties(List<Property> prop)
        {

            Properties = new List<Property>(9);
            DefaultProperties = new List<Property>(9);
            DefaultProperties.Add(new Property("name", "", ""));
            DefaultProperties.Add(new Property("phases", "", ""));
            DefaultProperties.Add(new Property("from", "", ""));
            DefaultProperties.Add(new Property("to", "", ""));
            DefaultProperties.Add(new Property("current_limit", "", "Amps"));
            DefaultProperties.Add(new Property("mean_replacement_time", "", "Seconds"));
            DefaultProperties.Add(new Property("phase_A_status", "", ""));
            DefaultProperties.Add(new Property("phase_B_status", "", ""));
            DefaultProperties.Add(new Property("phase_B_status", "", ""));
            DefaultProperties.Add(new Property("repair_dist_type", "", ""));
            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("fuse");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("fuse@{0},{1} - L={2:f4}<{3:f4}",
                StartPoint.PosAsString(),
                EndPoint.PosAsString(),
                HitUtil.Distance(StartPoint, EndPoint),
                HitUtil.RadiansToDegrees(HitUtil.LineAngleR(StartPoint, EndPoint, 0)));
        }
        public override IDrawObject Clone()
        {
            return new Solar(this);
        }


        public override string Id
        {
            get { return "fuse"; }
        }
        public override string toGLM()
        {
            String s = "object fuse {" + System.Environment.NewLine;
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
                if (m_p4.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p4);
            }
            canvas.DrawLine(canvas, pen, m_p1, m_p2);
            if (this.currentPoint != ePoint.FromPoint) canvas.DrawLine(canvas, pen, m_p3, m_p4);
            pen = new Pen(pen.Color, (float)3);
            if (horizontal)
            {
                canvas.DrawLine(canvas, pen, m_p2, m_p3);
                canvas.Graphics.DrawRectangle(pen, canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.1)).X, canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.1)).Y, canvas.ToScreen(new UnitPoint(m_p2.X + 0.8, m_p2.Y - 0.1)).X - canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.1)).X, canvas.ToScreen(new UnitPoint(m_p2.X + 0.8, m_p2.Y - 0.1)).Y- canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.1)).Y);
            }
            else
            {
                canvas.DrawLine(canvas, pen, m_p2, new UnitPoint(m_p2.X, m_p2.Y + 0.2));
                canvas.Graphics.DrawRectangle(pen, canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.1)).X, canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.1)).Y, canvas.ToScreen(new UnitPoint(m_p2.X + 0.8, m_p2.Y - 0.1)).X - canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.1)).X, canvas.ToScreen(new UnitPoint(m_p2.X + 0.8, m_p2.Y - 0.1)).Y - canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.1)).Y);

            }
        }
    }
}
