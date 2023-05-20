using System;
using VContainer;

namespace Game.Achievements
{
    public class GameAchievements
    {
        private readonly Settings _settings;
        private readonly IAchievementsService _achievements;

        [Inject]
        public GameAchievements(Settings settings, IAchievementsService achievements)
        {
            _settings = settings;
            _achievements = achievements;
        }

        public void GameStarted() 
            => _achievements.TryAddAchievement(_settings.gameStarted);

        public void GameCompleted() 
            => _achievements.TryAddAchievement(_settings.gameCompleted);
        
        [Serializable]
        public class Settings
        {
            public string gameStarted;
            public string gameCompleted;
        }
    }
}