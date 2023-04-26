using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    public class TrapSpawnZone : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private List<TrapSpawnPoint> traps = new();

        private bool _isActive;

        private void OnValidate()
        {
            traps.Clear();
            
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out TrapSpawnPoint trap))
                    traps.Add(trap);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (_isActive)
                return;

            if (!other.TryGetComponent(out ISpawnZoneTrigger _))
                return;
            
            _isActive = true;

            foreach (TrapSpawnPoint trap in traps)
                trap.Activate();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            Vector3 position = transform.position;
            foreach (TrapSpawnPoint spawnZone in traps)
                Gizmos.DrawLine(position, spawnZone.transform.position);
        }
    }
}