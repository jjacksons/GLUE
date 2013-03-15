using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.powerflow
{
    class NodePointLC : NodePoint
    {
        public NodePointLC(ConfObject owner, ePoint id)
        {
            c_owner = owner;
            c_clone = m_owner.Clone() as ConfObject;
            m_pointId = id;
            m_originalPoint = GetPoint(m_pointId);
        }
    }
    class line_configuration : ConfObject
    {

        public line_configuration() { setupProperties(new List<Property>()); }
        public line_configuration(line_configuration old) { m_p1 = old.StartPoint; m_p2 = old.EndPoint; m_p3 = old.ToPoint; setupProperties(old.Properties); }
        private void setupProperties(List<Property> prop)
        {

            Properties = new List<Property>();
            DefaultProperties = new List<Property>(6);
            DefaultProperties.Add(new Property("name", "", ""));
            DefaultProperties.Add(new Property("conductor_A", "", ""));
            DefaultProperties.Add(new Property("conductor_B", "", ""));
            DefaultProperties.Add(new Property("conductor_C", "", ""));
            DefaultProperties.Add(new Property("conductor_N", "", ""));
            DefaultProperties.Add(new Property("spacing", "", ""));

            DefaultProperties.Add(new Property("z11", "", ""));
            DefaultProperties.Add(new Property("z12", "", ""));
            DefaultProperties.Add(new Property("z13", "", ""));
            DefaultProperties.Add(new Property("z21", "", ""));
            DefaultProperties.Add(new Property("z22", "", ""));
            DefaultProperties.Add(new Property("z23", "", ""));
            DefaultProperties.Add(new Property("z31", "", ""));
            DefaultProperties.Add(new Property("z32", "", ""));
            DefaultProperties.Add(new Property("z33", "", ""));
            
            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("line_configuration");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("line_configuration@{0}",
                StartPoint.PosAsString());
        }
        public override IDrawObject Clone()
        {
            return new line_configuration(this);
        }
        public override string toGLM()
        {
            String s = "    tobject line_configuration {" + System.Environment.NewLine;
            foreach (Property p in Properties)
                foreach (Property q in DefaultProperties)
                    if (p.name == q.name && p.value.ToString() != q.value.ToString()) s = s + "   " + p.name + " " + p.value.ToString() + ";" + System.Environment.NewLine;
            s = s + "   }" + System.Environment.NewLine;
            return s;
        }
        public override INodePoint NodePoint(ICanvas canvas, UnitPoint point)
        {
            float thWidth = ThresholdWidth(canvas, Width);
            if (HitUtil.CircleHitPoint(m_p1, thWidth, point))
                return new NodePointLC(this, NodePointLC.ePoint.FromPoint);
            if (HitUtil.CircleHitPoint(m_p2, thWidth, point))
                return new NodePointLC(this, NodePointLC.ePoint.StartPoint);
            if (HitUtil.CircleHitPoint(m_p3, thWidth, point))
                return new NodePointLC(this, NodePointLC.ePoint.EndPoint);

            return null;
        }

        public override string Id
        {
            get { return "line_configuration"; }
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

            unitrect.Inflate(3, 3);
            canvas.DrawArc(canvas, pen, m_p1, (float)0.03, 0, 360);
            float size = (canvas.ToScreen(new UnitPoint(m_p1.X + 1, m_p1.Y)).X - canvas.ToScreen(m_p1).X)/10;
            Font m_font = new System.Drawing.Font("Arial Black", size, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           
            canvas.Graphics.DrawString("Line Config", m_font, B, canvas.ToScreen(new UnitPoint(m_p1.X - 0.4, m_p1.Y + 0.2)).X, canvas.ToScreen(new UnitPoint(m_p1.X - 0.3, m_p1.Y + 0.2)).Y);

        }
    }
}
