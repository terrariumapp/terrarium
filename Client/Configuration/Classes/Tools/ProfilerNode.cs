//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                           
//------------------------------------------------------------------------------

using System;

namespace Terrarium.Tools
{
#if TRACE
    /// <summary>
    ///  Represents an invidual function or profiler node
    /// </summary>
    public class ProfilerNode
    {
        /// <summary>
        ///  Access to the high performance timers.
        /// </summary>
        private readonly TimeMonitor _timeMonitor;

        /// <summary>
        ///  True if start has been called
        /// </summary>
        private bool _isCounting;

        /// <summary>
        ///  The amount of time consumed by the last call
        /// </summary>
        private Int64 _lastTime;

        /// <summary>
        ///  The total amount of time consumed by all calls.
        /// </summary>
        private Int64 _runningTotal;

        /// <summary>
        ///  The number of times the node has been called.
        /// </summary>
        private Int32 _samples;

        /// <summary>
        ///  Creates a new node given an id.  Sets up the initial
        ///  TimeMonitor used to record high performance timings.
        /// </summary>
        /// <param name="id">ID for this node.</param>
        public ProfilerNode(string id)
        {
            _timeMonitor = new TimeMonitor();
            _isCounting = false;
        }

        /// <summary>
        ///  Start a timing session.
        /// </summary>
        public void Start()
        {
            _isCounting = true;
            _timeMonitor.Start();
        }

        /// <summary>
        ///  End a timing session and update the running total, samples, and
        ///  last timing variables.
        /// </summary>
        public void End()
        {
            if (!_isCounting) return;
            _lastTime = _timeMonitor.EndGetMicroseconds();
            _runningTotal += _lastTime;
            _samples++;
        }
    }
#endif
}