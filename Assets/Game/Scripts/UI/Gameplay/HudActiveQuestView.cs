using Game.Localization;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class HudActiveQuestView 
    {
        private readonly HudSettings _settings;
        private readonly ILocalizationService _localization;

        private Label _activeQuestLabel;

        public HudActiveQuestView(HudSettings settings, ILocalizationService localization)
        {
            _settings = settings;
            _localization = localization;
        }

        public void Create(VisualElement root)
        {
            var container = root.Q<VisualElement>(LayoutNames.Hud.WIDGET_ACTIVE_QUEST);
            _activeQuestLabel = container.Q<Label>(LayoutNames.Hud.WIDGET_ACTIVE_QUEST_LABEL);

            UpdateText();
            
            _localization.Changed += OnLocalizationChanged;
        }

        public void Destroy()
            => _localization.Changed -= OnLocalizationChanged;

        private void OnLocalizationChanged()
            => UpdateText();

        private void UpdateText()
        {

            _activeQuestLabel.text = _settings.ActiveQuest.GetLocalizedString();
        }
    }
}