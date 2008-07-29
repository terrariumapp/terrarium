//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace Terrarium.Tools
{
	using System;

	internal class StyleEditorApplication
	{
		[STAThread]
		internal static void Main( string[] args )
		{
			try
			{
				System.Windows.Forms.Application.EnableVisualStyles();
				System.Windows.Forms.Application.DoEvents();
			}
			catch{}

			MainForm mainForm = new MainForm();
			System.Windows.Forms.Application.Run( mainForm );
			mainForm.Dispose();
			mainForm = null;
		}
	}
}
