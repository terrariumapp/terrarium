//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    /// The target organism is too far away to perform the requested action.
    /// </summary>
    public class NotWithinDistanceException : OrganismException
    {
        internal NotWithinDistanceException() : base("Too far away to perform action.")
        {
        }
    }
}