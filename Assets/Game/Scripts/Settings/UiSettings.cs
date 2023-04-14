using Game.Level;
using Game.UI;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(menuName = "Settings/UI")]
    public class UiSettings : ScriptableObject
    {
        [SerializeField]
        private StartMenuView.Settings startMenu;
        
        [SerializeField]
        private PauseMenuView.Settings pauseMenu;
        
        [SerializeField]
        private GameOverView.Settings gameOver;
        
        [SerializeField]
        private LocalizationInteractionSettings interaction;

        [SerializeField]
        private Cutscene.Settings cutscene;

        public StartMenuView.Settings StartMenu => startMenu;
        public PauseMenuView.Settings PauseMenu => pauseMenu;
        public GameOverView.Settings GameOver => gameOver;

        public LocalizationInteractionSettings Interaction => interaction;
        public Cutscene.Settings Cutscene => cutscene;
    }
}