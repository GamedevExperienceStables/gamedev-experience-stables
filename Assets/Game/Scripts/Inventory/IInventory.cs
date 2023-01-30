using Game.Actors;

namespace Game.Inventory
{
    public interface IInventory
    {
        bool CanAddToBag(ItemDefinition definition, IActorController owner);
        bool TryAddToBag(ItemDefinition item, IActorController owner);
        void AddToBag(ItemDefinition item, IActorController owner);
        
        IReadOnlyMaterials Materials { get; }
        IReadOnlyRecipes Recipes { get; }
        IReadOnlyRunes Runes { get; }
    }
}