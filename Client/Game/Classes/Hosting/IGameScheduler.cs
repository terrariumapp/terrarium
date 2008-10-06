//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.IO;
using OrganismBase;
using Terrarium.Game;

namespace Terrarium.Hosting
{
    /// <summary>
    ///  Used to define a game scheduler that can be used to host organisms.
    /// </summary>
    public interface IGameScheduler
    {
        /// <summary>
        ///  Provides collection based access to the organisms in the scheduler.
        /// </summary>
        ICollection Organisms { get; }

        /// <summary>
        ///  Returns the number of creatures run per tick.
        /// </summary>
        int OrganismsPerTick { get; }

        /// <summary>
        ///  Control the number of ticks per second (which is really the number of buckets
        ///  the animals should be broken into).
        /// </summary>
        int TicksPerSec { get; set; }

        /// <summary>
        ///  Maximum amount of time a creature can run.
        /// </summary>
        int Quantum { get; set; }

        /// <summary>
        ///  The maximum amount of overtime that can be accrued before
        ///  a creature is penalized.
        /// </summary>
        Int64 MaxOverage { get; set; }

        /// <summary>
        ///  The maximum amount of time that a creature can run before
        ///  it is terminated.
        /// </summary>
        Int64 MaxAllowance { get; set; }

        /// <summary>
        ///  The world state.
        /// </summary>
        WorldState CurrentState { get; set; }

        /// <summary>
        ///  Set whether creatures should be penalized for time.
        /// </summary>
        Boolean PenalizeForTime { get; set; }

        /// <summary>
        ///  The AppDomain the organisms are run in.
        /// </summary>
        AppDomain OrganismAppDomain { get; }

        /// <summary>
        ///  Set the current game engine.
        /// </summary>
        GameEngine CurrentGameEngine { set; }

        /// <summary>
        ///  Suspend blacklisting entirely.
        /// </summary>
        bool SuspendBlacklisting { get; set; }

        /// <summary>
        ///  The method that notifies the scheduler to run one set of creatures.
        /// </summary>
        void Tick();

        /// <summary>
        ///  A method for adding a creature to the scheduler.
        /// </summary>
        /// <param name="org">The organism.</param>
        /// <param name="id">The unique ID of the organism.</param>
        void Add(Organism org, string id);

        /// <summary>
        ///  A method for retrieving an organism by ID from the scheduler.
        /// </summary>
        /// <param name="id">The unique ID of the organism.</param>
        /// <returns>The organism associated with the ID.</returns>
        Organism GetOrganism(string id);

        /// <summary>
        ///  Method for serializing all organisms in the scheduler.
        /// </summary>
        /// <param name="stream">The stream to serialize to.</param>
        void SerializeOrganisms(Stream stream);

        /// <summary>
        ///  Method for deserializing all organisms in the scheduler.
        /// </summary>
        /// <param name="stream">The stream to deserialize.</param>
        void DeserializeOrganisms(Stream stream);

        /// <summary>
        ///  Finalize the deserialization of the organisms.
        /// </summary>
        void CompleteOrganismDeserialization();

        /// <summary>
        ///  Used to remove organisms from the scheduler.  Should always
        ///  be called between ticks.
        /// </summary>
        /// <param name="organismID">The ID of the organism to remove.</param>
        void Remove(string organismID);

        /// <summary>
        ///  Create a new organism and add it to the scheduler.
        /// </summary>
        /// <param name="species">The type of the species to create.</param>
        /// <param name="id">The ID for the species.</param>
        void Create(Type species, string id);

        /// <summary>
        ///  Used to gather all actions that all animals want to perform from the scheduler.
        /// </summary>
        /// <returns>The rolled up tick actions.</returns>
        TickActions GatherTickActions();

        /// <summary>
        ///  Get the timing report for an organism.
        /// </summary>
        /// <param name="organismID">The ID of the organism.</param>
        /// <returns>A timing report for the organism.</returns>
        string GetOrganismTimingReport(string organismID);

        /// <summary>
        ///  Suspend blacklisting and penalizing temporarily.
        /// </summary>
        void TemporarilySuspendBlacklisting();

        /// <summary>
        ///  Close down the game scheduler releasing any resources.
        /// </summary>
        void Close();
    }
}