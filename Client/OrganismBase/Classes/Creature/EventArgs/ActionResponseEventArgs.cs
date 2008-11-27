//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  Base class used to define other events that are in response to an action
    /// </summary>
    /// <internal/>
    [Serializable]
    public abstract class ActionResponseEventArgs : OrganismEventArgs
    {
        /// <summary>
        ///  Creates a new set of EventArgs that can be used in events
        ///  to notify creatures of a completed action.
        /// </summary>
        /// <param name="actionID">The ID of the action that was completed.</param>
        /// <param name="action">The action that was completed.</param>
        protected ActionResponseEventArgs(int actionID, Action action)
        {
            ActionID = actionID;
            Action = action;
        }

        /// <summary>
        ///  Retrives the ID of the action.
        /// </summary>
        public int ActionID { get; private set; }

        /// <summary>
        ///  Retrieves the Acton that was completed.
        /// </summary>
        public Action Action { get; private set; }

        /// <summary>
        ///  Returns a string representation of these event arguments
        ///  by calling the ToString method on the action field.
        /// </summary>
        /// <returns>The results of the ToString method on the action field</returns>
        public override string ToString()
        {
            return Action.ToString();
        }
    }
}