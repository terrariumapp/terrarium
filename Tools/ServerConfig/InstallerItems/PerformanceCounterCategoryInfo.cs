//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System.Xml.Serialization;

namespace Terrarium.Server
{
    public class PerformanceCounterCategoryInfo
    {
        private string categoryHelp;
        private string categoryName;
        private CounterCreationDataInfo[] counterCreationDataInfos;

        public PerformanceCounterCategoryInfo()
        {
        }

        public PerformanceCounterCategoryInfo(string categoryName, string categoryHelp,
                                              CounterCreationDataInfo[] counterCreationDataInfos)
        {
            this.categoryHelp = categoryHelp;
            this.categoryName = categoryName;
            this.counterCreationDataInfos = counterCreationDataInfos;
        }

        [XmlAttribute("categoryHelp")]
        public string CategoryHelp
        {
            get { return categoryHelp; }
            set { categoryHelp = value; }
        }

        [XmlAttribute("categoryName")]
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        public CounterCreationDataInfo[] CounterCreationDataInfos
        {
            get { return counterCreationDataInfos; }
            set { counterCreationDataInfos = value; }
        }
    }
}