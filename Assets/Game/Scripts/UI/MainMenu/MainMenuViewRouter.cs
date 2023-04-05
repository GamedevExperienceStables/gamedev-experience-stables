namespace Game.UI
{
    public sealed class MainMenuViewRouter : PageRouter
    {
        private StartMenuView _startMenu;
        private MainMenuSettingsView _mainMenuSettingsView;
        private AboutView _aboutView;
        private ArtView _artView;

        private void Awake()
        {
            _startMenu = GetComponentInChildren<StartMenuView>();
            _mainMenuSettingsView = GetComponentInChildren<MainMenuSettingsView>();
            _aboutView = GetComponentInChildren<AboutView>();
            _artView = GetComponentInChildren<ArtView>();
        }

        private void Start()
        {
            _startMenu.Hide();
            _mainMenuSettingsView.Hide();
            _aboutView.Hide();
            _artView.Hide();

            ShowStartMenu();
        }

        public void ShowStartMenu()
            => _startMenu.Show();

        public void OpenSettings()
            => Open(_mainMenuSettingsView);

        public void OpenAbout()
            => Open(_aboutView);

        public void OpenArt()
            => Open(_artView);

        public void Back()
        {
            _startMenu.Activate();
            
            ActivePage.Hide();
        }

        protected override void Open(PageView view)
        {
            _startMenu.Deactivate();

            base.Open(view);
        }
    }
}