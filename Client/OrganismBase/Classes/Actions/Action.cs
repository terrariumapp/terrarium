//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   This class defines common properties for all actions that a creature
    ///   can perform.  These actions include movement, reproduction,
    ///   eating, attacking, etc...
    ///  </para>
    /// </summary>
    [Serializable]
    public abstract class Action
    {
        /// <summary>
        ///  Creates a new action given an organism's ID and the
        ///  next action ID.  Note that action ID's shouldn't be
        ///  reused.
        /// </summary>
        /// <param name="organismID">ID of the owning organism.</param>
        /// <param name="actionID">Organism Unique ID for this action.</param>
        internal Action(string organismID, int actionID)
        {
            OrganismID = organismID;
            ActionID = actionID;
        }

        /// <summary>
        ///  <para>
        ///   The ID of the creature requesting the action.  This will always be the
        ///   GUID/ID for your creature.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.String of the GUID/ID for the organism requesting the action.
        /// </returns>
        public string OrganismID { get; private set; }

        /// <summary>
        ///  <para>
        ///   A number that uniquely identifies this action for the game engine.
        ///   This can be used to profile how many actions your creature is taking
        ///   during its lifetime.  Or it can be used to write a chronology of
        ///   events for debugging purposes.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the incremental ID for this action.
        /// </returns>
        public int ActionID { get; private set; }
    }
}