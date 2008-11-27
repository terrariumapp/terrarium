//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------
using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Special object used to hold arguments passed to the
    ///   MoveCompletedEventHandler delegate.  This class provides
    ///   information about why the creature stopped, a MoveToAction
    ///   describing the movement that took place or was to take place,
    ///   and the state of the organism blocking the creatures path if
    ///   the reason for stopping was being blocked.
    ///  </para>
    /// </summary>
    [Serializable]
    public class MoveCompletedEventArgs : ActionResponseEventArgs
    {
        /// <internal/>
        public MoveCompletedEventArgs(int actionID, Action action, ReasonForStop reason,
                                      OrganismState blockingOrganism) : base(actionID, action)
        {
            Reason = reason;
            BlockingOrganism = blockingOrganism;
        }

        /// <summary>
        ///  <para>
        ///   Provides the original MoveToAction created as a result of the
        ///   BeginMoving function.  This can be used to get the MovementVector
        ///   which can be reused if the creature has not yet reached its
        ///   destination.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  MoveToAction describing the movement that was passed to BeginMoving.
        /// </returns>
        public MoveToAction MoveToAction
        {
            get { return (MoveToAction) Action; }
        }

        /// <summary>
        ///  <para>
        ///   Provides the reason for a creature being stopped.  This can either
        ///   be that the creature reached it's destination or was somehow blocked
        ///   by another creature.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  ReasonForStop enumeration presenting the reason for stopping a movement.
        /// </returns>
        public ReasonForStop Reason { get; private set; }

        /// <summary>
        ///  <para>
        ///   Provides the OrganismState of the blocking creature if one exists.  This
        ///   can be useful when writing event based movement algorithms and to find
        ///   camouflaged creatures.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  OrganismState of the blocking organism if Reason is equal to ReasonForStop.Blocked.
        /// </returns>
        public OrganismState BlockingOrganism { get; private set; }

        /// <summary>
        ///  <para>
        ///   Provides a string representation of this class for debugging
        ///   purposes.  Presents the reason for stopping in string form.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.String representing the contents of this class.
        /// </returns>
        public override string ToString()
        {
            return string.Format("#MoveCompleted {{Reason={0}, {1}}}", Reason, base.ToString());
        }
    }
}