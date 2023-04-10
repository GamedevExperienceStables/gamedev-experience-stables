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
    public class GameOverView : MonoBehaviour
    {
        private const float DELAY = 0.8f;
        
        private const float SHOW_DURATION = 1.2f;
        private const float HIDE_DURATION = 0.2f;

        private VisualElement _container;
        private GameplayViewModel _viewModel;
        
        private Button _buttonRestart;
        private Button _buttonMainMenu;
        
        private TimeSpan _showDuration;
        private TimeSpan _hideDuration;

        [Inject]
        public void Construct(GameplayViewModel viewModel)
            => _viewModel = viewModel;
        
        private void Awake()
        {
            _showDuration = TimeSpan.FromSeconds(SHOW_DURATION);
            _hideDuration = TimeSpan.FromSeconds(HIDE_DURATION);
            
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            _container = root.Q<VisualElement>(LayoutNames.GameOver.CONTAINER);
            _buttonRestart = root.Q<Button>(LayoutNames.GameOver.BUTTON_RESTART);
            _buttonMainMenu = root.Q<Button>(LayoutNames.GameOver.BUTTON_MAIN_MENU);

            _buttonRestart.clicked += RestartGame;
            _buttonMainMenu.clicked += GoToMainMenu;
        }

        private void OnDestroy()
        {
            _buttonRestart.clicked -= RestartGame;
            _buttonMainMenu.clicked -= GoToMainMenu;
        }

        private void RestartGame() 
            => _viewModel.RestartGame();

        private void GoToMainMenu() 
            => _viewModel.GoToMainMenu();


        public void HideImmediate()
        {
            _container.SetDisplay(false);
            _container.SetOpacity(0f);
        }
        
        public async UniTask ShowAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(DELAY), true);
            FadeIn(_showDuration);
            
            await UniTask.Delay(_showDuration, true);
        }

        public UniTask HideAsync()
        {
            FadeOut(_hideDuration);
            return UniTask.Delay(_hideDuration, true);
        }

        private void FadeIn(TimeSpan duration)
        {
            _container.SetDisplay(true);
            _container.experimental.animation
                .Start(new StyleValues { opacity = 1f }, (int)duration.TotalMilliseconds);
        }

        private void FadeOut(TimeSpan duration)
        {
            _container.experimental.animation
                .Start(new StyleValues { opacity = 0f }, (int)duration.TotalMilliseconds)
                .OnCompleted(() => _container.SetDisplay(false));
        }
    }
}