using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.ModuleItems.tape
{
    class recorder : Module
    {

        public recorder() { setupProperties(new List<Property>()); this.child = true; }
        public recorder(recorder old) { m_p1 = old.FromPoint; m_p2 = old.StartPoint; m_p3 = old.EndPoint; m_p4 = old.ToPoint; setupProperties(old.Properties); this.child = true; }
        private void setupProperties(List<Property> prop)
        {
            
            Properties = new List<Property>();
            DefaultProperties = new List<Property>(6);
            DefaultProperties.Add(new Property("name", "", ""));
            DefaultProperties.Add(new Property("parent", "", ""));
            DefaultProperties.Add(new Property("property", "", " "));
            DefaultProperties.Add(new Property("trigger", "", ""));
            DefaultProperties.Add(new Property("file", "", ""));
            DefaultProperties.Add(new Property("interval", "TS_NEVER", ""));
            DefaultProperties.Add(new Property("limit", "0", ""));
            DefaultProperties.Add(new Property("multifile", "", ""));
            DefaultProperties.Add(new Property("plotcommands", "", ""));
            DefaultProperties.Add(new Property("xdata", "", ""));
            DefaultProperties.Add(new Property("columns", "", ""));
            DefaultProperties.Add(new Property("output", "", ""));
            DefaultProperties.Add(new Property("header_units", "", ""));
            DefaultProperties.Add(new Property("line_units", "", ""));
            DefaultProperties.Add(new Property("DELTAMODE", "", ""));
            if (Properties.Count == 0) foreach (Property p in DefaultProperties) Properties.Add(p.clone());
        }
        public override void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("recorder");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public override string GetInfoAsString()
        {
            return string.Format("recorder@{0}",
                StartPoint.PosAsString());
        }
        public override IDrawObject Clone()
        {
            return new recorder(this);
        }

        public override string Id
        {
            get { return "recorder"; }
        }
        public override string toGLM()
        {
            String s = "object recorder {" + System.Environment.NewLine;
            foreach (Property p in Properties)
                foreach (Property q in DefaultProperties)
                    if (p.name == q.name && p.value.ToString() != q.value.ToString()) s = s + "    " + p.name + " " + p.value.ToString() + ";" + System.Environment.NewLine;
            s = s + "}" + System.Environment.NewLine;
            return s;
        }
        public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
        {
            FromPoint = StartPoint = EndPoint = ToPoint = point;
            int count = 1;
            foreach (IDrawObject i in layer.Objects) if (i.GetType() == this.GetType()) count++;
            foreach (Property p in properties) if (p.name == "name" && p.value == "") p.value = this.Id + count;
            foreach (Property p in properties) if (p.name == "file" && p.value == "") p.value = this.Id + count + ".csv";
            Width = layer.Width;
            Color = layer.Color;
            Selected = true;
        }
        public override void Draw(ICanvas canvas, RectangleF unitrect)
        {
            Color color = Color;
            Brush B = Brushes.Black;
            Pen pen = canvas.CreatePen(color, Width);
            if (Highlighted || Selected)
            {
                pen = Canvas.DrawTools.DrawUtils.SelectedPen;
                if (m_p1.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p1);
                if (m_p2.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p2);
                if (m_p3.IsEmpty == false) Canvas.DrawTools.DrawUtils.DrawNode(canvas, m_p3);
                B = Brushes.Magenta;
            }
            canvas.DrawLine(canvas, pen, m_p1, m_p2);
            pen = new Pen(pen.Color, (float)3);
            if (horizontal)
            {
                canvas.DrawLine(canvas, pen, m_p2, new UnitPoint(m_p2.X + 0.2, m_p2.Y));
                canvas.Graphics.DrawRectangle(pen, canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y + 0.1)).Y, canvas.ToScreen(m_p3).X - canvas.ToScreen(new UnitPoint(m_p2.X + 0.2, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.1)).Y - canvas.ToScreen(new UnitPoint(1, m_p2.Y + 0.1)).Y);
                float size = (canvas.ToScreen(new UnitPoint(m_p1.X + 1, m_p1.Y)).X - canvas.ToScreen(m_p1).X) / 10;
                Font m_font = new System.Drawing.Font("Arial Black", size, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                canvas.Graphics.DrawString("Recorder", m_font, B, canvas.ToScreen(new UnitPoint(m_p2.X + 0.25, m_p2.Y - 0.1)).X, canvas.ToScreen(new UnitPoint(m_p2.X - 0.3, m_p2.Y + 0.1)).Y);
                
            }
            else
            {
                canvas.DrawLine(canvas, pen, m_p2, new UnitPoint(m_p2.X, m_p2.Y - 0.2));
                canvas.Graphics.DrawRectangle(pen, canvas.ToScreen(new UnitPoint(m_p2.X - 0.4, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.2)).Y, canvas.ToScreen(new UnitPoint(m_p2.X + 0.4, 1)).X - canvas.ToScreen(new UnitPoint(m_p2.X - 0.4, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.4)).Y - canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.2)).Y);
                float size = (canvas.ToScreen(new UnitPoint(m_p1.X + 1, m_p1.Y)).X - canvas.ToScreen(m_p1).X) / 10;
                Font m_font = new System.Drawing.Font("Arial Black", size, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                canvas.Graphics.DrawString("Recorder", m_font, B, canvas.ToScreen(new UnitPoint(m_p2.X - 0.35, 1)).X, canvas.ToScreen(new UnitPoint(1, m_p2.Y - 0.2)).Y);
            }

        }
    }
}
