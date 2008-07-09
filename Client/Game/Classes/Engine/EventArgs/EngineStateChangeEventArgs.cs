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
        ///  The type of engine change.
        /// </summary>
        private EngineStateChangeType stateChange; 

        /// <summary>
        ///  The short message for the event.
        /// </summary>
        private string shortDescription;

        /// <summary>
        ///  A long description of the event.
        /// </summary>
        private string longDescription;

        /// <summary>
        ///  Creates a new event relating to a creature arriving.
        /// </summary>
        /// <param name="organismState">The creature arriving.</param>
        /// <returns>State change initialized for a teleporting creature with messages.</returns>
        public static EngineStateChangedEventArgs AnimalArrived(OrganismState organismState)
        {
            return new EngineStateChangedEventArgs
                (   EngineStateChangeType.AnimalTeleported
                , "A new " + ((Species) organismState.Species).Name + " arrived at " + DateTime.Now.TimeOfDay.ToString()
                , "A new " + ((Species) organismState.Species).Name + " was teleported into your world at " + DateTime.Now
                );
        }

        /// <summary>
        ///  Creates a new event relating to a creature being destroyed.
        /// </summary>
        /// <param name="organismState">The creature being destroyed.</param>
        /// <param name="reason">The reason the creature is being destroyed.</param>
        /// <returns>State change initialized for a creature being destroyed with messages.</returns>
        public static EngineStateChangedEventArgs AnimalDestroyed(OrganismState organismState, PopulationChangeReason reason)
        {
            string reasonDescription = "";

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

            return new EngineStateChangedEventArgs
                (   EngineStateChangeType.Other
                , "A " + ((Species) organismState.Species).Name + " was destroyed because it " + reasonDescription + "."
                , "A " + ((Species) organismState.Species).Name + " was destroyed because it " + reasonDescription + "."
                );
        }

        /// <summary>
        ///  Creates a new set of event arguments for a state change with short message.
        /// </summary>
        /// <param name="stateChange">The state change type</param>
        /// <param name="shortDescription">A short description</param>
        public EngineStateChangedEventArgs(EngineStateChangeType stateChange, string shortDescription)
        {
            this.stateChange = stateChange;
            this.shortDescription = shortDescription;
            this.longDescription = shortDescription;
        }

        /// <summary>
        ///  Creates a new set of event arguments for a state change with short and long messages.
        /// </summary>
        /// <param name="stateChange">The state change type</param>
        /// <param name="shortDescription">A short description</param>
        /// <param name="longDescription">A detailed description</param>
        public EngineStateChangedEventArgs(EngineStateChangeType stateChange, string shortDescription, string longDescription)
        {
            this.stateChange = stateChange;
            this.shortDescription = shortDescription;
            this.longDescription = longDescription;
        }

        /// <summary>
        ///  Retrieves the long description from these event arguments.
        /// </summary>
        public string LongDescription
        {
            get
            {
                return longDescription;
            }
        }

        /// <summary>
        ///  Retrieves the short description from these event arguments.
        /// </summary>
        public string ShortDescription
        {
            get
            {
                return shortDescription;
            }
        }

        /// <summary>
        ///  Retrieves the engine state change type for these event arguments.
        /// </summary>
        public EngineStateChangeType StateChange 
        {
            get 
            {
                return stateChange;
            }
        }
    }
}