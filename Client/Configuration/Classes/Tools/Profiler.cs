//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                       
//------------------------------------------------------------------------------

using System;
using System.Collections;

namespace Terrarium.Tools
{
#if TRACE
    /// <summary>
    ///  Encompasses the logic to profile the DirectDraw classes
    /// </summary>
    public class Profiler
    {
        /// <summary>
        ///  Contains named profiles
        /// </summary>
        Hashtable profileKeys;

        /// <summary>
        ///  Creates a new DirectDrawProfiler and clears any
        ///  profiler settings by creating a new Hashtable.
        /// </summary>
        public Profiler()
        {
            ClearProfiler();
        }

        /// <summary>
        ///  Clears any profiler data.
        /// </summary>
        public void ClearProfiler()
        {
            profileKeys = new Hashtable();
        }

        /// <summary>
        ///  Starts profiling the function or profile of the given name.
        /// </summary>
        /// <param name="functionName">Name of the function or profile to time.</param>
        public void Start(string functionName)
        {
            if (profileKeys.ContainsKey(functionName))
            {
                ProfilerNode node = (ProfilerNode) profileKeys[functionName];
                node.Start();
            }
            else
            {
                ProfilerNode node = new ProfilerNode(functionName);
                profileKeys.Add(functionName, node);
                node.Start();
            }
        }

        /// <summary>
        ///  Ends the profiling of a given function or profile node.
        /// </summary>
        /// <param name="functionName">Name of the function or profile node to stop timing.</param>
        public void End(string functionName)
        {
            if (profileKeys.ContainsKey(functionName))
            {
                ProfilerNode node = (ProfilerNode) profileKeys[functionName];
                node.End();
            }
        }

        /// <summary>
        ///  Default indexed property that gives access to nodes by
        ///  their key name.
        /// </summary>
        public ProfilerNode this[string key]
        {
            get
            {
                if (profileKeys.ContainsKey(key))
                {
                    return (ProfilerNode) profileKeys[key];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        ///  Returns all nodes currently in the profiler.  This can be used
        ///  for generated node reports.
        /// </summary>
        public ProfilerNode[] Nodes
        {
            get
            {
                int offset = 0;
                ProfilerNode[] nodeArray = new ProfilerNode[profileKeys.Count];

                foreach (object node in profileKeys.Values)
                {
                    nodeArray[offset] = (ProfilerNode) node;
                    offset++;
                }

                return nodeArray;
            }
        }
    }
#endif
}