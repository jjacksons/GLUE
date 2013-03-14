using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems
{
    
    abstract class ConfObject : Canvas.DrawTools.DrawObjectBase, IDrawObject, ISerialize
    {
        protected UnitPoint m_p1, m_p2, m_p3;
        protected ePoint currentPoint = ePoint.StartPoint;
        public bool horizontal = true;
        public bool haschild = false;
        public List<Property> Properties = new List<Property>();
        public List<Property> DefaultProperties = new List<Property>();
        [XmlSerializable]
        public UnitPoint StartPoint
        {
            get { return m_p1; }
            set { m_p1 = value; }
        }
        [XmlSerializable]
        public UnitPoint EndPoint
        {
            get { return m_p2; }
            set { m_p2 = value; }
        }
        [XmlSerializable]
        public UnitPoint ToPoint
        {
            get { return m_p3; }
            set { m_p3 = value; }
        }
        public String getUnit(String name)
        {
            foreach (Property p in DefaultProperties)
            {
                if (p.name == name) return p.unit;
            }
            return String.Empty;
        }
        public enum ePoint
        {
            StartPoint,
            EndPoint,
            ToPoint
        }
        public static string ObjectType;
        public virtual void Copy(ConfObject acopy)
        {
            base.Copy(acopy);
            m_p1 = acopy.m_p1;
            m_p2 = acopy.m_p2;
            m_p3 = acopy.m_p3;
            Selected = acopy.Selected;
        }
        public abstract IDrawObject Clone();
        static int ThresholdPixel = 6;
        protected static float ThresholdWidth(ICanvas canvas, float objectwidth)
        {
            return ThresholdWidth(canvas, objectwidth, ThresholdPixel);
        }
        public static float ThresholdWidth(ICanvas canvas, float objectwidth, float pixelwidth)
        {
            double minWidth = canvas.ToUnit(pixelwidth);
            double width = Math.Max(objectwidth / 2, minWidth);
            return (float)width;
        }
        public RectangleF GetBoundingRect(ICanvas canvas)
        {
            float thWidth = ThresholdWidth(canvas, Width);
            return ScreenUtils.GetRect(m_p1, m_p3, thWidth);
        }
        UnitPoint MidPoint(ICanvas canvas, UnitPoint p1, UnitPoint p2, UnitPoint hitpoint)
        {
            UnitPoint mid = HitUtil.LineMidpoint(p1, p2);
            float thWidth = ThresholdWidth(canvas, Width);
            if (HitUtil.CircleHitPoint(mid, thWidth, hitpoint))
                return mid;
            return UnitPoint.Empty;
        }
        public bool PointInObject(ICanvas canvas, UnitPoint point)
        {
            float thWidth = ThresholdWidth(canvas, Width);
            return HitUtil.IsPointInLine(m_p1, m_p2, point, thWidth) || HitUtil.IsPointInLine(m_p2, m_p3, point, thWidth) ;
        }
        public bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint)
        {
            RectangleF boundingrect = GetBoundingRect(canvas);
            if (anyPoint)
                return HitUtil.LineIntersectWithRect(m_p1, m_p2, rect);
            return rect.Contains(boundingrect);
        }
        public abstract void Draw(ICanvas canvas, RectangleF unitrect);

        public virtual void OnMouseMove(ICanvas canvas, UnitPoint point)
        {
            if (Control.ModifierKeys == Keys.Control && currentPoint == ePoint.StartPoint) point = HitUtil.OrthoPointD(m_p1, point, 45);
            if (Control.ModifierKeys == Keys.Control && currentPoint == ePoint.ToPoint) point = HitUtil.OrthoPointD(m_p3, point, 45);
            if (currentPoint == ePoint.StartPoint)
            {
                if (this.horizontal) m_p2 = new UnitPoint(point.X + 1, point.Y);
                else m_p2 = new UnitPoint(point.X, point.Y + 1);
                m_p3 = m_p2;
            }

        }
        public virtual eDrawObjectMouseDown OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            Selected = false;
            m_p1 = point;
            m_p2 = new UnitPoint(point.X, point.Y);
            m_p3 = m_p2;
            return eDrawObjectMouseDown.Done;
            
        }
        public void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            //Draw(canvas,new RectangleF());
        }
        public virtual void OnKeyDown(ICanvas canvas, KeyEventArgs e)
        {
        }
        public UnitPoint RepeatStartingPoint
        {
            get { return m_p2; }
        }
        public void Move(UnitPoint offset)
        {
            m_p1.X += offset.X;
            m_p1.Y += offset.Y;
            if (m_p2 == m_p3)
            {
                m_p3.X += offset.X;
                m_p3.Y += offset.Y;
            }
            m_p2.X += offset.X;
            m_p2.Y += offset.Y;
            
            

        }
        public abstract string GetInfoAsString();
        public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobjs, Type[] runningsnaptypes, Type usersnaptype)
        {
            float thWidth = ThresholdWidth(canvas, Width);
            if (runningsnaptypes != null)
            {
                foreach (Type snaptype in runningsnaptypes)
                {
                    if (snaptype == typeof(VertextSnapPoint))
                    {
                        if (HitUtil.CircleHitPoint(m_p1, thWidth, point))
                            return new VertextSnapPoint(canvas, this, m_p1);
                        if (HitUtil.CircleHitPoint(m_p2, thWidth, point))
                            return new VertextSnapPoint(canvas, this, m_p2);
                    }
                    if (snaptype == typeof(MidpointSnapPoint))
                    {
                        UnitPoint p = MidPoint(canvas, m_p1, m_p2, point);
                        if (p != UnitPoint.Empty)
                            return new MidpointSnapPoint(canvas, this, p);
                    }
                    if (snaptype == typeof(IntersectSnapPoint))
                    {
                        Module otherline = Utils.FindObjectTypeInList(this, otherobjs, typeof(Module)) as Module;
                        if (otherline == null)
                            continue;
                        UnitPoint p = HitUtil.LinesIntersectPoint(m_p1, m_p2, otherline.StartPoint, otherline.EndPoint);
                        if (p != UnitPoint.Empty)
                            return new IntersectSnapPoint(canvas, this, p);
                    }
                }
                return null;
            }

            if (usersnaptype == typeof(MidpointSnapPoint))
                return new MidpointSnapPoint(canvas, this, HitUtil.LineMidpoint(m_p1, m_p2));
            if (usersnaptype == typeof(IntersectSnapPoint))
            {
                Module otherline = Utils.FindObjectTypeInList(this, otherobjs, typeof(Module)) as Module;
                if (otherline == null)
                    return null;
                UnitPoint p = HitUtil.LinesIntersectPoint(m_p1, m_p2, otherline.StartPoint, otherline.EndPoint);
                if (p != UnitPoint.Empty)
                    return new IntersectSnapPoint(canvas, this, p);
            }
            if (usersnaptype == typeof(VertextSnapPoint))
            {
                double d1 = HitUtil.Distance(point, m_p1);
                double d2 = HitUtil.Distance(point, m_p2);
                if (d1 <= d2)
                    return new VertextSnapPoint(canvas, this, m_p1);
                return new VertextSnapPoint(canvas, this, m_p2);
            }
            if (usersnaptype == typeof(NearestSnapPoint))
            {
                UnitPoint p = HitUtil.NearestPointOnLine(m_p1, m_p2, point);
                if (p != UnitPoint.Empty)
                    return new NearestSnapPoint(canvas, this, p);
            }
            if (usersnaptype == typeof(PerpendicularSnapPoint))
            {
                UnitPoint p = HitUtil.NearestPointOnLine(m_p1, m_p2, point);
                if (p != UnitPoint.Empty)
                    return new PerpendicularSnapPoint(canvas, this, p);
            }
            return null;
        }
        public abstract INodePoint NodePoint(ICanvas canvas, UnitPoint point);
        public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
        {
            StartPoint = EndPoint = ToPoint = point;
            Width = layer.Width;
            Color = layer.Color;
            Selected = true;
        }
        public virtual string Id
        {
            get { return ObjectType; }
        }
        #region ISerialize
        public abstract void GetObjectData(XmlWriter wr);
        public void AfterSerializedIn() { }
        #endregion

    }
}
