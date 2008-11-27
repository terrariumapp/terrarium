//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    /// Not used anymore, but left for compatibility.
    /// </summary>
    public class IsDeadException : OrganismException
    {
        internal IsDeadException() : base("You can only defend against alive animals.")
        {
        }
    }
}