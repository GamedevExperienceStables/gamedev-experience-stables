using Game.Input;
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
        private readonly InputBindings _inputBindings;

        private VisualElement _container;

        private Label _label;

        [Inject]
        public InteractionView(InteractionViewModel viewModel, LocalizationInteraction interaction,
            ILocalizationService localization, InputBindings inputBindings)
        {
            _viewModel = viewModel;
            _interaction = interaction;
            _localization = localization;
            _inputBindings = inputBindings;

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
            InteractionPrompt prompt = _interaction.GetInteractionPrompt(_viewModel.CurrentInteraction);
            UpdateText(prompt.text);
        }

        public void Create(VisualElement root)
        {
            var container = root.Q<VisualElement>(LayoutNames.Hud.WIDGET_INTERACTION);
            _container = container.Q<VisualElement>(LayoutNames.Hud.WIDGET_INTERACTION_BLOCK);
            _label = container.Q<Label>(LayoutNames.Hud.WIDGET_INTERACTION_TEXT);
            
            InputKeyBinding inputKeyBinding = _inputBindings.GetBindingDisplay(InputGameplayActions.Interaction);
            var inputKey = container.Q<InputKey>(LayoutNames.Hud.WIDGET_INTERACTION_INPUT_KEY);
            inputKey.Bind(inputKeyBinding);
            
            Hide();
        }

        private void OnEnabled(Interaction interaction) 
            => ShowText(interaction);

        private void OnInteracted(Interaction interaction) 
            => ShowText(interaction);

        private void ShowText(Interaction interaction)
        {
            InteractionPrompt prompt = _interaction.GetInteractionPrompt(interaction);
            Show(prompt.text, prompt.canExecute);
        }

        private void OnDisabled()
            => Hide();

        private void Hide()
            => _container.AddToClassList(LayoutNames.Hud.WIDGET_INTERACTION_HIDDEN_CLASS_NAME);

        private void Show() 
            => _container.RemoveFromClassList(LayoutNames.Hud.WIDGET_INTERACTION_HIDDEN_CLASS_NAME);

        private void Show(string text, bool isEnabled)
        {
            UpdateText(text);
            UpdateState(isEnabled);

            Show();
        }

        private void UpdateState(bool isEnabled)
        {
            if (isEnabled)
                _container.AddToClassList(LayoutNames.Hud.WIDGET_INTERACTION_ENABLED_CLASS_NAME);
            else 
                _container.RemoveFromClassList(LayoutNames.Hud.WIDGET_INTERACTION_ENABLED_CLASS_NAME);
        }

        private void UpdateText(string text)
            => _label.text = text;
    }
}