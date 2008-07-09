//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------


using System;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Collections;
using System.Web;
using System.Text;
using System.Diagnostics;
using Terrarium.Server;

namespace Terrarium.Server
{
	/*
		Enum:       ReturnCode
	*/
	/// <summary>
	/// Enumeration of the return codes possible from the Terrarium Server ReportingService. 
	/// </summary>
	public enum ReturnCode 
	{
		/// <summary>
		/// Indicates a succesful operation.
		/// </summary>
		Success = 0,
		/// <summary>
		/// Indicates an attempt to register an item that has already been registered on the Terrarium Server.
		/// </summary>
		AlreadyExists = 1,
		/// <summary>
		/// ServerDown is used when the addition of a new peer registration fails 
		/// due to an error in adding the peer registration data to the database.
		/// </summary>
		ServerDown = 2,
		/// <summary>
		/// Identifies cases where the clients state is out of date.
		/// </summary>
		NodeTimedOut = 3,
		/// <summary>
		/// Identifies cases where the clients state has become corrupted.
		/// </summary>
		NodeCorrupted = 4,
		/// <summary>
		/// Identifies an organism that has been blacklisted on the Terrarium Server.
		/// The list of black-listed species is maintained by Terrarium Server administrators.
		/// </summary>
		OrganismBlacklisted = 5
	}

	/*
		Class:      ReportingService
	*/
	/// <summary>
	/// This class implements the reporting web service for the Terrarium Server.
	/// This web service takes reporting data from clients and inserts
	/// the data into a special history table.This data is later rolled up 
	/// by the NonPageServices class and turned into data points.
	/// These data points are used used when creating graphs of the server activity, 
	/// species populations, etc.
	/// </summary>
	
	public class ReportingService : WebService 
	{
		private static Hashtable _lastGuid = new Hashtable();
		private static PerformanceCounter reportingAllPerformanceCounter;
		private static PerformanceCounter reportingAllFailedPerformanceCounter;

		/*
			Method:     ReportingService
			Purpose:    This static constructor initializes all performance
			counters used by this reporting service.
		*/
		static ReportingService() 
		{
			try 
			{			
				reportingAllPerformanceCounter = InstallerInfo.CreatePerformanceCounter("AllReporting");
				reportingAllFailedPerformanceCounter = InstallerInfo.CreatePerformanceCounter("AllFailedReporting"); 
			}
			catch(Exception e) 
			{
				InstallerInfo.WriteEventLog("PerformanceCounters", "Could not create Performance Counter: " + e.ToString());
			}	
		}
	
		/*
			Method:     ReportPopulation
			Notes:    
        
			This function is capable of throttling users and throwing away their
			data to prevent cheating or DOS attacks.
        
			This function is also capable of detecting corrupted or out of date
			clients and alerting them as to such.  It can also detect if the client
			is reporting information on blacklisted creatures and notify the peer
			of that special case.
        
			There is also some code used to draw constraints on the data reported
			by a client.  In some cases a hacked client could report extremely high
			and bogus numbers.  This function will throw out any data that is heavily
			out of bounds to help prevent cheating.
		*/
		/// <summary>
		/// This method takes a dataset from the client along with
		/// a game state guid and the tick this data represents as its parameter. It uses this
		/// information to make insertions into the History table in the Terrarium Server database.
		/// </summary>
		/// <param name="data">A Dataset containing species population data.</param>
		/// <param name="guid">Guid associated with a peer connection.</param>
		/// <param name="currentTick">Integer representing the current tick (time increment).</param>
		/// <returns>A possible value from the ReturnCode enumeration.</returns>
		
		[WebMethod(EnableSession=true)]
		public int ReportPopulation(DataSet data, Guid guid, int currentTick) 
		{
			try 
			{
				if ( data == null || guid == Guid.Empty )
					InstallerInfo.WriteEventLog("Report", "Suspect: " + Context.Request.ServerVariables["REMOTE_ADDR"].ToString());


				// We need to check for blacklisted species, even if the client has been throttled.  If this gets
				// too demanding, think about caching the results
				bool foundBlacklisted = false;
				using (SqlConnection connection = new SqlConnection(ServerSettings.SpeciesDsn))
				{
					connection.Open();

					SqlCommand command = new SqlCommand("TerrariumCheckSpeciesBlacklist", connection);
					command.CommandType = CommandType.StoredProcedure;
					
					SqlParameter nameParameter = command.Parameters.Add("@Name", SqlDbType.VarChar, 255);

					DataTable table = data.Tables["History"];
					if ( table != null )
					{
						foreach(DataRow row in table.Rows)
						{
							nameParameter.Value = Convert.ToString(row["SpeciesName"]);
							SqlDataReader reader = command.ExecuteReader();
							if (reader.Read())
							{
								if (1 == Convert.ToInt32(reader["Blacklisted"]))
								{
									foundBlacklisted = true;
									break;
								}
							}
							reader.Close();
						}
					}
					if (foundBlacklisted == true)
					{
						return (int) ReturnCode.OrganismBlacklisted;
					}
				}


				if ( Throttle.Throttled(
					Context.Request.ServerVariables["REMOTE_ADDR"].ToString(),
					"ReportPopulation3Mins") )
					return (int) ReturnCode.Success;

				if ( Throttle.Throttled(
					Context.Request.ServerVariables["REMOTE_ADDR"].ToString(),
					"ReportPopulation12Hour") )
					return (int) ReturnCode.Success;

				Throttle.AddThrottle(
					Context.Request.ServerVariables["REMOTE_ADDR"].ToString(),
					"ReportPopulation3Mins",
					1,
					DateTime.Now.AddMinutes(3)
					);

				if ( _lastGuid.ContainsKey(Context.Request.ServerVariables["REMOTE_ADDR"].ToString()) ) 
				{
					if ( ((Guid) _lastGuid[Context.Request.ServerVariables["REMOTE_ADDR"].ToString()]) != guid ) 
					{
						Throttle.AddThrottle(
							Context.Request.ServerVariables["REMOTE_ADDR"].ToString(),
							"ReportPopulation12Hour",
							1,
							DateTime.Now.AddHours(12)
							);
						_lastGuid[Context.Request.ServerVariables["REMOTE_ADDR"].ToString()] = guid;
						return (int) ReturnCode.Success;
					}
				}

				_lastGuid[Context.Request.ServerVariables["REMOTE_ADDR"].ToString()] = guid;

				DateTime contactTime = DateTime.UtcNow;
				DataTable historyTable = data.Tables["History"];

				foreach(DataRow row in historyTable.Rows) 
				{
					// Set correcttime to false on all rows that aren't the current tick because they are data that
					// is old, but couldn't get to the server when it was fresh.  Only the data from the current tick
					// actually happened right now
					if((int) row["TickNumber"] != currentTick)
						row["CorrectTime"] = 0;

					row["ContactTime"] = contactTime;
				}

				bool blackListedPeers = false;
				bool validRecord = true;
				int totalPopulation = 0;
            
				using(SqlConnection myConnection = new SqlConnection(ServerSettings.SpeciesDsn)) 
				{
					myConnection.Open();

					// Declare all parameters, commands, etc.. right here
					SqlTransaction transaction = myConnection.BeginTransaction();
					SqlCommand lastContact;

					SqlParameter parmGuid;
					SqlParameter parmLastContact;
					SqlParameter parmLastTick;
					SqlParameter parmReturnVal;

					lastContact = new SqlCommand("TerrariumTimeoutReport", myConnection, transaction);
					lastContact.CommandType = CommandType.StoredProcedure;

					parmGuid = lastContact.Parameters.Add("@Guid", SqlDbType.UniqueIdentifier, 16);
					parmGuid.Value = guid;
					parmLastContact = lastContact.Parameters.Add("@LastContact", SqlDbType.DateTime, 8);
					parmLastContact.Value = contactTime;
					parmLastTick = lastContact.Parameters.Add("@LastTick", SqlDbType.Int, 4);
					parmLastTick.Value = currentTick;
					parmReturnVal = lastContact.Parameters.Add("@ReturnCode", SqlDbType.Int, 4);                
					parmReturnVal.Direction = ParameterDirection.Output;

					try 
					{
						lastContact.ExecuteNonQuery();
					
						if( ((int)parmReturnVal.Value) !=0 ) 
						{
							if((int)parmReturnVal.Value == 1)
							{
								return (int) ReturnCode.NodeTimedOut;
							}
							else if((int)parmReturnVal.Value == 2)
							{
								return (int) ReturnCode.NodeCorrupted;
							}
							else
							{
								InstallerInfo.WriteEventLog("Report", "Unknown return value from TerrariumTimeoutReport");
									
								if(reportingAllFailedPerformanceCounter != null)
								{
									reportingAllFailedPerformanceCounter.Increment();
								}
								return (int) ReturnCode.ServerDown;
							}						
						}
					}
					catch (Exception e) 
					{            
						if(e is SqlException)
						{
							InstallerInfo.WriteEventLog("Report", "Suspect: " + Context.Request.ServerVariables["REMOTE_ADDR"].ToString() + "\n\r" +
								"Sql Error Number: " + ((SqlException)e).Number + "\r\n" + e.ToString());
						}
						else
						{
							InstallerInfo.WriteEventLog("Report", "Suspect: " + Context.Request.ServerVariables["REMOTE_ADDR"].ToString() + "\n\r" + e.ToString());
							InstallerInfo.WriteEventLog("Report", "Unknown return value from TerrariumTimeoutReport");
						}
		
						if(reportingAllFailedPerformanceCounter != null)
						{
							reportingAllFailedPerformanceCounter.Increment();
						}
					}

					// Update the history data
					SqlCommand insertHistory = new SqlCommand("TerrariumInsertHistory", myConnection, transaction);
					insertHistory.CommandType = CommandType.StoredProcedure;

					parmGuid                                = insertHistory.Parameters.Add("@Guid", SqlDbType.UniqueIdentifier, 16);
					SqlParameter parmSpeciesName            = insertHistory.Parameters.Add("@SpeciesName", SqlDbType.VarChar, 255);
					SqlParameter parmContactTime            = insertHistory.Parameters.Add("@ContactTime", SqlDbType.DateTime, 8);
					SqlParameter parmClientTime             = insertHistory.Parameters.Add("@ClientTime", SqlDbType.DateTime, 8);
					SqlParameter parmCorrectTime            = insertHistory.Parameters.Add("@CorrectTime", SqlDbType.TinyInt, 1);
					SqlParameter parmTickNumber             = insertHistory.Parameters.Add("@TickNumber", SqlDbType.Int, 4);
					SqlParameter parmPopulation             = insertHistory.Parameters.Add("@Population", SqlDbType.Int, 4);
					SqlParameter parmBirthCount             = insertHistory.Parameters.Add("@BirthCount", SqlDbType.Int, 4);
					SqlParameter parmTeleportedToCount      = insertHistory.Parameters.Add("@TeleportedToCount", SqlDbType.Int, 4);
					SqlParameter parmStarvedCount           = insertHistory.Parameters.Add("@StarvedCount", SqlDbType.Int, 4);
					SqlParameter parmKilledCount            = insertHistory.Parameters.Add("@KilledCount", SqlDbType.Int, 4);
					SqlParameter parmTeleportedFromCount    = insertHistory.Parameters.Add("@TeleportedFromCount", SqlDbType.Int, 4);
					SqlParameter parmErrorCount             = insertHistory.Parameters.Add("@ErrorCount", SqlDbType.Int, 4);
					SqlParameter parmTimeoutCount           = insertHistory.Parameters.Add("@TimeoutCount", SqlDbType.Int, 4);
					SqlParameter parmSickCount              = insertHistory.Parameters.Add("@SickCount", SqlDbType.Int, 4);
					SqlParameter parmOldAgeCount            = insertHistory.Parameters.Add("@OldAgeCount", SqlDbType.Int, 4);
					SqlParameter parmSecurityViolationCount = insertHistory.Parameters.Add("@SecurityViolationCount", SqlDbType.Int, 4);
					SqlParameter parmBlackListed            = insertHistory.Parameters.Add("@BlackListed", SqlDbType.Int, 4);
					parmBlackListed.Direction               = ParameterDirection.Output;

					if ( data.Tables["History"].Rows.Count > 600 )
						validRecord = false;
					else 
					{
						foreach(DataRow dr in data.Tables["History"].Rows) 
						{
							if ( ((int)dr["TickNumber"]) != currentTick )
								continue;

							totalPopulation += ((int) dr["Population"]);
							if ( totalPopulation > 600 || ((int) dr["Population"]) > 340 || ((int) dr["Population"]) < 0)
								validRecord = false;

							parmGuid.Value                   = dr["Guid"];
							parmSpeciesName.Value            = dr["SpeciesName"];
							parmContactTime.Value            = dr["ContactTime"];
							parmClientTime.Value             = dr["ClientTime"];
							parmCorrectTime.Value            = dr["CorrectTime"];
							parmTickNumber.Value             = dr["TickNumber"];
							parmPopulation.Value             = dr["Population"];
							parmBirthCount.Value             = dr["BirthCount"];
							parmTeleportedToCount.Value      = dr["TeleportedToCount"];
							parmStarvedCount.Value           = dr["StarvedCount"];
							parmKilledCount.Value            = dr["KilledCount"];
							parmTeleportedFromCount.Value    = dr["TeleportedFromCount"];
							parmErrorCount.Value             = dr["ErrorCount"];
							parmTimeoutCount.Value           = dr["TimeoutCount"];
							parmSickCount.Value              = dr["SickCount"];
							parmOldAgeCount.Value            = dr["OldAgeCount"];
							parmSecurityViolationCount.Value = dr["SecurityViolationCount"];

							insertHistory.ExecuteNonQuery();
							if ( parmBlackListed.Value != null && parmBlackListed.Value != System.DBNull.Value && ((int)parmBlackListed.Value) == 1 ) 
							{
								blackListedPeers = true;
							}                    
						}
					}
                
					if ( validRecord )
						transaction.Commit();
					else
						transaction.Rollback();
				}

				if(reportingAllPerformanceCounter != null)
					reportingAllPerformanceCounter.Increment();
				
				if ( blackListedPeers )
					return (int) ReturnCode.OrganismBlacklisted;
            
				return (int) ReturnCode.Success;
			}
			catch(Exception e) 
			{            
				if(e is SqlException)
					InstallerInfo.WriteEventLog("Report", "Suspect: " + Context.Request.ServerVariables["REMOTE_ADDR"].ToString() + "\n\r" +
						"Sql Error Number: " + ((SqlException)e).Number + "\r\n" + e.ToString());
				else
					InstallerInfo.WriteEventLog("Report", "Suspect: " + Context.Request.ServerVariables["REMOTE_ADDR"].ToString() + "\n\r" +
						e.ToString());
            
				if(reportingAllFailedPerformanceCounter != null)
					reportingAllFailedPerformanceCounter.Increment();

				// Return success instead of ServerDown because, if the server is getting hammered, Success will tell clients to 
				// stop retrying, where ServerDown will tell them to keep doing it.
				return (int) ReturnCode.Success;
			}
		}
	}
}
