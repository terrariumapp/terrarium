//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Special object used to hold arguments passed to the
    ///   EatCompletedEventHandler delegate.  This class provides
    ///   information about whether a creature's eat action was
    ///   successful.
    ///  </para>
    /// </summary>
    [Serializable]
    public class EatCompletedEventArgs : ActionResponseEventArgs
    {
        /// <internal/>
        public EatCompletedEventArgs(int actionID, Action action, Boolean successful) : base(actionID, action)
        {
            Successful = successful;
        }

        /// <summary>
        ///  <para>
        ///   Provides information about the original EatAction and
        ///   the parameters passed into the BeginEating method.
        ///   This can be used to retrieve the state of the creature
        ///   you tried to eat.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  EatAction representing the original values passed to BeginEating.
        /// </returns>
        public EatAction EatAction
        {
            get { return (EatAction) Action; }
        }

        /// <summary>
        ///  <para>
        ///   Provides information about whether or not an eat action was
        ///   successful.  Often times if multiple creatures are eating
        ///   the state target, the target will run out of food value
        ///   before all creatures get to eat.  In this case the eat
        ///   won't be successful and the creature won't gain any
        ///   energy.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  True if your creature gained energy from eating, False otherwise.
        /// </returns>
        public bool Successful { get; private set; }

        /// <summary>
        ///  <para>
        ///   Provides a string representation of this class for debugging
        ///   purposes.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.String representing the contents of this class.
        /// </returns>
        public override string ToString()
        {
            return string.Format("#EatCompleted {{Successful={0}, {1}}}", Successful, base.ToString());
        }
    }
}