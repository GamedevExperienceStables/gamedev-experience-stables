using System;
using Game.Actors;

namespace Game.Inventory
{
    public interface IInventoryItems
    {
        bool CanAddToBag(ItemDefinition definition, IActorController owner);
        bool TryAddToBag(ItemDefinition item, IActorController owner);
        void AddToBag(ItemDefinition item, IActorController owner);
        void ClearBag();
        
        bool CanTransferToContainer(MaterialDefinition levelMaterial);
        void TransferToContainer(MaterialDefinition levelMaterial);

        IReadOnlyMaterials Materials { get; }
        
        bool IsContainerFull(MaterialDefinition levelMaterial);
        bool IsBagEmpty(MaterialDefinition levelMaterial);
        event Action<ItemDefinition> AddedToBag;
    }
}