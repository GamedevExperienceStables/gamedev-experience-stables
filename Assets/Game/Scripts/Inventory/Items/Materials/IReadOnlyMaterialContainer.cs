namespace Game.Inventory
{
    public interface IReadOnlyMaterialContainer
    {
        void Subscribe(MaterialContainer.MaterialChangedEvent callback);
        void UnSubscribe(MaterialContainer.MaterialChangedEvent callback);
        
        int GetCurrentValue(MaterialDefinition definition);
        int GetTotalValue(MaterialDefinition definition);
        
        bool IsFull(MaterialDefinition definition);
        bool IsEmpty(MaterialDefinition definition);
    }
}