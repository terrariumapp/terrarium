//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Describes the reason for the death of a creature.
    ///  </para>
    /// </summary>
    public enum PopulationChangeReason
    {
        /// <summary>
        ///  <para>
        ///   The creature is not dead yet.  This will be returned
        ///   by all creatures that are alive in the Terrarium and
        ///   is a place holder to represent an undead state.
        ///  </para>
        /// </summary>
        NotDead,
        /// <summary>
        ///  Represents initial population changes instituted
        ///  whenever the game state is deserialized.  These
        ///  creatures were not added to the game, but were
        ///  pre-existing.  When creating new reporting data
        ///  this has to be remembered.
        /// </summary>
        /// <internal/>
        Initial,
        /// <summary>
        ///  The creature was added to the population through
        ///  birth.
        /// </summary>
        /// <internal/>
        Born,
        /// <summary>
        ///  <para>
        ///   The creature died from old age.  This happens once
        ///   a creature has lived out its entire lifespan.
        ///  </para>
        /// </summary>
        OldAge,
        /// <summary>
        ///  The creature was removed from the engine because it
        ///  was teleported to another peer.
        /// </summary>
        /// <internal/>
        TeleportedTo,
        /// <summary>
        ///  <para>
        ///   The creature died do to starvation.  If a creature
        ///   runs completely out of energy then they are considered
        ///   starved.
        ///  </para>
        /// </summary>
        Starved,
        /// <summary>
        ///  <para>
        ///   The creature is not dead yet.  This will be returned
        ///   by all creatures that are alive in the Terrarium and
        ///   is a place holder to represent an undead state.
        ///  </para>
        /// </summary>
        Sick,
        /// <summary>
        ///  The creature was added to the engine because it was
        ///  successfully received from another peer.
        /// </summary>
        /// <internal/>
        TeleportedFrom,
        /// <summary>
        ///  <para>
        ///   The creature was killed by the attack of another
        ///   creature.
        ///  </para>
        /// </summary>
        Killed,
        /// <summary>
        ///  <para>
        ///   The creature threw an exception and was terminated.
        ///  </para>
        /// </summary>
        Error,
        /// <summary>
        ///  <para>
        ///   The creature performed an action that violated terrarium
        ///   security.
        ///  </para>
        /// </summary>
        SecurityViolation,
        /// <summary>
        ///  <para>
        ///   The creature used too much of a timeslice and was
        ///   timed out.
        ///  </para>
        /// </summary>
        Timeout,
        /// <summary>
        ///  The creature was removed from the game engine because
        ///  it was blacklisted by the hosting code due to a malicious
        ///  timeout.
        /// </summary>
        /// <internal/>
        OrganismBlacklisted
    }
}