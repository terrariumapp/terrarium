//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------


using System;
using System.Web.Services;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web;
using System.Diagnostics;
using Terrarium.Server;

namespace Terrarium.Server
{
    /*
        Enum:       RegisterPeerResult
    */
    /// <summary>
    /// This enum contains the values for different
    /// response codes when a client calls RegisterMyPeerGetCountAndPeerList.
    /// Success means the call was good. Failure is generally a database
    /// timeout or other database issue, whereas a GlobalFailure means we
    /// have disabled that version of the client and wish the user to upgrade.
    /// </summary>
    public enum RegisterPeerResult 
    {
        /// <summary>
        /// Indicates successful registration of a peer connection.
        /// </summary>
        Success,
        /// <summary>
        /// Indicates an unsuccessful attempt to register a peer connection.
        /// </summary>
        Failure,
        /// <summary>
        /// Indicates an unsuccessful attempt to register a peer connection.
        /// </summary>
        GlobalFailure
    }

    /*
        Enum:       PeerDiscoveryService
    */
    /// <summary>
    /// The Peer Discovery Service is the central location where
    /// peers can announce their existence and get information about other
    /// peers.  The primary services here are registering a user's email address,
    /// getting peer counts and lists, and registering a peer.
    /// </summary>

    public class PeerDiscoveryService : WebService 
    {
        /// <summary>
        /// PerformanceCounter for all monitored performance parameters on the Discovery Web Service.
        /// </summary>
        private static PerformanceCounter discoveryAllPerformanceCounter;
        /// <summary>
        /// PerformanceCounter for all monitored unsuccessful with the Discovery Web Service.
        /// </summary>
        private static PerformanceCounter discoveryAllFailuresPerformanceCounter;
        /// <summary>
        /// PerformanceCounter for monitoring peer registrations with the Discovery Web Service.
        /// </summary>
        private static PerformanceCounter discoveryRegistrationPerformanceCounter;
        /// <summary>
        /// PerformanceCounter for monitoring failed peer registration attempts with the Discovery Web Service.
        /// </summary>
        private static PerformanceCounter discoveryRegistrationFailuresPerformanceCounter;

        /// <summary>
        /// Instantiates performance counters for the PeerDiscoveryService.
        /// </summary>
        static PeerDiscoveryService() 
        {
            try 
            {            
                discoveryAllPerformanceCounter = InstallerInfo.CreatePerformanceCounter("AllDiscovery");
                discoveryAllFailuresPerformanceCounter = InstallerInfo.CreatePerformanceCounter("AllDiscoveryErrors");
                discoveryRegistrationPerformanceCounter = InstallerInfo.CreatePerformanceCounter("Registration");
                discoveryRegistrationFailuresPerformanceCounter = InstallerInfo.CreatePerformanceCounter("RegistrationErrors");
            } 
            catch(Exception e) 
            {
                InstallerInfo.WriteEventLog("PerformanceCounters", "could not create Performance Counter: " + e.ToString());
            }   
        }
        /// <summary>
        /// Registers a Terrarium client user into the server database.
        /// </summary>
        /// <param name="email">E-mail address of the Terrarium user</param>
        /// <returns>Boolean indicating success or failure of the user registration.</returns>
        
        [WebMethod]
        public Boolean RegisterUser(string email) 
        {
            string ipAddress = Context.Request.ServerVariables["REMOTE_ADDR"].ToString();

            try 
            {
                using(SqlConnection myConnection = new SqlConnection(ServerSettings.SpeciesDsn)) 
                {
                    myConnection.Open();
                    SqlCommand command = new SqlCommand("TerrariumRegisterUser", myConnection);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter parmEmail = command.Parameters.Add("@Email", SqlDbType.VarChar, 255);
                    parmEmail.Value = email;
                    SqlParameter parmIP = command.Parameters.Add("@IPAddress", SqlDbType.VarChar, 50);
                    parmIP.Value = ipAddress;

                    command.ExecuteNonQuery();
                    if(discoveryAllPerformanceCounter != null)
                        discoveryAllPerformanceCounter.Increment();
                    return true;
                }
            }
            catch(Exception e) 
            {
                InstallerInfo.WriteEventLog("RegisterUser", e.ToString());

                if(discoveryAllFailuresPerformanceCounter != null)
                    discoveryAllFailuresPerformanceCounter.Increment();

                return false;
            }
        }

        /// <summary>
        /// Obtains the number of peers currently connected to the Terrarium Server.
        /// </summary>
        /// <param name="version">String specifying the version number.</param>
        /// <param name="channel">String specifying the channel number.</param>
        /// <returns>Integer count of the number of peers for the specified version and channel number.</returns>
        
        [WebMethod]
        public int GetNumPeers(string version, string channel) 
        {
            if ( channel == null || version == null ) 
            {
                // Special versioning case, if all parameters are not specified then we return an appropriate error.
                InstallerInfo.WriteEventLog("GetNumPeers", "Suspect: " + Context.Request.ServerVariables["REMOTE_ADDR"].ToString());

                if(discoveryAllFailuresPerformanceCounter != null)
                    discoveryAllFailuresPerformanceCounter.Increment();

                return 0;
            }

            version = new Version(version).ToString(3);

            try 
            {
                using(SqlConnection myConnection = new SqlConnection(ServerSettings.SpeciesDsn)) 
                {
                    myConnection.Open();

                    SqlCommand mySqlCommand = new SqlCommand("TerrariumGrabNumPeers", myConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    SqlParameter parmVersion = mySqlCommand.Parameters.Add("@Version", SqlDbType.VarChar, 255);
                    parmVersion.Value = version;
                    SqlParameter parmChannel = mySqlCommand.Parameters.Add("@Channel", SqlDbType.VarChar, 255);
                    parmChannel.Value = channel;

                    object count = mySqlCommand.ExecuteScalar();
                    if(discoveryAllPerformanceCounter != null)
                        discoveryAllPerformanceCounter.Increment();

                    if(Convert.IsDBNull(count))
                        return 0;
                    else
                        return (int) count;
                }
            }
            catch(Exception e) 
            {
                InstallerInfo.WriteEventLog("GetNumPeers", e.ToString());

                if(discoveryAllFailuresPerformanceCounter != null)
                    discoveryAllFailuresPerformanceCounter.Increment();

                return 0;
            }
        }
        
        
        /// <summary>
        /// Validates a peer connection.
        /// </summary>
        /// <returns>A string representing the "REMOTE_ADDR" attribute from the Web Application ServerVariables collection.</returns>
        [WebMethod]
        public string ValidatePeer() 
        {
            if(discoveryAllPerformanceCounter != null)
                discoveryAllPerformanceCounter.Increment();

            return Context.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }

        

        
        /// <summary>
        /// Registers a peer connection with the Terrarium Server.
        /// </summary>
        /// <param name="version">String specifying the version number.</param>
        /// <param name="channel">String specifying the channel number.</param>
        /// <param name="guid">Guid (Globally Unique Identifier) for the peer connection.</param>
        /// <param name="peers">Dataset containing a list of peers.</param>
        /// <param name="count">Integer specifying the number of peers listed.</param>
        /// <returns>A possible value from the RegisterPeerResult enumeration.</returns>
        [WebMethod]
        public RegisterPeerResult RegisterMyPeerGetCountAndPeerList(string version, string channel, Guid guid, out DataSet peers, out int count) 
        {
            peers = new DataSet();
            count = 0;
    
            if ( channel == null || version == null ) 
            {
                // Special versioning case, if all parameters are not specified then we return an appropriate error.
                InstallerInfo.WriteEventLog("RegisterMyPeerGetCountAndPeerList", "Suspect: " + Context.Request.ServerVariables["REMOTE_ADDR"].ToString());

                if(discoveryAllFailuresPerformanceCounter != null)
                    discoveryAllFailuresPerformanceCounter.Increment();

                return RegisterPeerResult.GlobalFailure;
            }
        
            string fullVersion = new Version(version).ToString(4);
            version = new Version(version).ToString(3);
            string ipAddress = Context.Request.ServerVariables["REMOTE_ADDR"].ToString();
        
            try 
            {
                using(SqlConnection myConnection = new SqlConnection(ServerSettings.SpeciesDsn)) 
                {
                    myConnection.Open();

                    SqlCommand command = new SqlCommand("TerrariumRegisterPeerCountAndList", myConnection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter parmVersion = command.Parameters.Add("@Version", SqlDbType.VarChar, 255);
                    parmVersion.Value = version;
                    SqlParameter parmFullVersion = command.Parameters.Add("@FullVersion", SqlDbType.VarChar, 255);
                    parmFullVersion.Value = fullVersion;
                    SqlParameter parmChannel = command.Parameters.Add("@Channel", SqlDbType.VarChar, 255);
                    parmChannel.Value = channel;
                    SqlParameter parmIP = command.Parameters.Add("@IPAddress", SqlDbType.VarChar, 50);
                    parmIP.Value = ipAddress;
                    SqlParameter parmGuid = command.Parameters.Add("@Guid", SqlDbType.UniqueIdentifier, 16);
                    parmGuid.Value = guid;

                    SqlParameter parmDisabledError = command.Parameters.Add("@Disabled_Error", SqlDbType.Bit, 1);
                    parmDisabledError.Direction = ParameterDirection.Output;
                    SqlParameter parmPeerCount = command.Parameters.Add("@PeerCount", SqlDbType.Int, 4);
                    parmPeerCount.Direction = ParameterDirection.Output;

                    adapter.Fill(peers, "Peers");
                    count = (int) parmPeerCount.Value;

                    if(discoveryAllPerformanceCounter != null)
                        discoveryAllPerformanceCounter.Increment();
                    if(discoveryRegistrationPerformanceCounter != null)
                        discoveryRegistrationPerformanceCounter.Increment();

                    if ( ((bool) parmDisabledError.Value) ) 
                    {
                        return RegisterPeerResult.GlobalFailure;
                    }
                    else 
                    {
                        return RegisterPeerResult.Success;
                    }
                }
            } 
            catch(Exception e) 
            {
                InstallerInfo.WriteEventLog("RegisterMyPeerGetCountAndPeerList", e.ToString());

                if(discoveryRegistrationFailuresPerformanceCounter != null)
                    discoveryRegistrationFailuresPerformanceCounter.Increment();
                if(discoveryAllFailuresPerformanceCounter != null)
                    discoveryAllFailuresPerformanceCounter.Increment();
            }

            return RegisterPeerResult.Failure;
        }

		/// <summary>
		/// Checks to see if a specific version is disabled or not.  Used by the client at start up.
		/// This allows an admin to totally shutdown a version.
		/// </summary>
		/// <param name="version">String specifying the version number.</param>
		/// <param name="errorMessage">Out Parameter.  This is an admin message that the client will display if the version is sidabled</param>
		/// <returns>True if the version is enabled, false otherwise</returns>
		[WebMethod]
		public bool IsVersionDisabled( string version, out string errorMessage )
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(ServerSettings.SpeciesDsn))
				{
					connection.Open();

					SqlCommand command = new SqlCommand("TerrariumIsVersionDisabled", connection);
					command.CommandType = CommandType.StoredProcedure;
					
					string fullVersion = new Version(version).ToString(4);
					version = new Version(version).ToString(3);

					SqlParameter versionParameter = command.Parameters.Add("@Version", SqlDbType.VarChar, 255);
					versionParameter.Value = version;
					SqlParameter fullVersionParameter = command.Parameters.Add("@FullVersion", SqlDbType.VarChar, 255);
					fullVersionParameter.Value = fullVersion;

					SqlDataReader reader = command.ExecuteReader();

					if (reader.Read() == true)
					{
						bool disabled = Convert.ToBoolean(reader["Disabled"]);
						if ( disabled == true )
							errorMessage = Convert.ToString(reader["Message"]);
						else
							errorMessage = "";
						return disabled;
					}
					else
					{
						errorMessage = "";
						return true;
					}
				}
			}
			catch(Exception e)
			{
				InstallerInfo.WriteEventLog("CheckVersion", e.ToString());
				errorMessage = "";
				return true;
			}

		}
    }
}
