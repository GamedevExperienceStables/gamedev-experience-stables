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
            button.RegisterCallback<FocusInEvent>(OnHover);
            button.RegisterCallback<PointerEnterEvent>(OnHover);
            
            button.clicked += OnClick;
        }

        public void UnRegisterButton(Button button)
        {
            button.UnregisterCallback<FocusInEvent>(OnHover);
            button.UnregisterCallback<PointerEnterEvent>(OnHover);
            
            button.clicked -= OnClick;
        }

        private void OnHover(FocusInEvent evt)
            => OnHover();

        private void OnHover(PointerEnterEvent evt)
            => OnHover();

        private void OnHover()
            => _uiFx.Play(_settings.hoverFeedback);

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