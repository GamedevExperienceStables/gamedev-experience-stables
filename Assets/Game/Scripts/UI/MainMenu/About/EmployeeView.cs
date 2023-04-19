using UnityEngine.UIElements;

namespace Game.UI.About
{
    public class EmployeeView
    {
        private readonly EmployeeViewModel _viewModel;
        
        private VisualElement _info;
        private Image _icon;
        private VisualElement _description;
        private Label _nameLabel;
        private Label _positionLabel;
        private Button _linkButton;
        
        public void Create(VisualElement root)
        {
            _info = root.Q<VisualElement>("info");
            _icon = root.Q<Image>("icon");
            _description = root.Q<VisualElement>("description");
            _nameLabel = root.Q<Label>("name");
            _positionLabel = root.Q<Label>("position");
            _linkButton = root.Q<Button>("link");

            RegisterCallbacks();
        }
        
        private void Destroy() 
            => UnregisterCallbacks();
        
        private void RegisterCallbacks() 
            => _linkButton.clicked += OnLinkButton;

        private void UnregisterCallbacks() 
            => _linkButton.clicked -= OnLinkButton;
        
        
        private void OnLinkButton() 
            => ShowLinkModal();
        
        private void ShowLinkModal()
        {
            /*ModalContext context = ModalSettingsExtensions.CreateContext(_settings.linkModal, _viewModel.Link(url));
            _viewModel.ShowModal(context);*/
        }
    }
}