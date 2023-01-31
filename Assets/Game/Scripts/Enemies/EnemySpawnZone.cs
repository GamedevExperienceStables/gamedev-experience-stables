using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace Game.Enemies
{
    public class EnemySpawnZone : MonoBehaviour
    {
        [SerializeField]
        private List<EnemySpawnPoint> enemySpawnPoint;

        [SerializeField]
        private float respawnCounts = 1f;
        
        [SerializeField]
        private float respawnTimer = 0f;

        private float _timeSinceLastSpawn;
        private Transform _target;
        private bool _hasTarget;

        private Transform _spawnContainer;
        private EnemyFactory _factory;

        [Inject]
        public void Construct(EnemyFactory factory)
            => _factory = factory;

        public void Init(Transform spawnContainer)
            => _spawnContainer = spawnContainer;

        public void SetTarget(Transform target)
        {
            _target = target;
            _hasTarget = true;
        }

        public void Update()
        {
            UpdateTimer();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.transform == _target)
            {
                if (respawnCounts <= 0) return;

                respawnCounts -= 1f;
                ActivateSpawnPoints();
            }
        }

        private void UpdateTimer()
        {
            /*if (respawnCounts <= 0) return;

            respawnCounts -= 1f;
            ActivateSpawnPoints();*/
        }

        private void ActivateSpawnPoints()
        {
            enemySpawnPoint.ForEach(spawnPoint =>
            {
                spawnPoint.Init(_target);
                spawnPoint.SetTarget(_target);
            });
        }
    }
}