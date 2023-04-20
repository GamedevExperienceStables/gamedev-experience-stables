using System;
using System.Collections.Generic;
using Game.Level;
using Game.TimeManagement;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace Game.Enemies
{
    public class EnemySpawnGroup : MonoBehaviour, ICounterObject
    {
        [SerializeField, HideInInspector]
        private List<EnemySpawnPoint> enemySpawnPoints;

        [SerializeField, Min(1)]
        private int spawnCounts = 1;

        [SerializeField, Min(0f)]
        private float respawnTimer = 0f;

        private float _timeSinceLastSpawn;
        private Transform _target;
        private bool _canSpawn = true;

        private Transform _spawnContainer;
        private TimerUpdatable _spawnTimer;
        private bool _initialized;

        [ShowNonSerializedField]
        private int _spawnsLeft;

        // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
        public int RemainingCount => _spawnsLeft;
        
        public bool IsDirty { get; private set; }

        private void OnValidate()
        {
            enemySpawnPoints.Clear();
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out EnemySpawnPoint enemySpawnPoint))
                    enemySpawnPoints.Add(enemySpawnPoint);
            }
        }

        [Inject]
        public void Construct(TimerFactory factory)
        {
            _spawnTimer = factory.CreateTimer(TimeSpan.FromSeconds(respawnTimer), OnCompleteSpawnTimer);
        }

        public void Init(Transform spawnContainer, Transform target)
        {
            _spawnContainer = spawnContainer;
            _target = target;
            _spawnsLeft = spawnCounts;

            _initialized = true;
            
            InitSpawnPoints();
        }

        public void Update()
        {
            if (!_initialized)
                return;

            if (_canSpawn)
            {
                _spawnTimer.Stop();
                return;
            }

            _spawnTimer.Tick();
            if (!CheckAllEnemiesDead())
                RestartTimer();
        }

        public void Activate()
        {
            if (_spawnsLeft <= 0)
                return;

            if (!_canSpawn)
                return;

            _spawnsLeft--;
            ActivateSpawnPoints();
            _canSpawn = false;
            
            IsDirty = true;
        }
        
        public void SetCount(int count)
        {
            _spawnsLeft = Mathf.Clamp(count, 0, spawnCounts);
            
            IsDirty = true;
        }

        private void RestartTimer()
            => _spawnTimer.Start();

        private bool CheckAllEnemiesDead()
        {
            bool allEnemiesDead = true;
            foreach (EnemySpawnPoint spawnPoint in enemySpawnPoints)
            {
                allEnemiesDead &= spawnPoint.IsCleared;
            }

            return allEnemiesDead;
        }


        private void OnCompleteSpawnTimer()
            => _canSpawn = true;

        private void InitSpawnPoints()
        {
            enemySpawnPoints.ForEach(spawnPoint =>
            {
                spawnPoint.SetTarget(_target);
                spawnPoint.Init(_spawnContainer);
            });
        }
        
        private void ActivateSpawnPoints()
        {
            foreach (EnemySpawnPoint enemySpawnPoint in enemySpawnPoints) 
                enemySpawnPoint.Activate();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            Vector3 position = transform.position;
            foreach (EnemySpawnPoint spawnZone in enemySpawnPoints)
                Gizmos.DrawLine(position, spawnZone.transform.position);
        }

        private void OnDrawGizmos()
        {
            if (!_canSpawn)
                return;

            if (_spawnsLeft <= 0)
                return;

            Vector3 position = transform.position;
            Gizmos.DrawIcon(new Vector3(position.x, 2f, position.z), "fire.png", false);
        }
    }
}