namespace Game.Achievements
{
    public interface IAchievementsService
    {
        public bool TryAddAchievement(string achievement);
        public void RemoveAchievement(string achievement);

        public void ClearAchievements();
    }
}