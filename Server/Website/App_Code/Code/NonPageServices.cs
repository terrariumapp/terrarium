//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Timers;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Configuration;
using System.Collections;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Net;
using Microsoft.CSharp;
using System.Web;

namespace Terrarium.Server {
    /*
        Class:      NonPageServices
        Purpose:    The NonPageServices class is responsible for managing
        the reporting data roll-ups and the black box generation routines.
    */
	public class NonPageServices {
		static NonPageServices currentServices;
		private PerformanceCounter rollupExecutionTimePerformanceCounter;
		private Timer reportingTimer;
		int millisecondsToRollupData = 600000;  // 10 minutes - overriden by ReportSettings...
		private string dsn;
		bool isStarted = false;

        private NonPageServices() {
        }

        /*
            Method:     Start
            Purpose:    This method encompasses the code to interrogate the
            application's web configuration file and start the timers needed
            for black box genration and reporting to occur.
        */
		public void Start() {
			bool exceptionOccurred = false;

			try {			
				rollupExecutionTimePerformanceCounter = InstallerInfo.CreatePerformanceCounter("RollupExecution"); 

				millisecondsToRollupData = ServerSettings.MillisecondsToRollupData;
                
				if(ServerSettings.SpeciesDsn != string.Empty) {
                    dsn = ServerSettings.SpeciesDsn;
					reportingTimer = new Timer();
					reportingTimer.Interval = millisecondsToRollupData;
					reportingTimer.Elapsed += new ElapsedEventHandler(OnReportingTimerElapsed);
					reportingTimer.Enabled = true;
					isStarted = true;
				}
				else {
					InstallerInfo.WriteEventLog("Report", "Can't do rollups: Couldn't find Species entry in settings section of config file.");
				}
			}
            catch(Exception e) {
				exceptionOccurred = true;
				InstallerInfo.WriteEventLog("Report", e.ToString());
			}		

			if(!exceptionOccurred)
				InstallerInfo.WriteEventLog("Report", "NonPageServices started. Rolling up data every " + (millisecondsToRollupData / 1000).ToString() + " seconds.", EventLogEntryType.Information);
		}

        /*
            Property:   IsStarted
            Purpose:    Returns a true if the services have been started.
        */
		public bool IsStarted {
			get {
				return isStarted;
			}
		}

        /*
            Method:     Current
            Purpose:    Used to get the current NonPageServices
            object.  This is the only way to get an instance of
            NonPagServices since the constructor is marked private.
        */
		static public NonPageServices Current {
			get {
				if(currentServices == null) {
					currentServices = new NonPageServices();
				}

				return currentServices;
			}
		}

        /*
            Method:     OnReportingTimerElapsed
            Purpose:    Callback function used when the reporting timer has
            signalled.  This function is responsible for constructing a database
            call to the Terrarium reporting aggregation procedure.  Once called
            the latest data from clients will be rolled up to be displayed on
            the stats pages.
            
            After rollup is complete a call to ChartBuilder.ResetSpeciesList is
            made so that the latest data is shown on the website.
        */
		protected internal void OnReportingTimerElapsed(Object src, ElapsedEventArgs args) {
			bool exceptionOccurred = false;
			DateTime startTime = DateTime.Now;
		
			try {
				using(SqlConnection myConnection = new SqlConnection(dsn)) {
					myConnection.Open();
					SqlCommand command = new SqlCommand("TerrariumAggregate", myConnection);
					command.CommandType = CommandType.StoredProcedure;

					SqlParameter parmExpirationError = command.Parameters.Add("@Expiration_Error", SqlDbType.Int, 4);
					SqlParameter parmRollupError = command.Parameters.Add("@Rollup_Error", SqlDbType.Int, 4);
					SqlParameter parmTimeOutAddError = command.Parameters.Add("@Timeout_Add_Error", SqlDbType.Int, 4);
					SqlParameter parmTimeOutDeleteError = command.Parameters.Add("@Timeout_Delete_Error", SqlDbType.Int, 4);
					SqlParameter parmExtinctionError = command.Parameters.Add("@Extinction_Error", SqlDbType.Int, 4);

					parmExpirationError.Direction = ParameterDirection.Output;
					parmRollupError.Direction = ParameterDirection.Output;
					parmTimeOutAddError.Direction = ParameterDirection.Output;
					parmTimeOutDeleteError.Direction = ParameterDirection.Output;
					parmExtinctionError.Direction = ParameterDirection.Output;

					command.ExecuteNonQuery();

					if ( ((int) parmExpirationError.Value) != 0 )
						InstallerInfo.WriteEventLog("Report", "Expiration Code failed: " + parmExpirationError.Value);
					if ( ((int) parmRollupError.Value) != 0 )
						InstallerInfo.WriteEventLog("Report", "Rollup Code failed: " + parmRollupError.Value);
					if ( ((int) parmTimeOutAddError.Value) != 0 )
						InstallerInfo.WriteEventLog("Report", "Time Out Add Code failed: " + parmTimeOutAddError.Value);
					if ( ((int) parmTimeOutDeleteError.Value) != 0 )
						InstallerInfo.WriteEventLog("Report", "Time Out Delete Code failed: " + parmTimeOutDeleteError.Value);
					if ( ((int) parmExtinctionError.Value) != 0 )
						InstallerInfo.WriteEventLog("Report", "Extinction Code failed: " + parmExtinctionError.Value);
				}

                // Update the species list
                try {
                    ChartBuilder.RefreshSpeciesList();
                }
                catch(Exception e) {
                    InstallerInfo.WriteEventLog("SpeciesList", e.ToString());
                }
			}
			catch(Exception e) {
				exceptionOccurred = true;
				InstallerInfo.WriteEventLog("Report", e.ToString());
			}

			if(rollupExecutionTimePerformanceCounter != null) {
				if(exceptionOccurred)
					rollupExecutionTimePerformanceCounter.RawValue = 0;
				else {
					TimeSpan executionTime = DateTime.Now.Subtract(startTime);
					rollupExecutionTimePerformanceCounter.RawValue = executionTime.Milliseconds;
				}
			}
		}
	}
}