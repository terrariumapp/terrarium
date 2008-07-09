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
using System.Diagnostics;
using Terrarium.Server;

namespace Terrarium.Server
{
	/*
		Class:      WatsonService
	*/
	/// <summary>
	/// Enables logging of errors from Terrarium clients.
	/// </summary>
	public class WatsonService : WebService 
	{
		/*
			Method:      ReportError
		*/
		/// <summary>
		/// Takes a formatted DataSet from a client and inserts	a watson record into the database.
		/// </summary>
		/// <param name="data">DataSet containing error information.</param>
		[WebMethod]
		public void ReportError(DataSet data) 
		{
			try 
			{
				string ip = Context.Request.ServerVariables["REMOTE_ADDR"].ToString();

				if ( data.Tables["watson"].Columns["MachineName"] == null )
					data.Tables["watson"].Columns.Add("MachineName", typeof(string));

				foreach(DataRow dr in data.Tables["watson"].Rows) 
				{
					dr["MachineName"] = ip;
				}

				using(SqlConnection myConnection = new SqlConnection(ServerSettings.SpeciesDsn)) 
				{
					myConnection.Open();

					// Update the history data
					SqlDataAdapter adapter = new SqlDataAdapter();
					adapter.InsertCommand = new SqlCommand("TerrariumInsertWatson", myConnection);
					adapter.InsertCommand.CommandType = CommandType.StoredProcedure;

					adapter.InsertCommand.Parameters.Add("@LogType", SqlDbType.VarChar, 50, "LogType");
					adapter.InsertCommand.Parameters.Add("@MachineName", SqlDbType.VarChar, 255, "MachineName");
					adapter.InsertCommand.Parameters.Add("@OSVersion", SqlDbType.VarChar, 50, "OSVersion");
					adapter.InsertCommand.Parameters.Add("@GameVersion", SqlDbType.VarChar, 50, "GameVersion");
					adapter.InsertCommand.Parameters.Add("@CLRVersion", SqlDbType.VarChar, 50, "CLRVersion");
					adapter.InsertCommand.Parameters.Add("@ErrorLog", SqlDbType.Text, Int32.MaxValue, "ErrorLog");
					adapter.InsertCommand.Parameters.Add("@UserEmail", SqlDbType.Text, Int32.MaxValue, "UserEmail");
					adapter.InsertCommand.Parameters.Add("@UserComment", SqlDbType.Text, Int32.MaxValue, "UserComment");

					adapter.Update(data, "Watson");
				}
			}
			catch(Exception e) 
			{
				InstallerInfo.WriteEventLog("Watson", e.ToString());
			}
		}
	}
}
