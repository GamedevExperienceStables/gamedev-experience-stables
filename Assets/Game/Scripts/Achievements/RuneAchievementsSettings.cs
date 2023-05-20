using System;
using System.Collections.Generic;
using Game.Inventory;
using UnityEngine;

namespace Game.Achievements
{
    [Serializable]
    public class RuneAchievementsSettings
    {
        [SerializeField]
        private List<RuneAchievement> items = new();

        public bool GetKey(RuneDefinition rune, out string key)
        {
            foreach (RuneAchievement item in items)
            {
                if (item.rune != rune)
                    continue;
                
                key = item.key;
                return true;
            }

            key = default;
            return false;
        }

        [Serializable]
        public struct RuneAchievement
        {
            public string key;
            public RuneDefinition rune;
        }
    }
}