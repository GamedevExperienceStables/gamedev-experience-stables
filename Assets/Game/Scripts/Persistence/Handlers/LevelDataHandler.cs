using System.Linq;
using Game.GameFlow;
using Game.Level;
using Game.Settings;
using UnityEngine;
using VContainer;

namespace Game.Persistence
{
    public class LevelDataHandler
    {
        private readonly LevelsSettings _levelsSettings;
        private readonly GameData _gameData;
        private readonly LocationDataHandler _locationDataHandler;

        [Inject]
        public LevelDataHandler(LevelsSettings levelsSettings, GameData gameData, LocationDataHandler locationDataHandler)
        {
            _levelsSettings = levelsSettings;
            _gameData = gameData;
            _locationDataHandler = locationDataHandler;
        }

        public void Reset()
        {
            _gameData.CurrentLevel = _levelsSettings.Levels.First();

            _locationDataHandler.Init(_levelsSettings.LevelStartPoint);
        }

        public void Import(GameSaveData.LevelSaveData data)
        {
            _gameData.CurrentLevel = GetLevel(data.id);
            
            _locationDataHandler.Init(_levelsSettings.LevelStartPoint);
        }

        public GameSaveData.LevelSaveData Export()
        {
            return new GameSaveData.LevelSaveData
            {
                id = _gameData.CurrentLevel.Id,
            };
        }

        private LevelSettings GetLevel(string levelId)
        {
            var levels = _levelsSettings.Levels;

            LevelSettings found = levels.Find(level => level.Id == levelId);
            if (found)
                return found;

            Debug.LogError($"Level '{levelId}' not found. Used first level");
            return levels.First();
        }
    }
}