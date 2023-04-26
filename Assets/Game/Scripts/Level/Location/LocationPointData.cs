namespace Game.Level
{
    public class LocationPointData : ILocationPoint
    {
        public LocationPointData(ILocationDefinition location, ILocationPointKey pointKey)
        {
            Location = location;
            PointKey = pointKey;
        }

        public ILocationDefinition Location { get; }
        public ILocationPointKey PointKey { get; }
    }
}