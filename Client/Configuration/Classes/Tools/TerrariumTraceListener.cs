//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                         
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Terrarium.Configuration;

namespace Terrarium.Tools 
{
    /// <summary>
    ///  The TerrariumTraceListener is responsible for stopping the
    ///  painting timer whenever an error or assertion occurs.
    /// </summary>
    public sealed class TerrariumTraceListener : DefaultTraceListener
    {
        /// <summary>
        ///  Maximum number of characters to store in the trace buffer.
        ///  A trace buffer can be built of multiple tracings and this
        ///  value is responsible for controlling truncation of the buffer.
        /// </summary>
        const int traceBufferMaxLength = 20000;
        /// <summary>
        ///  Holds an instance of the default trace listener
        /// </summary>
        private DefaultTraceListener defaultListener = new DefaultTraceListener();
        /// <summary>
        ///  Enables logging traces to a debug text file
        /// </summary>
        private Boolean enableLogging = false;
        /// <summary>
        ///  Holds an instance of the drawing timer so that it can be stopped
        ///  when an error or assertion occurs.
        /// </summary>
        private System.Windows.Forms.Timer timerToStop;
        /// <summary>
        ///  Builds a tracing string using a performance optimized string builder
        /// </summary>
        private StringBuilder traceBuilder = new StringBuilder();

        /// <summary>
        ///  Creates a default TerrariumTraceListener with logging turned off
        /// </summary>
        public TerrariumTraceListener()
        {
        }

        /// <summary>
        ///  Creates a TerrariumTraceListener with a custom value for logging.
        /// </summary>
        /// <param name="loggingFlag">If True logging is turned on, off otherwise</param>
        public TerrariumTraceListener(Boolean loggingFlag) 
        {
            enableLogging = loggingFlag;
        }

        /// <summary>
        ///  Retreives or sets the current drawing timer.
        /// </summary>
        public System.Windows.Forms.Timer TimerToStop
        {
            get 
            {
                return timerToStop;
            }

            set 
            {
                timerToStop = value;
            }
        }

        /// <summary>
        ///  Used for logging failure messages
        /// </summary>
        /// <param name="message">Message to log</param>
        public override void Fail(string message)
        {
            if (timerToStop != null)
            {
                timerToStop.Enabled = false;
            }
        
            ErrorLog.LogFailedAssertion(message + "\r\n" + Environment.StackTrace, traceBuilder.ToString());
            ShowErrorDialog("ASSERTION FAILURE: " + message, Environment.StackTrace);
            if (timerToStop != null)
            {
                timerToStop.Enabled = true;
            }
        }

        /// <summary>
        ///  Used for logging failure messages, along with a detailed message
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="detailMessage">Detailed error message.</param>
        public override void Fail(string message, string detailMessage)
        {
            if (timerToStop != null)
            {
                timerToStop.Enabled = false;
            }
            ErrorLog.LogFailedAssertion(message + "\r\n" + detailMessage + "\r\n" + Environment.StackTrace, traceBuilder.ToString());
            ShowErrorDialog("ASSERTION FAILURE: " + message + "\r\n" + detailMessage, Environment.StackTrace);
            if (timerToStop != null)
            {
                timerToStop.Enabled = true;
            }
        }

        /// <summary>
        ///  Used for logging standard messages
        /// </summary>
        /// <param name="message">Message to log.</param>
        public override void Write(string message)
        {
            if (traceBuilder.Length >= traceBufferMaxLength)
            {
                traceBuilder.Remove(0, message.Length);
            }

            if (enableLogging)
            {
                try
                {
                    using (FileStream logFileStream = File.Open("logfile.txt",FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                    {
                        logFileStream.Seek(0, SeekOrigin.End);              
                        StreamWriter logFile = new StreamWriter(logFileStream);
                        logFile.WriteLine(message);
                        logFile.Close();
                    }
                }
                catch
                {
                }
            }

            traceBuilder.Insert(0, message);

            defaultListener.Write(message); // If you leave this in it doubles messages.
        }

        /// <summary>
        ///  Used for logging categorized messages
        /// </summary>
        /// <param name="o">Object to log, ToString will be called.</param>
        /// <param name="category">Message category.</param>
        public override void Write(object o, string category)
        {
            Write("Category: " + category + "Message: " + o.ToString());
        }

        /// <summary>
        ///  Used for logging categorized messages
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="category">Message category.</param>
        public override void Write(string message, string category)
        {
            Write("Category: " + category + "Message: " + message);
        }

        /// <summary>
        ///  Used for logging standard messages
        /// </summary>
        /// <param name="o">Object to log, ToString will be called.</param>
        public override void Write(object o)
        {
            Write(o.ToString());
        }

        /// <summary>
        ///  Used for categorized message logging
        /// </summary>
        /// <param name="o">Object to log, ToString will be called.</param>
        /// <param name="category">Message category.</param>
        public override void WriteLine(object o, string category)
        {
            WriteLine("Category: " + category + "Message: " + o.ToString());
        }

        /// <summary>
        ///  Used for logging categorized messages
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="category">Message category.</param>
        public override void WriteLine(string message, string category)
        {
            WriteLine("Category: " + category + "Message: " + message);
        }

        /// <summary>
        ///  Used for logging standard messages
        /// </summary>
        /// <param name="o">Object to log, ToString will be called.</param>
        public override void WriteLine(object o) 
        {
            WriteLine(o.ToString());

        }

        /// <summary>
        ///  Used for logging standard messages
        /// </summary>
        /// <param name="message">Message to log.</param>
        public override void WriteLine(string message)
        {
            Write(message + "\r\n");
        }

        /// <summary>
        ///  Creates and displays an error dialog if game errors are enabled.
        ///  If the dialog fails to show then the Application is exited.
        ///  If the user clicks the Cancel button the Applicaton is exited.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="stackTrace"></param>
        private void ShowErrorDialog(string message, string stackTrace)
        {
            // Disable Errors in Demo Mode
            if (!GameConfig.ShowErrors)
            {
                Debug.WriteLine("Not showing error dialog since ShowErrors == false");
                return;
            }

            DialogResult result = DialogResult.Cancel;
            try
            {
                string errorMsg = "An error occurred please contact the adminstrator with the following information:\n\n";
                errorMsg = errorMsg + message + "\r\n" + stackTrace;
                result = MessageBox.Show(errorMsg, "Application Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop);
            }
            catch
            {
                try
                {
                    MessageBox.Show("Fatal Error", "Fatal Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }

            // Exit if the user clicks the Cancel button
            if (result == DialogResult.Cancel)
            {
                Application.Exit();
            }
        }
    }
}