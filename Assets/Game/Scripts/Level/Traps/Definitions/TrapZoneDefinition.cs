using System;
using System.Collections.Generic;
using Game.Actors;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

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
        
        [FormerlySerializedAs("layerMask")]
        [Header("Deprecated (Will removed in next update)")]
        [SerializeField, Obsolete]
        private LayerMask layerMaskDeprecated = ~0;

        [FormerlySerializedAs("removeEffectsOnExit")]
        [SerializeField, Obsolete]
        private bool removeEffectsOnExitDeprecated;

        [FormerlySerializedAs("effects")]
        [SerializeField, Obsolete]
        private List<EffectDefinition> effectsDeprecated;

        private bool HasDurability => durability > 0;

        public int Durability => durability;
        public List<TrapZoneEffectBehaviour> Behaviours => behaviours.Behaviours;

        public GameObject OnDestroyVFX => onDestroyVFX;
    }
}