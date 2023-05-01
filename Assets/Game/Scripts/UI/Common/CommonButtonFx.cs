using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class CommonButtonFx
    {
        private readonly UiFxService _uiFx;
        private readonly Settings _settings;

        public CommonButtonFx(UiFxService uiFx, Settings settings)
        {
            _uiFx = uiFx;
            _settings = settings;
        }

        public void RegisterButton(Button button)
        {
            button.RegisterCallback<NavigationMoveEvent>(OnNavigationMove);
            button.RegisterCallback<PointerEnterEvent>(OnHover);

            button.clicked += OnClick;
        }

        public void UnRegisterButton(Button button)
        {
            button.UnregisterCallback<NavigationMoveEvent>(OnNavigationMove);
            button.UnregisterCallback<PointerEnterEvent>(OnHover);

            button.clicked -= OnClick;
        }

        private void OnNavigationMove(NavigationMoveEvent evt) 
            => OnHover(evt.target as VisualElement);

        private void OnHover(PointerEnterEvent evt)
            => OnHover(evt.target as VisualElement);

        private void OnHover(VisualElement element)
        {
            if (!element.enabledInHierarchy)
                return;

            _uiFx.Play(_settings.hoverFeedback);
        }

        private void OnClick()
            => _uiFx.Play(_settings.clickFeedback);


        [Serializable]
        public class Settings
        {
            public GameObject hoverFeedback;
            public GameObject clickFeedback;
        }
    }
}