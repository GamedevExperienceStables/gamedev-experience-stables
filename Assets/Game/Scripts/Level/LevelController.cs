using Game.Settings;
using VContainer;

namespace Game.Level
{
    public class LevelController
    {
        private readonly LevelData _levelData;

        [Inject]
        public LevelController(LevelData levelData) 
            => _levelData = levelData;

        public void InitLevel(LevelDefinition level, LocationPointStaticDefinition startPoint)
        {
            _levelData.CurrentLevel = level;

            _levelData.LastLocation = default;
            _levelData.CurrentLocation = startPoint;
        }

        public LevelDefinition GetCurrentLevel() 
            => _levelData.CurrentLevel;

        public string GetCurrentLevelId()
            => _levelData.CurrentLevel.Id;

        public LocationPointData GetCurrentLocation()
            => _levelData.CurrentLocation;

        public bool TryGetLastLocation(out LocationPointData location)
        {
            LocationPointData lastLocation = _levelData.LastLocation;
            if (!lastLocation.IsValid())
            {
                location = default;
                return false;
            }

            location = lastLocation;
            return true;
        }

        public void SetLocation(LocationPointDefinition targetPoint)
        {
            _levelData.LastLocation = _levelData.CurrentLocation;

            LocationDefinition location = GetLocationDefinition(targetPoint);

            var newLocation = new LocationPointData(location, targetPoint.PointKey);
            _levelData.CurrentLocation = newLocation;
        }

        private LocationDefinition GetLocationDefinition(LocationPointDefinition definition) =>
            definition switch
            {
                LocationPointDynamicDefinition => _levelData.CurrentLevel.Location,
                LocationPointStaticDefinition staticLocation => staticLocation.Location,
                _ => null
            };
    }
}