//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using OrganismBase;

namespace Terrarium.Game
{
    /// <summary>
    ///  Exception thrown when the Terrarium client realizes
    ///  it hasn't reported to the reporting server for over 48
    ///  hours.
    /// </summary>
    [Serializable]
    public class StateTimedOutException : GameEngineException
    {
        /// <summary>
        ///  Generates a default StateTimedOutException
        /// </summary>
        public StateTimedOutException() : base("Your ecosystem state timed out.")
        {
        }
    }
}