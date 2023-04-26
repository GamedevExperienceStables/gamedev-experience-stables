using System;
using Game.Inventory;
using UnityEngine;

namespace Game.Settings
{
    [Serializable]
    public class LevelGoalSettings
    {
        [SerializeField]
        private MaterialDefinition material;

        [SerializeField, Min(0)]
        private int count;

        public MaterialDefinition Material => material;

        public int Count => count;
    }
}