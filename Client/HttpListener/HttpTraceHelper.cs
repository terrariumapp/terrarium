//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

using Terrarium.Tools;

namespace Terrarium.Net
{
    // The HttpListener has a large amount of tracing built into it to help
    // narrow down bugs in the implementation as well as network issues.
    // The different tracing switches are described in the properties that coorespond
    // to them below
    // All tracing is done through the Debug class, which writes these errors to the standard
    // debugger output by default.  The static constructor below shows a commented out example
    // of how to get this output to go to a file instead.
    public sealed class HttpTraceHelper
    {
        private static double millisecondsPerTicks;
        private static double startTickCountMicroseconds;

        private static TraceSwitch api;
        private static TraceSwitch internalLog;
        private static TraceSwitch protocol;
        private static TraceSwitch socket;
        private static TraceSwitch exceptionThrown;
        private static TraceSwitch exceptionCaught;

        static HttpTraceHelper()
        {
            //  Stream stream = new FileStream(@"WebListener.log", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            //  TraceListener listener = new TextWriterTraceListener(stream);
            //  Debug.AutoFlush = true;
            //  Debug.Listeners.Add(listener);

            double ticksPerSecond = 0;
            QueryPerformanceFrequency(ref ticksPerSecond);
            millisecondsPerTicks = 1000.0/(double)ticksPerSecond;
            QueryPerformanceCounter(ref startTickCountMicroseconds);
        }

        public static TraceSwitch Api
        {
            get
            {
                if (api == null)
                {
                    api = new TraceSwitch("WebListener.Api", "Enable tracing for calls made to the System.Net WebListener public APIs");
                }
                return api;
            }
        }

        public static TraceSwitch InternalLog
        {
            get
            {
                if (internalLog == null)
                {
                    internalLog = new TraceSwitch("WebListener.InternalLog", "Enable tracing for public logging made by the System.Net WebListener classes");
                }
                return internalLog;
            }
        }

        public static TraceSwitch Protocol
        {
            get
            {
                if (protocol == null)
                {
                    protocol = new TraceSwitch("WebListener.Protocol", "Enable tracing for Protocol handling made by the System.Net WebListener classes");
                }
                return protocol;
            }
        }

        public static TraceSwitch Socket
        {
            get
            {
                if (socket == null)
                {
                    socket = new TraceSwitch("WebListener.Socket", "Enable tracing for Socket calls made by the System.Net WebListener classes");
                }
                return socket;
            }
        }

        public static TraceSwitch ExceptionThrown
        {
            get
            {
                if (exceptionThrown == null)
                {
                    exceptionThrown = new TraceSwitch("WebListener.Exception", "Enable tracing for all Exceptions thrown by the System.Net WebListener classes");
                }
                return exceptionThrown;
            }
        }

        public static TraceSwitch ExceptionCaught
        {
            get
            {
                if (exceptionCaught == null)
                {
                    exceptionCaught = new TraceSwitch("WebListener.Exception", "Enable tracing for all Exceptions caught by the System.Net WebListener classes");
                }
                return exceptionCaught;
            }
        }

        //
        // validation helper methods
        //
        public static bool IsBlankString(string stringValue)
        {
            return stringValue==null||stringValue.Length==0;
        }

        public static string MakeStringNull(string stringValue)
        {
            return IsBlankString(stringValue) ? null : stringValue;
        }

        public static string MakeStringEmpty(string stringValue)
        {
            return IsBlankString(stringValue) ? string.Empty : stringValue;
        }

        public static string ToString(object objectValue) 
        {
            if (objectValue == null)
            {
                return "(null)";
            }
            else if (objectValue is string && ((string)objectValue).Length == 0)
            {
                return "(string.empty)";
            }
            else
            {
                return objectValue.ToString();
            }
        }

        public static string HashString(object objectValue) 
        {
            if (objectValue == null)
            {
                return "(null)";
            }
            else if (objectValue is string && ((string)objectValue).Length == 0)
            {
                return "(string.empty)";
            }
            else
            {
                return objectValue.GetHashCode().ToString();
            }
        }

        public static void WriteLine(string msg) 
        {
            Debug.WriteLine(msg);
        }

        //public static void WriteLine(bool suppressHeader, string msg)
        //{
        //    if (!suppressHeader)
        //    {
        //        double counter = 0;
        //        QueryPerformanceCounter(ref counter);
        //        if (startTickCountMicroseconds > counter)
        //        {
        //            // counter reset, restart from 0
        //            startTickCountMicroseconds = counter;
        //        }
        //        counter -= startTickCountMicroseconds;
        //        string tickString = new TimeSpan((long) (counter/100D)).ToString();
        //        if (tickString.Length < 16)
        //        {
        //            tickString += ".0000000";
        //        }
        //        msg = "["+ tickString + "@" + Thread.CurrentThread.GetHashCode().ToString() + "] " + msg;
        //    }

        //    Debug.WriteLine(msg);
        //}

        /// <summary>
        ///  Used to query the system timer frequency.  Returns counts per
        ///  second.  Ref parameter is a LARGE_INTEGER
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute()]
        [DllImport("kernel32", CharSet=CharSet.Auto)]
        static extern int QueryPerformanceFrequency(ref double quadpart);

        /// <summary>
        ///  Used to query the number of elapsed intervals in the system
        ///  timer.
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute()]
        [DllImport("kernel32", CharSet=CharSet.Auto)]
        static extern int QueryPerformanceCounter(ref double quadpart);
    }
}