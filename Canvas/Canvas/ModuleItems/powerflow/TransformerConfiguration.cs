using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.powerflow
{
    class transformer_configuration : ModuleItems.Module
    {

        public transformer_configuration() { setupProperties(new List<Property>()); }
        public transformer_configuration(transformer_configuration old) { m_p1 = old.StartPoint; m_p2 = old.EndPoint; m_p3 = old.ToPoint; setupProperties(old.Properties); }
        private void setupProperties(List<Property> prop)
        {

            Properties = new List<Property>();
            DefaultProperties = new List<Property>(6);
            DefaultProperties.Add(new Property("name", "", ""));
            DefaultProperties.Add(new Property("connect_type", "", ""));
            DefaultProperties.Add(new Property("install_type", "", ""));
            DefaultProperties.Add(new Property("primary_voltage", "", "V"));
            DefaultProperties.Add(new Property("secondary_voltage", "", "V"));
            DefaultProperties.Add(new Property("power_rating", "", "KVA"));
            DefaultProperties.Add(new Property("powerA_rating", "", "KVA"));
            DefaultProperties.Add(new Property("powerB_rating", "", "KVA"));
            DefaultProperties.Add(new Property("powerC_rating", "", "KVA"));
            DefaultProperties.Add(new Property("resistance", "", "OHM PU"));
            DefaultProperties.Add(new Property("reactance", "", "OHM PU"));
            DefaultProperties.Add(new Property("impedance", "", "OHM PU"));
            DefaultProperties.Add(new Property("shunt_impedance", "", "OHM PU"));
            DefaultProperties.Add(new Property("impedance1", "", "OHM PU"));
            DefaultProperties.Add(new Property("impedance2", "", "OHM PU"));
            DefaultProperties.Add(new Property("full_load_loss", "", "OHM PU"));
            DefaultProperties.Add(new Property("core_coil_weight", "", "pounds"));
            DefaultProperties.Add(new Property("tank_fittings_weight", "", "pounds"));
            DefaultProperties.Add(new Property("oil_volume", "", "gallons"));
            DefaultProperties.Add(new Property("rated_winding_hot_spot_rise", "", "C"));
            DefaultProperties.Add(new Property("rated_top_oil_rise", "", "C"));
            DefaultProperties.Add(new Property("rated_winding_time_constant", "", "H"));
            DefaultProperties.Add(new Property("installed_insulation_life", "", "H"));
            DefaultProperties.Add(new Property("coolant_type", "", ""));
            DefaultProperties.Add(new Property("cooling_type", "", ""));
            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("transformer_configuration");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("transformer_configuration@{0}",
                StartPoint.PosAsString());
        }
        public override IDrawObject Clone()
        {
            return new transformer_configuration(this);
        }
        public override string toGLM()
        {
            String s = "object transformer_configuration {" + System.Environment.NewLine;
            foreach (Property p in Properties)
                foreach (Property q in DefaultProperties)
                    if (p.name == q.name && p.value.ToString() != q.value.ToString()) s = s + "    " + p.name + " " + p.value.ToString() + ";" + System.Environment.NewLine;
            s = s + "}" + System.Environment.NewLine;
            return s;
        }

        public override string Id
        {
            get { return "transformer_configuration"; }
        }
        public override void Draw(ICanvas canvas, RectangleF unitrect)
        {
            Brush B = Brushes.Black;
            Color color = Color;
            Pen pen = canvas.CreatePen(color, Width);
            if (Highlighted || Selected)
            {
                pen = Canvas.DrawTools.DrawUtils.SelectedPen;
                B = Brushes.Magenta;
                //if (m_p1.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p1);
            }
            pen = new Pen(pen.Color, (float)6);

            unitrect.Inflate(3, 3);
            canvas.DrawArc(canvas, pen, m_p1, (float)0.03, 0, 360);
            float size = (canvas.ToScreen(new UnitPoint(m_p1.X + 1, m_p1.Y)).X - canvas.ToScreen(m_p1).X)/10;
            Font m_font = new System.Drawing.Font("Arial Black", size, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           
            canvas.Graphics.DrawString("TX Config", m_font, B, canvas.ToScreen(new UnitPoint(m_p1.X - 0.4, m_p1.Y + 0.2)).X, canvas.ToScreen(new UnitPoint(m_p1.X - 0.3, m_p1.Y + 0.2)).Y);

        }
    }
}
