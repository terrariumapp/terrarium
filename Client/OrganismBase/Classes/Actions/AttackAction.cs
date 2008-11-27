//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   This class represents a command to attack another creature initiated by the
    ///   BeginAttacking method.  This class can be used to get the target creature
    ///   for the current attack action.
    ///  </para>
    /// </summary>
    [Serializable]
    public class AttackAction : Action
    {
        /// <summary>
        ///  Creates a new attack action targeting a specific creature.
        /// </summary>
        /// <param name="organismID">Attacking organism's ID</param>
        /// <param name="actionID">Organism Unique ID for action.</param>
        /// <param name="targetAnimal">The creature to attack.</param>
        internal AttackAction(string organismID, int actionID, AnimalState targetAnimal) : base(organismID, actionID)
        {
            TargetAnimal = targetAnimal;
        }

        /// <summary>
        ///  <para>
        ///   Returns information about the Animal your creature chose to attack by
        ///   using the BeginAttacking method.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  AnimalState object for the creature you attacked using the BeginAttacking method.
        /// </returns>
        public AnimalState TargetAnimal { get; private set; }

        /// <summary>
        ///  <para>
        ///   A textual representation of the AttackAction object that can be useful in
        ///   debugging your creature's attack code.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.String for the textual representation of the AttackAction.
        /// </returns>
        public override string ToString()
        {
            return string.Format("TargetAnimal={0}", TargetAnimal.ID);
        }
    }
}