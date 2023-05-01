using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class PauseSettingsView : PageView
    {
        private PauseViewRouter _router;
        private Button _backButton;

        private SettingsView _settings;
        
        private CommonFx _commonFx;

        [Inject]
        public void Construct(SettingsView settings, CommonFx commonFx)
        {
            _settings = settings;
            _commonFx = commonFx;
        }

        protected override void OnAwake()
        {
            SetContent(LayoutNames.PauseSettings.CONTAINER);

            _settings.Create(Content);

            _router = GetComponent<PauseViewRouter>();

            _backButton = Content.Q<Button>(LayoutNames.PauseSettings.BUTTON_BACK);
            _backButton.clicked += OnBack;
            
            _commonFx.RegisterButton(_backButton, ButtonStyle.Primary);
        }

        private void OnDestroy()
        {
            _settings.Destroy();

            _backButton.clicked -= OnBack;
            
            _commonFx.UnRegisterButton(_backButton, ButtonStyle.Primary);
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