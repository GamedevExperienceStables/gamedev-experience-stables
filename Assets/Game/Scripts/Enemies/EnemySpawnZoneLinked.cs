using System.Collections.Generic;
using Game.Level;
using UnityEditor;
using UnityEngine;

namespace Game.Enemies
{
    public class EnemySpawnZoneLinked : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private List<EnemySpawnGroup> linkedGroups = new();

        private void OnValidate()
        {
            linkedGroups.Clear();
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out EnemySpawnGroup group))
                    linkedGroups.Add(group);
            }
        }

        private void OnEnable()
        {
            foreach (EnemySpawnGroup group in linkedGroups)
                group.Cleared += OnCleared;
        }

        private void OnDisable()
        {
            foreach (EnemySpawnGroup group in linkedGroups)
                group.Cleared -= OnCleared;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out ISpawnZoneTrigger _))
                return;

            ActivateNext();
        }

        private void OnCleared()
            => ActivateNext();

        private void ActivateNext()
        {
            foreach (EnemySpawnGroup group in linkedGroups)
            {
                bool cleared = !group.IsActive;
                if (cleared)
                    continue;

                group.Activate();
                break;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 offset = Vector3.up * 0.5f;
            int lastIndex = linkedGroups.Count - 1;
            for (int i = 0; i < lastIndex; i++)
            {
                int nextIndex = i + 1;

                Vector3 position = linkedGroups[i].transform.position;
                Handles.Label(position + offset, (i + 1).ToString());

                Vector3 nextPosition = linkedGroups[nextIndex].transform.position;
                if (i == lastIndex - 1)
                    Handles.Label(nextPosition + offset, (nextIndex + 1).ToString());

                Debug.DrawLine(position, nextPosition, Color.yellow);
            }
        }
    }
}