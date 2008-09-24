//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System.Xml.Serialization;

namespace Terrarium.Server
{
    public class EventLogInfo
    {
        private string id = "";
        private string source = "";

        public EventLogInfo()
        {
        }

        public EventLogInfo(string source)
        {
            this.source = source;
        }

        [XmlAttribute("source")]
        public string Source
        {
            get { return source; }

            set { source = value; }
        }

        [XmlAttribute("id")]
        public string ID
        {
            get { return id; }

            set { id = value; }
        }
    }
}