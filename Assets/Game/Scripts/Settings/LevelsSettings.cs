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

        public LocationPointStaticDefinition LevelStartPoint => levelStartPoint;

        public LevelDefinition FindLevelById(string levelId)
        {
            LevelDefinition found = levels.Find(level => level.Id == levelId);
            return found;
        }

        public LevelDefinition GetFirstLevel() 
            => Levels.First();
    }
}