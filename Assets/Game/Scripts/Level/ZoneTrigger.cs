using System;
using MoreMountains.Tools;
using UnityEngine;

namespace Game.Level
{
    public class ZoneTrigger : MonoBehaviour
    {
        [SerializeField]
        private LayerMask layers;

        public event Action<GameObject> TriggerEntered;
        public event Action<GameObject> TriggerExited;

        private void OnTriggerEnter(Collider other)
        {
            if (layers.MMContains(other.gameObject.layer))
            {
                TriggerEntered?.Invoke(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (layers.MMContains(other.gameObject.layer))
            {
                TriggerExited?.Invoke(other.gameObject);
            }
        }
    }
}