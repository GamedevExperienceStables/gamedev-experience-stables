using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class ArtView : PageView<ArtViewModel>
    {
        private IList<Label> _headerLabels;
        private Button _buttonDownload;
        private Button _buttonBack;

        private Settings _settings;
        
        [Inject]
        public void Construct(TeamsView teams, Settings settings)
        {
            _settings = settings;
        }

        protected override void OnAwake()
        {
            SetContent(LayoutNames.StartMenu.PAGE_ART);
            
            _headerLabels = Content.Q<VisualElement>(LayoutNames.StartMenu.PAGE_HEADING)
                .Query<Label>().ToList();
            _buttonDownload = Content.Q<Button>(LayoutNames.StartMenu.PAGE_DOWNLOAD);
            _buttonBack = Content.Q<Button>(LayoutNames.StartMenu.BUTTON_BACK);
            
            RegisterCallbacks();
        }
        
        private void Start()
            => UpdateText();

        private void OnDestroy() 
            => UnregisterCallbacks();

        private void RegisterCallbacks()
        {
            _buttonBack.clicked += OnBackButton;

            _localization.Changed += OnLocalisationChanged;
        }

        private void UnregisterCallbacks()
        {
            _buttonBack.clicked -= OnBackButton;

            _localization.Changed -= OnLocalisationChanged;
        }

        public override void Show()
        {
            Content.SetEnabled(true);
            Content.RemoveFromClassList(LayoutNames.StartMenu.PAGE_HIDDEN_CLASS_NAME);
        }

        public override void Hide()
        {
            Content.SetEnabled(false);
            Content.AddToClassList(LayoutNames.StartMenu.PAGE_HIDDEN_CLASS_NAME);
        }
        
        private void OnLocalisationChanged()
            => UpdateText();
        
        private void UpdateText()
        {
            foreach (Label headerLabel in _headerLabels)
                headerLabel.text = _settings.header.GetLocalizedString();

            _buttonDownload.text = _settings.download.GetLocalizedString();
            _buttonBack.text = _settings.back.GetLocalizedString();
        }

        private void OnBackButton() 
            => ViewModel.Back();
        
        [Serializable]
        public class Settings
        {
            public LocalizedString header;
            public LocalizedString download;
            public LocalizedString back;

            public ModalSettings linkModal;
        }
    }
}