using System;
using Cysharp.Threading.Tasks;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using VContainer;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class InventoryView : MonoBehaviour
    {
        private const int OFFSET_BOTTOM = -100;

        [SerializeField, Min(0f)]
        private float showDuration = 0.4f;

        [SerializeField, Min(0f)]
        private float hideDuration = 0.2f;

        private GameplayViewModel _viewModel;

        private VisualElement _container;
        private Button _buttonClose;

        private TimeSpan _showDuration;
        private TimeSpan _hideDuration;

        [Inject]
        public void Construct(GameplayViewModel viewModel)
            => _viewModel = viewModel;

        private void Awake()
        {
            _showDuration = TimeSpan.FromSeconds(showDuration);
            _hideDuration = TimeSpan.FromSeconds(hideDuration);

            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            _container = root.Q<VisualElement>(LayoutNames.Inventory.CONTAINER);
            _buttonClose = root.Q<Button>(LayoutNames.Inventory.BUTTON_CLOSE);

            _buttonClose.clicked += OnCloseClicked;
        }

        private void OnDestroy()
        {
            _buttonClose.clicked -= OnCloseClicked;
        }

        private void OnCloseClicked()
            => _viewModel.CloseInventory();

        public void HideImmediate()
        {
            _container.SetDisplay(false);
            _container.SetOpacity(0f);
            _container.style.bottom = OFFSET_BOTTOM;
        }

        public UniTask ShowAsync()
        {
            FadeIn(_showDuration);
            return UniTask.Delay(_showDuration);
        }

        public UniTask HideAsync()
        {
            FadeOut(_hideDuration);
            return UniTask.Delay(_hideDuration);
        }

        private void FadeIn(TimeSpan duration)
        {
            _container.SetDisplay(true);
            _container.experimental.animation
                .Start(new StyleValues { opacity = 1f, bottom = 0 }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InOutSine);
        }

        private void FadeOut(TimeSpan duration)
        {
            _container.experimental.animation
                .Start(new StyleValues { opacity = 0f, bottom = OFFSET_BOTTOM }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InOutSine)
                .OnCompleted(() => _container.SetDisplay(false));
        }
    }
}