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
        ///  Creates a new KilledOrganism based on the ID, the reason for death, and
        ///  any extra information that can be used when the organism is removed.
        /// </summary>
        /// <param name="id">The Unique ID of the organism.</param>
        /// <param name="reason">The reason the organism was killed.</param>
        /// <param name="extraInformation">Extra information about the death.</param>
        public KilledOrganism(string id, PopulationChangeReason reason, string extraInformation)
        {
            ID = id;
            DeathReason = reason;
            ExtraInformation = extraInformation;
        }

        /// <summary>
        ///  Creates a new KilledOrganism based on ID and reason for death.
        /// </summary>
        /// <param name="id">The Unique ID of the organism.</param>
        /// <param name="reason">The reason the organism was killed.</param>
        public KilledOrganism(string id, PopulationChangeReason reason)
        {
            ExtraInformation = "";
            ID = id;
            DeathReason = reason;
        }

        /// <summary>
        ///  Creates a new KilledOrganism based on information in an OrganismState.
        /// </summary>
        /// <param name="state">The state object that ID and death reason will be pulled from.</param>
        public KilledOrganism(OrganismState state)
        {
            ExtraInformation = "";
            ID = state.ID;
            DeathReason = state.DeathReason;
        }

        /// <summary>
        ///  Retrieves the ID of the killed organism.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        ///  Retrieves textual information about the death of the organism.
        /// </summary>
        public string ExtraInformation { get; private set; }

        /// <summary>
        ///  Retrieves the reason this organism was killed.
        /// </summary>
        public PopulationChangeReason DeathReason { get; private set; }
    }
}