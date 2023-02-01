using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace Game.Enemies
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private EnemyDefinition enemy;

        [SerializeField]
        private float spawnCount = 3f;
        
        [SerializeField]
        private float spawnInterval = 5f;

        [SerializeField]
        private MMF_Player spawnFeedback;

        private float _timeSinceLastSpawn;
        private Transform _target;
        private bool _hasTarget;

        private Transform _spawnContainer;
        private EnemyFactory _factory;
        private List<EnemyController> _enemies;

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
            if (!_hasTarget)
            {
                return;
            }

            UpdateTimer();
        }

        private void UpdateTimer()
        {
            if (spawnCount <= 0) return;
            
            _timeSinceLastSpawn += Time.deltaTime;
            if (_timeSinceLastSpawn < spawnInterval) return;

            spawnCount -= 1f;
            _timeSinceLastSpawn = 0f;
            Spawn();
        }

        private void Spawn()
        {
            EnemyController enemyInstance = _factory.Create(enemy, transform, _target, _spawnContainer);
            _enemies.Add(enemyInstance);

            PlaySpawnFeedback();
        }

        private void PlaySpawnFeedback()
        {
            if (spawnFeedback)
            {
                spawnFeedback.PlayFeedbacks();
            }
        }
    }
}