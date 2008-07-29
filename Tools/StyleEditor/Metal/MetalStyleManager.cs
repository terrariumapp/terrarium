//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Terrarium.Metal
{
	public sealed class MetalStyleManager
	{
		private static bool				useStyles;
		private static MetalStyle[]		loadedStyles;
		private static MetalStyle		activeStyle;
		private static MetalStyle		systemStyle;

		static MetalStyleManager()
		{
			// We only create it once so that we can change the colors
			// that clients will be referencing on the fly
			systemStyle = new MetalStyle();
			systemStyle.Name = "(System)";

			useStyles = true;
			activeStyle = new MetalStyle();
			RefreshSystemStyle();
		}

		public static void Refresh()
		{
			ArrayList styleFileList = new ArrayList();

			string[] styleFiles = Directory.GetFiles( Environment.CurrentDirectory, "*.style" );
			foreach( string styleFile in styleFiles )
				styleFileList.Add( styleFile );

			// The Terrarium directory may not exist...
			try
			{
				string terrariumDocumentsDirectory = Environment.GetFolderPath( Environment.SpecialFolder.Personal ) + "\\Terrarium";
				styleFiles = Directory.GetFiles( terrariumDocumentsDirectory, "*.style" );
				foreach( string styleFile in styleFiles )
					styleFileList.Add( styleFile );
			}
			catch{}

			ArrayList styleList = new ArrayList();

			MetalStyle defaultStyle = new MetalStyle();
			defaultStyle.Name = "(Default)";
			styleList.Add( defaultStyle );

			styleList.Add( systemStyle);

			foreach( string styleFile in styleFileList )
			{
				MetalStyle style = MetalStyleManager.LoadStyle( styleFile );
				styleList.Add( style );
			}

			loadedStyles = (MetalStyle[])styleList.ToArray(typeof(MetalStyle));
		}

		public static void SetStyle( string styleName )
		{
			foreach( MetalStyle style in loadedStyles )
			{
				if ( style.Name == styleName )
				{
					activeStyle = style;
					break;
				}
			}
		}

		public static bool UseStyles
		{
			get
			{
				return useStyles;
			}
			set
			{
				useStyles = value;
			}
		}

		public static MetalStyle[] Styles
		{
			get
			{
				return loadedStyles;
			}
		}

		public static MetalStyle Active
		{
			get
			{
				return activeStyle;
			}
			set
			{
				activeStyle = value;
			}
		}

		public static MetalStyle SystemStyle
		{
			get
			{
				return systemStyle;
			}
		}

		public static MetalStyle LoadStyle( string fileName )
		{
			try
			{
				System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer( typeof(MetalStyle) );

				System.IO.Stream stream = System.IO.File.OpenRead( fileName );
				MetalStyle style = (MetalStyle)serializer.Deserialize( stream );
				stream.Close();
				stream = null;

				return style;
			}
			catch
			{
				// TODO: Log
				return new MetalStyle();
			}
		}

		public static void RefreshSystemStyle()
		{
			systemStyle.PanelRaised.Bottom = System.Windows.Forms.ControlPaint.Dark( SystemColors.ActiveCaption );
			systemStyle.PanelRaised.Top = System.Windows.Forms.ControlPaint.LightLight( SystemColors.ActiveCaption );
			
			systemStyle.PanelSunk.Top = System.Windows.Forms.ControlPaint.Dark( SystemColors.AppWorkspace );
			systemStyle.PanelSunk.Bottom = SystemColors.AppWorkspace; //System.Windows.Forms.ControlPaint.Light( SystemColors.AppWorkspace );

			systemStyle.ButtonNormal.Bottom = System.Windows.Forms.ControlPaint.Dark( SystemColors.Control );
			systemStyle.ButtonNormal.Top = System.Windows.Forms.ControlPaint.LightLight( SystemColors.Control );

			systemStyle.ButtonHover.Bottom = System.Windows.Forms.ControlPaint.Dark( SystemColors.Highlight );
			systemStyle.ButtonHover.Top = System.Windows.Forms.ControlPaint.Light( SystemColors.Highlight );

			systemStyle.ButtonPressed.Top = System.Windows.Forms.ControlPaint.Dark( SystemColors.ControlDarkDark );
			systemStyle.ButtonPressed.Bottom = System.Windows.Forms.ControlPaint.LightLight( SystemColors.ControlDarkDark );
		}

		private MetalStyleManager(){}

	}
}
