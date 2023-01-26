using System;
using Game.Settings;
using JetBrains.Annotations;

namespace Game.GameFlow
{
    [UsedImplicitly]
    public class GameData
    {
        public TimeSpan PlayTime { get; set; }
        public DateTime SessionStartTime { get; set; }
    }
}