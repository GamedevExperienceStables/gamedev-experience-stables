using System;
using Game.Settings;
using UnityEngine;
using VContainer;

namespace Game.Inventory
{
    public class InventoryData
    {
        [Inject]
        public InventoryData(Settings settings, LevelsSettings levelsSettings)
        {
            Materials = new Materials(levelsSettings.Levels, settings.BagMaxStack);
            Recipes = new Recipes();
            Runes = new Runes();
        }

        public Materials Materials { get; }
        public Recipes Recipes { get; }
        public Runes Runes { get; }

        [Serializable]
        public class Settings
        {
            [SerializeField, Min(0)]
            private int bagMaxStack = 10;

            public int BagMaxStack => bagMaxStack;
        }
    }
}