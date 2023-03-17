using System;
using Cysharp.Threading.Tasks;
using Game.Utils;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class PauseView : MonoBehaviour
    { 
        [SerializeField]
        private float showDuration = 0.2f;

        [SerializeField]
        private float hideDuration = 0.2f;

        [Header("Blur")]
        [SerializeField]
        private Volume globalVolume;

        private VisualElement _container;

        private TimeSpan _showDuration;
        private TimeSpan _hideDuration;

        private BlurHandler _blurHandler;
        
        private PauseMenuView _menu;
        private PauseSettingsView _settings;
        
        private PauseViewRouter _router;

        private void Awake()
        {
            _router = GetComponent<PauseViewRouter>();
            
            _blurHandler = new BlurHandler(globalVolume);

            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            _container = root.Q<VisualElement>(LayoutNames.Pause.CONTAINER);
            
            _showDuration = TimeSpan.FromSeconds(showDuration);
            _hideDuration = TimeSpan.FromSeconds(hideDuration);
            
            _router.OpenMenu();
        }

        public void ShowImmediate()
            => _container.SetDisplay(true);

        public void HideImmediate()
        {
            _container.SetDisplay(false);
            _container.SetOpacity(0f);
            
            _blurHandler.HideImmediate();
        }

        public UniTask ShowAsync()
        {
            FadeIn(_showDuration);
            return UniTask.Delay(_showDuration, true);
        }

        public async UniTask HideAsync()
        {
            FadeOut(_hideDuration);
            await UniTask.Delay(_hideDuration, true);
            
            _router.ToRoot();
        }

        private void FadeIn(TimeSpan duration)
        {
            _blurHandler.FadeIn(duration);

            _container.SetDisplay(true);
            _container.experimental.animation
                .Start(new StyleValues { opacity = 1f }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InCubic);
        }

        private void FadeOut(TimeSpan duration)
        {
            _blurHandler.FadeOut(duration);

            _container.experimental.animation
                .Start(new StyleValues { opacity = 0f }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InCubic)
                .OnCompleted(() => _container.SetDisplay(false));
        }
    }
}