using System.Collections.Generic;
using Game.Localization;
using Game.Utils;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class MainMenuSettingsView : PageView
    {
        private Button _buttonBack;
        private MainMenuViewRouter _router;

        private SettingsView _settingsView;
        private IList<Label> _headerLabels;
        
        private ILocalizationService _localisation;
        private StartMenuView.Settings _settings;

        [Inject]
        public void Construct(SettingsView settingsView, ILocalizationService localisation, StartMenuView.Settings settings)
        {
            _settingsView = settingsView;
            _localisation = localisation;
            _settings = settings;
        }

        protected override void OnAwake()
        {
            SetContent(LayoutNames.StartMenu.PAGE_SETTINGS);
            
            _router = GetComponentInParent<MainMenuViewRouter>();
            
            _settingsView.Create(Content);

            _buttonBack = Content.Q<Button>(LayoutNames.StartMenu.BUTTON_BACK);
            _buttonBack.clicked += OnBackButton;
            
            _headerLabels = Content.Q<VisualElement>(LayoutNames.StartMenu.PAGE_HEADING)
                .Query<Label>().ToList();
            
            _localisation.Changed += OnLocalisationChanged;
        }

        private void OnDestroy()
        {
            _settingsView.Destroy();

            _buttonBack.clicked -= OnBackButton;
            
            _localisation.Changed -= OnLocalisationChanged;
        }

        private void OnLocalisationChanged()
            => UpdateText();

        private void UpdateText()
        {
            foreach (Label headerLabel in _headerLabels)
                headerLabel.text = _settings.settings.button.GetLocalizedString();
        }

        public override void Show()
        {
            _settingsView.Init();
            
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