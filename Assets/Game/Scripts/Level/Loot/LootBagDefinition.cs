using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "Data/Loot/Bag")]
    public class LootBagDefinition : ScriptableObject
    {
        [SerializeField]
        private List<LootDefinitionItem> items;

        public List<LootDefinitionItem> Items => items;
    }
}