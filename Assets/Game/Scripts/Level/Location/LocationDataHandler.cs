using Game.GameFlow;
using VContainer;

namespace Game.Level
{
    public class LocationDataHandler
    {
        private readonly GameData _gameData;
        private readonly LocationData _locationData;

        [Inject]
        public LocationDataHandler(GameData gameData, LocationData locationData)
        {
            _gameData = gameData;
            _locationData = locationData;
        }

        public void Init(LocationPointData location)
        {
            _locationData.LastLocation = default;
            _locationData.CurrentLocation = location;
        }

        public bool CanTransferTo(LocationPointDefinition targetLocation)
            => _locationData.allowExit;

        public void SetLocation(LocationPointDefinition targetPoint)
        {
            _locationData.LastLocation = _locationData.CurrentLocation;

            LocationDefinition location = GetLocationDefinition(targetPoint);
            
            var newLocation = new LocationPointData(location, targetPoint.PointKey);
            _locationData.CurrentLocation = newLocation;
        }

        private LocationDefinition GetLocationDefinition(LocationPointDefinition definition) =>
            definition switch
            {
                LocationPointDynamicDefinition => _gameData.CurrentLevel.Location,
                LocationPointStaticDefinition staticLocation => staticLocation.Location,
                _ => null
            };
    }
}