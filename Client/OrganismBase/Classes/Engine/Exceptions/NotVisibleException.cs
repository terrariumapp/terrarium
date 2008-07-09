//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    /// You tried to perform an action on another organism that was not visible to you.
    /// </summary>
    public class NotVisibleException : OrganismException 
    {
        internal NotVisibleException() : base("This organism is not visible.")
        {
        }
    }
}