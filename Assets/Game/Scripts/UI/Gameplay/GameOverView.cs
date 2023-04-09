using System;
using Cysharp.Threading.Tasks;
using Game.Localization;
using Game.Utils;
using UnityEngine;
using UnityEngine.Localization;
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
        
        private Label _caption;
        private Label _description;
        private Button _buttonRestart;
        private Button _buttonMainMenu;
        
        private TimeSpan _showDuration;
        private TimeSpan _hideDuration;
        
        private ILocalizationService _localization;
        private Settings _settings;

        [Inject]
        public void Construct(GameplayViewModel viewModel, ILocalizationService localization, Settings settings)
        {
            _viewModel = viewModel;
            _localization = localization;
            _settings = settings;
        }

        private void Awake()
        {
            _showDuration = TimeSpan.FromSeconds(SHOW_DURATION);
            _hideDuration = TimeSpan.FromSeconds(HIDE_DURATION);
            
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            _container = root.Q<VisualElement>(LayoutNames.GameOver.CONTAINER);
            _caption = root.Q<Label>(LayoutNames.GameOver.CAPTION);
            _description = root.Q<Label>(LayoutNames.GameOver.DESCRIPTION);
            _buttonRestart = root.Q<Button>(LayoutNames.GameOver.BUTTON_RESTART);
            _buttonMainMenu = root.Q<Button>(LayoutNames.GameOver.BUTTON_MAIN_MENU);

            _buttonRestart.clicked += RestartGame;
            _buttonMainMenu.clicked += GoToMainMenu;
            
            _localization.Changed += OnLocalisationChanged;
        }
        
        private void Start()
            => UpdateText();

        private void OnDestroy()
        {
            _buttonRestart.clicked -= RestartGame;
            _buttonMainMenu.clicked -= GoToMainMenu;
            
            _localization.Changed -= OnLocalisationChanged;
        }
        
        private void OnLocalisationChanged()
            => UpdateText();

        private void RestartGame() 
            => _viewModel.RestartGame();

        private void GoToMainMenu() 
            => _viewModel.GoToMainMenu();


        public void HideImmediate()
        {
            _container.SetDisplay(false);
            _container.SetOpacity(0f);
        }
        
        private void UpdateText()
        {
            _caption.text = _settings.caption.label.GetLocalizedString();
            _description.text = _settings.description.label.GetLocalizedString();
            _buttonRestart.text = _settings.lastSave.label.GetLocalizedString();
            _buttonMainMenu.text =_settings.mainMenu.label.GetLocalizedString();
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
        
        [Serializable]
        public class Settings
        {
            [Serializable]
            public struct Page
            {
                public LocalizedString label;
            }

            public Page caption;
            public Page description;
            public Page lastSave;
            public Page mainMenu;
        }
    }
}