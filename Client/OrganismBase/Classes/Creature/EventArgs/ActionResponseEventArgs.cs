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
        ///  The ID of the action.  This is creature dependent and will
        ///  be sequential each time a new action is generated.
        /// </summary>
        int actionID;
        /// <summary>
        ///  The actual action that was performed.
        /// </summary>
        Action action;

        /// <summary>
        ///  Creates a new set of EventArgs that can be used in events
        ///  to notify creatures of a completed action.
        /// </summary>
        /// <param name="actionID">The ID of the action that was completed.</param>
        /// <param name="action">The action that was completed.</param>
        protected ActionResponseEventArgs(int actionID, Action action)
        {
            this.actionID = actionID;
            this.action = action;
        }

        /// <summary>
        ///  Retrives the ID of the action.
        /// </summary>
        public int ActionID 
        {
            get 
            {
                return actionID;
            }
        }
    
        /// <summary>
        ///  Retrieves the Acton that was completed.
        /// </summary>
        public Action Action
        {
            get
            {
                return action;
            }
        }

        /// <summary>
        ///  Returns a string representation of these event arguments
        ///  by calling the ToString method on the action field.
        /// </summary>
        /// <returns>The results of the ToString method on the action field</returns>
        public override string ToString()
        {
            return action.ToString();
        }
    }
}