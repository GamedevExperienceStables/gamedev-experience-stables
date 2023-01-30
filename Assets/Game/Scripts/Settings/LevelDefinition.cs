﻿using Game.Level;
using Game.Utils.Persistence;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(menuName = "Data/Level")]
    public class LevelDefinition : SerializableScriptableObject
    {
        [SerializeField]
        private LocationDefinition location;

        [SerializeField]
        private LevelGoalSettings goal;

        public LocationDefinition Location => location;

        public LevelGoalSettings Goal => goal;
    }
}