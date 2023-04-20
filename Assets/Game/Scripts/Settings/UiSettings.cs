using System;
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
        private MainMenuSettingsView.Settings settingsMenu;
        
        [SerializeField]
        private SettingsView.Settings settingsControlsMenu;
        
        [SerializeField]
        private PauseMenuView.Settings pauseMenu;
        
        [SerializeField]
        private GameOverView.Settings gameOver;
        
        [SerializeField]
        private LocalizationInteractionSettings interaction;

        [SerializeField]
        private Cutscene.Settings cutscene;
        
        [SerializeField]
        private AboutSettings about;

        public StartMenuView.Settings StartMenu => startMenu;
        public MainMenuSettingsView.Settings SettingsMenu => settingsMenu;
        public SettingsView.Settings SettingsControlsMenuMenu => settingsControlsMenu;
        public PauseMenuView.Settings PauseMenu => pauseMenu;
        public GameOverView.Settings GameOver => gameOver;

        public LocalizationInteractionSettings Interaction => interaction;
        public Cutscene.Settings Cutscene => cutscene;

        public AboutSettings About => about;
    }
}