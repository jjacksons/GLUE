using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.powerflow
{
    class line_spacing : ModuleItems.Module
    {

        public line_spacing() { setupProperties(new List<Property>()); }
        public line_spacing(line_spacing old) { m_p1 = old.StartPoint; m_p2 = old.EndPoint; m_p3 = old.ToPoint; setupProperties(old.Properties); }
        private void setupProperties(List<Property> prop)
        {

            Properties = new List<Property>();
            DefaultProperties = new List<Property>(6);
            DefaultProperties.Add(new Property("name", "", ""));
            DefaultProperties.Add(new Property("distance_AB", "", "feet"));
            DefaultProperties.Add(new Property("distance_BC", "", "feet"));
            DefaultProperties.Add(new Property("distance_CA", "", "feet"));
            DefaultProperties.Add(new Property("distance_AN", "", "feet"));
            DefaultProperties.Add(new Property("distance_BN", "", "feet"));
            DefaultProperties.Add(new Property("distance_CN", "", "feet"));
            DefaultProperties.Add(new Property("distance_AE", "", "feet"));
            DefaultProperties.Add(new Property("distance_BE", "", "feet"));
            DefaultProperties.Add(new Property("distance_CE", "", "feet"));
            DefaultProperties.Add(new Property("distance_NE", "", "feet"));
            
            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("line_spacing");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("line_spacing@{0}",
                StartPoint.PosAsString());
        }
        public override IDrawObject Clone()
        {
            return new line_spacing(this);
        }
        public override string toGLM()
        {
            String s = "object line_spacing {" + System.Environment.NewLine;
            foreach (Property p in Properties)
                foreach (Property q in DefaultProperties)
                    if (p.name == q.name && p.value.ToString() != q.value.ToString()) s = s + "    " + p.name + " " + p.value.ToString() + ";" + System.Environment.NewLine;
            s = s + "}" + System.Environment.NewLine;
            return s;
        }

        public override string Id
        {
            get { return "line_spacing"; }
        }
        public override void Draw(ICanvas canvas, RectangleF unitrect)
        {
            Brush B = Brushes.Black;
            Color color = Color;
            Pen pen = canvas.CreatePen(color, Width);
            if (Highlighted || Selected)
            {
                pen = Canvas.DrawTools.DrawUtils.SelectedPen;
                B = Brushes.Magenta;
                //if (m_p1.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p1);
            }
            pen = new Pen(pen.Color, (float)6);
            canvas.DrawArc(canvas, pen, m_p1, (float)0.03, 0, 360);
            float size = (canvas.ToScreen(new UnitPoint(m_p1.X + 1, m_p1.Y)).X - canvas.ToScreen(m_p1).X)/10;
            Font m_font = new System.Drawing.Font("Arial Black", size, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            canvas.Graphics.DrawString("Line Spacing", m_font, B, canvas.ToScreen(new UnitPoint(m_p1.X - 0.4, m_p1.Y + 0.2)).X, canvas.ToScreen(new UnitPoint(m_p1.X - 0.3, m_p1.Y + 0.2)).Y);

        }
    }
}
