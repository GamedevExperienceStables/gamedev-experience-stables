﻿using Game.Actors.Health;
using Game.Stats;
using Game.Utils;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = "Damage/Area")]
    public class DamageDefinitionArea : DamageDefinition
    {
        private const float DEBUG_DURATION = 0.5f;

        [SerializeField, Min(0)]
        private float baseDamage;

        [SerializeField, Min(0f)]
        private float radius = 1f;

        [SerializeField]
        private GameObject hitFeedback;

        public override bool TryDealDamage(Transform source, Transform target, Vector3 hitPoint)
        {
#if UNITY_EDITOR
            DebugExtensions.DebugWireSphere(hitPoint, radius: radius, duration: DEBUG_DURATION);
#endif
            var results = Physics.OverlapSphere(hitPoint, radius, ~0, QueryTriggerInteraction.Ignore);
            foreach (Collider collider in results)
            {
                Vector3 closestPoint = collider.ClosestPointOnBounds(source.position);
                if (base.TryDealDamage(source, collider.transform, closestPoint))
                {
#if UNITY_EDITOR
                    DebugExtensions.DebugPoint(closestPoint, Color.red, duration: DEBUG_DURATION);
#endif
                }
            }

            return false;
        }

        protected override void OnDamage(Transform source, DamageableController damageable, Vector3 hitPoint)
        {
            PlayHitFeedback(source);
            ApplyDamage(damageable);
        }

        private void PlayHitFeedback(Transform source)
        {
            if (hitFeedback)
                Instantiate(hitFeedback, source.position, source.rotation);
        }

        private void ApplyDamage(DamageableController damageable)
            => damageable.Damage(new StatModifier(-baseDamage, StatsModifierType.Flat));
    }
}