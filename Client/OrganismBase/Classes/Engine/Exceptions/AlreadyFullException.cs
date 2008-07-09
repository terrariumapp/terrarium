//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    /// The animal is already full, so this action doesn't make sense.
    /// </summary>
    public class AlreadyFullException : OrganismException
    {
        internal AlreadyFullException() : base("Organism is already full.")
        {
        }
    }
}