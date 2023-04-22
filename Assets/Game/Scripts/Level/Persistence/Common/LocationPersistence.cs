using System.Collections.Generic;

namespace Game.Level
{
    public abstract class LocationPersistence<T>
    {
        private readonly Dictionary<string, T> _items = new();

        public bool TryGetValue(string id, out T data)
            => _items.TryGetValue(id, out data);

        public void SetValue(string id, T value)
            => _items[id] = value;

        public void Clear()
            => _items.Clear();

        public Dictionary<string, T>.Enumerator GetEnumerator()
            => _items.GetEnumerator();
    }
}