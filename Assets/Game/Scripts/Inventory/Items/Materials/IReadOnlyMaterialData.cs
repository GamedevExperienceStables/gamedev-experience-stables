namespace Game.Inventory
{
    public interface IReadOnlyMaterialData
    {
        MaterialDefinition Definition { get; }
        int Total { get; }
        int Current { get; set; }
    }
}