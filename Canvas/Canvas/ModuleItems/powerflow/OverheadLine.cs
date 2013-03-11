using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.powerflow
{
    class NodePointOverheadLine : NodePoint
        {
        public NodePointOverheadLine(Module owner, ePoint id) {
            m_owner = owner;
            m_clone = m_owner.Clone() as Module;
            m_pointId = id;
            m_originalPoint = GetPoint(m_pointId);
        }
        }
    class overhead_line : Module
    {

        public overhead_line() { setupProperties(new List<Property>()); }
        public overhead_line(overhead_line old) { m_p1 = old.FromPoint; m_p2 = old.StartPoint; m_p3 = old.EndPoint; setupProperties(old.Properties); }
        private void setupProperties( List<Property> prop)
        {
            Properties = new List<Property>();
            DefaultProperties = new List<Property>(6);
            DefaultProperties.Add(new Property("name", ""));
            DefaultProperties.Add(new Property("phases", ""));
            DefaultProperties.Add(new Property("from", ""));
            DefaultProperties.Add(new Property("to", ""));
            DefaultProperties.Add(new Property("length", "0"));
            DefaultProperties.Add(new Property("configuration", ""));
            if (Properties.Count ==0) foreach (Property p in DefaultProperties) Properties.Add(p);
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("overhead_line");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("overhead_line@{0},{1} - L={2:f4}<{3:f4}",
                StartPoint.PosAsString(),
                EndPoint.PosAsString(),
                HitUtil.Distance(StartPoint, EndPoint),
                HitUtil.RadiansToDegrees(HitUtil.LineAngleR(StartPoint, EndPoint, 0)));
        }
        public override IDrawObject Clone()
        {
            return new overhead_line(this);
        }
        public override INodePoint NodePoint(ICanvas canvas, UnitPoint point)
        {
            float thWidth = ThresholdWidth(canvas, Width);
            if (HitUtil.CircleHitPoint(m_p1, thWidth, point))
                return new NodePointOverheadLine(this, NodePointOverheadLine.ePoint.FromPoint);
            if (HitUtil.CircleHitPoint(m_p2, thWidth, point))
                return new NodePointOverheadLine(this, NodePointOverheadLine.ePoint.StartPoint);
            if (HitUtil.CircleHitPoint(m_p3, thWidth, point))
                return new NodePointOverheadLine(this, NodePointOverheadLine.ePoint.EndPoint);

            return null;
        }
        
        public override string Id
        {
            get { return "overhead_line"; }
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
            if(this.currentPoint != ePoint.FromPoint)canvas.DrawLine(canvas, pen, m_p3, m_p4);
            pen = new Pen(pen.Color, (float)3);
            canvas.DrawLine(canvas,pen,m_p2,m_p3);


        }
    }
}
