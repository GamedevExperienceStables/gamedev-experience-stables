using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemies
{
    public class EnemySpawnZone : MonoBehaviour
    {
        [SerializeField]
        private List<EnemySpawnPoint> enemySpawnPoints;

        [SerializeField, Min(1f)]
        private float spawnCounts = 1f;
        [SerializeField, Min(0f)]
        private float respawnTimer = 0f;

        private float _timeSinceLastSpawn;
        private Transform _target;
        private bool _activated = true;

        private Transform _spawnContainer;

        public void Init(Transform spawnContainer)
            => _spawnContainer = spawnContainer;

        public void SetTarget(Transform target)
            => _target = target;

        public void Update()
        {
            if (_activated)
            {
                _timeSinceLastSpawn = 0f;
                return;
            }
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            bool allEnemiesDead = true;
            foreach (EnemySpawnPoint spawnPoint in enemySpawnPoints)
            {
                allEnemiesDead &= (spawnPoint.EnemiesLeft <= 0);
            }

            if (!allEnemiesDead) return;
            _timeSinceLastSpawn += Time.deltaTime;
            
            if (_timeSinceLastSpawn < respawnTimer) return;
            _timeSinceLastSpawn = 0f;
            _activated = true;
        }

        public void Activate()
        {
            if (spawnCounts <= 0) 
                return;
            if (!_activated)
                return;

            spawnCounts -= 1f;
            ActivateSpawnPoints();
            _activated = false;
        }

        private void ActivateSpawnPoints()
        {
            enemySpawnPoints.ForEach(spawnPoint =>
            {
                spawnPoint.Init(_spawnContainer);
                spawnPoint.SetTarget(_target);
            });
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            Vector3 position = transform.position;
            foreach (EnemySpawnPoint spawnZone in enemySpawnPoints)
                Gizmos.DrawLine(position, spawnZone.transform.position);

            if (!_activated) 
                return;
            if (spawnCounts <= 0) 
                return;
            Gizmos.DrawIcon(new Vector3(position.x, 2f, position.z), "warning.png", false);
        }
    }
}