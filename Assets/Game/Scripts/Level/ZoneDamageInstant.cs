using Game.Actors;
using Game.Actors.Health;
using Game.Stats;
using UnityEngine;

namespace Game.Level
{
    [RequireComponent(typeof(ZoneTrigger))]
    public class ZoneDamageInstant : MonoBehaviour
    {
        [SerializeField, Min(1)]
        private float damage;

        private ZoneTrigger _trigger;

        private void Awake() 
            => _trigger = GetComponent<ZoneTrigger>();

        private void OnEnable() 
            => _trigger.TriggerEntered += OnTriggerEntered;

        private void OnDisable() 
            => _trigger.TriggerEntered -= OnTriggerEntered;

        private void OnTriggerEntered(GameObject other)
        {
            if (!other.TryGetComponent(out IActorController actor))
                return;

            if (actor.TryGetComponent(out DamageableController damageable)) 
                damageable.Damage(damage);
        }
    }
}