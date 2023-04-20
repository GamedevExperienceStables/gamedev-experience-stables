using System.Collections.Generic;
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
        
        private VisualTreeAsset _teamTemplate;

        [Inject]
        public void Construct(TeamsView teams) 
            => _teams = teams;

        protected override void OnAwake()
        {
            SetContent(LayoutNames.StartMenu.PAGE_ABOUT);

            _headerLabels = Content.Q<VisualElement>(LayoutNames.StartMenu.PAGE_HEADING)
                .Query<Label>().ToList();
            
            _teams.Create(Content);

            _buttonBack = Content.Q<Button>(LayoutNames.StartMenu.BUTTON_BACK);

            RegisterCallbacks();
        }

        private void OnDestroy()
        {
            _teams.Destroy();
            
            UnregisterCallbacks();
        }

        private void RegisterCallbacks()
            => _buttonBack.clicked += OnBackButton;

        private void UnregisterCallbacks()
            => _buttonBack.clicked -= OnBackButton;

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

        private void OnBackButton()
            => ViewModel.Back();
    }
}