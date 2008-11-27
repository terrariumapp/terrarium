//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------
using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   This class represents a movement command initiated by the BeginMoving method.
    ///   This class can be used to determine the final location and speed that your
    ///   creature is heading during multi-tick movements.
    ///  </para>
    /// </summary>
    [Serializable]
    public class MoveToAction : Action
    {
        /// <summary>
        ///  Creates a new movement action for an organism.
        /// </summary>
        /// <param name="organismID">The ID of the organism the action will work on.</param>
        /// <param name="actionID">The incremental ID representing this action.</param>
        /// <param name="moveTo">The movement vector that defines this action.</param>
        internal MoveToAction(string organismID, int actionID, MovementVector moveTo) : base(organismID, actionID)
        {
            MovementVector = moveTo;
        }

        /// <summary>
        ///  <para>
        ///   A MovementVector object representing the current speed and destination
        ///   that was passed to the BeginMoving method.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  MovementVector object representing your creature's destination and speed.
        /// </returns>
        public MovementVector MovementVector { get; private set; }

        /// <summary>
        ///  <para>
        ///   A textual representation of the MoveToAction object that can be useful in
        ///   debugging your creature's movement code.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.String for the textual representation of the MoveToAction.
        /// </returns>
        public override string ToString()
        {
            return MovementVector.ToString();
        }
    }
}