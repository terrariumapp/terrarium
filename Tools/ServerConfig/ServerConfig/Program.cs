//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.Windows.Forms;

namespace ServerConfig
{
	class Program
	{
		private static void PrintBanner()
		{
			Console.WriteLine( "Terrarium 1.2 Server Installer");
			Console.WriteLine("Copyright (c) Microsoft Corporation.");
			Console.WriteLine("");
		}

		private static void PrintUsage()
		{
			Console.WriteLine("Usage: serverutil.exe <vroot name> <database name> <database password>");
		}

		[STAThread()]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.DoEvents();

			WizardForm form = new WizardForm();
			Application.Run(form);
		}
	}
}
