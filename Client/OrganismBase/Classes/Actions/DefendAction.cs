//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   This class represents a command to defend against an attack from another
    ///   creature and is initiated by using the BeginDefending method.  This class
    ///   can be used to get the target creature for the current defend action.
    ///  </para>
    /// </summary>
    [Serializable]
    public class DefendAction : Action
    {
        /// <summary>
        ///  Creates a new defend action to defend against a particular target
        ///  creature.
        /// </summary>
        /// <param name="organismID">Defending organism's ID</param>
        /// <param name="actionID">Creature Unique ID for action.</param>
        /// <param name="targetAnimal">The state representing the creature to defend against.</param>
        internal DefendAction(string organismID, int actionID, AnimalState targetAnimal) : base(organismID, actionID)
        {
            TargetAnimal = targetAnimal;
        }

        /// <summary>
        ///  <para>
        ///   Returns the Animal your creature chose to defend against by
        ///   using the BeginDefending method.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  AnimalState object for the creature you're defending against.
        /// </returns>
        public AnimalState TargetAnimal { get; private set; }

        /// <summary>
        ///  <para>
        ///   A textual representation of the DefendAction object that can be useful in
        ///   debugging your creature's defense code.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.String for the textual representation of the DefendAction.
        /// </returns>
        public override string ToString()
        {
            return string.Format("TargetAnimal={0}", TargetAnimal.ID);
        }
    }
}