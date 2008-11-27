//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

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
        private static readonly double _startTickCountMicroseconds;
        private static TraceSwitch _api;
        private static TraceSwitch _exceptionCaught;
        private static TraceSwitch _exceptionThrown;
        private static TraceSwitch _internalLog;
        private static TraceSwitch _protocol;
        private static TraceSwitch _socket;

        static HttpTraceHelper()
        {
            //  Stream stream = new FileStream(@"WebListener.log", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            //  TraceListener listener = new TextWriterTraceListener(stream);
            //  Debug.AutoFlush = true;
            //  Debug.Listeners.Add(listener);

            double ticksPerSecond = 0;
            QueryPerformanceFrequency(ref ticksPerSecond);
            QueryPerformanceCounter(ref _startTickCountMicroseconds);
        }

        public static TraceSwitch Api
        {
            get
            {
                if (_api == null)
                {
                    _api = new TraceSwitch("WebListener.Api",
                                           "Enable tracing for calls made to the System.Net WebListener public APIs");
                }
                return _api;
            }
        }

        public static TraceSwitch InternalLog
        {
            get
            {
                if (_internalLog == null)
                {
                    _internalLog = new TraceSwitch("WebListener.InternalLog",
                                                   "Enable tracing for public logging made by the System.Net WebListener classes");
                }
                return _internalLog;
            }
        }

        public static TraceSwitch Protocol
        {
            get
            {
                if (_protocol == null)
                {
                    _protocol = new TraceSwitch("WebListener.Protocol",
                                                "Enable tracing for Protocol handling made by the System.Net WebListener classes");
                }
                return _protocol;
            }
        }

        public static TraceSwitch Socket
        {
            get
            {
                if (_socket == null)
                {
                    _socket = new TraceSwitch("WebListener.Socket",
                                              "Enable tracing for Socket calls made by the System.Net WebListener classes");
                }
                return _socket;
            }
        }

        public static TraceSwitch ExceptionThrown
        {
            get
            {
                if (_exceptionThrown == null)
                {
                    _exceptionThrown = new TraceSwitch("WebListener.Exception",
                                                       "Enable tracing for all Exceptions thrown by the System.Net WebListener classes");
                }
                return _exceptionThrown;
            }
        }

        public static TraceSwitch ExceptionCaught
        {
            get
            {
                if (_exceptionCaught == null)
                {
                    _exceptionCaught = new TraceSwitch("WebListener.Exception",
                                                       "Enable tracing for all Exceptions caught by the System.Net WebListener classes");
                }
                return _exceptionCaught;
            }
        }

        //
        // validation helper methods
        //
        public static bool IsBlankString(string stringValue)
        {
            return string.IsNullOrEmpty(stringValue);
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
            if (objectValue is string && ((string) objectValue).Length == 0)
            {
                return "(string.empty)";
            }
            return objectValue.ToString();
        }

        public static string HashString(object objectValue)
        {
            if (objectValue == null)
            {
                return "(null)";
            }
            if (objectValue is string && ((string) objectValue).Length == 0)
            {
                return "(string.empty)";
            }
            return objectValue.GetHashCode().ToString();
        }

        public static void WriteLine(string msg)
        {
            Debug.WriteLine(msg);
        }

        /// <summary>
        ///  Used to query the system timer frequency.  Returns counts per
        ///  second.  Ref parameter is a LARGE_INTEGER
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern int QueryPerformanceFrequency(ref double quadpart);

        /// <summary>
        ///  Used to query the number of elapsed intervals in the system
        ///  timer.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern int QueryPerformanceCounter(ref double quadpart);
    }
}