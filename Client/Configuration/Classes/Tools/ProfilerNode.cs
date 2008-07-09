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
        TimeMonitor tm;

        /// <summary>
        ///  ID of this profiler node (generally a function name)
        /// </summary>
        string id;

        /// <summary>
        ///  True if start has been called
        /// </summary>
        bool counting;

        /// <summary>
        ///  The amount of time consumed by the last call
        /// </summary>
        Int64 lasttime;

        /// <summary>
        ///  The total amount of time consumed by all calls.
        /// </summary>
        Int64 runningtotal;

        /// <summary>
        ///  The number of times the node has been called.
        /// </summary>
        Int32 samples;

        /// <summary>
        ///  Creates a new node given an id.  Sets up the initial
        ///  TimeMonitor used to record high performance timings.
        /// </summary>
        /// <param name="id">ID for this node.</param>
        public ProfilerNode(string id)
        {
            tm = new TimeMonitor();
            this.id = id;
            counting = false;
        }

        /// <summary>
        ///  Start a timing session.
        /// </summary>
        public void Start()
        {
            counting = true;
            tm.Start();
        }

        /// <summary>
        ///  End a timing session and update the running total, samples, and
        ///  last timing variables.
        /// </summary>
        public void End()
        {
            if (counting)
            {
                lasttime = tm.EndGetMicroseconds();
                runningtotal += lasttime;
                samples++;
            }
        }
    }
#endif
}