//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    /// Not enough time has elapsed since last reproduction.
    /// </summary>
    public class NotReadyToReproduceException : OrganismException
    {
        internal NotReadyToReproduceException() : base("Not enough time has elapsed since last reproduction.")
        {
        }
    }
}