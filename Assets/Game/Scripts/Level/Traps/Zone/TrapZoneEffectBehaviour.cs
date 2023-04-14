using System;
using System.Collections.Generic;
using Game.Actors;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Level
{
    [Serializable]
    public class TrapZoneEffectBehaviour
    {
        [SerializeField]
        private LayerMask layerMask = ~0;
        
        [SerializeField]
        private TrapZoneBehaviour behaviour;

        [SerializeField, Expandable]
        private List<EffectDefinition> effects;

        public IEnumerable<EffectDefinition> Effects => effects;
        public bool RemoveEffectsOnExit => false;
        public LayerMask LayerMask => layerMask;

        public TrapZoneBehaviour Behaviour => behaviour;
    }
}