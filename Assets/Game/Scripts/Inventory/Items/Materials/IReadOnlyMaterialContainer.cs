namespace Game.Inventory
{
    public interface IReadOnlyMaterialContainer
    {
        void Subscribe(MaterialContainer.MaterialChangedEvent callback);
        void UnSubscribe(MaterialContainer.MaterialChangedEvent callback);

        IReadOnlyMaterialData GetMaterialData(MaterialDefinition definition);
        
        int GetCurrentValue(MaterialDefinition definition);
        int GetTotalValue(MaterialDefinition definition);
        
        bool IsFull(MaterialDefinition definition);
        bool IsEmpty(MaterialDefinition definition);
    }
}