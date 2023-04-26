using Game.Input;
using Game.Localization;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class HudPromptView
    {
        private readonly HudSettings _settings;
        private readonly ILocalizationService _localization;
        private readonly InputBindings _inputBindings;

        private Label _inventoryPromptLabel;

        public HudPromptView(HudSettings settings, ILocalizationService localization, InputBindings inputBindings)
        {
            _settings = settings;
            _localization = localization;
            _inputBindings = inputBindings;
        }

        public void Create(VisualElement root)
        {
            InitInventoryPrompt(root);

            UpdateText();

            _localization.Changed += OnLocalizationChanged;
        }

        private void InitInventoryPrompt(VisualElement root)
        {
            var inventoryPrompt = root.Q<VisualElement>(LayoutNames.Hud.WIDGET_PROMPT_INVENTORY);
            _inventoryPromptLabel = inventoryPrompt.Q<Label>(LayoutNames.Hud.WIDGET_PROMPT_LABEL);

            var inventoryPromptKey = inventoryPrompt.Q<InputKey>();
            InputKeyBinding inputKey = _inputBindings.GetBindingDisplay(InputGameplayActions.Inventory);
            inventoryPromptKey.Bind(inputKey);
        }

        public void Destroy()
            => _localization.Changed -= OnLocalizationChanged;

        private void OnLocalizationChanged()
            => UpdateText();

        private void UpdateText()
            => _inventoryPromptLabel.text = _settings.InventoryPrompt.GetLocalizedString();
    }
}