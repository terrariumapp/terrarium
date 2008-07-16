//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Terrarium.Tools
{
    /// <summary>
    ///  This class provides in-direct access to the high
    ///  frequency performance counters for very accurate
    ///  timings.
    /// </summary>
    public sealed class TimeMonitor
    {
        /// <summary>
        ///  The number of microseconds in a single second
        /// </summary>
        private const int MicroSecondsPerSecond = 1000000;

        /// <summary>
        ///  A value representing the overhead of calling the time monitor class
        /// </summary>
        private readonly double _apiOverHead;

        /// <summary>
        ///  Number of ticks per second retrieved using QueryPerformanceFrequency
        /// </summary>
        private readonly double _ticksPerSec;

        /// <summary>
        ///  The starting value of the timer class, as set by a call to Start
        /// </summary>
        private double _startCounter = -1;

        /// <summary>
        ///  Initialize a new time monitor for computing small differences in time.
        ///  Only short timings should be made since thread switching can cause
        ///  abnormal values in the windows environment.
        /// </summary>
        public TimeMonitor()
        {
            QueryPerformanceFrequency(ref _ticksPerSec);
            double start = 0;
            double end = 0;

            // try to compute an approximate cost for invoking the API
            // so we can substract this from our timings
            QueryPerformanceCounter(ref start);
            QueryPerformanceCounter(ref end);
            _apiOverHead = end - start;

            Start();
            EndGetMicroseconds();
        }

        /// <summary>
        ///  Determines if the current time monitor has been started already.
        /// </summary>
        public Boolean IsStarted
        {
            get { return _startCounter != -1; }
        }

        /// <summary>
        ///  Used to retrieve the number of seconds elapsed in this counter.
        ///  This should only be called after a call to Start.
        /// </summary>
        /// <returns>The amount of time elapsed in seconds.</returns>
        public double GetCounterSeconds()
        {
            double counter = -1;

            QueryPerformanceCounter(ref counter);

            return (counter/_ticksPerSec);
        }

        /// <summary>
        ///  Starts the timing class by making a call to QueryPerformanceCounter
        ///  for the current time.
        /// </summary>
        public void Start()
        {
            QueryPerformanceCounter(ref _startCounter);
        }

        /// <summary>
        ///  Used to retreive the number of microseconds elapsed in this counter.
        ///  This should only be called after a call to Start.  This method
        ///  subtracts out the overhead time of calling the Start and End methods.
        /// </summary>
        /// <returns>The number of elapsed microseconds</returns>
        public Int64 EndGetMicroseconds()
        {
            double endCounter = 0;
            QueryPerformanceCounter(ref endCounter);

            // Do this after query so we don't pay the cost of the if statement
            if (_startCounter == -1)
            {
                throw new ApplicationException("Start must be called before calling End.");
            }

            Int64 elapsedTime = FreqToMicroSecs(endCounter - _startCounter);

            // Make sure people don't call End multiple times since we only subtract out the
            // time of the calls once
            _startCounter = -1;

            return elapsedTime;
        }

        /// <summary>
        ///  Method used to turn an elapsed time in frequency counts into an elapsed
        ///  natural time.  This method also removes the overhead of calling the 
        ///  performance counter methods.
        /// </summary>
        /// <param name="ticks">The number of elapsed ticks</param>
        /// <returns>The number of elapsed microseconds</returns>
        private Int64 FreqToMicroSecs(double ticks)
        {
            double secs = (ticks - _apiOverHead)/_ticksPerSec;
            return (Int64) (secs*MicroSecondsPerSecond);
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