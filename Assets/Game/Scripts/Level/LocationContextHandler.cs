using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace Game.Level
{
    public class LocationContextHandler : ILocationContext
    {
        private LocationContext _context;

        public bool Initialized { get; private set; }

        public ILocationDefinition Location { get; private set; }
        public ILocationBounds LocationBounds { get; private set; }
        
        public void Init(LocationContext context, ILocationDefinition locationDefinition)
        {
            _context = context;
            
            Location = locationDefinition;
            LocationBounds = FindBounds();

            Initialized = true;
        }

        public void Clear()
        {
            _context = null;
            
            Location = null;
            LocationBounds = null;
            
            Initialized = false;
        }

        public void AddChild(GameObject go)
            => go.transform.SetParent(_context.transform);

        public IList<T> FindAll<T>()
            => _context.GetComponentsInChildren<T>();

        public LocationPoint FindLocationPoint(ILocationPointKey locationPointKey)
        {
            var points = _context.GetComponentsInChildren<LocationPoint>();
            if (points.Length == 0)
                throw new NoNullAllowedException("Not found any spawn points");

            foreach (LocationPoint point in points)
            {
                if (point.PointKey == locationPointKey)
                    return point;
            }

            Debug.LogWarning($"Not found '{locationPointKey}' spawn point, will be used first on location");
            return points.First();
        }
        
        private ILocationBounds FindBounds()
            => _context.GetComponentInChildren<ILocationBounds>();
    }
}