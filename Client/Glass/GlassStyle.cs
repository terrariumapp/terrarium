//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Terrarium.Glass
{
	[ Serializable() ]
	public class GlassStyle : IXmlSerializable
	{
		protected string			name = "GlassStyle1";

		//protected GlassGradient panel = new GlassGradient(Color.FromArgb(192,192,192), Color.FromArgb(96,96,96));
        //protected GlassGradient panel = new GlassGradient(Color.FromArgb(120, 200, 120), Color.FromArgb(0, 80, 0));
        //protected GlassGradient panel = new GlassGradient(Color.FromArgb(255, 128, 128), Color.FromArgb(96, 0, 0));
        protected GlassGradient panel = new GlassGradient(Color.FromArgb(128, 128, 255), Color.FromArgb(0, 0, 96));
        //protected GlassGradient panel = new GlassGradient(Color.FromArgb(255, 255, 128), Color.FromArgb(96, 96, 0));
        //protected GlassGradient panel = new GlassGradient(Color.FromArgb(216,216,216), Color.FromArgb(152,152,152));
        //protected GlassGradient panel = new GlassGradient(Color.FromArgb(255, 128, 255), Color.FromArgb(96, 0, 96));

		protected GlassGradient buttonNormal = new GlassGradient(Color.FromArgb(160,160,160), Color.FromArgb(64,64,64));

        protected GlassGradient buttonHover = new GlassGradient(Color.FromArgb(128, 255, 128), Color.FromArgb(0, 96, 0));
        protected GlassGradient buttonPressed = new GlassGradient(Color.FromArgb(0, 64, 0), Color.FromArgb(96, 160, 96));
        //protected GlassGradient buttonHover = new GlassGradient(Color.FromArgb(255, 160, 32), Color.FromArgb(160, 80, 0));
        //protected GlassGradient buttonPressed = new GlassGradient(Color.FromArgb(160, 80, 0), Color.FromArgb(255,128,0));
        
        protected GlassGradient buttonDisabled = new GlassGradient(Color.FromArgb(64, 64, 64), Color.FromArgb(96, 96, 96));
		
        protected GlassGradient		buttonHighlight = new GlassGradient( Color.FromArgb( 216,216,216 ), Color.FromArgb( 128,128,128 ) );
		
		protected Font				font = new Font( "Verdana", 6.75f, FontStyle.Bold );
		protected Color				foreColor = Color.White;
		protected bool				fontShadow = true;

		protected Color				dialogColor = Color.FromArgb(16,16,16);
		protected Color borderColor = Color.FromArgb(32, 32, 32);

		protected bool				panelIsGlass = true;
		protected bool buttonIsGlass = true;

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		[ Category( "Panel" ) ]
		public GlassGradient Panel
		{
			get
			{
				return this.panel;
			}
			set
			{
				this.panel = value;
			}
		}

		[ Category( "Button" ) ]
		public GlassGradient ButtonNormal
		{
			get
			{
				return this.buttonNormal;
			}
			set
			{
				this.buttonNormal = value;
			}
		}

		[ Category( "Button" ) ]
		public GlassGradient ButtonHover
		{
			get
			{
				return this.buttonHover;
			}
			set
			{
				this.buttonHover = value;
			}
		}

		[ Category( "Button" ) ]
		public GlassGradient ButtonPressed
		{
			get
			{
				return this.buttonPressed;
			}
			set
			{
				this.buttonPressed = value;
			}
		}

		[ Category( "Button" ) ]
		public GlassGradient ButtonDisabled
		{
			get
			{
				return this.buttonDisabled;
			}
			set
			{
				this.buttonDisabled = value;
			}
		}

		[ Category( "Button" ) ]
		public GlassGradient ButtonHighlight
		{
			get
			{
				return this.buttonHighlight;
			}
			set
			{
				this.buttonHighlight = value;
			}
		}

		public Font Font
		{
			get
			{
				return this.font;
			}
			set
			{
				this.font = value;
			}
		}
		
		public Color ForeColor
		{
			get
			{
				return this.foreColor;
			}
			set
			{
				this.foreColor = value;
			}
		}

		public bool FontShadow
		{
			get
			{
				return this.fontShadow;
			}
			set
			{
				this.fontShadow = value;
			}
		}

		public Color DialogColor
		{
			get
			{
				return this.dialogColor;
			}
			set
			{
				this.dialogColor = value;
			}
		}
		
		public Color BorderColor
		{
			get
			{
				return this.borderColor;
			}
			set
			{
				this.borderColor = value;
			}
		}

		public bool PanelIsGlass
		{
			get
			{
				return this.panelIsGlass;
			}
			set
			{
				this.panelIsGlass = value;
			}
		}

		public bool ButtonIsGlass
		{
			get
			{
				return this.buttonIsGlass;
			}
			set
			{
				this.buttonIsGlass = value;
			}
		}

		#region IXmlSerializable Members

		private void WriteGradient(XmlWriter writer, string gradientName, GlassGradient gradient)
		{
			writer.WriteStartElement(gradientName);

			writer.WriteStartElement("Top");

			writer.WriteString(String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", gradient.Top.A, gradient.Top.R, gradient.Top.G, gradient.Top.B));

			writer.WriteEndElement();

			writer.WriteStartElement("Bottom");

			writer.WriteString(String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", gradient.Bottom.A, gradient.Bottom.R, gradient.Bottom.G, gradient.Bottom.B));

			writer.WriteEndElement();

			writer.WriteEndElement();
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("Name", this.name);

			this.WriteGradient(writer, "Panel", this.Panel);

			this.WriteGradient(writer, "ButtonNormal", this.buttonNormal);
			this.WriteGradient(writer, "ButtonHover", this.buttonHover);
			this.WriteGradient(writer, "ButtonPressed", this.buttonPressed);
			this.WriteGradient(writer, "ButtonDisabled", this.buttonDisabled);
			this.WriteGradient(writer, "ButtonHighlight", this.buttonHighlight);

			writer.WriteStartElement("Font");
			writer.WriteAttributeString("Name", this.font.Name);


			writer.WriteAttributeString("Size", this.font.Size.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));

			writer.WriteAttributeString("Style", Convert.ToString((int)this.font.Style));
			writer.WriteAttributeString("Color", String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", this.foreColor.A, this.foreColor.R, this.foreColor.G, this.foreColor.B));
			writer.WriteAttributeString("Shadow", Convert.ToString(this.fontShadow));
			writer.WriteEndElement();

			writer.WriteStartElement("BorderColor");
			writer.WriteString(String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", this.BorderColor.A, this.BorderColor.R, this.BorderColor.G, this.BorderColor.B));
			writer.WriteEndElement();

			writer.WriteStartElement("DialogColor");
			writer.WriteString(String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", this.DialogColor.A, this.DialogColor.R, this.DialogColor.G, this.DialogColor.B));
			writer.WriteEndElement();

			writer.WriteStartElement("PanelIsGlass");
			writer.WriteString(this.PanelIsGlass.ToString());
			writer.WriteEndElement();

			writer.WriteStartElement("ButtonIsGlass");
			writer.WriteString(this.ButtonIsGlass.ToString());
			writer.WriteEndElement();

		}

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			// Need to add GlassStyle.GetSchema implementation
			return null;
		}

		private Color ParseColor(string colorString)
		{
			int alpha = Convert.ToByte(colorString.Substring(1, 2), 16);
			int red = Convert.ToByte(colorString.Substring(3, 2), 16);
			int green = Convert.ToByte(colorString.Substring(5, 2), 16);
			int blue = Convert.ToByte(colorString.Substring(7, 2), 16);

			return Color.FromArgb(alpha, red, green, blue);
		}

		private void ReadGradient(XmlReader reader, string gradientName, GlassGradient gradient)
		{
			try
			{
				reader.ReadStartElement(gradientName);

				reader.ReadStartElement("Top");
				gradient.Top = this.ParseColor(reader.ReadString());
				reader.ReadEndElement();

				reader.ReadStartElement("Bottom");
				gradient.Bottom = this.ParseColor(reader.ReadString());
				reader.ReadEndElement();

				reader.ReadEndElement();
			}
			catch { }
		}

		public void ReadXml(XmlReader reader)
		{
			if (reader.Name == "GlassStyle")
			{
				this.name = reader.GetAttribute("Name");

				reader.ReadStartElement();

				ReadGradient(reader, "Panel", this.Panel);

				ReadGradient(reader, "ButtonNormal", this.buttonNormal);
				ReadGradient(reader, "ButtonHover", this.buttonHover);
				ReadGradient(reader, "ButtonPressed", this.buttonPressed);
				ReadGradient(reader, "ButtonDisabled", this.buttonDisabled);
				ReadGradient(reader, "ButtonHighlight", this.buttonHighlight);

				string fontName = reader.GetAttribute("Name");

				// Fixes regional number formatting problem
				float fontSize = Single.Parse(reader.GetAttribute("Size"), System.Globalization.NumberFormatInfo.InvariantInfo);

				FontStyle fontStyle = (FontStyle)Convert.ToInt32(reader.GetAttribute("Style"));
				this.ForeColor = ParseColor(reader.GetAttribute("Color"));
				this.FontShadow = Convert.ToBoolean(reader.GetAttribute("Shadow"));

				reader.Read();
				this.BorderColor = ParseColor(reader.ReadString());
				reader.ReadEndElement();

				reader.Read();
				this.DialogColor = ParseColor(reader.ReadString());
				reader.ReadEndElement();

				reader.Read();
				this.PanelIsGlass = Convert.ToBoolean(reader.ReadString());
				reader.ReadEndElement();

				reader.Read();
				this.ButtonIsGlass = Convert.ToBoolean(reader.ReadString());
				reader.ReadEndElement();

				reader.ReadEndElement();

				this.Font = new Font(fontName, fontSize, (FontStyle)fontStyle);
			}
		}

		#endregion
	}

}
