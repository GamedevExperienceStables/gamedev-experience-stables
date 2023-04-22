using Game.Inventory;
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

        public void InitLevel(LevelDefinition level, ILocationPoint startPoint)
        {
            var levelLocations = new LevelLocations();
            InitLevel(level, startPoint, levelLocations);
        }

        public void InitLevel(LevelDefinition level, ILocationPoint startPoint, LevelLocations locations)
        {
            _levelData.CurrentLevel = level;

            _levelData.LastLocationPoint = default;
            _levelData.CurrentLocationPoint = startPoint;

            _levelData.Locations = locations;
        }

        public LevelDefinition GetCurrentLevel()
            => _levelData.CurrentLevel;

        public string GetCurrentLevelId()
            => _levelData.CurrentLevel.Id;

        public ILocationPoint GetCurrentLocationPoint()
            => _levelData.CurrentLocationPoint;

        public bool TryGetLastLocationPoint(out ILocationPoint location)
        {
            ILocationPoint lastLocationPoint = _levelData.LastLocationPoint;
            if (lastLocationPoint is null)
            {
                location = default;
                return false;
            }

            location = lastLocationPoint;
            return true;
        }

        public void SetLocationPoint(ILocationPointKeyOwner targetPoint)
        {
            _levelData.LastLocationPoint = _levelData.CurrentLocationPoint;

            if (targetPoint is ILocationPoint locationPoint)
            {
                _levelData.CurrentLocationPoint = locationPoint;
                return;
            }

            ILocationDefinition location = _levelData.CurrentLevel.Location;
            _levelData.CurrentLocationPoint = new LocationPointData(location, targetPoint.PointKey);
        }

        public MaterialDefinition GetCurrentLevelGoalMaterial() 
            => _levelData.CurrentLevel.Goal.Material;

        public LevelLocations GetLocations() 
            => _levelData.Locations;

        public LocationData GetOrCreateLocationData(ILocationDefinition locationDefinition)
        {
            if (!_levelData.Locations.TryGetLocation(locationDefinition, out LocationData location))
                location = _levelData.Locations.CreateLocation(locationDefinition);
            
            return location;
        }
    }
}