using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class MainMenuSettingsView : PageView
    {
        private Button _buttonBack;
        private MainMenuViewRouter _router;

        private SettingsView _settings;

        [Inject]
        public void Construct(SettingsView settings) 
            => _settings = settings;

        protected override void OnAwake()
        {
            _router = GetComponentInParent<MainMenuViewRouter>();

            _settings.Create(Content);

            _buttonBack = Content.Q<Button>(LayoutNames.StartMenu.BUTTON_BACK);
            _buttonBack.clicked += OnBackButton;
        }

        private void OnDestroy()
        {
            _settings.Destroy();

            _buttonBack.clicked -= OnBackButton;
        }

        public override void Show()
        {
            _settings.Init();

            base.Show();
        }

        private void OnBackButton()
            => _router.Back();
    }
}