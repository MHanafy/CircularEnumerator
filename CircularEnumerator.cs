using System;
using System.Collections;
using System.Collections.Generic;

namespace CircularEnumerator
{
    public class CircularEnumerator<T> : IEnumerator<T>
    {
        private readonly int _startIndex;
        private readonly int _endIndex;
        private readonly int _lastIndex;
        private readonly IList<T> _list;

        public int CurrentIndex { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="startIndex"></param>
        /// <param name="count">Number of items to enumerate, when not provided iterates all items in the list starting from provided startIndex</param>
        public CircularEnumerator(IList<T> list, int startIndex = 0, int? count = null)
        {
            _lastIndex = list.Count - 1;
            _list = list;
            _startIndex = startIndex;
            if (count.HasValue)
            {
                _endIndex = _startIndex + count.Value -1;
                if (_endIndex > _lastIndex) _endIndex -= _lastIndex + 1;
            }
            else
            {
                _endIndex = _startIndex - 1;
                if (_endIndex < 0) _endIndex = _lastIndex;
            }
            CurrentIndex = _startIndex;
            if(startIndex > _lastIndex) throw new ArgumentException("Invalid value for startIndex");
            if(count.HasValue && count.Value > list.Count) throw new ArgumentException("Invalid value for count, must be a maximum of list.Count");
        }
        public bool MoveNext()
        {
            CurrentIndex++;
            //Normal case, no start over
            if (_startIndex <= _endIndex)
            {
                //No need to verify last index because it's verified in the constructor
                if (CurrentIndex > _endIndex) CurrentIndex = _startIndex;
            }
            //Start over case, i.e. start from 9 to the end of the list, then ends at index 2
            else
            {
                if (CurrentIndex > _lastIndex) CurrentIndex = 0;
                if (CurrentIndex > _endIndex && CurrentIndex < _startIndex) CurrentIndex = _startIndex;
            }

            //Always return true because this is a circular enumerator
            return true;
        }

        public void Reset()
        {
            CurrentIndex = _startIndex;
        }

        public T Current => _list[CurrentIndex];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
           //Nothing to dispose
        }
    }
}