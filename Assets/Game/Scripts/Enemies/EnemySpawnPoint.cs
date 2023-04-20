using System;
using Game.Actors.Health;
using Game.Level;
using Game.TimeManagement;
using MoreMountains.Feedbacks;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace Game.Enemies
{
    public class EnemySpawnPoint : MonoBehaviour, ICounterObject
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

        [ShowNonSerializedField]
        private int _spawnsLeft;

        [ShowNonSerializedField]
        private int _enemiesLeft;

        private Transform _target;
        private bool _isActive;

        private Transform _spawnContainer;
        private EnemyFactory _factory;
        private TimerUpdatable _spawnTimer;

        public bool IsCleared => _enemiesLeft <= 0;
        
        // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
        public int RemainingCount => _spawnsLeft;

        public bool IsDirty { get; private set; }

        [Inject]
        public void Construct(EnemyFactory enemyFactory, TimerFactory timerFactory)
        {
            _factory = enemyFactory;
            _spawnTimer = timerFactory.CreateTimer(TimeSpan.FromSeconds(spawnInterval), Spawn, isLooped: true);
        }

        public void Init(Transform spawnContainer)
        {
            _spawnContainer = spawnContainer;
            _spawnsLeft = spawnCount;
            _enemiesLeft = spawnCount;
        }
        
        public void Activate()
        {
            _isActive = true;

            IsDirty = true;
        }

        public void SetTarget(Transform target)
            => _target = target;

        public void SetCount(int count)
        {
            _spawnsLeft = Mathf.Clamp(count, 0, spawnCount);

            IsDirty = true;
        }

        public void Update()
        {
            if (!_isActive)
                return;

            if (_spawnsLeft <= 0)
                return;

            if (_spawnsLeft.Equals(spawnCount))
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

            _spawnsLeft--;

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