namespace Game.Level
{
    public class LocationData
    {
        public LocationData(ILocationDefinition locationDefinition) 
            => Definition = locationDefinition;

        public ILocationDefinition Definition { get; private set; }
        
        public LocationPersistenceInt Counters { get; } = new();
        public LocationPersistenceLoot Loot { get; } = new();
    }
}