using System.Collections;
using System.Collections.Generic;

namespace Game.Level
{
    public class LocationPersistenceFact : IEnumerable<string>
    {
        private readonly HashSet<string> _items = new();

        public bool Contains(string id)
            => _items.Contains(id);

        public void AddFact(string id)
            => _items.Add(id);

        public void Clear()
            => _items.Clear();

        public IEnumerator<string> GetEnumerator() 
            => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();
    }
}