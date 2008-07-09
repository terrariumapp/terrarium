//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using DxVBLib;

using Terrarium.Tools;

namespace Terrarium.Renderer.DirectX 
{
    /// <summary>
    ///  Provides access to the DirectDraw and DirectX interfaces
    /// </summary>
    public class ManagedDirectX
    {
        /// <summary>
        ///  Holds an instance of the DirectX7 native object
        /// </summary>
        static DirectX7            directX;
        /// <summary>
        ///  Holds an instance of the DirectDraw7 native object
        /// </summary>
        static DirectDraw7         directDraw;

#if TRACE
        /// <summary>
        ///  Holds an instance of the DirectDrawProfiler timing object.
        /// </summary>
        static Profiler  directDrawProfiler;

        /// <summary>
        ///  Provides access to the DirectDrawProfiler timing object.
        /// </summary>
        public static Profiler Profiler
        {
            get
            {
                if (directDrawProfiler == null)
                {
                    directDrawProfiler = new Profiler();
                }

                return directDrawProfiler;
            }
        }
#endif

        /// <summary>
        ///  Provides access to the native DirectDraw7 object
        /// </summary>
        public static DirectDraw7 DirectDraw
        {
            get
            {
                try
                {
                    if (directDraw == null)
                    {
                        directDraw = DirectX.DirectDrawCreate( "" );
                    }
                
                    return directDraw;
                }
                catch (Exception exc)
                {
                    throw new DirectXException("Error obtaining DirectDraw interface", exc);
                }
            }
        }

        /// <summary>
        ///  Provides access to the native DirectX7 object
        /// </summary>
        public static DirectX7 DirectX
        {
            get
            {
                try
                {
                    if (directX == null)
                    {
                        directX = new DirectX7();
                    }
                    return directX;
                }
                catch (Exception exc)
                {
                    throw new DirectXException("Error obtaining DirectX interface", exc);
                }
            }
        }
    }
}