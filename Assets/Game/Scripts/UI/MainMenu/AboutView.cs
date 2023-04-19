using System;
using System.Collections.Generic;
using Game.UI.About;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class AboutView : PageView<AboutViewModel>
    {
        private List<TeamView> _teamViews;

        private IList<Label> _headerLabels;
        private VisualElement _listContent;
        private Button _buttonBack;

        protected override void OnAwake()
        {
            SetContent(LayoutNames.StartMenu.PAGE_ABOUT);
             
            _headerLabels = Content.Q<VisualElement>(LayoutNames.StartMenu.PAGE_HEADING)
                .Query<Label>().ToList();
            _listContent = Content.Q<VisualElement>("list-content-element");
            _buttonBack = Content.Q<Button>(LayoutNames.StartMenu.BUTTON_BACK);
            
            RegisterCallbacks();
        }
        
        private void OnDestroy() 
            => UnregisterCallbacks();

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

        private void InitTeams()
        {
            foreach (TeamView teamView in _teamViews)
            {
                //_listContent.Add(teamView);
            }
        }

        private void OnBackButton() 
            => ViewModel.Back();
        
        [Serializable]
        public class Settings
        {
            [Serializable]
            public struct Page
            {
                public LocalizedString label;
            }

            public Page header;
            public Page back;
        }
    }
}