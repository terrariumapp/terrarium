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
            _systemStyle = new GlassStyle {Name = "(System)"};
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
            var styleFileList = new ArrayList();

            var styleFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.style");
            foreach (var styleFile in styleFiles)
                styleFileList.Add(styleFile);

            // The Terrarium directory may not exist...
            try
            {
                var terrariumDocumentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal) +
                                                  "\\Terrarium";
                styleFiles = Directory.GetFiles(terrariumDocumentsDirectory, "*.style");
                foreach (var styleFile in styleFiles)
                    styleFileList.Add(styleFile);
            }
            catch
            {
            }

            var styleList = new ArrayList();

            var defaultStyle = new GlassStyle {Name = "(Default)"};
            styleList.Add(defaultStyle);
            styleList.Add(_systemStyle);

            foreach (string styleFile in styleFileList)
            {
                var style = LoadStyle(styleFile);
                styleList.Add(style);
            }

            _loadedStyles = (GlassStyle[]) styleList.ToArray(typeof (GlassStyle));
        }

        public static void SetStyle(string styleName)
        {
            foreach (var style in _loadedStyles)
            {
                if (style.Name != styleName) continue;
                _activeStyle = style;
                break;
            }
        }

        public static GlassStyle LoadStyle(string fileName)
        {
            try
            {
                var serializer = new XmlSerializer(typeof (GlassStyle));

                Stream stream = File.OpenRead(fileName);
                var style = (GlassStyle) serializer.Deserialize(stream);
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