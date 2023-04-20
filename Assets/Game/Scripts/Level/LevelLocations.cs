using System.Collections;
using System.Collections.Generic;

namespace Game.Level
{
    public class LevelLocations : IEnumerable<LocationData>
    {
        private readonly Dictionary<ILocationDefinition, LocationData> _locations = new();

        public LocationData CreateLocation(ILocationDefinition locationDefinition)
        {
            var locationData = new LocationData(locationDefinition);
            _locations[locationDefinition] = locationData;

            return locationData;
        }

        public bool TryGetLocation(ILocationDefinition locationDefinition, out LocationData locationData)
            => _locations.TryGetValue(locationDefinition, out locationData);

        public IEnumerator<LocationData> GetEnumerator()
            => _locations.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}