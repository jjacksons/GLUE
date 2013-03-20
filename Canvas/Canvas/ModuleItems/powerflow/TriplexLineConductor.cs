using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.powerflow
{
    class triplex_line_conductor : Module
    {

        public triplex_line_conductor() { setupProperties(new List<Property>()); }
        public triplex_line_conductor(triplex_line_conductor old) { m_p1 = old.FromPoint; m_p2 = old.StartPoint; m_p3 = old.EndPoint; m_p4 = old.ToPoint; setupProperties(old.Properties); }
        private void setupProperties( List<Property> prop)
        {

            Properties = new List<Property>();
            DefaultProperties = new List<Property>(6);
            DefaultProperties.Add(new Property("name", "",""));
            DefaultProperties.Add(new Property("geometric_mean_radius", "", "feet"));
            DefaultProperties.Add(new Property("resistance", "", "Ohm/mile"));
            DefaultProperties.Add(new Property("diameter", "", "inches"));
            DefaultProperties.Add(new Property("rating.summer.continuous", "", " A"));
            DefaultProperties.Add(new Property("rating.summer.emergency", "", "A"));
            DefaultProperties.Add(new Property("rating.winter.continuous", "", " A"));
            DefaultProperties.Add(new Property("rating.winter.emergency", "", "A"));
            if (Properties.Count ==0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("triplex_line_conductor");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("triplex_line_conductor@{0},{1}",
                StartPoint.PosAsString(),
                EndPoint.PosAsString());
        }
        public override IDrawObject Clone()
        {
            return new triplex_line_conductor(this);
        }
        public override string toGLM()
        {
            String s = "object triplex_line_conductor {" + System.Environment.NewLine;
            foreach (Property p in Properties)
                foreach (Property q in DefaultProperties)
                    if (p.name == q.name && p.value.ToString() != q.value.ToString()) s = s + "    " + p.name + " " + p.value.ToString() + ";" + System.Environment.NewLine;
            s = s + "}" + System.Environment.NewLine;
            return s;
        }
        
        public override string Id
        {
            get { return "triplex_line_conductor"; }
        }
        public override void Draw(ICanvas canvas, RectangleF unitrect)
        {
            Color color = Color;
            Brush B = Brushes.Black;
            Pen pen = canvas.CreatePen(color, Width);
            if (Highlighted || Selected)
            {
                pen = Canvas.DrawTools.DrawUtils.SelectedPen;
                B = Brushes.Magenta;
            }
            pen = new Pen(pen.Color, (float)6);
            canvas.DrawArc(canvas, pen, m_p1, (float)0.03, 0, 360);
            float size = (canvas.ToScreen(new UnitPoint(m_p1.X + 1, m_p1.Y)).X - canvas.ToScreen(m_p1).X) / 10;
            Font m_font = new System.Drawing.Font("Arial Black", size, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            canvas.Graphics.DrawString("Tp Line Conductor", m_font, B, canvas.ToScreen(new UnitPoint(m_p1.X - 0.4, m_p1.Y + 0.2)).X, canvas.ToScreen(new UnitPoint(m_p1.X - 0.3, m_p1.Y + 0.2)).Y);

        }
    }
}
