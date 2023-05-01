using Game.Inventory;
using Game.Utils.DataTable;
using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "★ Loot/Loot Item")]
    public class LootItemDefinition : DataTableItemDefinition
    {
        [SerializeField]
        private ItemDefinition item;

        [SerializeField]
        private LootItem prefab;

        [Header("FX")]
        [SerializeField]
        private GameObject pickupFeedback;

        public LootItem Prefab => prefab;
        public ItemDefinition ItemDefinition => item;
        public GameObject PickupFeedback => pickupFeedback;
    }
}