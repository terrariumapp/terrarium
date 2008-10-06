//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;

namespace Terrarium.Game
{
    /// <summary>
    ///  Used to notify clients that the world vector of the
    ///  engine has been changed.
    /// </summary>
    public sealed class WorldVectorChangedEventArgs : EventArgs
    {
        /// <summary>
        ///  The new world vector
        /// </summary>
        private readonly WorldVector _newVector;

        /// <summary>
        ///  The old world vector
        /// </summary>
        private readonly WorldVector _oldVector;

        /// <summary>
        ///  Creates a new set of event arguments for when the world vector changes.
        /// </summary>
        /// <param name="oldVector">The previous world vector object.</param>
        /// <param name="newVector">The new world vector object.</param>
        public WorldVectorChangedEventArgs(WorldVector oldVector, WorldVector newVector)
        {
            _oldVector = oldVector;
            _newVector = newVector;
        }

        /// <summary>
        ///  Retrieves the old world vector object.
        /// </summary>
        public WorldVector OldVector
        {
            get { return _oldVector; }
        }

        /// <summary>
        ///  Retrieves the new world vector object.
        /// </summary>
        public WorldVector NewVector
        {
            get { return _newVector; }
        }
    }
}