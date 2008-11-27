//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Special object used to hold arguments passed to the
    ///   ReproduceCompletedEventHandler delegate.  This class
    ///   contains information about the results of reproduction.
    ///  </para>
    /// </summary>
    [Serializable]
    public class ReproduceCompletedEventArgs : ActionResponseEventArgs
    {
        /// <internal/>
        public ReproduceCompletedEventArgs(int actionID, Action action) : base(actionID, action)
        {
        }

        /// <summary>
        ///  <para>
        ///   Provides information about the original ReproduceAction and
        ///   the parameters passed into the BeginReproduction method.
        ///   This can be used to retrieve the Dna byte array that was
        ///   passed to your creature's child.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  ReproduceAction representing the original values passed to BeginReproduction.
        /// </returns>
        public ReproduceAction ReproduceAction
        {
            get { return (ReproduceAction) Action; }
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
            return string.Format("#ReproduceCompleted {{{0}}}", base.ToString());
        }
    }
}