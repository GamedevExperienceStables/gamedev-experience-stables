using Game.Level;
using Game.Localization;
using Game.Utils;
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
            _container = root.Q<VisualElement>(LayoutNames.Hud.WIDGET_INTERACTION);
            _container.SetDisplay(false);

            _label = _container.Q<Label>(LayoutNames.Hud.WIDGET_INTERACTION_TEXT);
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

        private void Show(string text)
        {
            UpdateText(text);

            _container.SetDisplay(true);
        }

        private void UpdateText(string text)
            => _label.text = text;

        private void Hide()
            => _container.SetDisplay(false);
    }
}