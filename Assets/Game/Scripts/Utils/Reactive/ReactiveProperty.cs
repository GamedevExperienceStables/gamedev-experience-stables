using System;
using System.Collections.Generic;

namespace Game.Utils.Reactive
{
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private event Action<T> ValueChanged;

        private T _value;

        private readonly EqualityComparer<T> _comparer = EqualityComparer<T>.Default;

        public ReactiveProperty(T value) 
            => _value = value;

        public T Value
        {
            get => _value;
            set
            {
                if (_comparer.Equals(_value, value)) 
                    return;
                
                _value = value;
                
                ValueChanged?.Invoke(_value);
            }
        }
        
        public void SetSilent(T value) 
            => _value = value;

        public void Subscribe(Action<T> action) 
            => ValueChanged += action;

        public void UnSubscribe(Action<T> action) 
            => ValueChanged -= action;
    }
}