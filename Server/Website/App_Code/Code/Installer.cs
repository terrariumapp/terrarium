//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------


using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Reflection;
using System.IO;
using System.Collections;

namespace Terrarium.Server {
    [RunInstaller(true)]
    public class WebsiteInstaller: Installer {
        public WebsiteInstaller() {
            InstallerInfo info = InstallerInfo.GetInstallerInfo();

            // Add a default one that we can always fall back to
            EventLogInstaller myEventLogInstaller = new EventLogInstaller();
            myEventLogInstaller.Source = InstallerInfo.DefaultEventLogSource;
            Installers.Add(myEventLogInstaller);   

            foreach(EventLogInfo source in info.EventLogInfos) { 
                myEventLogInstaller = new EventLogInstaller();
                myEventLogInstaller.Source = source.Source;
                Installers.Add(myEventLogInstaller);   
            }

            foreach(PerformanceCounterCategoryInfo performanceCounter in info.PerformanceCounterCategoryInfos) {
                PerformanceCounterInstaller myCounterInstaller = new PerformanceCounterInstaller();
                myCounterInstaller.CategoryHelp = performanceCounter.CategoryHelp;
                myCounterInstaller.CategoryName = performanceCounter.CategoryName;
                ArrayList counters = new ArrayList();
                foreach(CounterCreationDataInfo creationDataInfo in performanceCounter.CounterCreationDataInfos)
                    counters.Add(new CounterCreationData(creationDataInfo.CounterName, creationDataInfo.CounterHelp, creationDataInfo.CounterType));

                myCounterInstaller.Counters.AddRange((CounterCreationData []) counters.ToArray(typeof(CounterCreationData)));
                Installers.Add(myCounterInstaller);
            }
        }
    }

    public class InstallerInfo {
        EventLogInfo [] eventLogInfos;
        PerformanceCounterCategoryInfo [] performanceCounterCategoryInfos;
        static InstallerInfo installerInfo;

        public PerformanceCounterCategoryInfo [] PerformanceCounterCategoryInfos {
            get {
                return performanceCounterCategoryInfos;
            }

            set {
                performanceCounterCategoryInfos = value;
            }
        }

        public EventLogInfo [] EventLogInfos {
            get {
                return eventLogInfos;
            }

            set {
                eventLogInfos = value;
            }
        }

        public static string DefaultEventLogSource {
            get {
                return  "Website Installer";
            }
        }

        public static void WriteEventLog(string id, string entry) {
            WriteEventLog(id, entry, EventLogEntryType.Error);
        }

        public static void WriteEventLog(string id, string entry, EventLogEntryType type) {
            InstallerInfo installerInfo = GetInstallerInfo();
            string source = DefaultEventLogSource;

            foreach(EventLogInfo info in installerInfo.EventLogInfos) {
                if(info.ID.ToLower() == id.ToLower()) {
                    source = info.Source;
                    break;
                }
            }

            EventLog myLog = new EventLog();
            myLog.Source = source;
            myLog.WriteEntry(entry, type);
        }

        public static PerformanceCounter CreatePerformanceCounter(string id) {
            InstallerInfo installerInfo = GetInstallerInfo();

            foreach(PerformanceCounterCategoryInfo info in installerInfo.PerformanceCounterCategoryInfos) {
                foreach(CounterCreationDataInfo ccd in info.CounterCreationDataInfos) {
                    if(ccd.ID.ToLower() == id.ToLower())
                        return new PerformanceCounter(info.CategoryName, ccd.CounterName, ccd.InstanceName, false);
                }
            }

            return null;
        }

        public static InstallerInfo GetInstallerInfo() {
            if(installerInfo != null)
                return installerInfo;

            XmlSerializer serializer = new XmlSerializer(typeof(InstallerInfo));

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(Resources.Resource.install);
            writer.Flush();
            stream.Position = 0;

            installerInfo = (InstallerInfo)serializer.Deserialize(stream);

            stream.Close();

            //using(TextReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("install.xml"))) {
            //    installerInfo = (InstallerInfo) serializer.Deserialize(reader);
            //}

            return installerInfo;
        }
    }

    public class EventLogInfo {
        string source = "";
        string id = "";

        public EventLogInfo() {
        }

        public EventLogInfo(string source) {
            this.source = source;
        }
    
        [XmlAttribute("source")]
        public string Source {
            get {
                return source;
            }

            set {
                source = value;
            }
        }

        [XmlAttribute("id")]
        public string ID {
            get {
                return id;
            }

            set {
                id = value;
            }
        }
    }

    public class PerformanceCounterCategoryInfo {
        string categoryHelp;
        string categoryName;
        CounterCreationDataInfo [] counterCreationDataInfos;

        public PerformanceCounterCategoryInfo() {
        }

        public PerformanceCounterCategoryInfo(string categoryName, string categoryHelp, CounterCreationDataInfo [] counterCreationDataInfos) {
            this.categoryHelp = categoryHelp;
            this.categoryName = categoryName;
            this.counterCreationDataInfos = counterCreationDataInfos;
        }

        [XmlAttribute("categoryHelp")]
        public string CategoryHelp {
            get {
                return categoryHelp;
            }
            set {
                categoryHelp = value;
            }
        }

        [XmlAttribute("categoryName")]
        public string CategoryName {
            get {
                return categoryName;
            }
            set {
                categoryName = value;
            }
        }

        public CounterCreationDataInfo [] CounterCreationDataInfos {
            get {
                return counterCreationDataInfos;
            }
            set {
                counterCreationDataInfos = value;
            }
        }
    }

    public class CounterCreationDataInfo {
        string counterName;
        string instanceName;
        string counterHelp;
        string id;
        PerformanceCounterType counterType;

        public CounterCreationDataInfo() {
        }

        public CounterCreationDataInfo(string counterName, string instanceName, PerformanceCounterType counterType, string counterHelp) {
            this.counterName = counterName;
            this.instanceName = instanceName;
            this.counterHelp = counterHelp;
            this.counterType = counterType;
        }

        [XmlAttribute("counterHelp")]
        public string CounterHelp {
            get {
                return counterHelp;
            }
            set {
                counterHelp = value;
            }
        }

        [XmlAttribute("counterName")]
        public string CounterName {
            get {
                return counterName;
            }
            set {
                counterName = value;
            }
        }

        [XmlAttribute("instanceName")]
        public string InstanceName {
            get {
                return instanceName;
            }
            set {
                instanceName = value;
            }
        }

        [XmlAttribute("id")]
        public string ID {
            get {
                return id;
            }
            set {
                id = value;
            }
        }

        [XmlAttribute("counterType")]
        public PerformanceCounterType CounterType {
            get {
                return counterType;
            }
            set {
                counterType = value;
            }
        }
    }
}
