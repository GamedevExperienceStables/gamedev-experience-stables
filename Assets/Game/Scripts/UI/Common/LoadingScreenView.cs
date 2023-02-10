using System;
using Cysharp.Threading.Tasks;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

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

        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            _container = root.Q<VisualElement>(LayoutNames.LoadingScreen.CONTAINER);
            _container.SetOpacity(0f);
            
            _showDuration = TimeSpan.FromSeconds(showDuration);
            _hideDuration = TimeSpan.FromSeconds(hideDuration);
        }

        public void Show()
        {
            if (_isActive)
                return;

            FadeIn(_showDuration);
            _isActive = true;
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
            
            await UniTask.Delay(_showDuration);
            
            _isActive = true;
        }

        public async UniTask HideAsync()
        {
            if (_isActive)
                return;
            
            Hide();
            await UniTask.Delay(_hideDuration);

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
    }
}