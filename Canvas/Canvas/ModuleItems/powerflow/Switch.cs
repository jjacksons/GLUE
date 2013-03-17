using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.powerflow
{
    class NodePointSwitch : NodePoint
    {
        public NodePointSwitch(Module owner, ePoint id)
        {
            m_owner = owner;
            m_clone = m_owner.Clone() as Module;
            m_pointId = id;
            m_originalPoint = GetPoint(m_pointId);
        }
    }
    class Switch : Module
    {

        public Switch() { setupProperties(new List<Property>()); tofrom = true; }
        public Switch(Switch old) { m_p1 = old.FromPoint; m_p2 = old.StartPoint; m_p3 = old.EndPoint; m_p4 = old.ToPoint; setupProperties(old.Properties); tofrom = true; }
        private void setupProperties(List<Property> prop)
        {

            Properties = new List<Property>(9);
            DefaultProperties = new List<Property>(9);
            DefaultProperties.Add(new Property("name", "", ""));
            DefaultProperties.Add(new Property("phases", "", ""));
            DefaultProperties.Add(new Property("from", "", ""));
            DefaultProperties.Add(new Property("to", "", ""));
            DefaultProperties.Add(new Property("status", "", ""));
            DefaultProperties.Add(new Property("phase_A_state", "", ""));
            DefaultProperties.Add(new Property("phase_B_state", "", ""));
            DefaultProperties.Add(new Property("phase_C_state", "", ""));
            DefaultProperties.Add(new Property("operating_mode", "", ""));
            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("Switch");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("Switch@{0},{1} - L={2:f4}<{3:f4}",
                StartPoint.PosAsString(),
                EndPoint.PosAsString(),
                HitUtil.Distance(StartPoint, EndPoint),
                HitUtil.RadiansToDegrees(HitUtil.LineAngleR(StartPoint, EndPoint, 0)));
        }
        public override IDrawObject Clone()
        {
            return new Switch(this);
        }
        public override string toGLM()
        {
            String s = "object switch {" + System.Environment.NewLine;
            foreach (Property p in Properties)
                foreach (Property q in DefaultProperties)
                    if (p.name == q.name && p.value.ToString() != q.value.ToString()) s = s + "    " + p.name + " " + p.value.ToString() + ";" + System.Environment.NewLine;
            s = s + "}" + System.Environment.NewLine;
            return s;
        }
        public override INodePoint NodePoint(ICanvas canvas, UnitPoint point)
        {
            float thWidth = ThresholdWidth(canvas, Width);
            if (HitUtil.CircleHitPoint(m_p1, thWidth, point))
                return new NodePointSwitch(this, NodePointSwitch.ePoint.FromPoint);
            if (HitUtil.CircleHitPoint(m_p2, thWidth, point))
                return new NodePointSwitch(this, NodePointSwitch.ePoint.StartPoint);
            if (HitUtil.CircleHitPoint(m_p3, thWidth, point))
                return new NodePointSwitch(this, NodePointSwitch.ePoint.EndPoint);
            if (HitUtil.CircleHitPoint(m_p4, thWidth, point))
                return new NodePointSwitch(this, NodePointSwitch.ePoint.ToPoint);
            return null;
        }

        public override string Id
        {
            get { return "Switch"; }
        }
        public bool isclosed()
        {
            foreach (Property p in Properties) if (p.name == "status" && (string)p.value == "CLOSED") return true;
            return false;
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
            if (m_p4 != m_p3) canvas.DrawLine(canvas, pen, m_p3, m_p4);
            pen = new Pen(pen.Color, (float)3);
            if (horizontal)
            {
                canvas.DrawLine(canvas, pen, m_p2, new UnitPoint(m_p2.X + 0.2, m_p2.Y));
                canvas.DrawArc(canvas, pen, new UnitPoint(m_p2.X + 0.2, m_p2.Y), (float)0.03, 0, 360);
                if (isclosed()) canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.2, m_p2.Y), new UnitPoint(m_p2.X + 0.8, m_p2.Y+0.05));
                else canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.2, m_p2.Y), new UnitPoint(m_p2.X + 0.8, m_p2.Y + 0.4));
                canvas.DrawArc(canvas, pen, new UnitPoint(m_p2.X + 0.8, m_p2.Y), (float)0.03, 0, 360);
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.8, m_p2.Y), m_p3);
            }
            else
            {
                canvas.DrawLine(canvas, pen, m_p2, new UnitPoint(m_p2.X, m_p2.Y + 0.2));
                canvas.DrawArc(canvas, pen, new UnitPoint(m_p2.X, m_p2.Y + 0.2), (float)0.03, 0, 360);
                if (isclosed()) canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X, m_p2.Y + 0.2), new UnitPoint(m_p2.X , m_p2.Y+ 0.8));
                else canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X, m_p2.Y + 0.2), new UnitPoint(m_p2.X + 0.4, m_p2.Y + 0.8));
                canvas.DrawArc(canvas, pen, new UnitPoint(m_p2.X , m_p2.Y+ 0.8), (float)0.03, 0, 360);
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X, m_p2.Y + 0.8), m_p3);
            }
        }
    }
}
