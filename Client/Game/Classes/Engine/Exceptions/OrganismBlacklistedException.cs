//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;

namespace Terrarium.Game
{
    /// <summary>
    ///  Exception is thrown whenever an organism needs to be blacklisted.
    /// </summary>
    [Serializable]
    public sealed class OrganismBlacklistedException : Exception
    {
    }
}