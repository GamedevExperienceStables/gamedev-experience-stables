using System;

namespace Game.UI
{
    public struct ModalContext
    {
        public string title;
        public string message;
        public Action onConfirm;
        public Action onCancel;
        public ModalStyle style;
    }
}