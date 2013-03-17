using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.powerflow
{
    class NodePointMeter : NodePoint
    {
        public NodePointMeter(Module owner, ePoint id)
        {
            m_owner = owner;
            m_clone = m_owner.Clone() as Module;
            m_pointId = id;
            m_originalPoint = GetPoint(m_pointId);
        }
    }
    class meter : Module
    {

        public meter() { setupProperties(new List<Property>());}
        public meter(meter old) { m_p1 = old.FromPoint; m_p2 = old.StartPoint; m_p3 = old.EndPoint; m_p4 = old.ToPoint; setupProperties(old.Properties); }
        private void setupProperties(List<Property> prop)
        {
            
            Properties = new List<Property>();
            DefaultProperties = new List<Property>(6);
            DefaultProperties.Add(new Property("name", "", ""));
            DefaultProperties.Add(new Property("phases", "", ""));
            DefaultProperties.Add(new Property("nominal_voltage", "", " V"));
            DefaultProperties.Add(new Property("measured_real_energy", "", " WH"));
            DefaultProperties.Add(new Property("measured_reactive_energy", "", " VAH"));
            DefaultProperties.Add(new Property("measured_power", "", " VA"));
            DefaultProperties.Add(new Property("bustype", "", ""));
            DefaultProperties.Add(new Property("maximum_voltage_error", "", " Volts"));
            DefaultProperties.Add(new Property("busflags", "", ""));
            DefaultProperties.Add(new Property("measured_power_A", "", " VA"));
            DefaultProperties.Add(new Property("measured_power_B", "", " VA"));
            DefaultProperties.Add(new Property("measured_power_C", "", " VA"));
            DefaultProperties.Add(new Property("measured_demand", "", " W"));
            DefaultProperties.Add(new Property("measured_real_power", "", " W"));
            DefaultProperties.Add(new Property("measured_reactive_power", "", " W"));
            DefaultProperties.Add(new Property("measured_voltage_A", "", " V"));
            DefaultProperties.Add(new Property("measured_voltage_B", "", " V"));
            DefaultProperties.Add(new Property("measured_voltage_C", "", " V"));
            DefaultProperties.Add(new Property("measured_current_A", "", " A"));
            DefaultProperties.Add(new Property("measured_current_B", "", " A"));
            DefaultProperties.Add(new Property("measured_current_C", "", " A"));
            DefaultProperties.Add(new Property("bill_day", "", ""));
            DefaultProperties.Add(new Property("price", "", " $/kwh"));
            DefaultProperties.Add(new Property("monthly_fee", "", " $"));
            DefaultProperties.Add(new Property("monthly_bill", "", " $"));
            DefaultProperties.Add(new Property("previous_monthly_bill", "", " $"));
            DefaultProperties.Add(new Property("monthly_energy", "", " kwh"));
            DefaultProperties.Add(new Property("previous_monthly_energy", "", " kwh"));
            DefaultProperties.Add(new Property("bill_mode", "", ""));
            DefaultProperties.Add(new Property("power_market", "", " Seconds"));
            DefaultProperties.Add(new Property("first_tier_price", "", " Seconds"));
            DefaultProperties.Add(new Property("second_tier_price", "", " Seconds"));
            DefaultProperties.Add(new Property("third_tier_price", "", " Seconds"));
            DefaultProperties.Add(new Property("first_tier_energy", "", " Seconds"));
            DefaultProperties.Add(new Property("second_tier_energy", "", " Seconds"));
            DefaultProperties.Add(new Property("third_tier_energy", "", " Seconds"));



            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("meter");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("meter@{0}",
                StartPoint.PosAsString());
        }
        public override IDrawObject Clone()
        {
            return new meter(this);
        }
        public override string toGLM()
        {
            String s = "object meter {" + System.Environment.NewLine;
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
                return new NodePointMeter(this, NodePointMeter.ePoint.FromPoint);
            if (HitUtil.CircleHitPoint(m_p2, thWidth, point))
                return new NodePointMeter(this, NodePointMeter.ePoint.StartPoint);
            if (HitUtil.CircleHitPoint(m_p3, thWidth, point))
                return new NodePointMeter(this, NodePointMeter.ePoint.EndPoint);
            if (HitUtil.CircleHitPoint(m_p4, thWidth, point))
                return new NodePointMeter(this, NodePointMeter.ePoint.ToPoint);

            return null;
        }

        public override string Id
        {
            get { return "meter"; }
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
            
            canvas.DrawArc(canvas,pen,m_p1,(float)0.05,0,360);
            pen = new Pen(pen.Color, (float)3);
            canvas.Graphics.DrawRectangle(pen, canvas.ToScreen(new UnitPoint(m_p1.X - 0.2, m_p1.Y - 0.2)).X, canvas.ToScreen(new UnitPoint(m_p1.X + 0.2, m_p1.Y + 0.2)).Y, canvas.ToScreen(new UnitPoint(m_p1.X + 0.2, m_p1.Y +0.2)).X - canvas.ToScreen(new UnitPoint(m_p1.X - 0.2, m_p1.Y - 0.2)).X, canvas.ToScreen(new UnitPoint(m_p1.X - 0.2, m_p1.Y - 0.2)).Y - canvas.ToScreen(new UnitPoint(m_p1.X + 0.2, m_p1.Y + 0.2)).Y);


        }
    }
}
