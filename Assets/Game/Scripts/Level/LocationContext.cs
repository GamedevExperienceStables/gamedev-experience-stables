using System.Collections.Generic;
using System.Data;
using System.Linq;
using Game.Enemies;
using UnityEngine;

namespace Game.Level
{
    public class LocationContext : MonoBehaviour
    {
        public void InitEnemySpawners(Transform target)
        {
            var enemySpawnZones = GetComponentsInChildren<EnemySpawnGroup>();
            if (enemySpawnZones.Length == 0)
                return;

            Transform spawnContainer = CreateContainer();
            foreach (EnemySpawnGroup spawnZone in enemySpawnZones)
                spawnZone.Init(spawnContainer, target);
        }


        public IEnumerable<ILocationCounter> FindCounters()
            => GetComponentsInChildren<ILocationCounter>();


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

        private static Transform CreateContainer()
            => new GameObject("EnemiesContainer").transform;
    }
}