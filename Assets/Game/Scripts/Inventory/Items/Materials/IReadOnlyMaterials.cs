namespace Game.Inventory
{
    public interface IReadOnlyMaterials
    {
        IReadOnlyMaterialContainer Container { get; }
        IReadOnlyMaterialContainer Bag { get; }
    }
}