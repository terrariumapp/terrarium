//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    /// You can only reproduce when current reproduction is finished.
    /// </summary>
    public class AlreadyReproducingException : OrganismException
    {
        internal AlreadyReproducingException() : base("Can only reproduce when current reproduction is finished.")
        {
        }
    }
}