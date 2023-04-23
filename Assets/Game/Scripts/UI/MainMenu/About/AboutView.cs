using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class AboutView : PageView<AboutViewModel>
    {
        private List<TeamView> _teamViews;

        private IList<Label> _headerLabels;
        private VisualElement _listContent;
        private Button _buttonBack;

        private TeamsView _teams;
        private AboutSettings _settings;
        
        private VisualTreeAsset _teamTemplate;
        private ScrollView _scrollContainer;

        [Inject]
        public void Construct(TeamsView teams, AboutSettings settings)
        {
            _teams = teams;
            _settings = settings;
        }

        protected override void OnAwake()
        {
            SetContent(LayoutNames.StartMenu.PAGE_ABOUT);

            _headerLabels = Content.Q<VisualElement>(LayoutNames.StartMenu.PAGE_HEADING)
                .Query<Label>().ToList();
            
            _scrollContainer = Content.Q<ScrollView>(LayoutNames.StartMenu.SCROLL_CONTAINER);
            
            _teams.Create(Content);

            _buttonBack = Content.Q<Button>(LayoutNames.StartMenu.BUTTON_BACK);

            RegisterCallbacks();
        }
        
        private void Start()
            => UpdateText();

        private void OnDestroy()
        {
            _teams.Destroy();
            
            UnregisterCallbacks();
        }

        private void RegisterCallbacks()
        {
            _buttonBack.clicked += OnBackButton;

            localization.Changed += OnLocalisationChanged;
        }

        private void UnregisterCallbacks()
        {
            _buttonBack.clicked -= OnBackButton;

            localization.Changed -= OnLocalisationChanged;
        }

        public override void Show()
        {
            Content.SetEnabled(true);
            Content.RemoveFromClassList(LayoutNames.StartMenu.PAGE_HIDDEN_CLASS_NAME);
            
            _scrollContainer.scrollOffset = Vector2.zero;
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

            _buttonBack.text = _settings.back.GetLocalizedString();
        }

        private void OnBackButton()
            => ViewModel.Back();
    }
}