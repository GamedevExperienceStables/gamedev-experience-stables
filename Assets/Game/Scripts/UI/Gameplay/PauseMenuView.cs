using System;
using Cysharp.Threading.Tasks;
using Game.Level;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using VContainer;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class PauseMenuView : MonoBehaviour
    {
        [SerializeField]
        private float showDuration = 0.2f;

        [SerializeField]
        private float hideDuration = 0.2f;

        private Button _buttonResume;
        private Button _buttonMainMenu;

        private LocationController _level;

        private GameplayViewModel _viewModel;
        private VisualElement _container;

        private TimeSpan _showDuration;
        private TimeSpan _hideDuration;

        [Inject]
        public void Construct(GameplayViewModel viewModel)
            => _viewModel = viewModel;

        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            _container = root.Q<VisualElement>(LayoutNames.PauseMenu.CONTAINER);
            _buttonResume = root.Q<Button>(LayoutNames.PauseMenu.BUTTON_RESUME);
            _buttonMainMenu = root.Q<Button>(LayoutNames.PauseMenu.BUTTON_MAIN_MENU);

            _buttonResume.clicked += ResumeGame;
            _buttonMainMenu.clicked += GoToMainMenu;

            _showDuration = TimeSpan.FromSeconds(showDuration);
            _hideDuration = TimeSpan.FromSeconds(hideDuration);
        }

        private void OnDestroy()
        {
            _buttonResume.clicked -= ResumeGame;
            _buttonMainMenu.clicked -= GoToMainMenu;
        }

        public void Show()
            => FadeIn(_showDuration);

        public void Hide()
            => FadeOut(_hideDuration);

        public void ShowImmediate()
            => _container.SetDisplay(true);

        public void HideImmediate()
            => _container.SetDisplay(false);

        public UniTask ShowAsync()
        {
            Show();
            return UniTask.Delay(_showDuration, true);
        }

        public UniTask HideAsync()
        {
            Hide();
            return UniTask.Delay(_hideDuration, true);
        }

        private void ResumeGame()
            => _viewModel.ResumeGame();

        private void GoToMainMenu()
            => _viewModel.GoToMainMenu();

        private void FadeIn(TimeSpan duration)
        {
            _container.SetDisplay(true);
            _container.experimental.animation
                .Start(new StyleValues { opacity = 1f }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InCubic)
                .OnCompleted(() => _buttonResume.Focus());
        }

        private void FadeOut(TimeSpan duration)
        {
            _container.experimental.animation
                .Start(new StyleValues { opacity = 0f }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InCubic)
                .OnCompleted(() => { _container.SetDisplay(false); });
        }
    }
}