using Game.Inventory;

namespace Game.Level
{
    public class LootItem : Pickable
    {
        public ItemDefinition Definition { get; private set; }

        public void Init(ItemDefinition definition) 
            => Definition = definition;
    }
}