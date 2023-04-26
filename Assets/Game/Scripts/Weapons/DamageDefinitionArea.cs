using System.Collections.Generic;
using Game.Actors;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = "Damage/Area")]
    public class DamageDefinitionArea : DamageDefinition
    {
        [SerializeField, Min(0)]
        private float baseDamage;

        [SerializeField, Min(0f)]
        private float radius = 1f;

        [SerializeField]
        private GameObject hitFeedback;

        [SerializeField]
        private LayerMask layerMask = ~0;

        [SerializeField, Expandable]
        private List<EffectDefinition> effects;

        public float Radius => radius;
        public float Damage => baseDamage;
        public GameObject HitFeedback => hitFeedback;
        public LayerMask Layers => layerMask;
        public IEnumerable<EffectDefinition> Effects => effects;
    }
}