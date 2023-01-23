namespace Game.Level
{
    public struct LocationPointData
    {
        public readonly LocationDefinition location;
        public readonly LocationPointKey pointKey;

        public LocationPointData(LocationDefinition location, LocationPointKey pointKey)
        {
            this.location = location;
            this.pointKey = pointKey;
        }

        public static implicit operator LocationPointData(LocationPointStaticDefinition definition)
            => new(definition.Location, definition.PointKey);
    }

    public static class LocationPointDataExtensions
    {
        public static bool IsValid(this LocationPointData data)
            => data.location && data.pointKey;
    }
}