using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace Game.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private EnemyDefinition enemy;

        [FormerlySerializedAs("interval")]
        [SerializeField]
        private float spawnInterval = 4f;

        [SerializeField]
        private MMF_Player spawnFeedback;

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
            if (!_hasTarget)
            {
                return;
            }

            UpdateTimer();
        }

        private void UpdateTimer()
        {
            _timeSinceLastSpawn += Time.deltaTime;
            if (_timeSinceLastSpawn < spawnInterval)
            {
                return;
            }

            _timeSinceLastSpawn = 0f;
            Spawn();
        }

        private void Spawn()
        {
            _factory.Create(enemy, transform, _target, _spawnContainer);

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