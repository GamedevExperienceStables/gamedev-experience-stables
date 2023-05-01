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
        private VisualElement _container;

        private CommonFx _commonFx;

        [Inject]
        public void Construct(ModalViewModel viewModel, CommonFx commonFx)
        {
            _viewModel = viewModel;
            _commonFx = commonFx;
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _container = _root.Q<VisualElement>(LayoutNames.Modal.CONTAINER);
            _buttonConfirm = _container.Q<Button>(LayoutNames.Modal.BUTTON_CONFIRM);
            _buttonCancel = _container.Q<Button>(LayoutNames.Modal.BUTTON_CANCEL);

            _textTitle = _container.Q<Label>(LayoutNames.Modal.TITLE);
            _blockMessage = _container.Q<VisualElement>(LayoutNames.Modal.BLOCK_MESSAGE);
            _textMessage = _container.Q<Label>(LayoutNames.Modal.TEXT_MESSAGE);

            Hide();
        }

        private void Start()
        {
            _viewModel.SubscribeRequest(OnShow);
            _viewModel.SubscribeForceClose(OnForceClose);

            _buttonConfirm.clicked += OnConfirm;
            _buttonCancel.clicked += OnCancel;

            _commonFx.RegisterButton(_buttonConfirm, ButtonStyle.Modal);
            _commonFx.RegisterButton(_buttonCancel, ButtonStyle.Modal);
        }

        private void OnDestroy()
        {
            _viewModel.UnsubscribeRequest(OnShow);
            _viewModel.UnsubscribeForceClose(OnForceClose);

            _buttonConfirm.clicked -= OnConfirm;
            _buttonCancel.clicked -= OnCancel;

            _commonFx.UnRegisterButton(_buttonConfirm, ButtonStyle.Modal);
            _commonFx.UnRegisterButton(_buttonCancel, ButtonStyle.Modal);
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

            SetStyle(context);
        }

        private void SetStyle(ModalContext context)
        {
            if (context.style == ModalStyle.Wide)
                SetWideStyle();
            else
                SetNarrowStyle();
        }

        private void SetWideStyle()
            => _container.AddToClassList(LayoutNames.Modal.WIDE_CLASS_NAME);

        private void SetNarrowStyle()
            => _container.RemoveFromClassList(LayoutNames.Modal.WIDE_CLASS_NAME);

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