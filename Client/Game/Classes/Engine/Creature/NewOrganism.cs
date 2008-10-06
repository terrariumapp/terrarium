//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using OrganismBase;

namespace Terrarium.Game
{
    /// <summary>
    ///  Contains the information needed to intialize a new organism within the Terrarium.
    /// </summary>
    public class NewOrganism
    {
        /// <summary>
        ///  The DNA of the new creature if it was born.
        /// </summary>
        private readonly byte[] _dna;

        /// <summary>
        ///  The state of the new creature.  This is pre-created and pre-located.
        /// </summary>
        private readonly OrganismState _state;

        /// <summary>
        ///  This determines if the creature should be added at a random location.
        /// </summary>
        private Boolean _addAtRandomLocation = true;

        /// <summary>
        ///  Creates a NewOrganism object intialized with the creature's state, some
        ///  DNA information if available.  The state object must be immutable for
        ///  this to succeed.
        /// </summary>
        /// <param name="state">The state of the organism to be added.</param>
        /// <param name="dna">The DNA the creature gets initialized with.</param>
        public NewOrganism(OrganismState state, byte[] dna)
        {
            // Must be mutable
            Debug.Assert(!state.IsImmutable);

            _state = state;
            _dna = dna;
        }

        /// <summary>
        ///  Provides access to the state object of the new organism.
        /// </summary>
        public OrganismState State
        {
            get { return _state; }
        }

        /// <summary>
        ///  Determines if the creature should be added at a random location
        ///  or the location available in their organism state.
        /// </summary>
        public Boolean AddAtRandomLocation
        {
            get { return _addAtRandomLocation; }

            set { _addAtRandomLocation = value; }
        }

        /// <summary>
        ///  Provides access to the byte array that represents the DNA that
        ///  will be passed to the creature.  A clone is made because array
        ///  references are not read-only, and can be modified at the member
        ///  level.
        /// </summary>
        public byte[] Dna
        {
            get
            {
                if (_dna != null)
                {
                    return (byte[]) _dna.Clone();
                }

                return new byte[0]; // Empty byte array
            }
        }
    }
}