using Game.Enemies;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Managers
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private EnemyController enemyPrefab;

        [FormerlySerializedAs("interval")]
        [SerializeField]
        private float spawnInterval = 4f;

        [SerializeField]
        private MMF_Player spawnFeedback;

        private float _timeSinceLastSpawn;
        private Transform _target;
        private bool _hasTarget;

        private Transform _spawnContainer;

        public void Init(Transform spawnContainer)
        {
            _spawnContainer = spawnContainer;
        }

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
            Transform t = transform;
            EnemyController enemy = Instantiate(enemyPrefab, _spawnContainer);
            enemy.SetPositionAndRotation(t.position, t.rotation);
            enemy.SetTarget(_target);

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