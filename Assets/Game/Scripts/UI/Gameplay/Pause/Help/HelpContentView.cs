using Game.Localization;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class HelpContentView
    {
        private readonly HelpContentData _settings;

        private Label _description;
        private Label _header;
        
        private VisualElement _listContent;
        private VisualTreeAsset _template;
        
        private readonly ControlTemplateViewFactory _controlFactory;
        private readonly ILocalizationService _localization;
        
        public HelpContentView(HelpSettings settings, ControlTemplateViewFactory controlFactory, ILocalizationService localisation)
        {
            _settings = settings.HelpData;
            _controlFactory = controlFactory;
            _localization = localisation;
        }
        
        public void Create(VisualElement root)
        {
            _description = root.Q<Label>("description");
            _header = root.Q<Label>("header");
            
            _listContent = root.Q<VisualElement>("control-content");
            _template = _listContent.Q<TemplateContainer>("control-template").templateSource;
            
            _listContent.Clear();

            CreateControls();
            UpdateText();
            _localization.Changed += OnLocalisationChanged;
        }

        public void Destroy()
            => _localization.Changed -= OnLocalisationChanged;
        
        private void CreateControls()
        {
            foreach (HelpContentData.Control control in _settings.controls)
            {
                TemplateContainer element = _template.Instantiate();
                _listContent.Add(element);

                _controlFactory.Create(element, control);
            }
        }
        
        private void UpdateText()
        {
            _description.text = _settings.description.GetLocalizedString();
            _header.text = _settings.header.GetLocalizedString();
        }


        private void OnLocalisationChanged()
            => UpdateText();
    }
}