//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                       
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using Terrarium.Configuration;

namespace Terrarium.Tools
{
    /// <summary>
    ///  Centralized logging class for loggin handle/unhandled exceptions.  This class
    ///  caches various calls in order to generate a dataset when a fatal error occurs.
    ///  This dataset is then formatted and sent to the Terrarium Watson Service.
    /// </summary>
    /// <threadsafe />
    public class ErrorLog
    {
        /// <summary>
        ///  A Mutex object used to make the ErrorLog class threadsafe and to synchronize
        ///  access to certain methods and resources.
        /// </summary>
        private static readonly Mutex _syncObject = new Mutex(false);

        /// <summary>
        ///  The name of the machine the client is running on
        /// </summary>
        private static string _machineName = "<not set>";

        /// <summary>
        ///  Used to set and retrieve the name of the machine the client is running
        ///  on.
        /// </summary>
        public static string MachineName
        {
            get { return _machineName; }

            set { _machineName = value; }
        }

        /// <summary>
        ///  Log handled exceptions.  This can be used in order to review handled
        ///  exceptions to make sure things are happening correctly.
        /// </summary>
        /// <param name="e">Exception to handle</param>
        public static void LogHandledException(Exception e)
        {
#if DEBUG
            try
            {
                Trace.WriteLine("[DEBUGGING ONLY] HANDLED Exception: " + FormatException(e));
            }
            catch
            {
            }
#endif
        }

        /// <summary>
        ///  Formats and exception object.  Currently, only the SocketException
        ///  makes use fo the custom formatting logic.  Other formats can easily
        ///  be added.
        /// </summary>
        /// <param name="e">Exception to format</param>
        /// <returns>A formatted exception string</returns>
        public static string FormatException(Exception e)
        {
            if (e is SocketException)
            {
                return String.Format("SocketException({0}): {1}\r\n{2}", ((SocketException) e).NativeErrorCode,
                                     e.Message, e.StackTrace);
            }
            return e.ToString();
        }

        /// <summary>
        ///  Logs a failed assertion.  Any failed assertions launch the Watson
        ///  dialog which is then used to submit an error report to the Terrarium
        ///  Watson Service.
        /// </summary>
        /// <param name="message">Message for the assertion</param>
        /// <param name="traces">A series of cached previous tracings</param>
        public static void LogFailedAssertion(string message, string traces)
        {
            _syncObject.WaitOne();

            try
            {
                if (!GameConfig.ShowErrors)
                {
                    Debug.WriteLine("Not showing watson dialog since ShowErrors == false");
                    return;
                }
            }
            catch (Exception exception)
            {
                // Catch all exceptions because we don't want the user to get into an infinite loop
                // and if the website is down, we'll just throw away the data
                LogHandledException(exception);
            }
            finally
            {
                _syncObject.ReleaseMutex();
            }
        }

        /// <summary>
        ///  Logs a failed assertion.  Any failed assertions launch the Watson
        ///  dialog which is then used to submit an error report to the Terrarium
        ///  Watson Service.
        /// </summary>
        /// <param name="message">Message for the assertion</param>
        public static void LogFailedAssertion(string message)
        {
            LogFailedAssertion(message, "");
        }

        /// <summary>
        ///  Creates an error dataset to be reported to the Terrarium Watson Service.
        ///  Each dataset has information about the machine, the OS, the version of
        ///  the game, the type of log, and optionally some an email and user comment.
        /// </summary>
        /// <param name="logType">The type of log Assertion or Unhandled Exception generally.</param>
        /// <param name="errorLog">The error log text, generally a compilation of the exception and previous tracings.</param>
        /// <returns>A Watson Service DataSet that can be used in a web service call.</returns>
        public static DataSet CreateErrorLogDataSet(string logType, string errorLog)
        {
            var data = new DataSet {Locale = CultureInfo.InvariantCulture};

            var watsonTable = data.Tables.Add("Watson");
            watsonTable.Columns.Add("LogType", typeof (String));
            watsonTable.Columns.Add("OSVersion", typeof (String));
            watsonTable.Columns.Add("GameVersion", typeof (String));
            watsonTable.Columns.Add("CLRVersion", typeof (String));
            watsonTable.Columns.Add("UserEmail", typeof (String));
            
            var dcComment = new DataColumn("UserComment", typeof (String)) {MaxLength = Int32.MaxValue};
            watsonTable.Columns.Add(dcComment);
            
            var dcErrorLog = new DataColumn("ErrorLog", typeof (String)) {MaxLength = Int32.MaxValue};
            watsonTable.Columns.Add(dcErrorLog);

            var row = watsonTable.NewRow();
            row["LogType"] = logType;
            row["ErrorLog"] = errorLog;
            row["OSVersion"] = Environment.OSVersion;
            row["UserEmail"] = "";
            row["UserComment"] = "";
            row["GameVersion"] = Assembly.GetExecutingAssembly().GetName().Version;
            row["CLRVersion"] = Environment.Version;
            watsonTable.Rows.Add(row);

            return data;
        }
    }
}