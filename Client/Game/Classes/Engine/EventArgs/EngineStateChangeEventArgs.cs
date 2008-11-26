//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using OrganismBase;

namespace Terrarium.Game
{
    /// <summary>
    ///     An EngineStateChangeEvent is raised when some interesting change 
    ///     in state occurs such as an animal being born or dying or being 
    ///     teleported into your world or out. 
    ///     
    ///     Its main purpose is to provide some useful feedback to the UI
    ///     
    ///     This EventArgs describes that event. It contains the type of state change and 
    ///     a short description and a long description
    /// </summary>
    public sealed class EngineStateChangedEventArgs : EventArgs
    {
        /// <summary>
        ///  Creates a new set of event arguments for a state change with short message.
        /// </summary>
        /// <param name="stateChange">The state change type</param>
        /// <param name="shortDescription">A short description</param>
        public EngineStateChangedEventArgs(EngineStateChangeType stateChange, string shortDescription)
        {
            StateChange = stateChange;
            ShortDescription = shortDescription;
            LongDescription = shortDescription;
        }

        /// <summary>
        ///  Creates a new set of event arguments for a state change with short and long messages.
        /// </summary>
        /// <param name="stateChange">The state change type</param>
        /// <param name="shortDescription">A short description</param>
        /// <param name="longDescription">A detailed description</param>
        public EngineStateChangedEventArgs(EngineStateChangeType stateChange, string shortDescription,
                                           string longDescription)
        {
            StateChange = stateChange;
            ShortDescription = shortDescription;
            LongDescription = longDescription;
        }

        /// <summary>
        ///  Retrieves the long description from these event arguments.
        /// </summary>
        public string LongDescription { get; private set; }

        /// <summary>
        ///  Retrieves the short description from these event arguments.
        /// </summary>
        public string ShortDescription { get; private set; }

        /// <summary>
        ///  Retrieves the engine state change type for these event arguments.
        /// </summary>
        public EngineStateChangeType StateChange { get; private set; }

        /// <summary>
        ///  Creates a new event relating to a creature arriving.
        /// </summary>
        /// <param name="organismState">The creature arriving.</param>
        /// <returns>State change initialized for a teleporting creature with messages.</returns>
        public static EngineStateChangedEventArgs AnimalArrived(OrganismState organismState)
        {
            return new EngineStateChangedEventArgs(
                EngineStateChangeType.AnimalTeleported,
                string.Format("A new {0} arrived at {1}", ((Species) organismState.Species).Name, DateTime.Now.TimeOfDay),
                string.Format("A new {0} was teleported into your world at {1}", ((Species) organismState.Species).Name,
                              DateTime.Now)
                );
        }

        /// <summary>
        ///  Creates a new event relating to a creature being destroyed.
        /// </summary>
        /// <param name="organismState">The creature being destroyed.</param>
        /// <param name="reason">The reason the creature is being destroyed.</param>
        /// <returns>State change initialized for a creature being destroyed with messages.</returns>
        public static EngineStateChangedEventArgs AnimalDestroyed(OrganismState organismState,
                                                                  PopulationChangeReason reason)
        {
            var reasonDescription = "";

            switch (reason)
            {
                case PopulationChangeReason.Timeout:
                    reasonDescription = "thought for too long";
                    break;

                case PopulationChangeReason.Error:
                    reasonDescription = "had an error";
                    break;

                case PopulationChangeReason.SecurityViolation:
                    reasonDescription = "attempted to violate security";
                    break;

                case PopulationChangeReason.OrganismBlacklisted:
                    reasonDescription = "is blacklisted due to past bad behavior and won't be loaded";
                    break;
            }

            return new EngineStateChangedEventArgs(
                EngineStateChangeType.Other,
                string.Format("A {0} was destroyed because it {1}.", ((Species) organismState.Species).Name,
                              reasonDescription),
                string.Format("A {0} was destroyed because it {1}.", ((Species) organismState.Species).Name,
                              reasonDescription)
                );
        }
    }
}