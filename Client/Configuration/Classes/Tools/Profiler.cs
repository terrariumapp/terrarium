//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                       
//------------------------------------------------------------------------------

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
        private Hashtable _profileKeys;

        /// <summary>
        ///  Creates a new DirectDrawProfiler and clears any
        ///  profiler settings by creating a new Hashtable.
        /// </summary>
        public Profiler()
        {
            ClearProfiler();
        }

        /// <summary>
        ///  Default indexed property that gives access to nodes by
        ///  their key name.
        /// </summary>
        public ProfilerNode this[string key]
        {
            get { return _profileKeys.ContainsKey(key) ? (ProfilerNode) _profileKeys[key] : null; }
        }

        /// <summary>
        ///  Returns all nodes currently in the profiler.  This can be used
        ///  for generated node reports.
        /// </summary>
        public ProfilerNode[] Nodes
        {
            get
            {
                var offset = 0;
                var nodeArray = new ProfilerNode[_profileKeys.Count];

                foreach (var node in _profileKeys.Values)
                {
                    nodeArray[offset] = (ProfilerNode) node;
                    offset++;
                }

                return nodeArray;
            }
        }

        /// <summary>
        ///  Clears any profiler data.
        /// </summary>
        public void ClearProfiler()
        {
            _profileKeys = new Hashtable();
        }

        /// <summary>
        ///  Starts profiling the function or profile of the given name.
        /// </summary>
        /// <param name="functionName">Name of the function or profile to time.</param>
        public void Start(string functionName)
        {
            if (_profileKeys.ContainsKey(functionName))
            {
                var node = (ProfilerNode) _profileKeys[functionName];
                node.Start();
            }
            else
            {
                var node = new ProfilerNode(functionName);
                _profileKeys.Add(functionName, node);
                node.Start();
            }
        }

        /// <summary>
        ///  Ends the profiling of a given function or profile node.
        /// </summary>
        /// <param name="functionName">Name of the function or profile node to stop timing.</param>
        public void End(string functionName)
        {
            if (!_profileKeys.ContainsKey(functionName)) return;
            var node = (ProfilerNode) _profileKeys[functionName];
            node.End();
        }
    }
#endif
}