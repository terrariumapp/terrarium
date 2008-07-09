//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    /// Not enough energy to perform this action.
    /// </summary>
    public class NotEnoughEnergyException : OrganismException
    {
        internal NotEnoughEnergyException() : base("Not enough energy to perform this action.")
        {
        }
    }
}