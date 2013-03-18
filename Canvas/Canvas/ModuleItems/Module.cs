using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems
{
    class NP : INodePoint {
        public enum ePoint {
            FromPoint,
            StartPoint,
            EndPoint,
            ToPoint
        }
        public enum eorientation {
            Vertical,
            Horizontal,
        }
        protected Module m_owner;
        protected Module m_clone;
        protected UnitPoint m_originalPoint;
        protected UnitPoint m_endPoint;
        protected ePoint m_pointId;
        static bool m_angleLocked = false;
        public NP() { }
        public NP(Module owner, ePoint id)
        {
            m_owner = owner;
            m_clone = m_owner.Clone() as Module;
            m_pointId = id;
            m_originalPoint = GetPoint(m_pointId);
        }
        #region INodePoint Members
        public IDrawObject GetClone()
        {
            return m_clone;
        }
        public IDrawObject GetOriginal()
        {
            return m_owner;
        }
        public void SetPosition(UnitPoint pos)
        {
            if (Control.ModifierKeys == Keys.Control)
                pos = HitUtil.OrthoPointD(OtherPoint(m_pointId), pos, 45);
            if (m_angleLocked || Control.ModifierKeys == (Keys)(Keys.Control | Keys.Shift))
                pos = HitUtil.NearestPointOnLine(m_owner.StartPoint, m_owner.EndPoint, pos, true);
            SetPoint(m_pointId, pos, m_clone);
        }
        public void Finish()
        {
            m_endPoint = GetPoint(m_pointId);
            m_owner.FromPoint = m_clone.FromPoint;
            m_owner.StartPoint = m_clone.StartPoint;
            m_owner.EndPoint = m_clone.EndPoint;
            m_owner.ToPoint = m_clone.ToPoint;
            m_clone = null;
        }
        public void Cancel()
        {
        }
        public void Undo()
        {
            SetPoint(m_pointId, m_originalPoint, m_owner);
        }
        public void Redo()
        {
            SetPoint(m_pointId, m_endPoint, m_owner);
        }
        public void OnKeyDown(ICanvas canvas, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.L)
            {
                m_angleLocked = !m_angleLocked;
                e.Handled = true;
            }
        }
        #endregion
        protected UnitPoint GetPoint(ePoint pointid)
        {
            if (pointid == ePoint.FromPoint)
                return m_clone.FromPoint;
            if (pointid == ePoint.StartPoint)
                return m_clone.StartPoint;
            if (pointid == ePoint.EndPoint)
                return m_clone.EndPoint;
            if (pointid == ePoint.ToPoint)
                return m_clone.ToPoint;
            return m_owner.StartPoint;
        }
        protected UnitPoint OtherPoint(ePoint currentpointid)
        {
            if (currentpointid == ePoint.StartPoint)
                return GetPoint(ePoint.EndPoint);
            return GetPoint(ePoint.StartPoint);
        }
        public Module getOwner() { return m_owner; }
        public ePoint getID(){return m_pointId;}
        protected void SetPoint(ePoint pointid, UnitPoint point, Module mod)
        {
            if (pointid == ePoint.FromPoint)
            {
                mod.FromPoint = point;
                
            }
            if (pointid == ePoint.ToPoint) mod.ToPoint = point;
            if (pointid == ePoint.StartPoint)
            {
                mod.StartPoint = point;
                if ((mod.horizontal && mod.EndPoint.X == mod.StartPoint.X + 1 && mod.EndPoint.Y == mod.StartPoint.Y) || (!mod.horizontal && mod.EndPoint.Y == mod.StartPoint.Y+1 && mod.EndPoint.X == mod.StartPoint.X)) return;
                if (mod.horizontal) point.X += 1;
                else point.Y +=1;
                SetPoint(ePoint.EndPoint, point, mod);
                
            }
            if (pointid == ePoint.EndPoint)
            {
                mod.EndPoint = point;
                if ((mod.horizontal && mod.EndPoint.X == mod.StartPoint.X + 1 && mod.EndPoint.Y == mod.StartPoint.Y) || (!mod.horizontal && mod.EndPoint.Y == mod.StartPoint.Y + 1 && mod.EndPoint.X == mod.StartPoint.X)) return;
                if (mod.horizontal) point.X -= 1;
                else point.Y -= 1;
                SetPoint(ePoint.StartPoint, point, mod);
                
            }
        }
    }
    abstract class Module : Canvas.DrawTools.DrawObjectBase, IDrawObject, ISerialize
    {
        
        protected UnitPoint m_p1, m_p2, m_p3, m_p4;
        
        protected ePoint currentPoint = ePoint.FromPoint;
        [XmlSerializable]
        public bool horizontal = true;
        [XmlSerializable]
        public bool tofrom = false;
        [XmlSerializable]
        public bool child = false;
        [XmlSerializable]
        public bool conf = false;
        [XmlSerializable]
        public Module to_connections
        {
            get;
            set;
        }
        [XmlSerializable]
        public Module from_connections { get; set; }
        [XmlSerializable]
        protected List<Property> properties = new List<Property>();
        [XmlSerializable]
        public List<Property> Properties
        {
            get { return properties; }
            set { properties = value; }
        }
        protected List<Property> defaultProperties = new List<Property>();
        public List<Property> DefaultProperties
        {
            get { return defaultProperties; }
            set { defaultProperties = value; }
        }
        [XmlSerializable]
        public UnitPoint FromPoint
        {
            get { return m_p1; }
            set { m_p1 = value; }
        }
        [XmlSerializable]
        public UnitPoint StartPoint
        {
            get { return m_p2; }
            set { m_p2 = value; }
        }
        [XmlSerializable]
        public UnitPoint EndPoint
        {
            get { return m_p3; }
            set { m_p3 = value; }
        }
        [XmlSerializable]
        public UnitPoint ToPoint
        {
            get { return m_p4; }
            set { m_p4 = value; }
        }
        [XmlSerializable]
        public String getUnit(String name)
        {
            foreach (Property p in DefaultProperties)
            {
                if (p.name == name) return p.unit;
            }
            return String.Empty;
        }
        [XmlSerializable]
        public enum ePoint {
            FromPoint,
            StartPoint,
            EndPoint,
            ToPoint
        }
        public static string ObjectType;
        public virtual void Copy(Module acopy)
        {
            base.Copy(acopy);
            m_p1 = acopy.m_p1;
            m_p2 = acopy.m_p2;
            m_p3 = acopy.m_p3;
            Selected = acopy.Selected;
        }
        public abstract IDrawObject Clone();
        public abstract String toGLM();
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
            if (currentPoint == ePoint.FromPoint) return ScreenUtils.GetRect(m_p1, m_p3, thWidth);
            double x = Math.Min(m_p1.X, Math.Min(m_p2.X, Math.Min(m_p3.X, m_p4.X)));
            double y = Math.Min(m_p1.Y, Math.Min(m_p2.Y, Math.Min(m_p3.Y, m_p4.Y)));
            double x2 = Math.Max(m_p1.X, Math.Max(m_p2.X, Math.Max(m_p3.X, m_p4.X)));
            double y2 = Math.Max(m_p1.Y, Math.Max(m_p2.Y, Math.Max(m_p3.Y, m_p4.Y)));
            double w = Math.Abs(x2-x);
            double h = Math.Abs(y2-y);
            RectangleF rect = ScreenUtils.GetRect(x, y, w, h);
            rect.Inflate((float)Width+(float)0.5, (float)Width+(float)0.5);
            return rect;
        }
        UnitPoint MidPoint(ICanvas canvas, UnitPoint p1, UnitPoint p2, UnitPoint hitpoint)
        {
            UnitPoint mid = HitUtil.LineMidpoint(p1, p2);
            float thWidth = ThresholdWidth(canvas, Width);
            if (HitUtil.CircleHitPoint(mid, thWidth, hitpoint))
                return mid;
            return UnitPoint.Empty;
        }
        public virtual bool  PointInObject(ICanvas canvas, UnitPoint point)
        {
            float thWidth = ThresholdWidth(canvas, Width);
            return HitUtil.IsPointInLine(m_p1, m_p2, point, thWidth) || HitUtil.IsPointInLine(m_p2, m_p3, point, thWidth) || HitUtil.IsPointInLine(m_p3, m_p4, point, thWidth);
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
            if (Control.ModifierKeys == Keys.Control && currentPoint == ePoint.FromPoint)point = HitUtil.OrthoPointD(m_p1, point, 45);
            if (Control.ModifierKeys == Keys.Control && currentPoint == ePoint.EndPoint) point = HitUtil.OrthoPointD(m_p3, point, 45);
            if (currentPoint == ePoint.StartPoint)
            {
                m_p2 = point;
                if (this.horizontal) m_p3 = new UnitPoint(point.X + 1, point.Y);
                else m_p3 = new UnitPoint(point.X, point.Y - 1);
                m_p4 = m_p3;
            }
            if (currentPoint == ePoint.EndPoint) m_p4 = point;

        }
        public virtual eDrawObjectMouseDown OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            if (!tofrom && !child) return eDrawObjectMouseDown.Done;
            if (currentPoint == ePoint.FromPoint)
            {
                m_p1 = point;
                currentPoint = ePoint.StartPoint;
                foreach (IDrawObject i in canvas.DataModel.GetHitObjects(canvas, point)) if (i.GetType().ToString().IndexOf("Module") >= 0 && i != this) from_connections = (ModuleItems.Module)i;
                if (from_connections != null) foreach (Property p in properties) if (p.name == "from") foreach (Property q in from_connections.properties) if (q.name == "name") p.value = q.value;
                return eDrawObjectMouseDown.Continue;
            }
            if (currentPoint == ePoint.StartPoint)
            {
                currentPoint = ePoint.EndPoint;
                if (snappoint is PerpendicularSnapPoint && snappoint.Owner is Module)
                {
                    Module src = snappoint.Owner as Module;
                    m_p2 = HitUtil.NearestPointOnLine(src.FromPoint, src.EndPoint, m_p1, true);
                    if (horizontal) m_p3 = new UnitPoint(m_p2.X + 1, m_p2.Y);
                    else m_p3 = new UnitPoint(m_p2.X, m_p2.Y - 1);
                    if (child) return eDrawObjectMouseDown.Done;
                    return eDrawObjectMouseDown.Continue;
                }
                if (snappoint is PerpendicularSnapPoint && snappoint.Owner is Canvas.DrawTools.Line)
                {
                    Canvas.DrawTools.Line src = snappoint.Owner as Canvas.DrawTools.Line;
                    m_p2 = HitUtil.NearestPointOnLine(src.P1, src.P2, m_p1, true);
                    if (horizontal) m_p3 = new UnitPoint(m_p2.X + 1, m_p2.Y);
                    else m_p3 = new UnitPoint(m_p2.X, m_p2.Y - 1);
                    if (child) return eDrawObjectMouseDown.Done;
                    return eDrawObjectMouseDown.Continue;
                }
                if (snappoint is PerpendicularSnapPoint && snappoint.Owner is Canvas.DrawTools.Arc)
                {
                    Canvas.DrawTools.Arc src = snappoint.Owner as Canvas.DrawTools.Arc;
                    m_p2 = HitUtil.NearestPointOnCircle(src.Center, src.Radius, m_p1, 0);
                    if (horizontal) m_p3 = new UnitPoint(m_p2.X + 1, m_p2.Y);
                    else m_p3 = new UnitPoint(m_p2.X, m_p2.Y - 1);
                    if (child) return eDrawObjectMouseDown.Done;
                    return eDrawObjectMouseDown.Continue;
                }
                if (Control.ModifierKeys == Keys.Control)
                    point = HitUtil.OrthoPointD(m_p1, point, 45);
                m_p2 = point;
                if (horizontal) m_p3 = new UnitPoint(m_p2.X + 1, m_p2.Y);
                else m_p3 = new UnitPoint(m_p2.X, m_p2.Y - 1);
                if (child) return eDrawObjectMouseDown.Done;
                return eDrawObjectMouseDown.Continue;
            }
            Selected = false;
            currentPoint = ePoint.ToPoint;
            if (snappoint is PerpendicularSnapPoint && snappoint.Owner is Module)
            {
                Module src = snappoint.Owner as Module;
                m_p4 = HitUtil.NearestPointOnLine(src.FromPoint, src.EndPoint, m_p3, true);
                return eDrawObjectMouseDown.Done;
            }
            if (snappoint is PerpendicularSnapPoint && snappoint.Owner is Canvas.DrawTools.Line)
            {
                Canvas.DrawTools.Line src = snappoint.Owner as Canvas.DrawTools.Line;
                m_p4 = HitUtil.NearestPointOnLine(src.P1, src.P2, m_p3, true);
                return eDrawObjectMouseDown.Done;
            }
            if (snappoint is PerpendicularSnapPoint && snappoint.Owner is Canvas.DrawTools.Arc)
            {
                Canvas.DrawTools.Arc src = snappoint.Owner as Canvas.DrawTools.Arc;
                m_p4 = HitUtil.NearestPointOnCircle(src.Center, src.Radius, m_p3, 0);
                return eDrawObjectMouseDown.Done;
            }
            if (Control.ModifierKeys == Keys.Control)
                point = HitUtil.OrthoPointD(m_p1, point, 45);
            m_p4 = point;
            foreach (IDrawObject i in canvas.DataModel.GetHitObjects(canvas, point)) if (i.GetType().ToString().IndexOf("Module") >= 0 && i != this) to_connections = (ModuleItems.Module)i;
            if (to_connections != null) foreach (Property p in properties) if (p.name == "to") foreach (Property q in to_connections.properties) if (q.name == "name") p.value = q.value;
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
            m_p2.X += offset.X;
            m_p2.Y += offset.Y;
            m_p3.X += offset.X;
            m_p3.Y += offset.Y;
            m_p4.X += offset.X;
            m_p4.Y += offset.Y;
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
                        if (HitUtil.CircleHitPoint(m_p3, thWidth, point))
                            return new VertextSnapPoint(canvas, this, m_p3);
                        if (HitUtil.CircleHitPoint(m_p4, thWidth, point))
                            return new VertextSnapPoint(canvas, this, m_p4);
                    }
                    if (snaptype == typeof(MidpointSnapPoint))
                    {
                        UnitPoint p = MidPoint(canvas, m_p2, m_p3, point);
                        if (p != UnitPoint.Empty)
                            return new MidpointSnapPoint(canvas, this, p);
                    }
                    if (snaptype == typeof(IntersectSnapPoint))
                    {
                        Module otherline = Utils.FindObjectTypeInList(this, otherobjs, typeof(Module)) as Module;
                        if (otherline == null)
                            continue;
                        UnitPoint p = HitUtil.LinesIntersectPoint(m_p1, m_p2, otherline.m_p1, otherline.m_p2);
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
                UnitPoint p = HitUtil.LinesIntersectPoint(m_p1, m_p2, otherline.m_p1, otherline.m_p2);
                if (p != UnitPoint.Empty)
                    return new IntersectSnapPoint(canvas, this, p);
            }
            if (usersnaptype == typeof(VertextSnapPoint))
            {
                double d1 = HitUtil.Distance(point, m_p1);
                double d2 = HitUtil.Distance(point, m_p2);
                double d3 = HitUtil.Distance(point, m_p3);
                double d4 = HitUtil.Distance(point, m_p4);
                if (d1 <= d2 && d1 <= d3 && d1 <= d4) return new VertextSnapPoint(canvas, this, m_p1);
                if (d2 <= d1 && d2 <= d3 && d2 <= d4) return new VertextSnapPoint(canvas, this, m_p1);
                if (d3 <= d2 && d3 <= d1 && d3 <= d4) return new VertextSnapPoint(canvas, this, m_p1);
                return new VertextSnapPoint(canvas, this, m_p4);
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
        public INodePoint NodePoint(ICanvas canvas, UnitPoint point)
        {
            float thWidth = ThresholdWidth(canvas, Width);
            if (HitUtil.CircleHitPoint(m_p1, thWidth, point))
                return new NP(this, NP.ePoint.FromPoint);
            if (HitUtil.CircleHitPoint(m_p2, thWidth, point))
                return new NP(this, NP.ePoint.StartPoint);
            if (HitUtil.CircleHitPoint(m_p3, thWidth, point))
                return new NP(this, NP.ePoint.EndPoint);
            if (HitUtil.CircleHitPoint(m_p4, thWidth, point))
                return new NP(this, NP.ePoint.ToPoint);
            return null;
        }
        public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
        {
            FromPoint  = StartPoint = EndPoint = ToPoint = point;
            Width = layer.Width;
            Color = layer.Color;
            Selected = true;
        }
        public virtual string Id {
            get { return ObjectType; }
        }
        #region ISerialize
        public abstract void GetObjectData(XmlWriter wr);
        public void AfterSerializedIn(){}
        #endregion

    }
    public class Property
    {
        public String name { get; set; }
        public Object value { get; set; }
        public String unit { get; set; }
        public Property() { }
        public Property(string Name, object Value, String Unit) { name = Name; value = Value; unit = Unit; }
        public Property clone() { return new Property(name, value,unit); }
    }
}
