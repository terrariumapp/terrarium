//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

namespace Terrarium.Game
{
    /// <summary>
    ///     An EngineStateChangeEvent is raised when some interesting change 
    ///     in state occurs such as an animal being born or dying or being 
    ///     teleported into your world or out. 
    /// </summary>
    public enum EngineStateChangeType
    {
        /// <summary>
        ///  The engine is giving notification of a teleportation event.
        /// </summary>
        AnimalTeleported = 0,
        /// <summary>
        ///  The engine is giving notification of a random event.
        /// </summary>
        Other = 1,
        /// <summary>
        ///  The engine is giving notification of developer information.
        /// </summary>
        DeveloperInformation = 2
    }
}