//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using OrganismBase;

namespace Terrarium.Game
{
    /// <summary>
    ///  An exception used whenever the game state has
    ///  become corrupted.  This is generally as a result
    ///  of feedback from the reporting web service.
    /// </summary>
    [Serializable]
    public sealed class StateCorruptedException : GameEngineException
    {
        /// <summary>
        ///  The last tick reported to the reporting server.
        /// </summary>
        private readonly int _lastReportedTick;

        /// <summary>
        ///  Creates a new corruption exception using the last reported tick
        ///  information.
        /// </summary>
        /// <param name="lastReportedTick">The game tick number for the last reported tick.</param>
        public StateCorruptedException(int lastReportedTick) : base("Your Ecosystem state is corrupted, restarting.")
        {
            _lastReportedTick = lastReportedTick;
        }

        /// <summary>
        ///  Provides access to the tick ID of the last reported tick.
        /// </summary>
        public int LastReportedTick
        {
            get { return _lastReportedTick; }
        }
    }
}