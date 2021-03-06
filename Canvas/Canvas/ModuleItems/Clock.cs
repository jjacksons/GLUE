﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems
{
    class clock : Module
    {

        public clock() { setupProperties(new List<Property>());}
        public clock(clock old) { m_p1 = old.FromPoint; m_p2 = old.StartPoint; m_p3 = old.EndPoint; m_p4 = old.ToPoint; setupProperties(old.Properties); }
        private void setupProperties(List<Property> prop)
        {
            
            Properties = new List<Property>();
            DefaultProperties = new List<Property>(6);
            DefaultProperties.Add(new Property("timezone", "", ""));
            DefaultProperties.Add(new Property("timestamp", "", ""));
            DefaultProperties.Add(new Property("starttime", "", ""));
            DefaultProperties.Add(new Property("stoptime", "", ""));

            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("clock");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("clock@{0}",
                StartPoint.PosAsString());
        }
        public override IDrawObject Clone()
        {
            return new clock(this);
        }
        public override string toGLM()
        {
            String s = "clock {"+System.Environment.NewLine;
            foreach (Property p in Properties)
                foreach (Property q in DefaultProperties)
                    if (p.name == q.name && p.value.ToString() != q.value.ToString()) s = s + "    " + p.name + " " + p.value.ToString() + ";"+System.Environment.NewLine;
            s = s + "}"+ System.Environment.NewLine;
            return s;
        }
        public override string Id
        {
            get { return "clock"; }
        }
        public override void Draw(ICanvas canvas, RectangleF unitrect)
        {
            Color color = Color;
            Pen pen = canvas.CreatePen(color, Width);
            if (Highlighted || Selected)
            {
                pen = Canvas.DrawTools.DrawUtils.SelectedPen;
                //if (m_p1.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p1);
            }
            pen = new Pen(pen.Color, (float)3);
            
            unitrect.Inflate(3, 3);
            canvas.DrawArc(canvas,pen,m_p1,(float)0.15,0,360);
            canvas.DrawLine(canvas, pen, m_p1, new UnitPoint(m_p1.X, m_p1.Y + 0.12));
            canvas.DrawLine(canvas, pen, m_p1, new UnitPoint(m_p1.X+0.08, m_p1.Y - 0.08));


        }
    }
}
