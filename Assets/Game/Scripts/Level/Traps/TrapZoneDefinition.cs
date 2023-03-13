using System.Collections.Generic;
using Game.Actors;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = MENU_PATH + "Zone")]
    public class TrapZoneDefinition : TrapDefinition
    {
        [SerializeField]
        private LayerMask layerMask = ~0;

        [Space]
        [SerializeField]
        private bool removeEffectsOnExit;

        [SerializeField, Expandable]
        private List<EffectDefinition> effects;

        public LayerMask LayerMask => layerMask;
        public bool RemoveEffectsOnExit => removeEffectsOnExit;
        public IEnumerable<EffectDefinition> Effects => effects;
    }
}