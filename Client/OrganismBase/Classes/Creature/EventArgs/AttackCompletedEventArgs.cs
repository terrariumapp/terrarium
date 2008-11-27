//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Special object used to hold arguments passed to the
    ///   AttackCompletedEventHandler delegate.  This class provides
    ///   information about the results of the attack including
    ///   damage inflicted, whether the creature was killed, or whether
    ///   the creature escaped.
    ///  </para>
    /// </summary>
    [Serializable]
    public class AttackCompletedEventArgs : ActionResponseEventArgs
    {
        /// <summary>
        ///  Initializes a new set of event arguments that can be used
        ///  in an event handler to notify the creature that an attack
        ///  has been completed.
        /// </summary>
        /// <param name="actionID">The ID of the action.</param>
        /// <param name="action">The action being completed.</param>
        /// <param name="killed">Whether the target was killed.</param>
        /// <param name="escaped">Whether the target escaped.</param>
        /// <param name="inflictedDamage">How much damage was inflicted.</param>
        /// <internal />
        public AttackCompletedEventArgs(int actionID, Action action, Boolean killed, Boolean escaped,
                                        int inflictedDamage) : base(actionID, action)
        {
            InflictedDamage = inflictedDamage;
            Escaped = escaped;
            Killed = killed;
        }

        /// <summary>
        ///  <para>
        ///   Provides the AttackAction that was created from the values
        ///   passed into the BeginAttack method.  This can be used to
        ///   retrieve a state object for your target creature to continue
        ///   your attack.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  AttackAction representing the values passed into BeginAttack
        /// </returns>
        public AttackAction AttackAction
        {
            get { return (AttackAction) Action; }
        }

        /// <summary>
        ///  <para>
        ///   Returns the amount of damage done to the target creature as
        ///   a result of the attack.  This number should be compared against
        ///   the amount of damage the creature can withstand to help compute
        ///   how difficult the creature will be to kill.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Int32 representing the amount of absolute damage inflicted.
        /// </returns>
        public int InflictedDamage { get; private set; }

        /// <summary>
        ///  <para>
        ///   Provides information about the status of the target creature.
        ///   If the creature was killed by your attack then Killed will
        ///   bet set to True.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  True if the target creature was killed, False otherwise.
        /// </returns>
        public bool Killed { get; private set; }

        /// <summary>
        ///  <para>
        ///   Provides information about the status of the target creature.
        ///   If the creature escaped your attack then Escaped will be
        ///   set to True.  Creatures can escape by moving out of distance
        ///   of an attack.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  True if the target creature escaped, False otherwise.
        /// </returns>
        public bool Escaped { get; private set; }

        /// <summary>
        ///  <para>
        ///   Provides a string representation of this class for debugging
        ///   purposes.  Gives a count of damage inflicted, and whether
        ///   or not the creature was killed or escaped.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.String representing the contents of this class.
        /// </returns>
        public override string ToString()
        {
            return string.Format("#AttackCompleted {{InflictedDamage={0}, Killed={1}, Escaped={2}, {3}}}",
                                 InflictedDamage, Killed, Escaped, base.ToString());
        }
    }
}