//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Special object used to hold arguments passed to the
    ///   AttackedEventHandler delegate.  Contains the state
    ///   of the creature attacking your creature.  This
    ///   is useful in setting up a good defense
    ///  </para>
    /// </summary>
    [Serializable]
    public class AttackedEventArgs : OrganismEventArgs
    {
        /// <summary>
        ///  Creates a new set of event arguments that can be used to notify
        ///  a creature that another creature has completed an attack action
        ///  against them.
        /// </summary>
        /// <param name="attacker">The state of the attacking creature.</param>
        /// <internal/>
        public AttackedEventArgs(AnimalState attacker)
        {
            Attacker = attacker;
        }

        /// <summary>
        ///  <para>
        ///   Provides the state of the creature attacking
        ///   your creature.  You can use this state to attack
        ///   the creature back, defend, or run away.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  AnimalState representing the state of the creature attacking you.
        /// </returns>
        public AnimalState Attacker { get; private set; }

        /// <summary>
        ///  <para>
        ///   Provides a string representation of this class for debugging
        ///   purposes.  Prints the Attacker's GUID so you can identify
        ///   the creature being attacked using the property browser.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.String representing the contents of this class.
        /// </returns>
        public override string ToString()
        {
            return string.Format("#Attacked {{Attacker = {0}}}", Attacker.ID);
        }
    }
}