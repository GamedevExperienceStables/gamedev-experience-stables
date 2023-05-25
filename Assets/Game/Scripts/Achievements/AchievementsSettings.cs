using UnityEngine;

namespace Game.Achievements
{
    [CreateAssetMenu(menuName = "Settings/Achievements")]
    public class AchievementsSettings : ScriptableObject
    {
        [SerializeField]
        private GameStats.Settings stats;

        [SerializeField]
        private GameAchievements.Settings game;

        [SerializeField]
        private RuneAchievementsSettings runeAchievements;

        public RuneAchievementsSettings Runes => runeAchievements;
        public GameAchievements.Settings Game => game;
        public GameStats.Settings Stats => stats;
    }
}