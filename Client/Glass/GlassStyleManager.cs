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

namespace Terrarium.Glass
{
	public sealed class GlassStyleManager
	{
		private static bool				useStyles;
		private static GlassStyle[]		loadedStyles;
		private static GlassStyle		activeStyle;
		private static GlassStyle		systemStyle;

		static GlassStyleManager()
		{
			// We only create it once so that we can change the colors
			// that clients will be referencing on the fly
			systemStyle = new GlassStyle();
			systemStyle.Name = "(System)";

			useStyles = true;
			activeStyle = new GlassStyle();
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

			GlassStyle defaultStyle = new GlassStyle();
			defaultStyle.Name = "(Default)";
			styleList.Add( defaultStyle );

			styleList.Add( systemStyle);

			foreach( string styleFile in styleFileList )
			{
				GlassStyle style = GlassStyleManager.LoadStyle( styleFile );
				styleList.Add( style );
			}

			loadedStyles = (GlassStyle[])styleList.ToArray(typeof(GlassStyle));
		}

		public static void SetStyle( string styleName )
		{
			foreach( GlassStyle style in loadedStyles )
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

		public static GlassStyle[] Styles
		{
			get
			{
				return loadedStyles;
			}
		}

		public static GlassStyle Active
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

		public static GlassStyle SystemStyle
		{
			get
			{
				return systemStyle;
			}
		}

		public static GlassStyle LoadStyle( string fileName )
		{
			try
			{
				System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer( typeof(GlassStyle) );

				System.IO.Stream stream = System.IO.File.OpenRead( fileName );
				GlassStyle style = (GlassStyle)serializer.Deserialize( stream );
				stream.Close();
				stream = null;

				return style;
			}
			catch
			{
				// Need to log error
				return new GlassStyle();
			}
		}

		public static void RefreshSystemStyle()
		{
			//systemStyle.PanelRaised.Bottom = System.Windows.Forms.ControlPaint.Dark( SystemColors.ActiveCaption );
			//systemStyle.PanelRaised.Top = System.Windows.Forms.ControlPaint.LightLight( SystemColors.ActiveCaption );
			
			//systemStyle.PanelSunk.Top = System.Windows.Forms.ControlPaint.Dark( SystemColors.AppWorkspace );
			//systemStyle.PanelSunk.Bottom = SystemColors.AppWorkspace; //System.Windows.Forms.ControlPaint.Light( SystemColors.AppWorkspace );

			//systemStyle.ButtonNormal.Bottom = System.Windows.Forms.ControlPaint.Dark( SystemColors.Control );
			//systemStyle.ButtonNormal.Top = System.Windows.Forms.ControlPaint.LightLight( SystemColors.Control );

			//systemStyle.ButtonHover.Bottom = System.Windows.Forms.ControlPaint.Dark( SystemColors.Highlight );
			//systemStyle.ButtonHover.Top = System.Windows.Forms.ControlPaint.Light( SystemColors.Highlight );

			//systemStyle.ButtonPressed.Top = System.Windows.Forms.ControlPaint.Dark( SystemColors.ControlDarkDark );
			//systemStyle.ButtonPressed.Bottom = System.Windows.Forms.ControlPaint.LightLight( SystemColors.ControlDarkDark );
		}

		private GlassStyleManager(){}

	}
}
