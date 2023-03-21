using System;
using System.Collections.Generic;

namespace Game.UI
{
    public class LocationMarkers
    {
        private readonly List<ILocationMarker> _items = new();

        public event Action<ILocationMarker> Added;
        public event Action<ILocationMarker> Removed;

        public void Add(ILocationMarker marker)
        {
            if (_items.Contains(marker))
                return;

            _items.Add(marker);
            Added?.Invoke(marker);
        }

        public void Remove(ILocationMarker marker)
        {
            if (!_items.Contains(marker))
                return;

            _items.Remove(marker);
            Removed?.Invoke(marker);
        }
    }
}