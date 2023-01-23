using System;

namespace Game.Utils.Reactive
{
    public interface IReadOnlyReactiveProperty<out T>
    {
        T Value { get; }
        void Subscribe(Action<T> action);
        void UnSubscribe(Action<T> action);
    }

    public interface IReactiveProperty<T> : IReadOnlyReactiveProperty<T>
    {
        new T Value { get; set; }

        void SetSilent(T value);
    }
}