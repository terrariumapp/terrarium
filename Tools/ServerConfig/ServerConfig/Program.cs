//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.Windows.Forms;

namespace ServerConfig
{
	static class Program
	{
		private static void PrintBanner()
		{
			Console.WriteLine( "Terrarium 2.0 Server Installer");
			Console.WriteLine("Copyright (c) Microsoft Corporation.");
			Console.WriteLine("");
		}

		private static void PrintUsage()
		{
			Console.WriteLine("Usage: serverconfig.exe <vroot name> <database name> <database password>");
		}

		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			WizardForm form = new WizardForm();
			Application.Run(form);
		}
	}
}
