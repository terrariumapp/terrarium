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
        Boolean isImmutable = false;

        internal AttackedEventArgsCollection()
        {
        }

        internal void MakeImmutable()
        {
            isImmutable = true;
        }
    
        internal Boolean IsImmutable
        {
            get
            {
                return isImmutable;
            }
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
            if (isImmutable)
            {
                throw new ApplicationException("Object is immutable.");
            }

            InnerList.Add(attackedEventArgs);
        }

        /// <summary>
        ///  Default indexed property to get at collection items
        ///  by index.
        /// </summary>
        public AttackedEventArgs this[int index]
        {
            get
            {
                return (AttackedEventArgs) InnerList[index];
            }
        }
    }
}