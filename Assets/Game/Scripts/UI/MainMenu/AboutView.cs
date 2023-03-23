using UnityEngine.UIElements;

namespace Game.UI
{
    public class AboutView : PageView<AboutViewModel>
    {
        private Button _buttonBack;

        protected override void OnAwake()
        {
            _buttonBack = Content.Q<Button>(LayoutNames.StartMenu.BUTTON_BACK);
            
            RegisterCallbacks();
        }
        
        private void OnDestroy() 
            => UnregisterCallbacks();

        private void RegisterCallbacks() 
            => _buttonBack.clicked += OnBackButton;

        private void UnregisterCallbacks() 
            => _buttonBack.clicked -= OnBackButton;

        private void OnBackButton() 
            => ViewModel.Back();
    }
}