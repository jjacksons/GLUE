using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.powerflow
{
    class regulator_configuration : ModuleItems.Module
    {

        public regulator_configuration() { setupProperties(new List<Property>()); }
        public regulator_configuration(regulator_configuration old) { m_p1 = old.StartPoint; m_p2 = old.EndPoint; m_p3 = old.ToPoint; setupProperties(old.Properties); }
        private void setupProperties(List<Property> prop)
        {

            Properties = new List<Property>();
            DefaultProperties = new List<Property>(6);
            DefaultProperties.Add(new Property("name", "", ""));
            DefaultProperties.Add(new Property("connect_type", "", ""));
            DefaultProperties.Add(new Property("band_center", "", ""));
            DefaultProperties.Add(new Property("band_width", "", "V"));
            DefaultProperties.Add(new Property("time_delay", "", "V"));
            DefaultProperties.Add(new Property("dwell_time", "", "KVA"));
            DefaultProperties.Add(new Property("raise_taps", "", "KVA"));
            DefaultProperties.Add(new Property("lower_taps", "", "KVA"));
            DefaultProperties.Add(new Property("current_transducer_ratio", "", "KVA"));
            DefaultProperties.Add(new Property("power_transducer_ratio", "", "OHM PU"));
            DefaultProperties.Add(new Property("compensator_r_setting_A", "", "OHM PU"));
            DefaultProperties.Add(new Property("compensator_r_setting_B", "", "OHM PU"));
            DefaultProperties.Add(new Property("compensator_r_setting_C", "", "OHM PU"));
            DefaultProperties.Add(new Property("compensator_x_setting_A", "", "OHM PU"));
            DefaultProperties.Add(new Property("compensator_x_setting_B", "", "OHM PU"));
            DefaultProperties.Add(new Property("compensator_x_setting_C", "", "OHM PU"));
            DefaultProperties.Add(new Property("CT_phase", "", "pounds"));
            DefaultProperties.Add(new Property("PT_phase", "", "pounds"));
            DefaultProperties.Add(new Property("regulation", "", "gallons"));
            DefaultProperties.Add(new Property("Control", "", "C"));
            DefaultProperties.Add(new Property("Type", "", "C"));
            DefaultProperties.Add(new Property("tap_pos_A", "", "H"));
            DefaultProperties.Add(new Property("tap_pos_B", "", "H"));
            DefaultProperties.Add(new Property("tap_pos_C", "", ""));
            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("regulator_configuration");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("regulator_configuration@{0}",
                StartPoint.PosAsString());
        }
        public override IDrawObject Clone()
        {
            return new regulator_configuration(this);
        }
        public override string toGLM()
        {
            String s = "object regulator_configuration {" + System.Environment.NewLine;
            foreach (Property p in Properties)
                foreach (Property q in DefaultProperties)
                    if (p.name == q.name && p.value.ToString() != q.value.ToString()) s = s + "    " + p.name + " " + p.value.ToString() + ";" + System.Environment.NewLine;
            s = s + "}" + System.Environment.NewLine;
            return s;
        }

        public override string Id
        {
            get { return "regulator_configuration"; }
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
           
            canvas.Graphics.DrawString("Reg Config", m_font, B, canvas.ToScreen(new UnitPoint(m_p1.X - 0.4, m_p1.Y + 0.2)).X, canvas.ToScreen(new UnitPoint(m_p1.X - 0.3, m_p1.Y + 0.2)).Y);

        }
    }
}
