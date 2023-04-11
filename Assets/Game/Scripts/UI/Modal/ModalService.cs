using System;

namespace Game.UI
{
    public class ModalService
    {
        public event Action<ModalContext> Requested;
        public event Action Closing;

        public void Request(ModalContext context)
            => Requested?.Invoke(context);

        public void ForceClose()
            => Closing?.Invoke();
    }
}