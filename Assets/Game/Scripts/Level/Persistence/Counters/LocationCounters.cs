using System.Collections;
using System.Collections.Generic;

namespace Game.Level
{
    public class LocationCounters : IEnumerable<LocationCounterData>
    {
        private readonly Dictionary<string, LocationCounterData> _items = new();

        public bool TryGetCounter(string id, out LocationCounterData data)
            => _items.TryGetValue(id, out data);

        public void SetCounter(string id, int count)
            => _items[id] = new LocationCounterData(id, count);


        public IEnumerator<LocationCounterData> GetEnumerator() 
            => _items.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();
    }
}