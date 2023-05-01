using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = MENU_PATH + "Zone")]
    public class TrapZoneDefinition : TrapDefinition
    {
        [Header("Zone")]
        [SerializeField, Min(0)]
        private int durability;

        [SerializeField, ShowIf(nameof(HasDurability))]
        private GameObject onDestroyVFX;

        [Space]
        [SerializeField, Expandable]
        private TrapZoneEffectBehaviours behaviours;

        private bool HasDurability => durability > 0;

        public int Durability => durability;
        public List<TrapZoneEffectBehaviour> Behaviours => behaviours.Behaviours;

        public GameObject OnDestroyVFX => onDestroyVFX;
    }
}