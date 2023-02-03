using System.Data;
using System.Linq;
using Game.Enemies;
using UnityEngine;

namespace Game.Level
{
    public class LocationContext : MonoBehaviour
    {
        private Transform _enemiesContainer;
        private LocationPoint _spawnPoint;

        public void InitEnemySpawners(Transform target)
        {
            var enemySpawnZones = GetComponentsInChildren<EnemySpawnZone>();
            if (enemySpawnZones.Length == 0)
                return;

            _enemiesContainer = CreateContainer();

            foreach (EnemySpawnZone spawnZone in enemySpawnZones)
            {
                spawnZone.Init(_enemiesContainer);
                spawnZone.SetTarget(target);
            }
        }

        public void DestroyEnemies()
        {
            if (_enemiesContainer)
                Destroy(_enemiesContainer.gameObject);
        }

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

            Debug.LogWarning($"Not found '{locationPointKey}' point, returned first point on location");
            return points.First();
        }

        private static Transform CreateContainer()
            => new GameObject("EnemiesContainer").transform;
    }
}