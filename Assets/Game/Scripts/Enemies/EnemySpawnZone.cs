using System;
using System.Collections.Generic;
using Game.TimeManagement;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace Game.Enemies
{
    public class EnemySpawnZone : MonoBehaviour
    {
        [FormerlySerializedAs("enemySpawnPoint")]
        [SerializeField]
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
        
        [Inject]
        public void Construct(TimerFactory factory){
            _spawnTimer = factory.CreateTimer(TimeSpan.FromSeconds(respawnTimer), OnCompleteSpawnTimer);
        }

        public void Init(Transform spawnContainer)
        {
            _spawnContainer = spawnContainer;
        }

        public void SetTarget(Transform target)
            => _target = target;

        public void Update()
        {
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
            if (spawnCounts <= 0) 
                return;
            
            if (!_canSpawn)
                return;

            spawnCounts--;
            ActivateSpawnPoints();
            _canSpawn = false;
        }

        private void RestartTimer()
            => _spawnTimer.Start();

        private bool CheckAllEnemiesDead()
        {
            bool allEnemiesDead = true;
            foreach (EnemySpawnPoint spawnPoint in enemySpawnPoints)
            {
                allEnemiesDead &= (spawnPoint.EnemiesLeft <= 0);
            }

            return allEnemiesDead;
        }


        private void OnCompleteSpawnTimer()
            => _canSpawn = true;

        private void ActivateSpawnPoints()
        {
            enemySpawnPoints.ForEach(spawnPoint =>
            {
                spawnPoint.SetTarget(_target);
                spawnPoint.Init(_spawnContainer);
            });
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
            
            if (spawnCounts <= 0) 
                return;
            
            Vector3 position = transform.position;
            Gizmos.DrawIcon(new Vector3(position.x, 2f, position.z), "fire.png", false);
        }
    }
}