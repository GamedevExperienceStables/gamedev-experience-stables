using Game.Actors.Health;
using Game.Stats;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = "Damage/Single")]
    public class DamageDefinitionSingle : DamageDefinition
    {
        [SerializeField, Min(0)]
        private float baseDamage;

        [SerializeField]
        private GameObject hitFeedback;

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