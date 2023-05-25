namespace Game.Achievements
{
    public class PlaceholderAchievements : IAchievementsService 
    {
        public bool TryAddAchievement(string achievement) 
            => false;

        public void RemoveAchievement(string achievement)
        {
            // used as placeholder  
        }

        public void ClearAchievements()
        {
            // used as placeholder
        }
    }
}