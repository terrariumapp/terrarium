//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

namespace Terrarium.Server.Messaging
{
	/// <summary>
	/// Summary description for Messaging.
	/// </summary>
	public class Messaging : System.Web.Services.WebService
	{
		public Messaging()
		{
		}

		[ WebMethod ] 
		public string GetWelcomeMessage()
		{
			try
			{
				return ServerSettings.WelcomeMessage;
			}
			catch
			{
				return "Welcome to .NET Terrarium 2.0!";
			}
		}


		[ WebMethod ]
		public string GetMessageOfTheDay()
		{
			try
			{
				return ServerSettings.MOTD;
			}
			catch
			{
				return "Have Fun!";
			}
		}

		[ WebMethod ]
		public string GetLatestVersion()
		{
			try
			{
				return ServerSettings.LatestVersion;
			}
			catch
			{
				return "1.0.0.0";
			}
		}
	}
}
