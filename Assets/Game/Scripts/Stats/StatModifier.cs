using System;
using UnityEngine;

namespace Game.Stats
{
    [Serializable]
    public class StatModifier
    {
        [SerializeField]
        private float value;

        [SerializeField]
        private StatsModifierType type;

        public StatModifier(float value, StatsModifierType type)
        {
            this.value = value;
            this.type = type;
        }

        public float Value => value;

        public StatsModifierType Type => type;
    }
}