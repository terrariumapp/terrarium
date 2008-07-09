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
        ///  The Unique ID of the organism.
        /// </summary>
        string id;

        /// <summary>
        ///  The reason the organism was killed.
        /// </summary>
        PopulationChangeReason deathReason;

        /// <summary>
        ///  Extra information about the death of the organism.
        /// </summary>
        string extraInformation = "";

        /// <summary>
        ///  Creates a new KilledOrganism based on the ID, the reason for death, and
        ///  any extra information that can be used when the organism is removed.
        /// </summary>
        /// <param name="id">The Unique ID of the organism.</param>
        /// <param name="reason">The reason the organism was killed.</param>
        /// <param name="extraInformation">Extra information about the death.</param>
        public KilledOrganism(string id, PopulationChangeReason reason, string extraInformation)
        {
            this.id = id;
            this.deathReason = reason;
            this.extraInformation = extraInformation;
        }

        /// <summary>
        ///  Creates a new KilledOrganism based on ID and reason for death.
        /// </summary>
        /// <param name="id">The Unique ID of the organism.</param>
        /// <param name="reason">The reason the organism was killed.</param>
        public KilledOrganism(string id, PopulationChangeReason reason)
        {
            this.id = id;
            this.deathReason = reason;
        }

        /// <summary>
        ///  Creates a new KilledOrganism based on information in an OrganismState.
        /// </summary>
        /// <param name="state">The state object that ID and death reason will be pulled from.</param>
        public KilledOrganism(OrganismState state)
        {
            this.id = state.ID;
            this.deathReason = state.DeathReason;
        }

        /// <summary>
        ///  Retrieves the ID of the killed organism.
        /// </summary>
        public string ID 
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        ///  Retrieves textual information about the death of the organism.
        /// </summary>
        public string ExtraInformation
        {
            get 
            {
                return extraInformation;
            }
        }

        /// <summary>
        ///  Retrieves the reason this organism was killed.
        /// </summary>
        public PopulationChangeReason DeathReason 
        {
            get 
            {
                return deathReason;
            }
        }
    }
}