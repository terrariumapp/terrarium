//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Collections;

namespace OrganismBase
{
    /// <internal/>
    [Serializable]
    public class AttackedEventArgsCollection : ReadOnlyCollectionBase
    {
        internal AttackedEventArgsCollection()
        {
        }

        internal Boolean IsImmutable { get; private set; }

        /// <summary>
        ///  Default indexed property to get at collection items
        ///  by index.
        /// </summary>
        public AttackedEventArgs this[int index]
        {
            get { return (AttackedEventArgs) InnerList[index]; }
        }

        internal void MakeImmutable()
        {
            IsImmutable = true;
        }

        /// <summary>
        ///  Adds a new AttackEventArgs to the collection for a creature.
        ///  When the creature executes the next tick, each of these will
        ///  create an Attacked event that can be used to determine which
        ///  of the one or more creatures attack in the previous tick.
        /// </summary>
        /// <param name="attackedEventArgs">Represents the creature doing the attacking</param>
        public void Add(AttackedEventArgs attackedEventArgs)
        {
            if (IsImmutable)
            {
                throw new ApplicationException("Object is immutable.");
            }

            InnerList.Add(attackedEventArgs);
        }
    }
}