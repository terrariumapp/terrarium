//------------------------------------------------------------------------------ 
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;

namespace Terrarium.Game
{
    /// <summary>
    ///  The worldvector contains:
    ///  currentState   - State of the world at a point in time
    ///  currentEvents  - The events that represent actions that were finished at this point in time
    ///  currentActions - The actions that were begun by organisms starting at this point in time
    ///                   These may be actions they took based on the events that occurred
    ///  This is called a "Vector" because it represents the current state, and the "direction" it wants to go
    ///  because it contains the actions to apply to it to get to the next state.
    /// </summary>
    [Serializable]
    public class WorldVector
    {
        private TickActions _currentActions;

        /// <summary>
        ///  Attach a new state to a world vector.
        /// </summary>
        /// <param name="state">The world state used ot init the vector.</param>
        public WorldVector(WorldState state)
        {
            if (!state.IsImmutable)
            {
                throw new ApplicationException("WorldState must be immutable to be added to vector.");
            }

            State = state;
        }

        /// <summary>
        ///  Provides access to the rolled up tick actions for the state object.
        /// </summary>
        public TickActions Actions
        {
            get { return _currentActions; }

            set
            {
                if (value == null)
                {
                    throw new ApplicationException("Actions can't be null.");
                }

                _currentActions = value;
            }
        }

        /// <summary>
        ///  Provides access to the current world state.
        /// </summary>
        public WorldState State { get; private set; }
    }
}