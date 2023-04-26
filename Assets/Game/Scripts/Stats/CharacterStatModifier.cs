using System;
using UnityEngine;

namespace Game.Stats
{
    [Serializable]
    public class CharacterStatModifier
    {
        [SerializeField]
        private CharacterStats stat;

        [SerializeField]
        private StatModifier modifier;

        public CharacterStats Stat => stat;
        public StatModifier Modifier => modifier;
    }
}