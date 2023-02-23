using Game.Actors;

namespace Game.Inventory
{
    public readonly struct ItemExecutionContext
    {
        public readonly IActorController target;
        public readonly IInventoryItems inventory;

        public ItemExecutionContext(IActorController target, IInventoryItems inventory)
        {
            this.target = target;
            this.inventory = inventory;
        }
    }
}