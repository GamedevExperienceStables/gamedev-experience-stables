using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Utils;
using NaughtyAttributes;
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

        private Image _icon;
        private IVisualElementScheduledItem _iconScheduler;

        private int _iconFrame;

        [Inject]
        public void Construct(Settings settings)
            => _settings = settings;

        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            _container = root.Q<VisualElement>(LayoutNames.LoadingScreen.CONTAINER);
            _container.SetOpacity(0f);

            _label = _container.Q<Label>(LayoutNames.LoadingScreen.LOADING_LABEL);

            _icon = _container.Q<Image>(LayoutNames.LoadingScreen.LOADING_ICON);
            _icon.sprite = _settings.animation[0];
            _iconScheduler = _icon.schedule.Execute(UpdateLoadingIcon).Every(_settings.animationIntervalMs);

            _showDuration = TimeSpan.FromSeconds(showDuration);
            _hideDuration = TimeSpan.FromSeconds(hideDuration);

            Stop();
        }

        [Button(enabledMode: EButtonEnableMode.Editor)]
        public void Show()
        {
            if (_isActive)
                return;

            UpdateText();
            PlayIconAnimation();

            FadeIn(_showDuration);
            _isActive = true;
        }

        private void UpdateText()
        {
            if (_label is not null)
                _label.text = _settings.loadingText.GetLocalizedString();
        }

        [Button(enabledMode: EButtonEnableMode.Editor)]
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
            _icon.AddToClassList(LayoutNames.LoadingScreen.LOADING_ICON_VISIBLE_CLASS_NAME);
            _container.RemoveFromClassList(LayoutNames.LoadingScreen.LOADING_HIDDEN_CLASS_NAME);

            _container.experimental.animation
                .Start(new StyleValues { opacity = 1f }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InCubic);
        }

        private void FadeOut(TimeSpan duration)
        {
            _container.experimental.animation
                .Start(new StyleValues { opacity = 0f }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InCubic)
                .OnCompleted(Stop);
        }

        private void Stop()
        {
            _container.AddToClassList(LayoutNames.LoadingScreen.LOADING_HIDDEN_CLASS_NAME);
            _icon.RemoveFromClassList(LayoutNames.LoadingScreen.LOADING_ICON_VISIBLE_CLASS_NAME);

            PauseIconAnimation();
        }

        private void PlayIconAnimation()
            => _iconScheduler.Resume();

        private void PauseIconAnimation()
            => _iconScheduler.Pause();

        private void UpdateLoadingIcon()
        {
            _iconFrame = (_iconFrame + 1) % _settings.animation.Count;
            
            _icon.sprite = _settings.animation[_iconFrame];
        }


        [Serializable]
        public class Settings
        {
            public LocalizedString loadingText;

            [Header("Animation")]
            public int animationIntervalMs = 30;

            public List<Sprite> animation;
        }
    }
}