//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------
using System;

namespace OrganismBase
{
    /// <summary>
    /// Only mature organisms can reproduce.
    /// </summary>
    public class NotMatureException : OrganismException
    {
        internal NotMatureException() : base("Only mature organisms can reproduce.")
        {
        }
    }
}