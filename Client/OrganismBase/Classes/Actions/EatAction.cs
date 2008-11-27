//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   This class represents a command to eat another creature or plant and is initiated
    ///   by using the BeginEating method.  This class can be used to get the target
    ///   creature for the current eat action.
    ///  </para>
    /// </summary>
    [Serializable]
    public class EatAction : Action
    {
        /// <summary>
        ///  Create a new eat action to eat a specific organism.
        /// </summary>
        /// <param name="organismID">Eating organism's ID</param>
        /// <param name="actionID">Unique Organism ID for action.</param>
        /// <param name="targetOrganism">The state representing the organism to eat.</param>
        internal EatAction(string organismID, int actionID, OrganismState targetOrganism) : base(organismID, actionID)
        {
            TargetOrganism = targetOrganism;
        }

        /// <summary>
        ///  <para>
        ///   Returns the organism your creature chose to eat by
        ///   using the BeginEating method.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  AnimalState object for the creature you're trying to eat.
        /// </returns>
        public OrganismState TargetOrganism { get; private set; }

        /// <summary>
        ///  <para>
        ///   A textual representation of the EatAction object that can be useful in
        ///   debugging your creature's eating code.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.String for the textual representation of the EatAction.
        /// </returns>
        public override string ToString()
        {
            return string.Format("TargetOrganism={0}", TargetOrganism.ID);
        }
    }
}