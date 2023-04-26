using Game.Inventory;

namespace Game.Level
{
    public class RocketContainerHandler
    {
        private readonly IInventoryItems _inventory;

        public RocketContainerHandler(IInventoryItems inventory)
            => _inventory = inventory;


        public float GetCurrentValue(MaterialDefinition definition)
            => _inventory.Materials.Container.GetCurrentValue(definition);

        public float GetTotalValue(MaterialDefinition targetMaterial)
            => _inventory.Materials.Container.GetTotalValue(targetMaterial);
    }
}