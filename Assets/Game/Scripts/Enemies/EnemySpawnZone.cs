using System;
using System.Collections.Generic;
using Game.Level;
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

        [SerializeField, Min(1f)]
        private float spawnCounts = 1f;

        [SerializeField, Min(0f)]
        private float respawnTimerS = 0f;

        private float _timeSinceLastSpawn;
        private Transform _target;
        private bool _hasTarget;

        private Transform _spawnContainer;

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

        private void UpdateTimer()
        {
            // Написать обработчик после зачистки зоны запустить таймер для повторного запуска зоны
        }

        public void Activate()
        {
            if (spawnCounts <= 0) return;
            //if (!_isTimerEnds) return;

            spawnCounts -= 1f;
            ActivateSpawnPoints();
        }

        private void ActivateSpawnPoints()
        {
            enemySpawnPoint.ForEach(spawnPoint =>
            {
                spawnPoint.Init(_spawnContainer);
                spawnPoint.SetTarget(_target);
            });
        }
    }
}