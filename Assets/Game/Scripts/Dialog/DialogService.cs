using System;
using System.Collections.Generic;

namespace Game.Dialog
{
    public class DialogService
    {
        private readonly Stack<DialogData> _stack = new();

        public event Action Showing;
        public event Action Closing;

        public void ShowRequest(DialogData data)
        {
            _stack.Push(data);

            if (_stack.Count == 1)
                Showing?.Invoke();
        }

        public bool PopDialog(out DialogData dialog)
            => _stack.TryPop(out dialog);

        public void CloseRequest() 
            => Closing?.Invoke();
    }
}