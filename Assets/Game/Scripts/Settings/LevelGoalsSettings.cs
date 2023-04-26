using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Settings
{
    [Serializable]
    public class LevelGoalsSettings
    {
        [SerializeField]
        private List<LevelGoalSettings> goals;
    }
}