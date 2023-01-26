using System.Linq;
using Game.Level;
using Game.Settings;
using UnityEngine;
using VContainer;

namespace Game.Persistence
{
    public class LevelDataHandler
    {
        private readonly LevelsSettings _levelsSettings;
        private readonly LevelData _levelData;

        [Inject]
        public LevelDataHandler(
            LevelsSettings levelsSettings,
            LevelData levelData
        )
        {
            _levelsSettings = levelsSettings;
            _levelData = levelData;
        }

        public void InitLevel(LevelDefinition level)
        {
            _levelData.CurrentLevel = level;

            _levelData.LastLocation = default;
            _levelData.CurrentLocation = _levelsSettings.LevelStartPoint;
        }

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

        public void Reset()
        {
            LevelDefinition firstLevel = _levelsSettings.Levels.First();
            InitLevel(firstLevel);
        }

        private LocationDefinition GetLocationDefinition(LocationPointDefinition definition) =>
            definition switch
            {
                LocationPointDynamicDefinition => _levelData.CurrentLevel.Location,
                LocationPointStaticDefinition staticLocation => staticLocation.Location,
                _ => null
            };

        public LevelDefinition FindLevelById(string levelId)
        {
            var levels = _levelsSettings.Levels;

            LevelDefinition found = levels.Find(level => level.Id == levelId);
            if (found)
                return found;

            Debug.LogError($"Level '{levelId}' not found. Used first level");
            return levels.First();
        }
    }
}