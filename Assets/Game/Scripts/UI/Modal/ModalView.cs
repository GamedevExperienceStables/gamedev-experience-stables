using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class ModalView : MonoBehaviour
    {
        private ModalViewModel _viewModel;
        private VisualElement _root;

        private Button _buttonConfirm;
        private Button _buttonCancel;

        private VisualElement _blockMessage;
        private Label _textTitle;
        private Label _textMessage;

        private ModalContext _currentContext;

        [Inject]
        public void Construct(ModalViewModel viewModel)
            => _viewModel = viewModel;

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            var container = _root.Q<VisualElement>(LayoutNames.Modal.CONTAINER);
            _buttonConfirm = container.Q<Button>(LayoutNames.Modal.BUTTON_CONFIRM);
            _buttonCancel = container.Q<Button>(LayoutNames.Modal.BUTTON_CANCEL);

            _textTitle = container.Q<Label>(LayoutNames.Modal.TITLE);
            _blockMessage = container.Q<VisualElement>(LayoutNames.Modal.BLOCK_MESSAGE);
            _textMessage = container.Q<Label>(LayoutNames.Modal.TEXT_MESSAGE);

            Hide();
        }

        private void Start()
        {
            _viewModel.SubscribeRequest(OnShow);
            _viewModel.SubscribeForceClose(OnForceClose);

            _buttonConfirm.clicked += OnConfirm;
            _buttonCancel.clicked += OnCancel;
        }

        private void OnDestroy()
        {
            _viewModel.UnsubscribeRequest(OnShow);
            _viewModel.UnsubscribeForceClose(OnForceClose);

            _buttonConfirm.clicked -= OnConfirm;
            _buttonCancel.clicked -= OnCancel;
        }

        private void OnConfirm()
        {
            _currentContext.onConfirm?.Invoke();
            Hide();
        }

        private void OnCancel()
        {
            _currentContext.onCancel?.Invoke();
            Hide();
        }


        private void OnForceClose()
            => Hide();

        private void OnShow(ModalContext context)
        {
            _currentContext = context;

            Setup(context);
            Show();
        }

        private void Setup(ModalContext context)
        {
            _textTitle.text = context.title;

            if (string.IsNullOrEmpty(context.message))
            {
                _blockMessage.SetDisplay(false);
                _textMessage.text = string.Empty;
            }
            else
            {
                _blockMessage.SetDisplay(true);
                _textMessage.text = context.message;
            }
        }

        private void Show()
            => _root.SetDisplay(true);

        private void Hide()
        {
            _root.SetDisplay(false);

            CleanUp();
        }

        private void CleanUp()
            => _currentContext = default;
    }
}