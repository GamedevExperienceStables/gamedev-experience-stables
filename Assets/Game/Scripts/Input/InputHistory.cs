using System;
using System.Collections.Generic;

namespace Game.Input
{
    public class InputHistory<T>
    {
        private readonly LinkedList<T> _list = new();

        private readonly int _maxLength;

        public InputHistory(int maxLength, T initialState)
        {
            _maxLength = maxLength;

            Push(initialState);
        }

        public void Push(T item)
        {
            Clamp();
            _list.AddFirst(item);
        }

        public void Replace(T item)
        {
            _list.RemoveFirst();
            _list.AddFirst(item);
        }

        public void Back(int depth)
        {
            depth = Math.Clamp(depth, 0, _list.Count);
            for (int i = 0; i < depth; i++)
                _list.RemoveFirst();
        }

        private void Clamp()
        {
            if (_list.Count >= _maxLength)
                _list.RemoveLast();
        }

        public T Current
            => _list.First.Value;
    }
}