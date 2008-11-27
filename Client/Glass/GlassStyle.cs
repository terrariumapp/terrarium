//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Terrarium.Glass
{
    [Serializable]
    public class GlassStyle : IXmlSerializable
    {
        protected Color borderColor = Color.FromArgb(32, 32, 32);

        protected GlassGradient buttonDisabled = new GlassGradient(Color.FromArgb(64, 64, 64),
                                                                   Color.FromArgb(96, 96, 96));

        protected GlassGradient buttonHighlight = new GlassGradient(Color.FromArgb(216, 216, 216),
                                                                    Color.FromArgb(128, 128, 128));

        protected GlassGradient buttonHover = new GlassGradient(Color.FromArgb(128, 255, 128), Color.FromArgb(0, 96, 0));
        protected bool buttonIsGlass = true;

        protected GlassGradient buttonNormal = new GlassGradient(Color.FromArgb(160, 160, 160),
                                                                 Color.FromArgb(64, 64, 64));

        protected GlassGradient buttonPressed = new GlassGradient(Color.FromArgb(0, 64, 0), Color.FromArgb(96, 160, 96));
        protected Color dialogColor = Color.FromArgb(16, 16, 16);

        protected Font font = new Font("Verdana", 6.75f, FontStyle.Bold);
        protected bool fontShadow = true;
        protected Color foreColor = Color.White;
        protected string name = "GlassStyle1";

        protected GlassGradient panel = new GlassGradient(Color.FromArgb(128, 128, 255), Color.FromArgb(0, 0, 96));

        protected bool panelIsGlass = true;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Category("Panel")]
        public GlassGradient Panel
        {
            get { return panel; }
            set { panel = value; }
        }

        [Category("Button")]
        public GlassGradient ButtonNormal
        {
            get { return buttonNormal; }
            set { buttonNormal = value; }
        }

        [Category("Button")]
        public GlassGradient ButtonHover
        {
            get { return buttonHover; }
            set { buttonHover = value; }
        }

        [Category("Button")]
        public GlassGradient ButtonPressed
        {
            get { return buttonPressed; }
            set { buttonPressed = value; }
        }

        [Category("Button")]
        public GlassGradient ButtonDisabled
        {
            get { return buttonDisabled; }
            set { buttonDisabled = value; }
        }

        [Category("Button")]
        public GlassGradient ButtonHighlight
        {
            get { return buttonHighlight; }
            set { buttonHighlight = value; }
        }

        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        public Color ForeColor
        {
            get { return foreColor; }
            set { foreColor = value; }
        }

        public bool FontShadow
        {
            get { return fontShadow; }
            set { fontShadow = value; }
        }

        public Color DialogColor
        {
            get { return dialogColor; }
            set { dialogColor = value; }
        }

        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }

        public bool PanelIsGlass
        {
            get { return panelIsGlass; }
            set { panelIsGlass = value; }
        }

        public bool ButtonIsGlass
        {
            get { return buttonIsGlass; }
            set { buttonIsGlass = value; }
        }

        #region IXmlSerializable Members

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Name", name);

            writeGradient(writer, "Panel", Panel);

            writeGradient(writer, "ButtonNormal", buttonNormal);
            writeGradient(writer, "ButtonHover", buttonHover);
            writeGradient(writer, "ButtonPressed", buttonPressed);
            writeGradient(writer, "ButtonDisabled", buttonDisabled);
            writeGradient(writer, "ButtonHighlight", buttonHighlight);

            writer.WriteStartElement("Font");
            writer.WriteAttributeString("Name", font.Name);

            writer.WriteAttributeString("Size", font.Size.ToString(NumberFormatInfo.InvariantInfo));

            writer.WriteAttributeString("Style", Convert.ToString((int) font.Style));
            writer.WriteAttributeString("Color",
                                        String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", foreColor.A, foreColor.R, foreColor.G,
                                                      foreColor.B));
            writer.WriteAttributeString("Shadow", Convert.ToString(fontShadow));
            writer.WriteEndElement();

            writer.WriteStartElement("BorderColor");
            writer.WriteString(String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", BorderColor.A, BorderColor.R, BorderColor.G,
                                             BorderColor.B));
            writer.WriteEndElement();

            writer.WriteStartElement("DialogColor");
            writer.WriteString(String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", DialogColor.A, DialogColor.R, DialogColor.G,
                                             DialogColor.B));
            writer.WriteEndElement();

            writer.WriteStartElement("PanelIsGlass");
            writer.WriteString(PanelIsGlass.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("ButtonIsGlass");
            writer.WriteString(ButtonIsGlass.ToString());
            writer.WriteEndElement();
        }

        public XmlSchema GetSchema()
        {
            // Need to add GlassStyle.GetSchema implementation
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.Name != "GlassStyle") return;
            name = reader.GetAttribute("Name");

            reader.ReadStartElement();

            readGradient(reader, "Panel", Panel);

            readGradient(reader, "ButtonNormal", buttonNormal);
            readGradient(reader, "ButtonHover", buttonHover);
            readGradient(reader, "ButtonPressed", buttonPressed);
            readGradient(reader, "ButtonDisabled", buttonDisabled);
            readGradient(reader, "ButtonHighlight", buttonHighlight);

            var fontName = reader.GetAttribute("Name");

            // Fixes regional number formatting problem
            var fontSize = Single.Parse(reader.GetAttribute("Size"), NumberFormatInfo.InvariantInfo);

            var fontStyle = (FontStyle) Convert.ToInt32(reader.GetAttribute("Style"));
            ForeColor = parseColor(reader.GetAttribute("Color"));
            FontShadow = Convert.ToBoolean(reader.GetAttribute("Shadow"));

            reader.Read();
            BorderColor = parseColor(reader.ReadString());
            reader.ReadEndElement();

            reader.Read();
            DialogColor = parseColor(reader.ReadString());
            reader.ReadEndElement();

            reader.Read();
            PanelIsGlass = Convert.ToBoolean(reader.ReadString());
            reader.ReadEndElement();

            reader.Read();
            ButtonIsGlass = Convert.ToBoolean(reader.ReadString());
            reader.ReadEndElement();

            reader.ReadEndElement();

            Font = new Font(fontName, fontSize, fontStyle);
        }

        #endregion

        private static void writeGradient(XmlWriter writer, string gradientName, GlassGradient gradient)
        {
            writer.WriteStartElement(gradientName);

            writer.WriteStartElement("Top");

            writer.WriteString(String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", gradient.Top.A, gradient.Top.R, gradient.Top.G,
                                             gradient.Top.B));

            writer.WriteEndElement();

            writer.WriteStartElement("Bottom");

            writer.WriteString(String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", gradient.Bottom.A, gradient.Bottom.R,
                                             gradient.Bottom.G, gradient.Bottom.B));

            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        private static Color parseColor(string colorString)
        {
            int alpha = Convert.ToByte(colorString.Substring(1, 2), 16);
            int red = Convert.ToByte(colorString.Substring(3, 2), 16);
            int green = Convert.ToByte(colorString.Substring(5, 2), 16);
            int blue = Convert.ToByte(colorString.Substring(7, 2), 16);
            return Color.FromArgb(alpha, red, green, blue);
        }

        private static void readGradient(XmlReader reader, string gradientName, GlassGradient gradient)
        {
            try
            {
                reader.ReadStartElement(gradientName);

                reader.ReadStartElement("Top");
                gradient.Top = parseColor(reader.ReadString());
                reader.ReadEndElement();

                reader.ReadStartElement("Bottom");
                gradient.Bottom = parseColor(reader.ReadString());
                reader.ReadEndElement();

                reader.ReadEndElement();
            }
            catch
            {
            }
        }
    }
}