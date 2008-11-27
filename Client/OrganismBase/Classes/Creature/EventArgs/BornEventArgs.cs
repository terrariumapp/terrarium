//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Special object used to hold arguments passed to the
    ///   BornEventHandler delegate.  Currently only a byte[]
    ///   containing special initialization values for the
    ///   new born creature is available.
    ///  </para>
    /// </summary>
    [Serializable]
    public class BornEventArgs : OrganismEventArgs
    {
        /// <internal/>
        public BornEventArgs(byte[] dna)
        {
            Dna = dna;
        }

        /// <summary>
        ///  <para>
        ///   Provides a method for a child to retrieve Dna in the form of
        ///   a byte[] from their parents.  This property may be null if
        ///   the parent chose not to pass any Dna to the child.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  a System.Byte[] of special data usable by a child creature.
        /// </returns>
        public byte[] Dna { get; private set; }

        /// <summary>
        ///  <para>
        ///   Used to get string information about this event args for
        ///   debugging purposes.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  A System.String containing the value '#Born'
        /// </returns>
        public override string ToString()
        {
            return "#Born";
        }
    }
}