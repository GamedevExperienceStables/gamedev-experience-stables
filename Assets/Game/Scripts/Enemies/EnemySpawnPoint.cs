using System;
using Game.Actors.Health;
using Game.TimeManagement;
using MoreMountains.Feedbacks;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace Game.Enemies
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        [SerializeField, Required]
        private EnemyDefinition enemy;

        [Space]
        [SerializeField, Min(1)]
        private int spawnCount = 3;

        [SerializeField, Min(1f)]
        private float spawnInterval = 5f;

        [Header("FX")]
        [SerializeField]
        private MMF_Player spawnFeedback;

        private int _spawnCount;
        private Transform _target;
        private bool _initialized;

        private Transform _spawnContainer;
        private EnemyFactory _factory;
        private TimerUpdatable _spawnTimer;

        private int _enemiesLeft;

        public bool IsCleared => _enemiesLeft <= 0;

        [Inject]
        public void Construct(EnemyFactory enemyFactory, TimerFactory timerFactory)
        {
            _factory = enemyFactory;
            _spawnTimer = timerFactory.CreateTimer(TimeSpan.FromSeconds(spawnInterval), Spawn, isLooped: true);
        }

        public void Init(Transform spawnContainer)
        {
            _spawnContainer = spawnContainer;
            _spawnCount = spawnCount;
            _enemiesLeft = spawnCount;
            _initialized = true;
        }

        public void SetTarget(Transform target)
            => _target = target;

        public void Update()
        {
            if (!_initialized)
                return;

            if (_spawnCount <= 0)
                return;

            if (_spawnCount.Equals(spawnCount))
            {
                Spawn();

                if (spawnCount <= 1)
                    return;

                _spawnTimer.Start();
            }

            _spawnTimer.Tick();
        }

        private void Spawn()
        {
            EnemyController instance = _factory.Create(enemy, transform, _target, _spawnContainer);
            var deathController = instance.GetComponent<DeathController>();
            deathController.Died += OnDied;
            _spawnCount--;

            PlaySpawnFeedback();
        }

        private void PlaySpawnFeedback()
        {
            if (spawnFeedback)
                spawnFeedback.PlayFeedbacks();
        }

        private void OnDied()
            => _enemiesLeft--;
    }
}