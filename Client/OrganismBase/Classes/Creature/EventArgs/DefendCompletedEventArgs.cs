//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Special object used to hold arguments passed to the
    ///   DefendCompletedEventHandler delegate.  This class provides
    ///   information about the creature that was blocked against so
    ///   it can be used for future defensive strategies.
    ///  </para>
    /// </summary>
    [Serializable]
    public class DefendCompletedEventArgs : ActionResponseEventArgs
    {
        /// <internal/>
        public DefendCompletedEventArgs(int actionID, Action action) : base(actionID, action)
        {
        }

        /// <summary>
        ///  <para>
        ///   The DefendAction object that holds information passed to the
        ///   BeginDefending method.  This can be used to retrieve the
        ///   AnimalState of the creature you're defending against.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  DefendAction class containing the target creature you defended against.
        /// </returns>
        public DefendAction DefendAction
        {
            get { return (DefendAction) Action; }
        }

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
            return string.Format("#DefendCompleted {{{0}}}", base.ToString());
        }
    }
}