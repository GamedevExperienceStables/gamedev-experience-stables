using System;
using UnityEngine;

namespace Game.UI
{
    public class AboutViewModel
    {
        private readonly MainMenuViewRouter _router;
        private readonly ModalService _modal;

        public AboutViewModel(MainMenuViewRouter router, ModalService modal)
        {
            _router = router;
            _modal = modal;
        }

        public void Back() 
            => _router.Back();

        public void ShowModal(ModalSettings settings, Action confirmCallback)
        {
            ModalContext context = ModalSettingsExtensions.CreateContext(settings, confirmCallback);
            _modal.Request(context);
        }

        public void OpenURL(string url) 
            => Application.OpenURL(url);
    }
}