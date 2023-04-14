using Game.Actors;
using Game.Actors.Health;
using UnityEngine;

namespace Game.Level
{
    [RequireComponent(typeof(ZoneTrigger))]
    public class ZoneDeath : MonoBehaviour
    {
        private ZoneTrigger _trigger;

        private void Awake() 
            => _trigger = GetComponent<ZoneTrigger>();

        private void OnEnable() 
            => _trigger.TriggerEntered += OnTriggerEntered;

        private void OnDisable() 
            => _trigger.TriggerEntered -= OnTriggerEntered;

        private static void OnTriggerEntered(GameObject other)
        {
            if (!other.TryGetComponent(out IActorController actor))
                return;

            if (actor.TryGetComponent(out DeathController death)) 
                death.Kill(DeathCause.PermanentDeath);
        }
    }
}