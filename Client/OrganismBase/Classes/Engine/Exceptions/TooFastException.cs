//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------
using System;

namespace OrganismBase
{
    /// <summary>
    /// This organism cannot move this fast.
    /// </summary>
    public class TooFastException : OrganismException
    {
        internal TooFastException() : base("This organism cannot move this fast.")
        {
        }
    }
}