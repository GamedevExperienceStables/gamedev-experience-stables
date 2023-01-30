using System.Linq;
using Game.Level;
using Game.Settings;
using UnityEngine;
using VContainer;

namespace Game.Persistence
{
    public class LevelImportExport
    {
        private readonly LevelController _level;
        private readonly LevelsSettings _settings;
        private readonly LocationPointStaticDefinition _startPoint;

        [Inject]
        public LevelImportExport(LevelController level, LevelsSettings settings)
        {
            _level = level;
            _settings = settings;
            _startPoint = _settings.LevelStartPoint;
        }

        public void Reset()
        {
            LevelDefinition firstLevel = _settings.Levels.First();
            _level.InitLevel(firstLevel, _startPoint);
        }

        public void Import(GameSaveData.LevelSaveData data)
        {
            LevelDefinition currentLevel = FindLevelById(data.id);
            _level.InitLevel(currentLevel, _startPoint);
        }

        public GameSaveData.LevelSaveData Export()
        {
            return new GameSaveData.LevelSaveData
            {
                id = _level.GetCurrentLevelId(),
            };
        }

        private LevelDefinition FindLevelById(string levelId)
        {
            var levels = _settings.Levels;

            LevelDefinition found = levels.Find(level => level.Id == levelId);
            if (found)
                return found;

            Debug.LogError($"Level '{levelId}' not found. Used first level");
            return levels.First();
        }
    }
}