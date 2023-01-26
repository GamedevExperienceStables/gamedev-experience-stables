using System;
using System.Collections.Generic;
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
    }
}