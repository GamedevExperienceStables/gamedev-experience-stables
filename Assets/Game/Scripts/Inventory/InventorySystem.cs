using UnityEngine;
using VContainer;

namespace Game.Inventory
{
    public class InventorySystem
    {
        private InventoryData _data;

        [Inject]
        public InventorySystem()
        {
        }

        public bool CanAddItem(ItemDefinition item)
        {
            return true;
        }

        public bool TryAddItem(ItemDefinition item)
        {
            if (!CanAddItem(item))
                return false;

            Debug.Log($"Picked: {item.name}");

            return true;
        }
    }
}