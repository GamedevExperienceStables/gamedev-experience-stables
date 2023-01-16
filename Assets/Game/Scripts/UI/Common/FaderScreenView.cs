using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class FaderScreenView : MonoBehaviour, IFaderScreen
    {
        private VisualElement _fader;

        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            _fader = root.Q<VisualElement>("fader");
        }

        public void FadeIn(TimeSpan duration)
        {
            FadeIn(1f, TimeSpan.FromSeconds(1));
        }

        public void FadeIn(float opacity, TimeSpan duration)
        {
            _fader.style.opacity = 0f;
            _fader.experimental.animation
                .Start(new StyleValues { opacity = opacity }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InCubic);
        }

        public void FadeOut()
        {
            FadeOut(TimeSpan.FromSeconds(1));
        }

        public void FadeOut(TimeSpan duration)
        {
            _fader.style.opacity = 1f;
            _fader.experimental.animation
                .Start(new StyleValues { opacity = 0f }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InCubic);
        }
    }
}