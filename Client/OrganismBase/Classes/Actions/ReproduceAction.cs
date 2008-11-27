//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   This class represents a command to reproduce and is initiated
    ///   by using the BeginReproducing method.  This class can be used to get the byte
    ///   array representing the DNA or data that will be sent to your creature's
    ///   offspring.
    ///  </para>
    /// </summary>
    [Serializable]
    public class ReproduceAction : Action
    {
        /// <summary>
        ///  A byte array representing the information that will be passed
        ///  from the parent to the child.
        /// </summary>
        private readonly byte[] _dna;

        /// <summary>
        ///  Creates a new ReproduceAction that defines information passed to
        ///  a child once a creature has given birth.  This object is held for
        ///  a period of time as the creature incubates before actually being used
        ///  to generate the events for the parent and child creatures.
        /// </summary>
        /// <param name="organismID">The unique ID of the parent creature.</param>
        /// <param name="actionID">The incremental ID representing this action.</param>
        /// <param name="dna">An array of bytes that is going to be passed to the child.</param>
        internal ReproduceAction(string organismID, int actionID, byte[] dna) : base(organismID, actionID)
        {
            _dna = dna;
        }

        /// <summary>
        ///  <para>
        ///   Returns the DNA information that you passed into the BeginReproduction
        ///   method.  This is only a copy of the original DNA since an actual pointer
        ///   to the array could allow the modification of individual elements.  This
        ///   is a small safety measure for using a non read-only type such as an array.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  System.Array of type Byte[] representing the DNA passed in the BeginReproduction method.
        /// </returns>
        public byte[] Dna
        {
            get
            {
                if (_dna != null)
                    return (byte[]) _dna.Clone();
                return null;
            }
        }
    }
}