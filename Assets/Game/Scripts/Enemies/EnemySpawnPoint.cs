using System;
using Game.Actors.Health;
using Game.Level;
using Game.TimeManagement;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace Game.Enemies
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        [SerializeField, Required, Expandable]
        private EnemyDefinition enemy;

        [Space]
        [SerializeField, Min(1)]
        private int spawnCount = 3;

        [SerializeField, Min(1f), ShowIf(nameof(CanRespawn))]
        private float spawnInterval = 5f;

        [Header("FX")]
        [SerializeField]
        private GameObject spawnFeedbackPrefab;

        [ShowNonSerializedField]
        private int _spawnsLeft;

        [ShowNonSerializedField]
        private int _enemiesLeft;

        private bool _isActive;
        private Transform _target;

        private Transform _spawnContainer;
        private EnemyFactory _factory;
        private TimerUpdatable _spawnTimer;
        private TimerPool _timers;
        
        private GameObject _spawnFeedback;
        private SpawnPool _spawnPool;

        private bool CanRespawn => spawnCount > 1;

        public bool IsCleared { get; private set; }
        public event Action Cleared;

        [Inject]
        public void Construct(EnemyFactory enemyFactory, TimerPool timers, SpawnPool spawnPool)
        {
            _factory = enemyFactory;
            _spawnPool = spawnPool;

            _timers = timers;
            _spawnTimer = _timers.GetTimer(TimeSpan.FromSeconds(spawnInterval), Spawn, isLooped: true);
        }

        private void OnDestroy()
            => _timers?.ReleaseTimer(_spawnTimer);

        public void Init(Transform spawnContainer)
            => _spawnContainer = spawnContainer;

        public void Activate()
        {
            if (!isActiveAndEnabled)
                return;

            if (_isActive && !IsCleared)
            {
                Debug.LogWarning($"Trying to activate a non-cleared spawn point {name}", gameObject);
                return;
            }

            _isActive = true;

            _spawnsLeft = spawnCount;
            _enemiesLeft = spawnCount;

            if (CanRespawn)
                _spawnTimer.Start();

            Spawn();
        }

        public void SetTarget(Transform target)
            => _target = target;

        private void Spawn()
        {
            _spawnsLeft--;

            SpawnEnemy();
            PlaySpawnFeedback();

            if (_spawnsLeft <= 0)
                _spawnTimer.Stop();
        }

        private void SpawnEnemy()
        {
            EnemyController instance = _factory.Create(enemy, transform, _target, _spawnContainer);
            var deathController = instance.GetComponent<DeathController>();
            deathController.Died += OnDied;
        }

        private void PlaySpawnFeedback()
        {
            if (spawnFeedbackPrefab)
                _spawnPool.Spawn(spawnFeedbackPrefab, transform.position, Quaternion.identity);
        }

        private void OnDied()
        {
            _enemiesLeft--;

            if (_enemiesLeft > 0)
                return;

            IsCleared = true;
            Cleared?.Invoke();
        }
    }
}