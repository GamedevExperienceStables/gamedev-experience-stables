using Game.Actors.Health;
using MoreMountains.Feedbacks;
using UnityEngine;
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

        private float _timeSinceLastSpawn = 0f;
        private float _spawnCount;
        private Transform _target;
        private bool _hasTarget;

        private Transform _spawnContainer;
        private EnemyFactory _factory;
        
        public float EnemiesLeft { get; private set; }

        [Inject]
        public void Construct(EnemyFactory factory)
            => _factory = factory;

        public void Init(Transform spawnContainer)
        {
            _spawnContainer = spawnContainer;
            _spawnCount = spawnCount;
        }

        public void Awake()
        {
            EnemiesLeft = spawnCount;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            _hasTarget = true;
        }

        public void Update()
        {
            if (!_hasTarget)
                return;

            UpdateTimer();
        }

        private void UpdateTimer()
        {
            if (_spawnCount <= 0) 
                return;
            if (_timeSinceLastSpawn >= spawnInterval) 
                _timeSinceLastSpawn = 0f;
            
            if (_timeSinceLastSpawn == 0f)
            {
                _spawnCount -= 1f;
                Spawn(); 
            }
            _timeSinceLastSpawn += Time.deltaTime;
        }

        private void Spawn()
        {
            EnemyController instance = _factory.Create(enemy, transform, _target, _spawnContainer);
            var deathController = instance.GetComponent<DeathController>();
            deathController.Died += OnDied;

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