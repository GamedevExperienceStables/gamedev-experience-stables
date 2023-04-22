using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    public class LocationContextHandler : ILocationContext
    {
        private LocationContext _context;

        public bool Initialized { get; private set; }

        public ILocationBounds LocationBounds { get; private set; }

        public IList<T> FindAll<T>()
            => _context.FindAll<T>();

        public LocationPoint FindLocationPoint(ILocationPointKey locationPoint)
            => _context.FindLocationPoint(locationPoint);

        public void Init(LocationContext context)
        {
            _context = context;
            LocationBounds = _context.FindBounds();

            Initialized = true;
        }

        public void Clear()
        {
            _context = null;

            Initialized = false;
        }

        public void AddChild(GameObject go)
            => go.transform.SetParent(_context.transform);
    }
}