using System;
using UnityEngine;

namespace Game.Level
{
    [Serializable]
    public class LootDefinitionItem
    {
        [SerializeField]
        private LootItemDefinition item;

        [SerializeField, Min(1)]
        private int count = 1;

        public LootItemDefinition Definition => item;
        public int Count => count;
    }
}