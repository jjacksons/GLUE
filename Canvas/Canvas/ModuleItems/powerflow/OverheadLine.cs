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
    class OverheadLine : Module
    {

        public OverheadLine() { setupProperties(); }
        public OverheadLine(UnitPoint frompoint, UnitPoint startpoint, UnitPoint endpoint) { m_p1 = frompoint; m_p2 = startpoint; m_p3 = endpoint; setupProperties(); }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("OverheadLine");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }
        public override string GetInfoAsString()
        {
            return string.Format("OverheadLine@{0},{1} - L={2:f4}<{3:f4}",
                StartPoint.PosAsString(),
                EndPoint.PosAsString(),
                HitUtil.Distance(StartPoint, EndPoint),
                HitUtil.RadiansToDegrees(HitUtil.LineAngleR(StartPoint, EndPoint, 0)));
        }
        public override IDrawObject Clone()
        {
            return new OverheadLine(m_p1, m_p2, m_p3);
        }
        public override INodePoint NodePoint(ICanvas canvas, UnitPoint point)
        {
            float thWidth = ThresholdWidth(canvas, Width);
            if (HitUtil.CircleHitPoint(m_p1, thWidth, point))
                return new NodePointOverheadLine(this, NodePointOverheadLine.ePoint.FromPoint);
            if (HitUtil.CircleHitPoint(m_p2, thWidth, point))
                return new NodePointOverheadLine(this, NodePointOverheadLine.ePoint.StartPoint);
            if (HitUtil.CircleHitPoint(m_p3, thWidth, point))
                return new NodePointOverheadLine(this, NodePointOverheadLine.ePoint.StartPoint);

            return null;
        }
        private void setupProperties()
        {
            for (int i = 1; i < 50; i++)
            {
                properties.Add(new Property("nameqwertyuiopasdfghjkl" + i, "propqwertyuiopasdfghjkl" + i));
            }
        }
        public override string Id
        {
            get { return "OverheadLine"; }
        }
        public override void Draw(ICanvas canvas, RectangleF unitrect)
        {
            Color color = Color;
            Pen pen = canvas.CreatePen(color, Width);
            if (Highlighted||Selected) pen = Canvas.DrawTools.DrawUtils.SelectedPen;
            canvas.DrawLine(canvas, pen, m_p1, m_p2);
            pen = new Pen(pen.Color, (float)3);
            canvas.DrawLine(canvas,pen,m_p2,new UnitPoint(m_p2.X+1,m_p2.Y ));
            if (m_p1.IsEmpty == false)Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p1);
            if (m_p2.IsEmpty == false)Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p2);
            if (m_p3.X != m_p2.X + 1) m_p3 = new UnitPoint(m_p2.X + 1, m_p2.Y);
            if (m_p3.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p3);

        }
    }
}
