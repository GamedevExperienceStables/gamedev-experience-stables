using System;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class SavingView
    {
        private readonly SavingViewModel _viewModel;
        private readonly Settings _settings;
        private VisualElement _container;
        private Label _text;

        [Inject]
        public SavingView(SavingViewModel viewModel, Settings settings)
        {
            _viewModel = viewModel;
            _settings = settings;
        }

        public void Create(VisualElement root)
        {
            var template = root.Q<VisualElement>(LayoutNames.Hud.WIDGET_SAVING);
            _container = template.Q<VisualElement>(LayoutNames.Hud.WIDGET_SAVING_CONTAINER);
            _text = _container.Q<Label>(LayoutNames.Hud.WIDGET_SAVING_TEXT);

            _viewModel.Subscribe(OnSavingChanged);
        }

        public void Destroy()
            => _viewModel.UnSubscribe(OnSavingChanged);

        private void OnSavingChanged(bool isSaving)
        {
            if (isSaving)
                OnSaving();
            else
                OnSaved();
        }

        private void OnSaving()
        {
            _text.text = _settings.savingText.GetLocalizedString();
            
            Show();
        }

        private void OnSaved()
        {
            _text.text = _settings.savedText.GetLocalizedString();
            
            Hide();
        }

        public void Show()
            => _container.AddToClassList(LayoutNames.Hud.WIDGET_SAVING_ENABLED_CLASS_NAME);

        public void Hide()
            => _container.RemoveFromClassList(LayoutNames.Hud.WIDGET_SAVING_ENABLED_CLASS_NAME);
        
        
        [Serializable]
        public class Settings
        {
            public LocalizedString savingText;
            public LocalizedString savedText;
        }
    }
}