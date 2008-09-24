//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace Terrarium.Server
{
    public class InstallerInfo
    {
        private static InstallerInfo installerInfo;
        private EventLogInfo[] eventLogInfos;
        private PerformanceCounterCategoryInfo[] performanceCounterCategoryInfos;

        public PerformanceCounterCategoryInfo[] PerformanceCounterCategoryInfos
        {
            get { return performanceCounterCategoryInfos; }

            set { performanceCounterCategoryInfos = value; }
        }

        public EventLogInfo[] EventLogInfos
        {
            get { return eventLogInfos; }

            set { eventLogInfos = value; }
        }

        public static string DefaultEventLogSource
        {
            get { return "Website Installer"; }
        }

        public static void WriteEventLog(string id, string entry)
        {
            WriteEventLog(id, entry, EventLogEntryType.Error);
        }

        public static void WriteEventLog(string id, string entry, EventLogEntryType type)
        {
            InstallerInfo info = GetInstallerInfo();
            string source = DefaultEventLogSource;

            foreach (EventLogInfo eventLogInfo in info.EventLogInfos)
            {
                if (eventLogInfo.ID.ToLower() != id.ToLower()) continue;
                source = eventLogInfo.Source;
                break;
            }

            EventLog myLog = new EventLog();
            myLog.Source = source;
            myLog.WriteEntry(entry, type);
        }

        public static PerformanceCounter CreatePerformanceCounter(string id)
        {
            InstallerInfo info = GetInstallerInfo();

            foreach (PerformanceCounterCategoryInfo categoryInfo in info.PerformanceCounterCategoryInfos)
            {
                foreach (CounterCreationDataInfo ccd in categoryInfo.CounterCreationDataInfos)
                {
                    if (ccd.ID.ToLower() == id.ToLower())
                        return new PerformanceCounter(categoryInfo.CategoryName, ccd.CounterName, ccd.InstanceName, false);
                }
            }

            return null;
        }

        public static InstallerInfo GetInstallerInfo()
        {
            if (installerInfo != null)
                return installerInfo;

            XmlSerializer serializer = new XmlSerializer(typeof (InstallerInfo));
            using (
                TextReader reader =
                    new StreamReader(
                        Assembly.GetExecutingAssembly().GetManifestResourceStream("InstallerItems.install.xml")))
            {
                installerInfo = (InstallerInfo) serializer.Deserialize(reader);
            }

            return installerInfo;
        }
    }
}