using System;
using System.Collections.Generic;
using Game.Level;
using Game.TimeManagement;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace Game.Enemies
{
    public class EnemySpawnGroup : MonoBehaviour, ISwitchObject
    {
        [SerializeField, HideInInspector]
        private List<EnemySpawnPoint> enemySpawnPoints = new();

        [SerializeField]
        private bool canRespawn;

        [SerializeField, Min(0.1f), ShowIf(nameof(canRespawn))]
        private float respawnTimer = 1f;

        [SerializeField, Min(1), ShowIf(nameof(canRespawn))]
        private int spawnCounts = 1;

        [ShowNonSerializedField]
        private bool _isCleared;

        private Transform _spawnContainer;
        private Transform _target;

        private bool _canSpawn = true;
        private int _spawnsLeft;

        private TimerPool _timers;
        private TimerUpdatable _spawnTimer;
        
        public event Action Cleared;


        private bool CanRespawn => canRespawn && respawnTimer > 0f && spawnCounts > 1 && _spawnsLeft == 0;

        public bool IsDirty { get; set; }

        public bool IsActive
        {
            get => !_isCleared;
            set => _isCleared = !value;
        }

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
        public void Construct(TimerPool timers)
        {
            _timers = timers;
            _spawnTimer = _timers.GetTimer(TimeSpan.FromSeconds(respawnTimer), OnCompleteSpawnTimer);
        }

        private void Start()
            => SubscribeOnSpawnPoints();

        private void OnDestroy()
        {
            _timers?.ReleaseTimer(_spawnTimer);

            UnsubscribeOnSpawnPoints();
        }

        private void SubscribeOnSpawnPoints()
        {
            foreach (EnemySpawnPoint spawnPoint in enemySpawnPoints)
                spawnPoint.Cleared += OnCleared;
        }

        private void UnsubscribeOnSpawnPoints()
        {
            foreach (EnemySpawnPoint spawnPoint in enemySpawnPoints)
                spawnPoint.Cleared -= OnCleared;
        }

        public void Init(Transform spawnContainer, Transform target)
        {
            _spawnContainer = spawnContainer;
            _target = target;
            _spawnsLeft = spawnCounts;

            InitSpawnPoints();
        }

        private void OnCleared()
        {
            if (!CheckAllEnemiesDead())
                return;

            if (CanRespawn)
            {
                RestartTimer();
                return;
            }

            _isCleared = true;
            
            Cleared?.Invoke();
        }

        public void Activate()
        {
            if (!isActiveAndEnabled)
                return;

            if (_isCleared)
                return;

            if (!_canSpawn)
                return;

            _spawnsLeft--;
            ActivateSpawnPoints();
            _canSpawn = false;

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

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            Vector3 position = transform.position;
            foreach (EnemySpawnPoint spawnZone in enemySpawnPoints)
                Gizmos.DrawLine(position, spawnZone.transform.position);
        }

        private void OnDrawGizmos()
        {
            if (_isCleared)
            {
                DrawIcon("CompleteIcon.png");
                return;
            }

            if (!_canSpawn)
                return;

            if (_spawnsLeft <= 0)
                return;

            DrawIcon("fire.png");
        }

        private void DrawIcon(string icon)
        {
            Vector3 position = transform.position;
            Gizmos.DrawIcon(new Vector3(position.x, 2f, position.z), icon, true);
        }
#endif
    }
}