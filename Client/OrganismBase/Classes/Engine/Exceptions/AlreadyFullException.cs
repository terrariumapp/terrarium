//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

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