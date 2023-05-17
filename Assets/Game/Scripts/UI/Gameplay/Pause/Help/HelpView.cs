using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class HelpView : PageView<PauseMenuViewModel>
    {
        private PauseViewRouter _router;
        private Button _backButton;
        private IList<Label> _headerLabels;
        
        private HelpContentView _helpContent;

        private HelpSettings _settings;
        private CommonFx _commonFx;

        [Inject]
        public void Construct(HelpSettings settings, HelpContentView helpContent, CommonFx commonFx)
        {
            _commonFx = commonFx;
            _helpContent = helpContent;
            _settings = settings;
        }

        protected override void OnAwake()
        {
            SetContent(LayoutNames.PauseSettings.CONTAINER);

            _router = GetComponent<PauseViewRouter>();

            _backButton = Content.Q<Button>(LayoutNames.PauseSettings.BUTTON_BACK);
            _backButton.clicked += OnBack;

            _headerLabels = Content.Q<VisualElement>(LayoutNames.PauseSettings.PAGE_HEADING)
                .Query<Label>().ToList();
            
            _helpContent.Create(Content);

            _commonFx.RegisterButton(_backButton, ButtonStyle.Primary);

            localization.Changed += OnLocalisationChanged;
        }

        private void Start()
            => UpdateText();

        private void OnDestroy()
        {

            _backButton.clicked -= OnBack;

            _commonFx.UnRegisterButton(_backButton, ButtonStyle.Primary);

            localization.Changed -= OnLocalisationChanged;
            _helpContent.Destroy();
        }

        public override void Show()
        {
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
    }
}