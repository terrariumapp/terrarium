//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    /// An animal must be dead to eat it.
    /// </summary>
    public class NotDeadException : OrganismException
    {
        internal NotDeadException() : base("The animal must be dead to eat it.")
        {
        }
    }
}