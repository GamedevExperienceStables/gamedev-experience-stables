using UnityEngine.UIElements;

namespace Game.UI
{
    public class PauseMenuView : PageView<PauseMenuViewModel>
    {
        private VisualElement _container;

        private Button _buttonResume;
        private Button _buttonMainMenu;
        private Button _buttonSettings;

        private PauseViewRouter _router;

        protected override void OnAwake()
        {
            SetContent(LayoutNames.PauseMenu.CONTAINER);

            _router = GetComponent<PauseViewRouter>();

            _buttonResume = Content.Q<Button>(LayoutNames.PauseMenu.BUTTON_RESUME);
            _buttonSettings = Content.Q<Button>(LayoutNames.PauseMenu.BUTTON_SETTINGS);
            _buttonMainMenu = Content.Q<Button>(LayoutNames.PauseMenu.BUTTON_MAIN_MENU);

            _buttonResume.clicked += OnResumeButton;
            _buttonMainMenu.clicked += OnMainMenuButton;
            _buttonSettings.clicked += OnSettingsButton;
        }

        public void OnDestroy()
        {
            _buttonResume.clicked -= OnResumeButton;
            _buttonMainMenu.clicked -= OnMainMenuButton;
            _buttonSettings.clicked -= OnSettingsButton;
        }

        private void OnResumeButton()
            => ViewModel.ResumeGame();

        private void OnMainMenuButton()
            => ViewModel.GoToMainMenu();

        private void OnSettingsButton()
            => _router.OpenSettings();
    }
}