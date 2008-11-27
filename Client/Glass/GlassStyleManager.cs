//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Terrarium.Glass
{
    public static class GlassStyleManager
    {
        static GlassStyleManager()
        {
            // We only create it once so that we can change the colors
            // that clients will be referencing on the fly
            SystemStyle = new GlassStyle {Name = "(System)"};
            UseStyles = true;
            Active = new GlassStyle();
        }

        public static bool UseStyles { get; set; }

        public static GlassStyle[] Styles { get; private set; }

        public static GlassStyle Active { get; set; }

        public static GlassStyle SystemStyle { get; private set; }

        public static void Refresh()
        {
            var styleFileList = new List<string>();

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

            var styleList = new List<GlassStyle>();

            var defaultStyle = new GlassStyle {Name = "(Default)"};
            styleList.Add(defaultStyle);
            styleList.Add(SystemStyle);

            foreach (var styleFile in styleFileList)
            {
                var style = LoadStyle(styleFile);
                styleList.Add(style);
            }

            Styles = styleList.ToArray();
        }

        public static void SetStyle(string styleName)
        {
            foreach (var style in Styles)
            {
                if (style.Name != styleName) continue;
                Active = style;
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