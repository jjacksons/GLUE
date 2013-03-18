using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.generator
{
    class solar : Module
    {

        public solar() { setupProperties(new List<Property>()); this.child = true; }
        public solar(solar old) { m_p1 = old.FromPoint; m_p2 = old.StartPoint; m_p3 = old.EndPoint; m_p4 = old.ToPoint; setupProperties(old.Properties); this.child = true; }
        private void setupProperties(List<Property> prop)
        {
            
            Properties = new List<Property>();
            DefaultProperties = new List<Property>(6);
            DefaultProperties.Add(new Property("name", "", ""));
            DefaultProperties.Add(new Property("parent", "", ""));
            DefaultProperties.Add(new Property("generator_mode", "SUPPLY_DRIVEN", ""));
            DefaultProperties.Add(new Property("generator_status", "ONLINE", " "));
            DefaultProperties.Add(new Property("panel_type", "SINGLE_CRYSTAL_SILICON", ""));
            DefaultProperties.Add(new Property("power_type", "", ""));
            DefaultProperties.Add(new Property("INSTALLATION_TYPE", "", ""));
            DefaultProperties.Add(new Property("SOLAR_TILT_MODEL", "DEFAULT", ""));
            DefaultProperties.Add(new Property("SOLAR_POWER_MODEL", "DEFAULT", ""));
            DefaultProperties.Add(new Property("a_coeff", "-2.81", ""));
            DefaultProperties.Add(new Property("b_coeff", "-0.0455", ""));
            DefaultProperties.Add(new Property("dT_coeff", "0.0", ""));
            DefaultProperties.Add(new Property("T_coeff", "-0.5", "%/degC"));
            DefaultProperties.Add(new Property("NOCT", "118.4", "F"));
            DefaultProperties.Add(new Property("Tmodule", "", "DegF"));
            DefaultProperties.Add(new Property("Tambient", "", "DegF"));
            DefaultProperties.Add(new Property("wind_speed", "", "mph"));
            DefaultProperties.Add(new Property("ambient_temperature", "", "DegF"));
            DefaultProperties.Add(new Property("Insolation", "", "W/sqF"));
            DefaultProperties.Add(new Property("Rinternal", "0.05", "OHM"));
            DefaultProperties.Add(new Property("Rated_Insolation", "92.902", "W/sqF"));
            DefaultProperties.Add(new Property("Pmax_temp_coeff", "", ""));
            DefaultProperties.Add(new Property("Voc_temp_coeff", "", ""));
            DefaultProperties.Add(new Property("V_Max", "79.34", "V"));
            DefaultProperties.Add(new Property("Voc_Max", "91.22", "V"));
            DefaultProperties.Add(new Property("Voc", "91.22", "V"));
            DefaultProperties.Add(new Property("efficiency", "0.1", ""));
            DefaultProperties.Add(new Property("area", "323", "sqf"));
            DefaultProperties.Add(new Property("soiling", "", ""));
            DefaultProperties.Add(new Property("derating", "", ""));
            DefaultProperties.Add(new Property("Rated kVA", "", "KVA"));
            DefaultProperties.Add(new Property("P_Out", "", "kW"));
            DefaultProperties.Add(new Property("V_Out", "", "V"));
            DefaultProperties.Add(new Property("I_Out", "", "A"));
            DefaultProperties.Add(new Property("VA_Out", "", "VA"));
            DefaultProperties.Add(new Property("weather", "", ""));
            DefaultProperties.Add(new Property("shading_factor", "1", "pu"));
            DefaultProperties.Add(new Property("tilt_angle", "45", "deg"));
            DefaultProperties.Add(new Property("orientation_azimuth", "0", "deg"));
            DefaultProperties.Add(new Property("latitude_angle_fix", "false", ""));
            DefaultProperties.Add(new Property("orientation", "DEFAULT", ""));
            DefaultProperties.Add(new Property("phases", "", ""));
            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("solar");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("solar@{0}",
                StartPoint.PosAsString());
        }
        public override IDrawObject Clone()
        {
            return new solar(this);
        }

        public override string Id
        {
            get { return "solar"; }
        }
        public override string toGLM()
        {
            String s = "object solar {" + System.Environment.NewLine;
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
                if (m_p1.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p1);
                if (m_p2.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p2);
                //if ((m_p3.X != m_p2.X + 1 || m_p3.Y != m_p2.Y) && this.horizontal) m_p3 = new UnitPoint(m_p2.X + 1, m_p2.Y);
                //if (m_p3.Y != m_p2.Y + 1 && !this.horizontal) m_p3 = new UnitPoint(m_p2.X, m_p2.Y + 1);
                if (m_p3.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p3);
            }
            canvas.DrawLine(canvas, pen, m_p1, m_p2);
            pen = new Pen(pen.Color, (float)3);
            if (horizontal)
            {
                canvas.DrawLine(canvas, pen, m_p2, new UnitPoint(m_p2.X+0.8,m_p2.Y));
                canvas.Graphics.DrawRectangle(pen, canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y + 0.4)).Y, canvas.ToScreen(new UnitPoint(m_p2.X + 0.8, 1)).X - canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.2)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.4)).Y - canvas.ToScreen(new UnitPoint(1, m_p2.Y +0.4)).Y);
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.3), new UnitPoint(m_p2.X + 0.8, m_p2.Y + 0.3));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.2), new UnitPoint(m_p2.X + 0.8, m_p2.Y + 0.2));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.2, m_p2.Y + 0.1), new UnitPoint(m_p2.X + 0.8, m_p2.Y + 0.1));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.2, m_p2.Y - 0.3), new UnitPoint(m_p2.X + 0.8, m_p2.Y - 0.3));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.2, m_p2.Y - 0.2), new UnitPoint(m_p2.X + 0.8, m_p2.Y - 0.2));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.2, m_p2.Y - 0.1), new UnitPoint(m_p2.X + 0.8, m_p2.Y - 0.1));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.3, m_p2.Y + 0.4), new UnitPoint(m_p2.X + 0.3, m_p2.Y - 0.4));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.4, m_p2.Y + 0.4), new UnitPoint(m_p2.X + 0.4, m_p2.Y - 0.4));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.5, m_p2.Y + 0.4), new UnitPoint(m_p2.X + 0.5, m_p2.Y - 0.4));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.6, m_p2.Y + 0.4), new UnitPoint(m_p2.X + 0.6, m_p2.Y - 0.4));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.7, m_p2.Y + 0.4), new UnitPoint(m_p2.X + 0.7, m_p2.Y - 0.4));

            }
            else
            {
                canvas.DrawLine(canvas, pen, m_p2, new UnitPoint(m_p2.X, m_p2.Y-0.8));
                canvas.Graphics.DrawRectangle(pen, canvas.ToScreen(new UnitPoint(m_p2.X - 0.4, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.2)).Y, canvas.ToScreen(new UnitPoint(m_p2.X + 0.4, 1)).X - canvas.ToScreen(new UnitPoint(m_p2.X - 0.4, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.8)).Y - canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.2)).Y);
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.3, m_p2.Y - 0.2), new UnitPoint(m_p2.X + 0.3, m_p2.Y - 0.8));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.2, m_p2.Y - 0.2), new UnitPoint(m_p2.X + 0.2, m_p2.Y - 0.8));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.1, m_p2.Y - 0.2), new UnitPoint(m_p2.X + 0.1, m_p2.Y - 0.8));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X -0.3, m_p2.Y -0.2), new UnitPoint(m_p2.X - 0.3, m_p2.Y - 0.8));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X -0.2, m_p2.Y -0.2), new UnitPoint(m_p2.X - 0.2, m_p2.Y - 0.8));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X -0.1, m_p2.Y -0.2), new UnitPoint(m_p2.X - 0.1, m_p2.Y - 0.8));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.4, m_p2.Y - 0.3), new UnitPoint(m_p2.X - 0.4, m_p2.Y - 0.3));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.4, m_p2.Y - 0.4), new UnitPoint(m_p2.X - 0.4, m_p2.Y - 0.4));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.4, m_p2.Y - 0.5), new UnitPoint(m_p2.X - 0.4, m_p2.Y - 0.5));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.4, m_p2.Y - 0.6), new UnitPoint(m_p2.X - 0.4, m_p2.Y - 0.6));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.4, m_p2.Y - 0.7), new UnitPoint(m_p2.X - 0.4, m_p2.Y - 0.7));
            }

        }
    }
}
