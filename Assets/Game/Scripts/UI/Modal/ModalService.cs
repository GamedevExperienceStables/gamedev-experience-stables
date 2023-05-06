using System;

namespace Game.UI
{
    public class ModalService
    {
        public event Action<ModalContext> Requested;
        public event Action Closing;
        
        public bool IsOpen { get; private set; }

        public void Request(ModalContext context)
        {
            Requested?.Invoke(context);
            IsOpen = true;
        }

        public void ForceClose()
        {
            Closing?.Invoke();
            IsOpen = false;
        }
    }
}