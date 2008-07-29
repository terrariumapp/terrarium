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

namespace Terrarium.Metal
{
	[ Serializable() ]
	public class MetalStyle : IXmlSerializable
	{
		protected string			name = "MetalStyle1";

		protected MetalGradient		panelRaised = new MetalGradient( Color.FromArgb( 160, 160, 160 ), Color.FromArgb( 32, 32, 32 ) );
		protected MetalGradient		panelSunk = new MetalGradient( Color.FromArgb( 0, 0, 0 ), Color.FromArgb( 96, 96, 96 ) );
		
		protected MetalGradient		buttonNormal = new MetalGradient( Color.FromArgb( 96, 96, 96 ), Color.FromArgb( 0, 0, 0 ) );
		protected MetalGradient		buttonHover = new MetalGradient( Color.FromArgb( 0, 255, 0 ), Color.FromArgb( 0, 64, 0 ) );
		protected MetalGradient		buttonPressed = new MetalGradient( Color.FromArgb( 0, 64, 0 ), Color.FromArgb( 0, 255, 0 ) );
		protected MetalGradient		buttonDisabled = new MetalGradient( Color.FromArgb( 64, 64, 64 ), Color.FromArgb( 64, 64, 64 ) );
		protected MetalGradient		buttonHighlight = new MetalGradient( Color.FromArgb( 192, 192, 192 ), Color.FromArgb( 96, 96, 96 ) );

		protected MetalGradient		ledIdle = new MetalGradient( Color.FromArgb( 64, 255, 64 ), Color.FromArgb( 0, 96, 0 ) );
		protected MetalGradient		ledWaiting = new MetalGradient( Color.FromArgb( 255, 255, 64 ), Color.FromArgb( 96, 96, 0 ) );
		protected MetalGradient		ledFailed = new MetalGradient( Color.FromArgb( 255,64 , 64 ), Color.FromArgb( 96, 0, 0 ) );
		
		protected Font				font = new Font( "Verdana", 6.75f, FontStyle.Bold );
		protected Color				foreColor = Color.White;
		protected bool				fontShadow = true;

		protected Color				borderColor = Color.Black;

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
		public MetalGradient PanelRaised
		{
			get
			{
				return this.panelRaised;
			}
			set
			{
				this.panelRaised = value;
			}
		}

		[ Category( "Panel" ) ]
		public MetalGradient PanelSunk
		{
			get
			{
				return this.panelSunk;
			}
			set
			{
				this.panelSunk = value;
			}
		}

		[ Category( "Button" ) ]
		public MetalGradient ButtonNormal
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
		public MetalGradient ButtonHover
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
		public MetalGradient ButtonPressed
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
		public MetalGradient ButtonDisabled
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
		public MetalGradient ButtonHighlight
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

		[ Category( "Led" ) ]
		public MetalGradient LedIdle
		{
			get
			{
				return this.ledIdle;
			}
			set
			{
				this.ledIdle = value;
			}
		}

		[ Category( "Led" ) ]
		public MetalGradient LedWaiting
		{
			get
			{
				return this.ledWaiting;
			}
			set
			{
				this.ledWaiting = value;
			}
		}
		[ Category( "Led" ) ]
		public MetalGradient LedFailed
		{
			get
			{
				return this.ledFailed;
			}
			set
			{
				this.ledFailed = value;
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


		#region IXmlSerializable Members

		private void WriteGradient( XmlWriter writer, string gradientName, MetalGradient gradient )
		{
			writer.WriteStartElement( gradientName );
			
			writer.WriteStartElement( "Top" );

			writer.WriteString(	String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", gradient.Top.A, gradient.Top.R, gradient.Top.G, gradient.Top.B));

			writer.WriteEndElement();
			
			writer.WriteStartElement( "Bottom" );
			
			writer.WriteString(	String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", gradient.Bottom.A, gradient.Bottom.R, gradient.Bottom.G, gradient.Bottom.B));

			writer.WriteEndElement();

			writer.WriteEndElement();
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString( "Name", this.name );

			this.WriteGradient( writer, "PanelRaised", this.PanelRaised );
			this.WriteGradient( writer, "PanelSunk", this.PanelSunk );

			this.WriteGradient( writer, "ButtonNormal", this.buttonNormal );
			this.WriteGradient( writer, "ButtonHover", this.buttonHover );
			this.WriteGradient( writer, "ButtonPressed", this.buttonPressed );
			this.WriteGradient( writer, "ButtonDisabled", this.buttonDisabled );
			this.WriteGradient( writer, "ButtonHighlight", this.buttonHighlight );

			this.WriteGradient( writer, "LedIdle", this.ledIdle );
			this.WriteGradient( writer, "LedWaiting", this.ledWaiting );
			this.WriteGradient( writer, "LedFailed", this.ledFailed );

			writer.WriteStartElement( "Font" );
			writer.WriteAttributeString( "Name", this.font.Name );

			
			writer.WriteAttributeString( "Size", this.font.Size.ToString( System.Globalization.NumberFormatInfo.InvariantInfo ) );

//			writer.WriteAttributeString( "Size", Convert.ToString( this.font.Size ) );

			writer.WriteAttributeString( "Style", Convert.ToString( (int)this.font.Style ) );
			writer.WriteAttributeString( "Color", String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", this.foreColor.A, this.foreColor.R, this.foreColor.G, this.foreColor.B) );
			writer.WriteAttributeString( "Shadow", Convert.ToString( this.fontShadow ) );
			writer.WriteEndElement();

			writer.WriteStartElement( "Border" );
			writer.WriteString(	String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", this.BorderColor.A, this.BorderColor.R, this.BorderColor.G, this.BorderColor.B));
			writer.WriteEndElement();
		}

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			// TODO:  Add MetalStyle.GetSchema implementation
			return null;
		}

		private Color ParseColor( string colorString )
		{
			int alpha = Convert.ToByte(colorString.Substring( 1,2 ), 16 );
			int red = Convert.ToByte(colorString.Substring( 3,2 ), 16 );
			int green = Convert.ToByte(colorString.Substring( 5,2 ), 16 );
			int blue = Convert.ToByte(colorString.Substring( 7,2 ), 16 );

			return Color.FromArgb( alpha, red, green, blue );
		}

		private void ReadGradient( XmlReader reader, string gradientName, MetalGradient gradient )
		{
			try
			{
				reader.ReadStartElement( gradientName );
				
				reader.ReadStartElement( "Top" );
				gradient.Top = this.ParseColor( reader.ReadString() );
				reader.ReadEndElement();

				reader.ReadStartElement( "Bottom" );
				gradient.Bottom = this.ParseColor( reader.ReadString() );
				reader.ReadEndElement();
				
				reader.ReadEndElement();
			}
			catch{}
		}

		public void ReadXml(XmlReader reader)
		{
			if ( reader.Name == "MetalStyle" )
			{
				this.name = reader.GetAttribute("Name");

				reader.ReadStartElement();

				ReadGradient( reader, "PanelRaised", this.PanelRaised );
				ReadGradient( reader, "PanelSunk", this.PanelSunk );

				ReadGradient( reader, "ButtonNormal", this.buttonNormal );
				ReadGradient( reader, "ButtonHover", this.buttonHover );
				ReadGradient( reader, "ButtonPressed", this.buttonPressed );
				ReadGradient( reader, "ButtonDisabled", this.buttonDisabled );
				ReadGradient( reader, "ButtonHighlight", this.buttonHighlight );

				ReadGradient( reader, "LedIdle", this.ledIdle );
				ReadGradient( reader, "LedWaiting", this.ledWaiting );
				ReadGradient( reader, "LedFailed", this.ledFailed );

				string fontName = reader.GetAttribute( "Name" );
				
				// Fixes regional number formatting problem
				float fontSize = Single.Parse( reader.GetAttribute( "Size" ), System.Globalization.NumberFormatInfo.InvariantInfo );

				FontStyle fontStyle = (FontStyle)Convert.ToInt32( reader.GetAttribute( "Style" ) );
				this.ForeColor = ParseColor( reader.GetAttribute( "Color" ) );
				this.FontShadow = Convert.ToBoolean( reader.GetAttribute( "Shadow" ) );

				reader.Read();
				this.BorderColor = ParseColor( reader.ReadString() );
				reader.ReadEndElement();

				reader.ReadEndElement();

				this.Font = new Font( fontName, fontSize, (FontStyle)fontStyle );
			}
		}

		#endregion
	}

}
