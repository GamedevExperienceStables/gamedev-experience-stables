using Game.Achievements;
using VContainer;

namespace Game.Steam
{
    public class SteamStats : IGameStatsService
    {
        private readonly SteamService _steam;

        [Inject]
        public SteamStats(SteamService steam) 
            => _steam = steam;

        public void IncreaseStat(string key)
        {
            int currentValue = _steam.GetStat(key);
            currentValue += 1;
            
            _steam.SetStat(key, currentValue);
        }
    }
}