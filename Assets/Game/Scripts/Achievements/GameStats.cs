using VContainer;

namespace Game.Achievements
{
    public class GameStats
    {
        private readonly IGameStatsService _stats;

        [Inject]
        public GameStats(IGameStatsService stats)
            => _stats = stats;

        public void IncreaseDeathCount()
            => _stats.IncreaseStat(GameStatsNames.DEATH_COUNT);
    }
}