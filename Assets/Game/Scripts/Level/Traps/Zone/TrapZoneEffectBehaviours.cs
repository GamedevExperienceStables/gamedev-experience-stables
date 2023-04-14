using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "Trap/Zone Effects")]
    public class TrapZoneEffectBehaviours : ScriptableObject
    {
        [SerializeField]
        private List<TrapZoneEffectBehaviour> behaviours;

        public List<TrapZoneEffectBehaviour> Behaviours => behaviours;
    }
}