//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections;

namespace Terrarium.Hosting 
{
    [Serializable] 
    class SimpleSequentialEnumerator : IEnumerator, ICloneable
    {
        private ArrayList _list;
        private int index;
        private Object currentElement;

        internal SimpleSequentialEnumerator(ArrayList list)
        {
            _list = list;
            index = -1;
        }

        public Object Clone()
        {
            return MemberwiseClone();
        }

        public virtual bool MoveNext()
        {
            if (index < (_list.Count-1))
            {
                index++;
                currentElement = _list[index];
                return true;
            }
            else
            {
                index = _list.Count;
            }
        
            return false;

        }

        public virtual object Current
        {
            get
            {
                if (index == -1)
                {
                    throw new InvalidOperationException("Enumeration not started");
                }
            
                if (index >= _list.Count)
                {
                    throw new InvalidOperationException("Enumeration ended");
                }

                return currentElement;
            }
        }

        public virtual void Reset()
        {
            currentElement = null;
            index = -1;
        }
    }
}