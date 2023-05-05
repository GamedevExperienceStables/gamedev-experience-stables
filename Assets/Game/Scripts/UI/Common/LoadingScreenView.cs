using System;
using Cysharp.Threading.Tasks;
using Game.Utils;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using VContainer;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class LoadingScreenView : MonoBehaviour, ILoadingScreen
    {
        [SerializeField]
        private float showDuration = 0.2f;
        
        [SerializeField]
        private float hideDuration = 0.2f;

        private TimeSpan _showDuration;
        private TimeSpan _hideDuration;

        private VisualElement _container;
        private bool _isActive;

        private Label _label;
        private Settings _settings;

        [Inject]
        public void Construct(Settings settings) 
            => _settings = settings;

        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            _container = root.Q<VisualElement>(LayoutNames.LoadingScreen.CONTAINER);
            _container.SetOpacity(0f);

            _label = _container.Q<Label>(LayoutNames.LoadingScreen.LOADING_LABEL);
            
            _showDuration = TimeSpan.FromSeconds(showDuration);
            _hideDuration = TimeSpan.FromSeconds(hideDuration);
        }

        public void Show()
        {
            if (_isActive)
                return;

            UpdateText();
            
            FadeIn(_showDuration);
            _isActive = true;
        }

        private void UpdateText()
        {
            if (_label is not null) 
                _label.text = _settings.loadingText.GetLocalizedString();
        }

        public void Hide()
        {
            if (!_isActive)
                return;

            FadeOut(_showDuration);
            _isActive = false;
        }

        public async UniTask ShowAsync()
        {
            if (_isActive)
                return;
            
            Show();
            
            await UniTask.Delay(_showDuration, DelayType.UnscaledDeltaTime);
            
            _isActive = true;
        }

        public async UniTask HideAsync()
        {
            if (_isActive)
                return;
            
            Hide();
            await UniTask.Delay(_hideDuration, DelayType.UnscaledDeltaTime);

            _isActive = false;
        }


        private void FadeIn(TimeSpan duration)
        {
            _container.SetDisplay(true);
            _container.experimental.animation
                .Start(new StyleValues { opacity = 1f }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InCubic);
        }

        private void FadeOut(TimeSpan duration)
        {
            _container.experimental.animation
                .Start(new StyleValues { opacity = 0f }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InCubic)
                .OnCompleted(() => _container.SetDisplay(false));
        }
        
        
        [Serializable]
        public class Settings
        {
            public LocalizedString loadingText;
        }
    }
}