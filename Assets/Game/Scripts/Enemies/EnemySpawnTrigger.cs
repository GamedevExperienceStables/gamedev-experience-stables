using System.Collections.Generic;
using Game.Level;
using UnityEngine;

namespace Game.Enemies
{
    public class EnemySpawnTrigger : MonoBehaviour
    {
        [SerializeField]
        private List<EnemySpawnZone> enemySpawnZones;

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ISpawnZoneTrigger _))
            {
                foreach (var enemySpawnZone in enemySpawnZones)
                {
                    enemySpawnZone.Activate();
                }
            }
        }
    }
}