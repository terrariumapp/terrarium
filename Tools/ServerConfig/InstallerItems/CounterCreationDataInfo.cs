//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System.Diagnostics;
using System.Xml.Serialization;

namespace Terrarium.Server
{
    public class CounterCreationDataInfo
    {
        private string counterHelp;
        private string counterName;
        private PerformanceCounterType counterType;
        private string id;
        private string instanceName;

        public CounterCreationDataInfo()
        {
        }

        public CounterCreationDataInfo(string counterName, string instanceName, PerformanceCounterType counterType,
                                       string counterHelp)
        {
            this.counterName = counterName;
            this.instanceName = instanceName;
            this.counterHelp = counterHelp;
            this.counterType = counterType;
        }

        [XmlAttribute("counterHelp")]
        public string CounterHelp
        {
            get { return counterHelp; }
            set { counterHelp = value; }
        }

        [XmlAttribute("counterName")]
        public string CounterName
        {
            get { return counterName; }
            set { counterName = value; }
        }

        [XmlAttribute("instanceName")]
        public string InstanceName
        {
            get { return instanceName; }
            set { instanceName = value; }
        }

        [XmlAttribute("id")]
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        [XmlAttribute("counterType")]
        public PerformanceCounterType CounterType
        {
            get { return counterType; }
            set { counterType = value; }
        }
    }
}