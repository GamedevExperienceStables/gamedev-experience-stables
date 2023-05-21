using System;
using VContainer;

namespace Game.Achievements
{
    public class GameStats
    {
        private readonly IGameStatsService _stats;
        private readonly Settings _settings;

        [Inject]
        public GameStats(IGameStatsService stats, Settings settings)
        {
            _stats = stats;
            _settings = settings;
        }

        public void IncreaseDeathCount()
            => _stats.IncreaseStat(_settings.deathCount);
        
        [Serializable]
        public class Settings
        {
            public string deathCount;
        }
    }
}