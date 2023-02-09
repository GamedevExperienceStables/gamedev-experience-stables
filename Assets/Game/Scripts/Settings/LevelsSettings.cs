using System;
using System.Collections.Generic;
using System.Linq;
using Game.Level;
using UnityEngine;

namespace Game.Settings
{
    [Serializable]
    public class LevelsSettings
    {
        [SerializeField]
        private LocationPointStaticDefinition levelStartPoint;

        [SerializeField]
        private List<LevelDefinition> levels;

        public List<LevelDefinition> Levels => levels;

        public ILocationPoint LevelStartPoint => levelStartPoint;

        public LevelDefinition FindLevelById(string levelId)
            => levels.Find(level => level.Id == levelId);

        public LevelDefinition GetFirstLevel()
            => Levels.First();

        public LevelDefinition GetNextLevel(LevelDefinition currentLevel)
        {
            int nextLevelIndex = GetNextLevelIndex(currentLevel);
            return levels[nextLevelIndex];
        }

        public bool IsLastLevel(LevelDefinition currentLevel)
        {
            int nextLevelIndex = GetNextLevelIndex(currentLevel);
            return nextLevelIndex >= levels.Count;
        }

        private int GetLevelIndex(LevelDefinition currentLevel)
        {
            int levelIndex = levels.IndexOf(currentLevel);
            if (levelIndex < 0)
                throw new KeyNotFoundException($"Level '{currentLevel.name}' not found in settings list");

            return levelIndex;
        }

        private int GetNextLevelIndex(LevelDefinition currentLevel)
        {
            int levelIndex = GetLevelIndex(currentLevel);
            int nextLevelIndex = levelIndex + 1;
            return nextLevelIndex;
        }
    }
}