//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections;

namespace Terrarium.Hosting
{
    [Serializable]
    internal class SimpleSequentialEnumerator : IEnumerator, ICloneable
    {
        private readonly ArrayList _list;
        private Object _currentElement;
        private int _index;

        internal SimpleSequentialEnumerator(ArrayList list)
        {
            _list = list;
            _index = -1;
        }

        #region ICloneable Members

        public Object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        #region IEnumerator Members

        public virtual bool MoveNext()
        {
            if (_index < (_list.Count - 1))
            {
                _index++;
                _currentElement = _list[_index];
                return true;
            }

            _index = _list.Count;

            return false;
        }

        public virtual object Current
        {
            get
            {
                if (_index == -1)
                {
                    throw new InvalidOperationException("Enumeration not started");
                }

                if (_index >= _list.Count)
                {
                    throw new InvalidOperationException("Enumeration ended");
                }

                return _currentElement;
            }
        }

        public virtual void Reset()
        {
            _currentElement = null;
            _index = -1;
        }

        #endregion
    }
}