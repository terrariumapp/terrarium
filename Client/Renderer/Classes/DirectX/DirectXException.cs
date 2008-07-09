//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------


using System;

namespace Terrarium.Renderer.DirectX 
{
    /// <summary>
    ///  Represents a DirectXException so that real COM/DirectX
    ///  exceptions can be wrapped into a managed exception and
    ///  handled by the application.
    /// </summary>
    public sealed class DirectXException : Exception
    {
        /// <summary>
        ///  Creates a new exception from a base message.
        /// </summary>
        /// <param name="msg">Custom message</param>
        public DirectXException(string msg) : base(msg)
        {
        }

        /// <summary>
        ///  Creates a new exception from a base message and an original exception.
        /// </summary>
        /// <param name="msg">Custom message</param>
        /// <param name="inner">The original exception</param>
        public DirectXException(string msg, Exception inner) : base(msg, inner)
        {
        }
    }
}