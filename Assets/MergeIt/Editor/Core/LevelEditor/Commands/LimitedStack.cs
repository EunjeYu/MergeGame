// Copyright (c) 2024, Awessets

using System.Collections.Generic;
using System.Linq;

namespace MergeIt.Editor.Core.LevelEditor.Commands
{
    public class LimitedStack<T>
    {
        private readonly LinkedList<T> _list = new();
        private readonly int _maxSize;
        
        public int Count => _list.Count;

        public LimitedStack(int maxSize)
        {
            _maxSize = maxSize;
        }

        public void Push(T item)
        {
            if (_list.Count >= _maxSize)
            {
                _list.RemoveLast();
            }
            _list.AddFirst(item);
        }

        public T Pop()
        {
            if (_list.Count == 0)
                return default;

            var value = _list.First.Value;
            _list.RemoveFirst();
            return value;
        }

        public T Peek()
        {
            return _list.Count > 0 ? _list.First.Value : default;
        }

        public bool Any()
        {
            return _list.Any();
        }
        
        public void Clear()
        {
            _list.Clear();
        }
    }
}
