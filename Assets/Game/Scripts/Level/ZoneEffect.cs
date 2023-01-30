using System.Collections.Generic;
using Game.Actors;
using UnityEngine;

namespace Game.Level
{
    public class ZoneEffect : MonoBehaviour
    {
        [SerializeField]
        private ZoneTrigger trigger;

        private readonly List<IActorController> _insideZone = new();

        private void OnEnable()
        {
            trigger.TriggerEntered += OnTriggerEntered;
            trigger.TriggerExited += OnTriggerExited;
        }

        private void OnDisable()
        {
            trigger.TriggerEntered -= OnTriggerEntered;
            trigger.TriggerExited -= OnTriggerExited;
        }

        private void OnTriggerEntered(GameObject other)
        {
            if (!other.TryGetComponent(out IActorController actor))
                return;

            if (_insideZone.Contains(actor))
                return;
            
            _insideZone.Add(actor);
        }

        private void OnTriggerExited(GameObject other)
        {
            if (!other.TryGetComponent(out IActorController actor))
                return;
            
            if (!_insideZone.Contains(actor))
                return;

            _insideZone.Remove(actor);
        }
    }
}