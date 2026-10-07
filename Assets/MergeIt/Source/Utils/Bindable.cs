// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;

namespace MergeIt.Core.Utils
{
    public class Bindable<T>
    {
        private event Action<T, T> ChangedFromToEvent = delegate { };
        private event Action<T> ChangedToEvent = delegate { };
        
        private T _value;
        private T _holdValue;
        private bool _isHeld;
        
        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value))
                {
                    return;
                }

                if (_isHeld)
                {
                    _holdValue = _value;
                    _value = value;

                    return;
                }

                T prevValue = _value;
                
                _value = value;
                
                ChangedFromToEvent?.Invoke(prevValue, value);
                ChangedToEvent?.Invoke(value);
            }
        }

        public void Subscribe(Action<T, T> onChanged, bool immediateCheck = false)
        {
            ChangedFromToEvent += onChanged;

            if (immediateCheck)
            {
                onChanged?.Invoke(default, _value);
            }
        }

        public void Subscribe(Action<T> onChanged, bool immediateCheck = false)
        {
            ChangedToEvent += onChanged;

            if (immediateCheck)
            {
                onChanged?.Invoke(_value);
            }
        }

        public void Unsubscribe(Action<T, T> onChanged)
        {
            ChangedFromToEvent -= onChanged;
        }
        
        public void Unsubscribe(Action<T> onChanged)
        {
            ChangedToEvent -= onChanged;
        }

        public void SetValueSilently(T value)
        {
            _value = value;
        }

        public void Hold()
        {
            _isHeld = true;
        }

        public void Release()
        {
            if (!_isHeld)
            {
                return;
            }

            _isHeld = false;

            T temp = _holdValue;
            _holdValue = default;

            if (!temp.Equals(_value))
            {
                ChangedFromToEvent?.Invoke(temp, _value);
                ChangedToEvent?.Invoke(_value);
            }
        }
    }
}