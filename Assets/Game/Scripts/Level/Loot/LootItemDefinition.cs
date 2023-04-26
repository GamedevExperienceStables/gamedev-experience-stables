using Game.Inventory;
using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "★ Loot/Loot Item")]
    public class LootItemDefinition : ScriptableObject
    {
        [SerializeField]
        private ItemDefinition item;

        [SerializeField]
        private LootItem prefab;
        
        public LootItem Prefab => prefab;
        public ItemDefinition ItemDefinition => item;
    }
}