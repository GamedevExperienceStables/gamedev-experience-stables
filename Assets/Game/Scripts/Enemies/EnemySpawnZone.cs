using System.Collections.Generic;
using Game.Level;
using UnityEngine;

namespace Game.Enemies
{
    public class EnemySpawnZone : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private List<EnemySpawnGroup> enemySpawnZones = new();

        private void OnValidate()
        {
            enemySpawnZones.Clear();
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out EnemySpawnGroup enemySpawnGroup))
                    enemySpawnZones.Add(enemySpawnGroup);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out ISpawnZoneTrigger _))
                return;

            foreach (EnemySpawnGroup enemySpawnZone in enemySpawnZones)
            {
                enemySpawnZone.Activate();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            Vector3 position = transform.position;
            foreach (EnemySpawnGroup spawnZone in enemySpawnZones)
                Gizmos.DrawLine(position, spawnZone.transform.position);
        }
    }
}