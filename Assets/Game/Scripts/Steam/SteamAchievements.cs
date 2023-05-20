using Game.Achievements;
using VContainer;

namespace Game.Steam
{
    public class SteamAchievements : IAchievementsService
    {
        private readonly SteamService _steam;

        [Inject]
        public SteamAchievements(SteamService steam) 
            => _steam = steam;

        public bool TryAddAchievement(string achievement)
        {
            if (HasAchievement(achievement))
                return false;

            _steam.SetAchievement(achievement);
            return true;
        }

        public void RemoveAchievement(string achievement) 
            => _steam.RemoveAchievement(achievement);

        public void ClearAchievements() 
            => _steam.ClearStatsAndAchievements();

        private bool HasAchievement(string achievement) 
            => _steam.HasAchievement(achievement);
    }
}