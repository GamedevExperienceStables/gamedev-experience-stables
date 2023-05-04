namespace Game.Level
{
    public class LocationPointKeyData : ILocationPointKey
    {
        public LocationPointKeyData(string id) 
            => Id = id;

        public string Id { get; }
    }
}