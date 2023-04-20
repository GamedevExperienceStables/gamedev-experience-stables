namespace Game.Level
{
    public readonly struct LocationCounterData
    {
        public readonly string id;
        public readonly int count;

        public LocationCounterData(string id, int count)
        {
            this.id = id;
            this.count = count;
        }
    }
}