using JetBrains.Annotations;

namespace Game.Inventory
{
    [UsedImplicitly]
    public class InventoryData
    {
        public Materials Materials { get; } = new();
        public Recipes Recipes { get; } = new();
        public Runes Runes { get; } = new();
    }
}