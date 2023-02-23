using System.Collections.Generic;

namespace Game.Inventory
{
    public struct InventoryInitialData
    {
        public IList<MaterialInitialData> container;
        public IList<MaterialInitialData> bag;

        public IList<RuneDefinition> runes;
        public IDictionary<RuneSlotId, RuneDefinition> slots;
    }
}