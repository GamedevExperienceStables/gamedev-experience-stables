namespace Game.Inventory
{
    public class MaterialData
    {
        private readonly MaterialDefinition _material;
        private readonly int _total;

        private int _current;

        public MaterialData(MaterialDefinition material, int total, int current)
        {
            _material = material;
            _current = current;
            _total = total;
        }
    }
}