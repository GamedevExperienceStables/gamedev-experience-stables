using System;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class PauseSettingsView : PageView<PauseMenuViewModel>
    {
        private PauseViewRouter _router;
        private Button _backButton;
        private IList<Label> _headerLabels;

        private SettingsView _settingsView;

        private Settings _settings;
        private CommonFx _commonFx;

        [Inject]
        public void Construct(SettingsView view, Settings settings, CommonFx commonFx)
        {
            _settingsView = view;
            _commonFx = commonFx;
            _settings = settings;
        }

        protected override void OnAwake()
        {
            SetContent(LayoutNames.PauseSettings.CONTAINER);

            _settingsView.Create(Content);

            _router = GetComponent<PauseViewRouter>();

            _backButton = Content.Q<Button>(LayoutNames.PauseSettings.BUTTON_BACK);
            _backButton.clicked += OnBack;

            _headerLabels = Content.Q<VisualElement>(LayoutNames.PauseSettings.PAGE_HEADING)
                .Query<Label>().ToList();

            _commonFx.RegisterButton(_backButton, ButtonStyle.Primary);

            localization.Changed += OnLocalisationChanged;
        }

        private void Start()
            => UpdateText();

        private void OnDestroy()
        {
            _settingsView.Destroy();

            _backButton.clicked -= OnBack;

            _commonFx.UnRegisterButton(_backButton, ButtonStyle.Primary);

            localization.Changed -= OnLocalisationChanged;
        }

        public override void Show()
        {
            _settingsView.Init();

            base.Show();
            
            ViewModel.SubscribeBack(OnBackRequested);
        }

        public override void Hide()
        {
            ViewModel.UnSubscribeBack(OnBackRequested);
            
            base.Hide();
        }

        private void OnLocalisationChanged()
            => UpdateText();

        private void UpdateText()
        {
            foreach (Label headerLabel in _headerLabels)
                headerLabel.text = _settings.heading.GetLocalizedString();
            
            _backButton.text = _settings.back.GetLocalizedString();
        }
        
        private void OnBackRequested() 
            => OnBack();

        private void OnBack()
            => _router.ToRoot();

        [Serializable]
        public class Settings
        {
            public LocalizedString heading;
            public LocalizedString back;
        }
    }
}