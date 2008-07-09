//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Terrarium.Game;

namespace Terrarium.Hosting 
{

    // AppMgr is a set of static routines used to set up the Game Scheduler
    // that does time slices for creatures as well as the security environment they
    // execute in.
    internal class AppMgr 
    { 
        // single instance of the game scheduler which runs in the untrusted app domain
        static IGameScheduler theScheduler = null;
        static IGameScheduler theLocalScheduler = null;

        // Creates the scheduler in the same appdomain as the rest of the game
        internal static IGameScheduler CreateSameDomainScheduler(GameEngine engine) 
        {
            if (theScheduler == null) 
            {
                theScheduler = new GameScheduler();
            }
            else
            {
                throw new ApplicationException("A Scheduler already exists.");
            }

            theScheduler.CurrentGameEngine = engine;

            return theScheduler;
        }

        // only call this from the organism app domain
        internal static IGameScheduler CurrentScheduler 
        {
            get
            { 
                return theLocalScheduler;
            }

            set
            {
                theLocalScheduler = value;
            }
        }

        internal static void DestroyScheduler()
        {
            if (theScheduler != null)
            {
                theScheduler.Close();
                theScheduler = null;
            }
        }

    }
}