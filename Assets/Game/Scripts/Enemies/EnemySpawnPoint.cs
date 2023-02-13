using System;
using Game.Actors.Health;
using Game.TimeManagement;
using MoreMountains.Feedbacks;
using UnityEngine;
using VContainer;

namespace Game.Enemies
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private EnemyDefinition enemy;

        [SerializeField, Min(1f)]
        private float spawnCount = 3f;
        
        [SerializeField, Min(1f)]
        private float spawnInterval = 5f;

        [SerializeField]
        private MMF_Player spawnFeedback;

        private float _spawnCount;
        private Transform _target;
        private bool _hasTarget;

        private Transform _spawnContainer;
        private EnemyFactory _factory;
        private TimerUpdatable _spawnTimer;
        
        public float EnemiesLeft { get; private set; }

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
        }

        public void Awake()
            => EnemiesLeft = spawnCount;

        public void SetTarget(Transform target)
        {
            _target = target;
            _hasTarget = true;
        }

        public void Update()
        {
            if (!_hasTarget)
                return;

            if (_spawnCount <= 0)
                return;

            if (_spawnCount.Equals(spawnCount))
            {
                Spawn();
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
            => EnemiesLeft--;
    }
}