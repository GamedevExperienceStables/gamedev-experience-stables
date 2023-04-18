using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "★ Loot/Bag")]
    public class LootBagDefinition : ScriptableObject
    {
        [SerializeField]
        private List<LootTableDefinitionItem> tables;

        [SerializeField]
        private List<LootDefinitionItem> items;

        public List<LootDefinitionItem> Items => items;

        public List<LootTableDefinitionItem> Tables => tables;
    }
}