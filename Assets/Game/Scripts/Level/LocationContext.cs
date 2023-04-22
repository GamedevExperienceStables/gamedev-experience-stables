using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace Game.Level
{
    public class LocationContext : MonoBehaviour
    {
        public IList<T> FindAll<T>()
            => GetComponentsInChildren<T>();


        public ILocationBounds FindBounds()
            => GetComponentInChildren<ILocationBounds>();


        public LocationPoint FindLocationPoint(ILocationPointKey locationPointKey)
        {
            var points = GetComponentsInChildren<LocationPoint>();
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
    }
}