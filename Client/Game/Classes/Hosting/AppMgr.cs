//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using Terrarium.Game;

namespace Terrarium.Hosting
{
    /// <summary>
    /// AppMgr is a set of static routines used to set up the Game Scheduler
    /// that does time slices for creatures as well as the security environment they
    /// execute in.
    /// </summary>
    internal class AppMgr
    {
        private static IGameScheduler _theScheduler;

        /// <summary>
        /// only call this from the organism app domain
        /// </summary>
        internal static IGameScheduler CurrentScheduler { get; set; }

        /// <summary>
        /// Creates the scheduler in the same appdomain as the rest of the game
        /// </summary>
        /// <param name="engine"></param>
        /// <returns></returns>
        internal static IGameScheduler CreateSameDomainScheduler(GameEngine engine)
        {
            if (_theScheduler == null)
            {
                _theScheduler = new GameScheduler();
            }
            else
            {
                throw new ApplicationException("A Scheduler already exists.");
            }

            _theScheduler.CurrentGameEngine = engine;

            return _theScheduler;
        }

        internal static void DestroyScheduler()
        {
            if (_theScheduler == null) return;
            _theScheduler.Close();
            _theScheduler = null;
        }
    }
}