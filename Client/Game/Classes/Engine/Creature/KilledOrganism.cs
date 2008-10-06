//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using OrganismBase;

namespace Terrarium.Game
{
    /// <summary>
    ///  Represents an organism that has been killed and will soon be removed
    ///  from the Terrarium game world.
    /// </summary>
    [Serializable]
    public class KilledOrganism
    {
        /// <summary>
        ///  The reason the organism was killed.
        /// </summary>
        private readonly PopulationChangeReason _deathReason;

        /// <summary>
        ///  Extra information about the death of the organism.
        /// </summary>
        private readonly string _extraInformation = "";

        /// <summary>
        ///  The Unique ID of the organism.
        /// </summary>
        private readonly string _id;

        /// <summary>
        ///  Creates a new KilledOrganism based on the ID, the reason for death, and
        ///  any extra information that can be used when the organism is removed.
        /// </summary>
        /// <param name="id">The Unique ID of the organism.</param>
        /// <param name="reason">The reason the organism was killed.</param>
        /// <param name="extraInformation">Extra information about the death.</param>
        public KilledOrganism(string id, PopulationChangeReason reason, string extraInformation)
        {
            _id = id;
            _deathReason = reason;
            _extraInformation = extraInformation;
        }

        /// <summary>
        ///  Creates a new KilledOrganism based on ID and reason for death.
        /// </summary>
        /// <param name="id">The Unique ID of the organism.</param>
        /// <param name="reason">The reason the organism was killed.</param>
        public KilledOrganism(string id, PopulationChangeReason reason)
        {
            _id = id;
            _deathReason = reason;
        }

        /// <summary>
        ///  Creates a new KilledOrganism based on information in an OrganismState.
        /// </summary>
        /// <param name="state">The state object that ID and death reason will be pulled from.</param>
        public KilledOrganism(OrganismState state)
        {
            _id = state.ID;
            _deathReason = state.DeathReason;
        }

        /// <summary>
        ///  Retrieves the ID of the killed organism.
        /// </summary>
        public string ID
        {
            get { return _id; }
        }

        /// <summary>
        ///  Retrieves textual information about the death of the organism.
        /// </summary>
        public string ExtraInformation
        {
            get { return _extraInformation; }
        }

        /// <summary>
        ///  Retrieves the reason this organism was killed.
        /// </summary>
        public PopulationChangeReason DeathReason
        {
            get { return _deathReason; }
        }
    }
}