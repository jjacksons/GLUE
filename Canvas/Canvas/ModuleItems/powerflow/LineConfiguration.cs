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
        public NodePointLC(line_configuration owner, ePoint id)
        {
            c_owner = owner;
            m_clone = m_owner.Clone() as Module;
            m_pointId = id;
            m_originalPoint = GetPoint(m_pointId);
        }
    }
    class line_configuration : ConfObject
    {

        public line_configuration() { setupProperties(new List<Property>()); haschild = true; }
        public line_configuration(line_configuration old) { m_p1 = old.StartPoint; m_p2 = old.EndPoint; m_p3 = old.ToPoint; setupProperties(old.Properties); haschild = true; }
        private void setupProperties(List<Property> prop)
        {
            Properties = new List<Property>(9);
            DefaultProperties = new List<Property>(9);
            DefaultProperties.Add(new Property("name", "", ""));
            DefaultProperties.Add(new Property("phases", "", ""));
            DefaultProperties.Add(new Property("from", "", ""));
            DefaultProperties.Add(new Property("to", "", ""));
            DefaultProperties.Add(new Property("status", "CLOSED", ""));
            DefaultProperties.Add(new Property("phase_A_state", "CLOSED", ""));
            DefaultProperties.Add(new Property("phase_B_state", "CLOSED", ""));
            DefaultProperties.Add(new Property("phase_C_state", "CLOSED", ""));
            DefaultProperties.Add(new Property("operating_mode", "BANKED", ""));
            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p);
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("line_configuration");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("Line Configuration@{0},{1} - L={2:f4}<{3:f4}",
                StartPoint.PosAsString(),
                EndPoint.PosAsString(),
                HitUtil.Distance(StartPoint, EndPoint),
                HitUtil.RadiansToDegrees(HitUtil.LineAngleR(StartPoint, EndPoint, 0)));
        }
        public override IDrawObject Clone()
        {
            return new line_configuration(this);
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
            Color color = Color;
            Pen pen = canvas.CreatePen(color, Width);
            if (Highlighted || Selected)
            {
                pen = Canvas.DrawTools.DrawUtils.SelectedPen;
                if (m_p1.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p1);
                if (m_p2.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p2);
                if (m_p3.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p3);
            }
            canvas.DrawLine(canvas, pen, m_p1, m_p2);
            pen = new Pen(pen.Color, (float)3);
            canvas.Graphics.DrawRectangle(pen, canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.1)).X, canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.1)).Y, canvas.ToScreen(new UnitPoint(m_p2.X + 0.8, m_p2.Y - 0.1)).X - canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.1)).X, canvas.ToScreen(new UnitPoint(m_p2.X + 0.8, m_p2.Y - 0.1)).Y - canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.1)).Y);


        }
    }
}
