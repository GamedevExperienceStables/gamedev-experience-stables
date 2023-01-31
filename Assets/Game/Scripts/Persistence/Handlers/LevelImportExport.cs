using System;
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
            LevelDefinition firstLevel = _settings.GetFirstLevel();
            _level.InitLevel(firstLevel, _startPoint);
        }

        public void Import(GameSaveData.Level data)
        {
            LevelDefinition currentLevel = _settings.FindLevelById(data.id);
            if (!currentLevel)
            {
                Debug.LogError($"Level '{data.id}' not found. Used first level");
                currentLevel = _settings.GetFirstLevel();
            }

            _level.InitLevel(currentLevel, _startPoint);
        }

        public GameSaveData.Level Export()
        {
            return new GameSaveData.Level
            {
                id = _level.GetCurrentLevelId(),
                pointsCleared = Array.Empty<string>()
            };
        }
    }
}