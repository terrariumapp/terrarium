using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Web.Services.Protocols;
using OrganismBase;
using Terrarium.Configuration;
using Terrarium.Forms;
using Terrarium.Services.Reporting;
using Terrarium.Tools;

namespace Terrarium.Game
{
    /// <summary>
    ///  Represents the population state of a Terrarium by handling
    ///  all game events that lead to adding or removing creatures
    ///  from the system.  Also handles all population change reasons
    ///  to give per reason reporting.
    /// </summary>
    [Serializable]
    public class PopulationData
    {
        private const int ticksToReport = 600; // 2 ticks per second * 60 seconds * 5 minutes
        private readonly bool reportDataToServer = true;
        private int currentTick = -1;
        private DataSet data;
        private bool firstTick = true;
        private int lastReportedTick = -1; // used only for debugging
        [NonSerialized] private TerrariumLed led;

        private bool nodeCorrupted;
        private bool organismBlacklisted;
        private DataSet peerTotalsData;

        [NonSerialized] private WebClientAsyncResult pendingAsyncResult;
        private DataTable populationChangeTable; // One row per species, current population, min and max

        [NonSerialized] private ReportingService reportingService;
        private bool reportNow;
        private DataTable tickTable; // One row per species, current population, min and max
        private bool timedOut;

        private DataTable // one row per tick
            totalsTable; // One row per species, current population, min and max

        /// <summary>
        ///  Creates a new PopulationData class that can be used to report data and
        ///  update a status LED.
        /// </summary>
        /// <param name="reportData">Should data be reported to the server.</param>
        /// <param name="led">An LED used to indicate status.</param>
        public PopulationData(bool reportData, TerrariumLed led)
        {
            reportDataToServer = reportData;
            this.led = led;
            ResetData();
        }

        /// <summary>
        ///  Remebers the last tick that data was reported to the server.
        /// </summary>
        public int LastReportedTick
        {
            get { return lastReportedTick; }
        }

        /// <summary>
        ///  Initializes the web service proxies.  These are dynamically generated and so
        ///  this method only generates them one time to improve memory and performance.
        /// </summary>
        public void InitWebService()
        {
            if (reportingService == null)
            {
                reportingService = new ReportingService();
                reportingService.Url = GameConfig.WebRoot + "/reporting/reportpopulation.asmx";
                reportingService.Timeout = 60000;
            }
        }

        /// <summary>
        ///  Closes the PopulationData class and stops in any progress or pending
        ///  connections.
        /// </summary>
        public void Close()
        {
            if (pendingAsyncResult != null)
            {
                pendingAsyncResult.Abort();
            }
        }

        /// <summary>
        ///  Resets the data in the PopulationData class to initial values.
        /// </summary>
        private void ResetData()
        {
            data = new DataSet();
            data.Locale = CultureInfo.InvariantCulture;

            populationChangeTable = data.Tables.Add("PopulationChange");
            populationChangeTable.Columns.Add("TickNumber", typeof (Int32));
            populationChangeTable.Columns.Add("Species", typeof (String));
            populationChangeTable.Columns.Add("Delta", typeof (Int32));
            populationChangeTable.Columns.Add("Reason", typeof (PopulationChangeReason));
            populationChangeTable.PrimaryKey = new DataColumn[]
                                                   {
                                                       populationChangeTable.Columns["TickNumber"],
                                                       populationChangeTable.Columns["Species"],
                                                       populationChangeTable.Columns["Reason"]
                                                   };

            tickTable = data.Tables.Add("Tick");
            tickTable.Columns.Add("Number", typeof (Int32));
            data.Relations.Add("TickPopulationChangeRelation", tickTable.Columns["Number"],
                               populationChangeTable.Columns["TickNumber"]);

            totalsTable = data.Tables.Add("Totals");
            totalsTable.Columns.Add("Species", typeof (String));
            data.Relations.Add("TotalsPopulationChangeRelation", totalsTable.Columns["Species"],
                               populationChangeTable.Columns["Species"]);

            totalsTable.Columns.Add("Population", typeof (Int32), "Sum(Child(TotalsPopulationChangeRelation).Delta)");
            DataColumn column = totalsTable.Columns.Add("MaxPopulation", typeof (Int32));
            column.DefaultValue = 0;
            column = totalsTable.Columns.Add("MinPopulation", typeof (Int32));
            column.DefaultValue = 0;

            totalsTable.PrimaryKey = new DataColumn[] {totalsTable.Columns["Species"]};
        }

        /// <summary>
        ///  Determines if the current tick is a reporting tick.  We only report data 
        ///  every so often, and a report tick is the tick when we report data.
        /// </summary>
        /// <param name="tickNumber">The current tick number.</param>
        /// <returns>True if the tick is a reporting tick, false otherwise.</returns>
        public bool IsReportingTick(int tickNumber)
        {
            return (tickNumber != 0 && tickNumber%ticksToReport == 0) || reportNow;
        }

        /// <summary>
        ///  Begins a game engine tick indicating the tick number and the world
        ///  state GUID.
        /// </summary>
        /// <param name="tickNumber">The current tick number.</param>
        /// <param name="guid">The world state GUID.</param>
        public void BeginTick(int tickNumber, Guid guid)
        {
            Debug.Assert(currentTick == -1, "End Tick not called");

            // The last time we updated the server it may have told us that
            // we were timed out or corrupted or that organisms were blacklisted.
            // Report those errors here so that they are easier to handle.
            if (timedOut)
            {
                throw new StateTimedOutException();
            }

            if (nodeCorrupted)
            {
                throw new StateCorruptedException(lastReportedTick);
            }

            if (organismBlacklisted)
            {
                // Make sure we don't throw again on shutdown
                organismBlacklisted = false;
                throw new OrganismBlacklistException();
            }

            Boolean resetData = false;
            DataSet reportData;
            DataTable reportTable = null;

            currentTick = tickNumber;

            // Every ticksToReport ticks (skipping tick 0), we report data to the server
            if (IsReportingTick(tickNumber))
            {
                reportNow = false;
                reportData = CreateReportDataSet();
                reportTable = reportData.Tables["History"];
                FillReportTable(reportTable, guid, currentTick);
                AddToTotals(ref peerTotalsData, reportData);

                if (reportDataToServer)
                {
                    ReportData(reportData, guid);
                    lastReportedTick = currentTick;
                }
                else
                {
                    lastReportedTick = -1;
                }

                // Reset the data and start over with current populations
                ResetData();
                resetData = true;
            }

            DataRow newRow = tickTable.NewRow();
            newRow["Number"] = tickNumber;
            tickTable.Rows.Add(newRow);

            // If we've sent the data to the server, reseed the new dataset with whatever the current 
            // population was from the previous dataset so we start with the correct count.
            if (resetData)
            {
                foreach (DataRow row in reportTable.Rows)
                {
                    if (NullToZero(row["Population"]) > 0)
                    {
                        CountOrganism((string) row["SpeciesName"], PopulationChangeReason.Initial,
                                      (int) row["Population"]);
                    }
                }
            }
        }

        /// <summary>
        ///  Gets a dataset of the current reporting statistics.  This data can
        ///  be used to update datagrids, charts, or other reporting facilities.
        /// </summary>
        /// <param name="peerGuid">The world state GUID for this peer.</param>
        /// <param name="tick">The current tick number.</param>
        /// <returns>The current world data.</returns>
        public DataSet GetCurrentReportingStats(Guid peerGuid, int tick)
        {
            // Get data since last reporting period
            DataSet newData = CreateReportDataSet();
            DataTable newTable = newData.Tables["History"];
            FillReportTable(newTable, peerGuid, tick);

            if (peerTotalsData != null)
            {
                // Duplicate the existing reported-to-server data
                DataSet mergedData = CreateReportDataSet();
                mergedData.Merge(peerTotalsData);
                AddToTotals(ref mergedData, newData);

                mergedData.Tables[0].DefaultView.AllowNew = false;
                mergedData.Tables[0].DefaultView.AllowEdit = false;
                return mergedData;
            }
            newData.Tables[0].DefaultView.AllowNew = false;
            newData.Tables[0].DefaultView.AllowEdit = false;
            return newData;
        }

        /// <summary>
        ///  Attemps to report data to the central server.
        /// </summary>
        /// <param name="newData">The data to be reported.</param>
        /// <param name="peerStateGuid">The peer's state GUID.</param>
        private void ReportData(DataSet newData, Guid peerStateGuid)
        {
            // If we're using the config file for discovery, don't do any reporting
            if (GameConfig.UseConfigForDiscovery)
            {
                return;
            }

            if (pendingAsyncResult == null)
            {
                DataSet allData = new DataSet();
                allData.Locale = CultureInfo.InvariantCulture;

                allData.Merge(newData);

                for (int i = (allData.Tables["History"].Rows.Count - 1); i >= 0; i--)
                {
                    if (((int) allData.Tables["History"].Rows[i]["OrganismBlacklistedCount"]) > 0)
                    {
                        allData.Tables["History"].Rows.RemoveAt(i);
                    }
                }

                InitWebService();

                if (led != null)
                {
                    led.LedState = LedStates.Waiting;
                }
                pendingAsyncResult =
                    (WebClientAsyncResult) reportingService.BeginReportPopulation(allData, peerStateGuid, currentTick,
                                                                                  ReportServiceCallback,
                                                                                  allData);

                // This is needed because if it completes synchronously, pendingAsyncResult is cleared in the
                // callback, and then it gets set *afterward* which we don't want since it's already done.
                if (pendingAsyncResult.CompletedSynchronously)
                {
                    pendingAsyncResult = null;
                }
            }
        }

        /// <summary>
        ///  Callback method used by the reporting service methods to enable
        ///  Async reporting.
        /// </summary>
        /// <param name="asyncResult">The results of the method.</param>
        private void ReportServiceCallback(IAsyncResult asyncResult)
        {
            try
            {
                int resultCode = reportingService.EndReportPopulation(asyncResult);

                if (resultCode == 3)
                {
                    // We were timed out because we didn't contact the server often enough
                    timedOut = true;
                }
                else if (resultCode == 4)
                {
                    // Our node is corrupted, reset it
                    nodeCorrupted = true;
                }
                else if (resultCode == 5)
                {
                    // We have an organism that needs to be blacklisted
                    organismBlacklisted = true;
                }
                else if (resultCode != 0)
                {
                }

                if (led != null)
                {
                    led.LedState = LedStates.Idle;
                }
            }
            catch (SoapException e)
            {
                ErrorLog.LogHandledException(e);
                if (led != null)
                {
                    led.LedState = LedStates.Failed;
                }
            }
            catch (Exception e)
            {
                ErrorLog.LogHandledException(e);

                if (led != null)
                {
                    led.LedState = LedStates.Failed;
                }
            }
            finally
            {
                pendingAsyncResult = null;
            }
        }

        /// <summary>
        ///  Ends a game tick.
        /// </summary>
        /// <param name="tickNumber">The tick number.  Should always be the same as the call to BeginTick.</param>
        public void EndTick(int tickNumber)
        {
            Debug.Assert(currentTick == tickNumber);

            // If it's the first tick, calculate the first minimum since it is the minimum for this set of data
            // and we might start out with organisms so it might not be zero
            if (firstTick)
            {
                firstTick = false;

                foreach (DataRow row in totalsTable.Rows)
                {
                    row["MinPopulation"] = row["Population"];
                }
            }

            // Calculate minimums and maximums
            foreach (DataRow row in totalsTable.Rows)
            {
                if ((int) row["Population"] > (int) row["MaxPopulation"])
                {
                    row["MaxPopulation"] = row["Population"];
                }

                if ((int) row["Population"] < (int) row["MinPopulation"])
                {
                    row["MinPopulation"] = row["Population"];
                }
            }

            currentTick = -1;
        }

        /// <summary>
        ///  Counts a new organism and adds it to the current dataset.
        /// </summary>
        /// <param name="speciesName">The name of the species being added.</param>
        /// <param name="reason">The reason for being added.</param>
        /// <param name="count">The number to add.</param>
        public void CountOrganism(string speciesName, PopulationChangeReason reason, int count)
        {
            DataRow row = populationChangeTable.Rows.Find(new Object[] {currentTick, speciesName, reason});
            if (row == null)
            {
                DataRow totalsRow = totalsTable.Rows.Find(new Object[] {speciesName});
                if (totalsRow == null)
                {
                    totalsRow = totalsTable.NewRow();
                    totalsRow["Species"] = speciesName;
                    totalsTable.Rows.Add(totalsRow);
                }

                row = populationChangeTable.NewRow();
                row["TickNumber"] = currentTick;
                row["Species"] = speciesName;
                row["Delta"] = count;
                row["Reason"] = reason;
                populationChangeTable.Rows.Add(row);
            }
            else
            {
                row["Delta"] = (int) row["Delta"] + count;
            }
        }

        /// <summary>
        ///  Counts a new organism and adds it to the reporting data.
        /// </summary>
        /// <param name="speciesName">The name of the species to add.</param>
        /// <param name="reason">The reason for bing added.</param>
        public void CountOrganism(string speciesName, PopulationChangeReason reason)
        {
            CountOrganism(speciesName, reason, 1);
        }

        /// <summary>
        ///  Counts a new organism and removes it from the reporting data.
        /// </summary>
        /// <param name="speciesName">The name of the species the data is for.</param>
        /// <param name="reason">The reason for removing.</param>
        /// <param name="count">The number to remove.</param>
        public void UncountOrganism(string speciesName, PopulationChangeReason reason, int count)
        {
            DataRow row = populationChangeTable.Rows.Find(new Object[] {currentTick, speciesName, reason});
            if (row == null)
            {
                DataRow totalsRow = totalsTable.Rows.Find(new Object[] {speciesName});

                // There should always be a totals row if we are decrementing population since
                // it stores the current population
                Debug.Assert(totalsRow != null);

                row = populationChangeTable.NewRow();
                row["TickNumber"] = currentTick;
                row["Species"] = speciesName;
                row["Delta"] = -count;
                row["Reason"] = reason;
                populationChangeTable.Rows.Add(row);
            }
            else
            {
                row["Delta"] = (int) row["Delta"] - count;
            }
        }

        /// <summary>
        ///  Counts a new organism and removes it from the data.
        /// </summary>
        /// <param name="speciesName">The name of the species the data is for.</param>
        /// <param name="reason">The reason the creature is being removed.</param>
        public void UncountOrganism(string speciesName, PopulationChangeReason reason)
        {
            UncountOrganism(speciesName, reason, 1);
        }

        /// <summary>
        ///  Get the current reporting data.
        /// </summary>
        /// <returns>The current reporting data.</returns>
        public DataSet Data()
        {
            return data;
        }

        /// <summary>
        ///  Creates a dataset in the form that the reporting server
        ///  is expecting.
        /// </summary>
        /// <returns>A reporting server compatible dataset.</returns>
        public DataSet CreateReportDataSet()
        {
            DataSet reportDataSet = new DataSet();
            reportDataSet.Locale = CultureInfo.InvariantCulture;

            DataTable totalsOnlyTable = reportDataSet.Tables.Add("History");
            totalsOnlyTable.Columns.Add("GUID", typeof (Guid));
            totalsOnlyTable.Columns.Add("TickNumber", typeof (Int32));
            totalsOnlyTable.Columns.Add("ContactTime", typeof (DateTime));
            totalsOnlyTable.Columns.Add("ClientTime", typeof (DateTime));
            totalsOnlyTable.Columns.Add("CorrectTime", typeof (Byte));
            totalsOnlyTable.Columns.Add("SpeciesName", typeof (String));
            totalsOnlyTable.Columns.Add("Population", typeof (Int32));
            totalsOnlyTable.Columns.Add("BirthCount", typeof (Int32));
            totalsOnlyTable.Columns.Add("TeleportedToCount", typeof (Int32));
            totalsOnlyTable.Columns.Add("StarvedCount", typeof (Int32));
            totalsOnlyTable.Columns.Add("KilledCount", typeof (Int32));
            totalsOnlyTable.Columns.Add("TeleportedFromCount", typeof (Int32));
            totalsOnlyTable.Columns.Add("ErrorCount", typeof (Int32));
            totalsOnlyTable.Columns.Add("TimeoutCount", typeof (Int32));
            totalsOnlyTable.Columns.Add("SickCount", typeof (Int32));
            totalsOnlyTable.Columns.Add("OldAgeCount", typeof (Int32));
            totalsOnlyTable.Columns.Add("SecurityViolationCount", typeof (Int32));
            totalsOnlyTable.Columns.Add("OrganismBlacklistedCount", typeof (Int32));

            totalsOnlyTable.PrimaryKey = new DataColumn[]
                                             {
                                                 totalsOnlyTable.Columns["GUID"],
                                                 totalsOnlyTable.Columns["TickNumber"],
                                                 totalsOnlyTable.Columns["SpeciesName"]
                                             };

            return reportDataSet;
        }

        /// <summary>
        ///  Fill the dataset with data in the format expected by the reporting server.
        /// </summary>
        /// <param name="totalsOnlyTable">A datatable in the reporting server format.</param>
        /// <param name="peerStateGuid">The current world state GUID.</param>
        /// <param name="tickNumber">The current tick number.</param>
        public void FillReportTable(DataTable totalsOnlyTable, Guid peerStateGuid, int tickNumber)
        {
            foreach (DataRow row in totalsTable.Rows)
            {
                DataRow newRow = totalsOnlyTable.NewRow();
                newRow["GUID"] = peerStateGuid;
                newRow["TickNumber"] = tickNumber;
                newRow["ContactTime"] = DateTime.UtcNow;
                newRow["ClientTime"] = DateTime.UtcNow;
                newRow["CorrectTime"] = 1;
                newRow["SpeciesName"] = row["Species"];
                newRow["Population"] = row["Population"];

                // Total up the delta across all ticks that we are reporting in the populationChangeTable

                // Events that increased population
                newRow["BirthCount"] =
                    NullToZero(populationChangeTable.Compute("Sum(Delta)",
                                                             "Species='" + ((string) row["Species"]).Replace("'", "''") +
                                                             "' AND Reason=" + ((Int32) PopulationChangeReason.Born)));
                newRow["TeleportedToCount"] =
                    NullToZero(populationChangeTable.Compute("Sum(Delta)",
                                                             "Species='" + ((string) row["Species"]).Replace("'", "''") +
                                                             "' AND Reason=" +
                                                             ((Int32) PopulationChangeReason.TeleportedTo)));

                // Events that decreased population
                newRow["StarvedCount"] =
                    -(NullToZero(populationChangeTable.Compute("Sum(Delta)",
                                                               "Species='" +
                                                               ((string) row["Species"]).Replace("'", "''") +
                                                               "' AND Reason=" +
                                                               ((Int32) PopulationChangeReason.Starved))));
                newRow["KilledCount"] =
                    -(NullToZero(populationChangeTable.Compute("Sum(Delta)",
                                                               "Species='" +
                                                               ((string) row["Species"]).Replace("'", "''") +
                                                               "' AND Reason=" + ((Int32) PopulationChangeReason.Killed))));
                newRow["TeleportedFromCount"] =
                    -(NullToZero(populationChangeTable.Compute("Sum(Delta)",
                                                               "Species='" +
                                                               ((string) row["Species"]).Replace("'", "''") +
                                                               "' AND Reason=" +
                                                               ((Int32) PopulationChangeReason.TeleportedFrom))));
                newRow["ErrorCount"] =
                    -(NullToZero(populationChangeTable.Compute("Sum(Delta)",
                                                               "Species='" +
                                                               ((string) row["Species"]).Replace("'", "''") +
                                                               "' AND Reason=" + ((Int32) PopulationChangeReason.Error))));
                newRow["TimeoutCount"] =
                    -(NullToZero(populationChangeTable.Compute("Sum(Delta)",
                                                               "Species='" +
                                                               ((string) row["Species"]).Replace("'", "''") +
                                                               "' AND Reason=" +
                                                               ((Int32) PopulationChangeReason.Timeout))));
                newRow["SickCount"] =
                    -(NullToZero(populationChangeTable.Compute("Sum(Delta)",
                                                               "Species='" +
                                                               ((string) row["Species"]).Replace("'", "''") +
                                                               "' AND Reason=" + ((Int32) PopulationChangeReason.Sick))));
                newRow["OldAgeCount"] =
                    -(NullToZero(populationChangeTable.Compute("Sum(Delta)",
                                                               "Species='" +
                                                               ((string) row["Species"]).Replace("'", "''") +
                                                               "' AND Reason=" + ((Int32) PopulationChangeReason.OldAge))));
                newRow["SecurityViolationCount"] =
                    -(NullToZero(populationChangeTable.Compute("Sum(Delta)",
                                                               "Species='" +
                                                               ((string) row["Species"]).Replace("'", "''") +
                                                               "' AND Reason=" +
                                                               ((Int32) PopulationChangeReason.SecurityViolation))));
                newRow["OrganismBlacklistedCount"] =
                    -(NullToZero(populationChangeTable.Compute("Sum(Delta)",
                                                               "Species='" +
                                                               ((string) row["Species"]).Replace("'", "''") +
                                                               "' AND Reason=" +
                                                               ((Int32) PopulationChangeReason.OrganismBlacklisted))));

                totalsOnlyTable.Rows.Add(newRow);
            }
        }

        /// <summary>
        ///  Add up reporting totals so that a cumulative report can be given
        ///  to the user via a dialog or other method.
        /// </summary>
        /// <param name="totalsData">The current totals data.</param>
        /// <param name="newData">The new data to be added.</param>
        public void AddToTotals(ref DataSet totalsData, DataSet newData)
        {
            if (totalsData == null)
            {
                DataSet mergedData = new DataSet();
                mergedData.Locale = CultureInfo.InvariantCulture;
                mergedData.Merge(newData);
                totalsData = mergedData;
            }
            else
            {
                DataTable existingTable = totalsData.Tables[0];
                DataTable newTable = newData.Tables[0];
                existingTable.DefaultView.AllowNew = true;
                existingTable.DefaultView.AllowEdit = true;
                foreach (DataRow row in newTable.Rows)
                {
                    DataRow[] existingRows =
                        existingTable.Select("SpeciesName='" + ((string) row["SpeciesName"]).Replace("'", "''") + "'");
                    Debug.Assert(existingRows.Length <= 1);
                    if (existingRows.Length == 0)
                    {
                        existingTable.ImportRow(row);
                    }
                    else
                    {
                        DataRow existingRow = existingRows[0];
                        existingRow["Population"] = row["Population"];
                        existingRow["BirthCount"] = addValues(existingRow["BirthCount"], row["BirthCount"]);
                        existingRow["TeleportedToCount"] = addValues(existingRow["TeleportedToCount"],
                                                                     row["TeleportedToCount"]);
                        existingRow["StarvedCount"] = addValues(existingRow["StarvedCount"], row["StarvedCount"]);
                        existingRow["KilledCount"] = addValues(existingRow["KilledCount"], row["KilledCount"]);
                        existingRow["TeleportedFromCount"] = addValues(existingRow["TeleportedFromCount"],
                                                                       row["TeleportedFromCount"]);
                        existingRow["TimeoutCount"] = addValues(existingRow["TimeoutCount"], row["TimeoutCount"]);
                        existingRow["SickCount"] = addValues(existingRow["SickCount"], row["SickCount"]);
                        existingRow["OldAgeCount"] = addValues(existingRow["OldAgeCount"], row["OldAgeCount"]);
                        existingRow["SecurityViolationCount"] = addValues(existingRow["SecurityViolationCount"],
                                                                          row["SecurityViolationCount"]);
                        existingRow["OrganismBlacklistedCount"] = addValues(existingRow["OrganismBlacklistedCount"],
                                                                            row["OrganismBlacklistedCount"]);
                    }
                }

                existingTable.DefaultView.AllowNew = false;
                existingTable.DefaultView.AllowEdit = false;
            }
        }

        /// <summary>
        ///  Helper function to add two database values together.
        /// </summary>
        /// <param name="value1">Value to be added to.</param>
        /// <param name="value2">Value to be added.</param>
        /// <returns>The sum of the two values.</returns>
        private static Int32 addValues(object value1, object value2)
        {
            return NullToZero(value1) + NullToZero(value2);
        }

        /// <summary>
        ///  Helper function to convert DBNull to 0
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The value or 0 if DBNull</returns>
        public static Int32 NullToZero(object value)
        {
            if (Convert.IsDBNull(value))
            {
                return 0;
            }
            return Convert.ToInt32(value);
        }
    }
}