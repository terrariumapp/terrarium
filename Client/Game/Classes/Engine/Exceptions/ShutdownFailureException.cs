//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using System.Reflection;
using OrganismBase;
using Terrarium.Configuration;

namespace Terrarium.Game
{
    /// <summary>
    ///  An exception used to indicate that a failure has occured that warrants
    ///  a Terrarium client shutdown.  This exception is implemented by the
    ///  engine class through the use of a special failure notification variable.
    /// </summary>
    [Serializable]
    public class ShutdownFailureException : GameEngineException
    {
        /// <summary>
        ///  Generates a default ShutdownFailureException
        /// </summary>
        public ShutdownFailureException()
            : base(string.Format(
                       "Terrarium Version Problem\n\nThere is a problem with your version of Terrarium.  Please see the 'Notes to Users of Old Versions' on {0} for more information on what to do to fix your version [{1}].",
                       GameConfig.WebRoot,
                       Assembly.GetExecutingAssembly().GetName().Version))
        {
        }
    }
}