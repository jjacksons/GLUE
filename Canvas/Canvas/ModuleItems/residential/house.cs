using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.residential
{
    class house : Module
    {

        public house() { setupProperties(new List<Property>()); child = true; }
        public house(house old) { m_p1 = old.FromPoint; m_p2 = old.StartPoint; m_p3 = old.EndPoint; m_p4 = old.ToPoint; setupProperties(old.Properties); child = true; }
        private void setupProperties( List<Property> prop)
        {

            Properties = new List<Property>();
            DefaultProperties = new List<Property>(6);
            DefaultProperties.Add (new ModuleItems.Property( "name", "",""));
            DefaultProperties.Add(new ModuleItems.Property("parent", "", ""));
            DefaultProperties.Add (new ModuleItems.Property( "weather ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "floor_area", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "gross_wall_area ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "ceiling_height", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "aspect_ratio", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "envelope_UA", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "window_wall_ratio", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "number_of_doors ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "exterior_wall_fraction", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "interior_exterior_wall_ratio", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "exterior_ceiling_fraction", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "exterior_floor_fraction ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "window_shading", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "window_exterior_transmission_coefficient", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "solar_heatgain_factor ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "airchange_per_hour ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "airchange_UA ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "UA", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "internal_gain ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "solar_gain ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "incident_solar_radiation ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "heat_cool_gain ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "include_solar_quadrant", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "heating_cop_curve", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "thermostat_deadband ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "int16 thermostat_cycle_time ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "thermostat_last_cycle_time ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "heating_point ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "cooling_point ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "design_heating_point ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "design_cooling_point ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "over_sizing_factor ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "design_heating_capacity ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "design_cooling_capacity ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "cooling_design_temperature ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "heating_design_temperature ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "design_peak_solar ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "design_internal_gains ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "air_heat_fraction ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "mass_solar_gain_fraction ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "mass_internal_gain_fraction ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "auxiliary_heat_capacity ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "aux_heat_deadband ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "aux_heat_temperature_lockout ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "aux_heat_time_delay ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "cooling_supply_air_temp ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "heating_supply_air_temp ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "duct_pressure_drop ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "fan_design_power ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "fan_low_power_fraction ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "fan_power ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "fan_design_airflow ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "fan_impedance_fraction ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "fan_power_fraction ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "fan_current_fraction ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "fan_power_factor ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "heating_demand ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "cooling_demand ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "heating_COP ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "cooling_COP ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "air_temperature ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "outdoor_temperature ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "outdoor_rh ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "mass_heat_capacity ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "mass_heat_coeff ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "mass_temperature ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "air_volume ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "air_mass ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "air_heat_capacity ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "latent_load_fraction ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "total_thermal_mass_per_floor_area", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "interior_surface_heat_transfer_coeff", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "number_of_stories ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "is_AUX_on ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "is_HEAT_on ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "is_COOL_on ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "thermal_storage_present ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "thermal_storage_in_use ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "system_type ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "auxiliary_strategy ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "system_mode ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "last_system_mode ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "heating_system_type", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "cooling_system_type", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "auxiliary_system_type", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "fan_type", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "thermal_integrity_level ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "glass_type ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "window_frame ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "glazing_treatment ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "glazing_layers ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "motor_model ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "motor_efficiency ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "last_mode_timer", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "hvac_motor_efficiency ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "hvac_motor_loss_power_factor ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "Rroof ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "Rwall ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "Rfloor ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "Rwindows ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "Rdoors ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "hvac_breaker_rating ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "hvac_power_factor ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "hvac_load ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "last_heating_load ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "last_cooling_load ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "hvac_power ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "total_load ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "panel ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "design_internal_gain_density ", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "compressor_on", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "compressor_count", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "hvac_last_on", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "hvac_last_off", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "hvac_period_length", "",""));
            DefaultProperties.Add (new ModuleItems.Property( "hvac_duty_cycle", "",""));
            DefaultProperties.Add(new ModuleItems.Property("thermostat_control ", "", ""));
            if (Properties.Count ==0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("house");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("house@{0},{1} - L={2:f4}<{3:f4}",
                StartPoint.PosAsString(),
                EndPoint.PosAsString(),
                HitUtil.Distance(StartPoint, EndPoint),
                HitUtil.RadiansToDegrees(HitUtil.LineAngleR(StartPoint, EndPoint, 0)));
        }
        public override IDrawObject Clone()
        {
            return new house(this);
        }

        public override string toGLM()
        {
            String s = "object house {" + System.Environment.NewLine;
            foreach (Property p in Properties)
                foreach (Property q in DefaultProperties)
                    if (p.name == q.name && p.value.ToString() != q.value.ToString()) s = s + "    " + p.name + " " + p.value.ToString() + ";" + System.Environment.NewLine;
            s = s + "}" + System.Environment.NewLine;
            return s;
        }
        
        public override string Id
        {
            get { return "house"; }
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
                if (m_p4.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p4);
            }
            canvas.DrawLine(canvas, pen, m_p1, m_p2);
            pen = new Pen(pen.Color, (float)4);
            
            if (horizontal)
            {
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.1, m_p2.Y), new UnitPoint(m_p2.X + 0.51, m_p2.Y + 0.4));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.9, m_p2.Y), new UnitPoint(m_p2.X + 0.49, m_p2.Y + 0.4));
                canvas.Graphics.DrawRectangle(pen, canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y + 0.1)).Y, canvas.ToScreen(new UnitPoint(m_p2.X + 0.8, 1)).X - canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.6)).Y - canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.1)).Y);
            }
            else
            {
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X - 0.4, m_p2.Y-0.5), new UnitPoint(m_p2.X + 0.01, m_p2.Y ));
                canvas.DrawLine(canvas, pen, new UnitPoint(m_p2.X + 0.4, m_p2.Y-0.5), new UnitPoint(m_p2.X + -0.01, m_p2.Y ));
                canvas.Graphics.DrawRectangle(pen, canvas.ToScreen(new UnitPoint(m_p2.X - 0.3, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.4)).Y, canvas.ToScreen(new UnitPoint(m_p2.X + 0.8, 1)).X - canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.6)).Y - canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.1)).Y);

            }
        }
    }
}
