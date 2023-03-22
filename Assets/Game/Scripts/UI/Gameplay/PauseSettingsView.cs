using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class PauseSettingsView : PageView
    {
        private PauseViewRouter _router;
        private Button _backButton;

        private SettingsView _settings;

        [Inject]
        public void Construct(SettingsView settings) 
            => _settings = settings;

        protected override void OnAwake()
        {
            SetContent(LayoutNames.PauseSettings.CONTAINER);

            _settings.Create(Content);

            _router = GetComponent<PauseViewRouter>();

            _backButton = Content.Q<Button>(LayoutNames.PauseSettings.BUTTON_BACK);
            _backButton.clicked += OnBack;
        }

        private void OnDestroy()
        {
            _settings.Destroy();

            _backButton.clicked -= OnBack;
        }

        public override void Show()
        {
            _settings.Init();

            base.Show();
        }

        private void OnBack()
            => _router.ToRoot();
    }
}