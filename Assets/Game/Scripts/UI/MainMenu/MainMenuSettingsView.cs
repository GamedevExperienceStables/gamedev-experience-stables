using Game.Utils;
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
            SetContent(LayoutNames.StartMenu.PAGE_SETTINGS);
            
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
            
            Content.SetVisibility(true);
        }

        public override void Hide()
        {
            Content.SetVisibility(false);
        }

        private void OnBackButton()
            => _router.Back();
    }
}