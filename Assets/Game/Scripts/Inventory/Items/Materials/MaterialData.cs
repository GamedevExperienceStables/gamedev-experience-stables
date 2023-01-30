namespace Game.Inventory
{
    public class MaterialData
    {
        public MaterialData(MaterialDefinition definition, int total, int current)
        {
            Definition = definition;
            Total = total;
            Current = current;
        }

        public MaterialDefinition Definition { get; }
        public int Total { get; }
        public int Current { get; set; }
    }
}