using Game.Utils;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class EmployeeView
    {
        private readonly AboutViewModel _viewModel;
        private readonly ModalSettings _linkModal;

        private VisualElement _info;
        private Image _icon;
        private VisualElement _description;
        private Label _nameLabel;
        private Label _positionLabel;
        private Button _linkButton;

        private string _url;

        public EmployeeView(AboutViewModel viewModel, AboutSettings settings)
        {
            _viewModel = viewModel;
            _linkModal = settings.LinkModal;
        }

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

        public void Destroy()
            => UnregisterCallbacks();

        public void SetData(EmployeeData employeeData, bool isEven)
        {
            if (!isEven)
            {
                _info.AddToClassList("employee--reversed");
                _description.AddToClassList("description--reversed");
            }
            
            _url = employeeData.url;

            bool isEmpty = string.IsNullOrEmpty(_url);
            _linkButton.SetDisplay(!isEmpty);

            _nameLabel.text = employeeData.name;
            _positionLabel.text = employeeData.position;

            if (employeeData.icon)
            {
                _icon.sprite = employeeData.icon;
                _icon.style.backgroundImage = default;
            }
        }

        private void RegisterCallbacks()
            => _linkButton.clicked += OnLinkButton;

        private void UnregisterCallbacks()
            => _linkButton.clicked -= OnLinkButton;


        private void OnLinkButton()
            => ShowLinkModal();

        private void ShowLinkModal()
        {
            ModalContext context = ModalSettingsExtensions.CreateContext(_linkModal, OpenLink);
            context.message = _url;
            
            _viewModel.ShowModal(context);
        }

        private void OpenLink()
            => _viewModel.OpenURL(_url);
    }
}