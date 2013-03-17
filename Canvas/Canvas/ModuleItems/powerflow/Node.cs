using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.powerflow
{
    class node : Module
    {

        public node() { setupProperties(new List<Property>());}
        public node(node old) { m_p1 = old.FromPoint; m_p2 = old.StartPoint; m_p3 = old.EndPoint; m_p4 = old.ToPoint; setupProperties(old.Properties); }
        private void setupProperties(List<Property> prop)
        {
            
            Properties = new List<Property>();
            DefaultProperties = new List<Property>(6);
            DefaultProperties.Add(new Property("name", "", ""));
            DefaultProperties.Add(new Property("phases", "", ""));
            DefaultProperties.Add(new Property("voltage_A", "", " complex"));
            DefaultProperties.Add(new Property("voltage_B", "", " complex"));
            DefaultProperties.Add(new Property("voltage_C", "0", " complex"));
            DefaultProperties.Add(new Property("bustype", "", ""));
            DefaultProperties.Add(new Property("maximum_voltage_error", "", " Volts"));
            DefaultProperties.Add(new Property("busflags", "", ""));
            DefaultProperties.Add(new Property("mean_repair_time", "", " Seconds"));

            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("node");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("node@{0}",
                StartPoint.PosAsString());
        }
        public override IDrawObject Clone()
        {
            return new node(this);
        }

        public override string Id
        {
            get { return "node"; }
        }
        public override string toGLM()
        {
            String s = "object node {" + System.Environment.NewLine;
            foreach (Property p in Properties)
                foreach (Property q in DefaultProperties)
                    if (p.name == q.name && p.value.ToString() != q.value.ToString()) s = s + "    " + p.name + " " + p.value.ToString() + ";" + System.Environment.NewLine;
            s = s + "}" + System.Environment.NewLine;
            return s;
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
            pen = new Pen(pen.Color, (float)6);
            
            unitrect.Inflate(3, 3);
            canvas.DrawArc(canvas,pen,m_p1,(float)0.05,0,360);

        }
    }
}
