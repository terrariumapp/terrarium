//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Resources;

public class ServerSetupHelper {

	public static void InstallACLs(string installRoot) 
	{
        string chartDataPath = Path.Combine(installRoot, "chartdata");
        string speciesAssemblyPath = Path.Combine(installRoot, "species\\assemblies");

        if ( !Directory.Exists(chartDataPath) ) { Directory.CreateDirectory(chartDataPath); }
        if ( !Directory.Exists(speciesAssemblyPath) ) { Directory.CreateDirectory(speciesAssemblyPath); }

		ProcessStartInfo psi = new ProcessStartInfo();
		psi.CreateNoWindow = true;
		psi.WindowStyle = ProcessWindowStyle.Hidden;
		psi.Arguments = "\"" + chartDataPath + "\" /C /E /G ASPNET:F";
		psi.FileName = "cacls.exe";

		Process process = Process.Start(psi);
		process.WaitForExit();

		psi.Arguments = "\"" + speciesAssemblyPath + "\" /C /E /G ASPNET:F";
		process = Process.Start(psi);
		process.WaitForExit();
    }
    
    public static void InstallDatabase(string connectionString, string dbName, string pass) 
	{
        string localSql = SqlScript;
        if ( localSql == null ) {
            return;
        }
        
        localSql = localSql.Replace("[DBName]", dbName);
        localSql = localSql.Replace(
            "exec sp_addlogin N'TerrariumUser', null, @logindb, @loginlang",
            "exec sp_addlogin N'TerrariumUser', '" + pass + "', @logindb, @loginlang"
        );
        
        InstallExecuteSQL(localSql, connectionString);
    }
    
    public static void InstallExecuteSQL(string sql, string connstr) 
	{
        using(SqlConnection conn = new SqlConnection(connstr)) {
            conn.Open();
            
            string[] commands = Regex.Split(sql, "GO");
            for(int i = 0; i < commands.Length; i++) {
                if ( commands[i].Trim() != "" ) {
                    SqlCommand comm = new SqlCommand(commands[i], conn);
                    comm.ExecuteNonQuery();
                }
            }
            conn.Close();
        }
    }
    
    public static void InstallWebConfig(string installRoot, string dsn) 
	{
        string localConfig = WebConfig;
        if ( localConfig == null ) {
            return;
        }
        
        localConfig = localConfig.Replace(
                "key=\"InstallRoot\" value=\"\"",
                "key=\"InstallRoot\" value=\"" + (new DirectoryInfo(installRoot)).FullName + "\"");
        localConfig = localConfig.Replace(
                "key=\"SpeciesDsn\" value=\"\"",
                "key=\"SpeciesDsn\" value=\"" + dsn + "\"");

        string filePath = Path.Combine(installRoot, "web.config");
        if ( File.Exists(filePath) ) { File.Delete(filePath); }
        using(StreamWriter sw = new StreamWriter(filePath, false)) {
            sw.Write(localConfig);
            sw.Close();
        }
    }

    public static void InstallWebRoot(string iisHost, string installRoot, string webRoot) 
	{
        DirectoryEntry deRootWeb = new DirectoryEntry("IIS://" + iisHost + "/W3SVC/1/ROOT");
        IISWebVirtualDir iRootWeb = new IISWebVirtualDir(deRootWeb);
        IISWebVirtualDir iTerrariumWeb = iRootWeb.AddWebVirtualDir(webRoot, installRoot);
    }
        
    private static string webConfig = null;
    private static string WebConfig {
        get {
            if ( webConfig == null ) {
                using(StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ServerConfig.web.config"))) {
                    webConfig = sr.ReadToEnd();
                }
                
                if ( webConfig == null || webConfig.Trim().Length == 0 ) {
                    webConfig = null;
                }
            }
            
            return webConfig;
        }
    }
    
    private static string sqlScript = null;
    private static string SqlScript {
        get {
            if ( sqlScript == null ) 
			{
                using(StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ServerConfig.SqlSetup.sql"))) {
                    sqlScript = sr.ReadToEnd();
                }
                
                if ( sqlScript == null || sqlScript.Trim().Length == 0 ) {
                    sqlScript = null;
                }
            }
            
            return sqlScript;
        }
    }
}