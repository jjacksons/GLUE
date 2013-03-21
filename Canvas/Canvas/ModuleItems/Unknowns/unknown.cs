using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.unknown
{
    class unknown : Module
    {
        protected string type;
        [XmlSerializable]
        public String Type
        {
            get { return type; }
            set { type = value; }
        }
        public unknown() { setupProperties(new List<Property>()); }
        public unknown(unknown old) { m_p1 = old.FromPoint; m_p2 = old.StartPoint; m_p3 = old.EndPoint; m_p4 = old.ToPoint; setupProperties(old.Properties);}
        private void setupProperties(List<Property> prop)
        {
            
            Properties = new List<Property>();
            DefaultProperties = new List<Property>(1);
            DefaultProperties.Add(new Property("name", "", ""));

            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override bool PointInObject(ICanvas canvas, UnitPoint point)
        {
            float thWidth = ThresholdWidth(canvas, Width) + (float)0.3;
            return HitUtil.IsPointInCircle(m_p1, thWidth, point, thWidth);
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("unknown");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format(type+"@{0}",
                StartPoint.PosAsString());
        }
        public override IDrawObject Clone()
        {
            return new unknown(this);
        }

        public override string Id
        {
            get { return "unknown"; }
        }
        public override string toGLM()
        {
            String s = "object "+type+" {" + System.Environment.NewLine;
            foreach (Property p in Properties)
                foreach (Property q in DefaultProperties)
                    if (p.name == q.name && p.value.ToString() != q.value.ToString()) s = s + "    " + p.name + " " + p.value.ToString() + ";" + System.Environment.NewLine;
            s = s + "}" + System.Environment.NewLine;
            return s;
        }
        public override void Draw(ICanvas canvas, RectangleF unitrect)
        {
            Color color = Color;
            Brush B = Brushes.Black;
            Pen pen = canvas.CreatePen(color, Width);
            if (Highlighted || Selected)
            {
                pen = Canvas.DrawTools.DrawUtils.SelectedPen;
                if (m_p1.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p1);
                if (m_p2.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p2);
                if (m_p3.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p3);
                B = Brushes.Magenta;
            }
            if(child || tofrom)canvas.DrawLine(canvas, pen, m_p1, m_p2);
            if (tofrom) canvas.DrawLine(canvas, pen, m_p3, m_p4);
            pen = new Pen(pen.Color, (float)3);
            if (horizontal)
            {
                float size = (canvas.ToScreen(new UnitPoint(m_p1.X + 1, m_p1.Y)).X - canvas.ToScreen(m_p1).X) / 2;
                Font m_font = new System.Drawing.Font("Arial Black", size, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                canvas.Graphics.DrawString("?", m_font, B, canvas.ToScreen(new UnitPoint(m_p2.X + (m_p3.X - m_p2.X)/2, m_p2.Y - 0.1)).X, canvas.ToScreen(new UnitPoint(m_p2.X - 0.3, m_p2.Y+0.5)).Y);
                
            }
            else
            {
                canvas.DrawLine(canvas, pen, m_p2, new UnitPoint(m_p2.X, m_p2.Y - 0.2));
                canvas.Graphics.DrawRectangle(pen, canvas.ToScreen(new UnitPoint(m_p2.X - 0.3, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.2)).Y, canvas.ToScreen(new UnitPoint(m_p2.X + 0.3, 1)).X - canvas.ToScreen(new UnitPoint(m_p2.X - 0.3, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.4)).Y - canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.2)).Y);
                float size = (canvas.ToScreen(new UnitPoint(m_p1.X + 1, m_p1.Y)).X - canvas.ToScreen(m_p1).X) / 2;
                Font m_font = new System.Drawing.Font("Arial Black", size, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                canvas.Graphics.DrawString("?", m_font, B, canvas.ToScreen(new UnitPoint(m_p2.X - 0.25, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - (m_p3.Y - m_p2.Y) / 2)).Y);
            }

        }
    }
}
