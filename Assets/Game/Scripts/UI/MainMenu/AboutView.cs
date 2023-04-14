using UnityEngine.UIElements;

namespace Game.UI
{
    public class AboutView : PageView<AboutViewModel>
    {
        private Button _buttonBack;

        protected override void OnAwake()
        {
            SetContent(LayoutNames.StartMenu.PAGE_ABOUT);
            
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

        private void OnBackButton() 
            => ViewModel.Back();
    }
}