using Game.Dialog;
using Game.Input;
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
        private PauseSettingsView.Settings pauseSettings;

        [SerializeField]
        private GameOverView.Settings gameOver;

        [SerializeField]
        private LocalizationInteractionSettings interaction;
        
        [SerializeField]
        private DialogNotification.Settings notification;

        [SerializeField]
        private LoadingScreenView.Settings loading;

        [SerializeField]
        private SavingView.Settings saving;

        [SerializeField]
        private HudSettings hud;

        [SerializeField]
        private Cutscene.Settings cutscene;

        [SerializeField]
        private AboutSettings about;

        [SerializeField]
        private ArtSettings artMenu;
        
        [SerializeField]
        private InputBindingsSettings inputBindings;

        public StartMenuView.Settings StartMenu => startMenu;
        public MainMenuSettingsView.Settings SettingsMenu => settingsMenu;
        public SettingsView.Settings SettingsControlsMenuMenu => settingsControlsMenu;
        public PauseMenuView.Settings PauseMenu => pauseMenu;
        public PauseSettingsView.Settings PauseSettings => pauseSettings;
        public GameOverView.Settings GameOver => gameOver;

        public LocalizationInteractionSettings Interaction => interaction;
        public Cutscene.Settings Cutscene => cutscene;

        public HudSettings Hud => hud;

        public LoadingScreenView.Settings Loading => loading;
        public SavingView.Settings Saving => saving;

        public AboutSettings About => about;
        public ArtSettings ArtMenu => artMenu;
        public DialogNotification.Settings Notification => notification;
        public InputBindingsSettings InputBindings => inputBindings;
    }
}