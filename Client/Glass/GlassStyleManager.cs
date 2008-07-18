//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

namespace Terrarium.Glass
{
    public static class GlassStyleManager
    {
        private static readonly GlassStyle _systemStyle;
        private static GlassStyle _activeStyle;
        private static GlassStyle[] _loadedStyles;
        private static bool _useStyles;

        static GlassStyleManager()
        {
            // We only create it once so that we can change the colors
            // that clients will be referencing on the fly
            _systemStyle = new GlassStyle();
            _systemStyle.Name = "(System)";

            _useStyles = true;
            _activeStyle = new GlassStyle();
        }

        public static bool UseStyles
        {
            get { return _useStyles; }
            set { _useStyles = value; }
        }

        public static GlassStyle[] Styles
        {
            get { return _loadedStyles; }
        }

        public static GlassStyle Active
        {
            get { return _activeStyle; }
            set { _activeStyle = value; }
        }

        public static GlassStyle SystemStyle
        {
            get { return _systemStyle; }
        }

        public static void Refresh()
        {
            ArrayList styleFileList = new ArrayList();

            string[] styleFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.style");
            foreach (string styleFile in styleFiles)
                styleFileList.Add(styleFile);

            // The Terrarium directory may not exist...
            try
            {
                string terrariumDocumentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal) +
                                                     "\\Terrarium";
                styleFiles = Directory.GetFiles(terrariumDocumentsDirectory, "*.style");
                foreach (string styleFile in styleFiles)
                    styleFileList.Add(styleFile);
            }
            catch
            {
            }

            ArrayList styleList = new ArrayList();

            GlassStyle defaultStyle = new GlassStyle();
            defaultStyle.Name = "(Default)";
            styleList.Add(defaultStyle);

            styleList.Add(_systemStyle);

            foreach (string styleFile in styleFileList)
            {
                GlassStyle style = LoadStyle(styleFile);
                styleList.Add(style);
            }

            _loadedStyles = (GlassStyle[]) styleList.ToArray(typeof (GlassStyle));
        }

        public static void SetStyle(string styleName)
        {
            foreach (GlassStyle style in _loadedStyles)
            {
                if (style.Name == styleName)
                {
                    _activeStyle = style;
                    break;
                }
            }
        }

        public static GlassStyle LoadStyle(string fileName)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof (GlassStyle));

                Stream stream = File.OpenRead(fileName);
                GlassStyle style = (GlassStyle) serializer.Deserialize(stream);
                stream.Close();

                return style;
            }
            catch
            {
                // Need to log error
                return new GlassStyle();
            }
        }
    }
}