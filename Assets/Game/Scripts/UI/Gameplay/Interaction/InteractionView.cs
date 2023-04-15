using Game.Level;
using Game.Localization;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public sealed class InteractionView
    {
        private readonly InteractionViewModel _viewModel;
        private readonly LocalizationInteraction _interaction;
        private readonly ILocalizationService _localization;

        private VisualElement _container;

        private Label _label;

        [Inject]
        public InteractionView(InteractionViewModel viewModel, LocalizationInteraction interaction,
            ILocalizationService localization)
        {
            _viewModel = viewModel;
            _interaction = interaction;
            _localization = localization;

            _viewModel.SubscribeEnabled(OnEnabled);
            _viewModel.SubscribeDisabled(OnDisabled);
            _viewModel.SubscribeInteracted(OnInteracted);

            _localization.Changed += OnLocalizationChanged;
        }

        public void Destroy()
        {
            _viewModel.UnSubscribeEnabled(OnEnabled);
            _viewModel.UnSubscribeDisabled(OnDisabled);
            _viewModel.UnSubscribeInteracted(OnInteracted);

            _localization.Changed -= OnLocalizationChanged;
        }

        private void OnLocalizationChanged()
        {
            string text = _interaction.GetText(_viewModel.CurrentInteraction);
            UpdateText(text);
        }

        public void Create(VisualElement root)
        {
            var container = root.Q<VisualElement>(LayoutNames.Hud.WIDGET_INTERACTION);
            _container = container.Q<VisualElement>(LayoutNames.Hud.WIDGET_INTERACTION_BLOCK);
            _label = container.Q<Label>(LayoutNames.Hud.WIDGET_INTERACTION_TEXT);
            
            Hide();
        }

        private void OnEnabled(Interaction interaction)
        {
            string text = _interaction.GetText(interaction);
            Show(text);
        }

        private void OnInteracted(Interaction interaction)
        {
            string text = _interaction.GetText(interaction);
            UpdateText(text);
        }

        private void OnDisabled()
            => Hide();

        private void Hide()
            => _container.AddToClassList(LayoutNames.Hud.WIDGET_INTERACTION_HIDDEN_CLASS_NAME);

        private void Show() 
            => _container.RemoveFromClassList(LayoutNames.Hud.WIDGET_INTERACTION_HIDDEN_CLASS_NAME);

        private void Show(string text)
        {
            UpdateText(text);

            Show();
        }

        private void UpdateText(string text)
            => _label.text = text;
    }
}