using System;

namespace Game.UI
{
    public class ModalViewModel
    {
        private readonly ModalService _modalService;

        public ModalViewModel(ModalService modalService)
            => _modalService = modalService;

        public void SubscribeRequest(Action<ModalContext> callback)
            => _modalService.Requested += callback;

        public void UnsubscribeRequest(Action<ModalContext> callback)
            => _modalService.Requested -= callback;

        public void SubscribeForceClose(Action callback) 
            => _modalService.Closing += callback;
        
        public void UnsubscribeForceClose(Action callback)
            => _modalService.Closing -= callback;
    }
}