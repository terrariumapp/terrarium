//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;

namespace Terrarium.Server
{
    [RunInstaller(true)]
    public class WebsiteInstaller : Installer
    {
        public WebsiteInstaller()
        {
            InstallerInfo info = InstallerInfo.GetInstallerInfo();

            // Add a default one that we can always fall back to
            EventLogInstaller myEventLogInstaller = new EventLogInstaller();
            myEventLogInstaller.Source = InstallerInfo.DefaultEventLogSource;
            Installers.Add(myEventLogInstaller);

            foreach (EventLogInfo source in info.EventLogInfos)
            {
                myEventLogInstaller = new EventLogInstaller();
                myEventLogInstaller.Source = source.Source;
                Installers.Add(myEventLogInstaller);
            }

            foreach (PerformanceCounterCategoryInfo performanceCounter in info.PerformanceCounterCategoryInfos)
            {
                PerformanceCounterInstaller myCounterInstaller = new PerformanceCounterInstaller();
                myCounterInstaller.CategoryHelp = performanceCounter.CategoryHelp;
                myCounterInstaller.CategoryName = performanceCounter.CategoryName;
                ArrayList counters = new ArrayList();
                foreach (CounterCreationDataInfo creationDataInfo in performanceCounter.CounterCreationDataInfos)
                    counters.Add(new CounterCreationData(creationDataInfo.CounterName, creationDataInfo.CounterHelp,
                                                         creationDataInfo.CounterType));

                myCounterInstaller.Counters.AddRange(
                    (CounterCreationData[]) counters.ToArray(typeof (CounterCreationData)));
                Installers.Add(myCounterInstaller);
            }
        }
    }
}