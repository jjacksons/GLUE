using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.powerflow
{
    class triplex_meter  : Module
    {

        public triplex_meter() { setupProperties(new List<Property>());}
        public triplex_meter(triplex_meter old) { m_p1 = old.FromPoint; m_p2 = old.StartPoint; m_p3 = old.EndPoint; m_p4 = old.ToPoint; setupProperties(old.Properties); }
        private void setupProperties(List<Property> prop)
        {
            
            Properties = new List<Property>();
            DefaultProperties = new List<Property>(6);
            DefaultProperties.Add(new Property("name", "", ""));
            DefaultProperties.Add(new Property("phases", "", ""));
            DefaultProperties.Add(new Property("nominal_voltage", "", " V"));
            DefaultProperties.Add(new Property("bustype", "", ""));
            DefaultProperties.Add(new Property("maximum_voltage_error", "", " Volts"));
            DefaultProperties.Add(new Property("busflags", "", ""));
            DefaultProperties.Add(new Property("voltage_A", "", " VA"));
            DefaultProperties.Add(new Property("voltage_B", "", " VA"));
            DefaultProperties.Add(new Property("voltage_C", "", " VA"));
            DefaultProperties.Add(new Property("bill_day", "", ""));
            DefaultProperties.Add(new Property("price", "", " $/kwh"));
            DefaultProperties.Add(new Property("monthly_fee", "", " $"));
            DefaultProperties.Add(new Property("monthly_energy", "", " kwh"));
            DefaultProperties.Add(new Property("previous_monthly_energy", "", " kwh"));
            DefaultProperties.Add(new Property("bill_mode", "", ""));
            DefaultProperties.Add(new Property("power_market", "", " "));
            DefaultProperties.Add(new Property("first_tier_price", "", " $"));
            DefaultProperties.Add(new Property("second_tier_price", "", " $"));
            DefaultProperties.Add(new Property("third_tier_price", "", " $"));
            DefaultProperties.Add(new Property("first_tier_energy", "", " kwh"));
            DefaultProperties.Add(new Property("second_tier_energy", "", " kwh"));
            DefaultProperties.Add(new Property("third_tier_energy", "", " kwh"));

            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("triplex_meter");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("triplex_meter@{0}",
                StartPoint.PosAsString());
        }
        public override IDrawObject Clone()
        {
            return new triplex_meter(this);
        }

        public override string Id
        {
            get { return "triplex_meter"; }
        }
        public override string toGLM()
        {
            String s = "object triplex_meter {" + System.Environment.NewLine;
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
            }
            pen = new Pen(pen.Color, (float)2);
            
            unitrect.Inflate(3, 3);
            canvas.DrawArc(canvas,pen,m_p1,(float)0.05,0,360);
            canvas.DrawArc(canvas,pen,m_p1,(float)0.08,0,360);
            canvas.DrawArc(canvas,pen,m_p1,(float)0.11,0,360);

        }
    }
}
