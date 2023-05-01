using System.Collections.Generic;
using Game.Actors;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = "Damage/Single")]
    public class DamageDefinitionSingle : DamageDefinition
    {
        [SerializeField, Min(0)]
        private float baseDamage;
        
        [SerializeField]
        private float pushForce;

        [SerializeField]
        private GameObject hitFeedback;

        [SerializeField, Expandable]
        private List<EffectDefinition> effects;

        public GameObject HitFeedback => hitFeedback;
        public float Damage => baseDamage;

        public IEnumerable<EffectDefinition> Effects => effects;
        public float PushForce => pushForce;
    }
}