namespace Game.Inventory
{
    public readonly struct MaterialChangedData
    {
        public readonly MaterialDefinition definition;
        public readonly int oldValue;
        public readonly int newValue;

        public MaterialChangedData(MaterialDefinition definition, int oldValue, int newValue)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.definition = definition;
        }
    }
}