using Game.Actors;

namespace Game.Inventory
{
    public readonly struct ItemExecutionContext
    {
        public readonly IActorController target;
        public readonly IInventory inventory;

        public ItemExecutionContext(IActorController target, IInventory inventory)
        {
            this.target = target;
            this.inventory = inventory;
        }
    }
}