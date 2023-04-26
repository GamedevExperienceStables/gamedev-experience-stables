using System;
using System.Collections.Generic;
using Game.Actors;
using UnityEngine;

namespace Game.Level
{
    public class ZoneEffect : MonoBehaviour
    {
        [SerializeField]
        private ZoneTrigger trigger;

        public List<IActorController> InsideZone { get; } = new();

        public event Action<IActorController> ActorAdded;
        public event Action<IActorController> ActorRemoved;

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

            if (InsideZone.Contains(actor))
                return;

            ActorAdded?.Invoke(actor);
            InsideZone.Add(actor);
        }

        private void OnTriggerExited(GameObject other)
        {
            if (!other.TryGetComponent(out IActorController actor))
                return;
            
            if (!InsideZone.Contains(actor))
                return;

            ActorRemoved?.Invoke(actor);
            InsideZone.Remove(actor);
        }
    }
}