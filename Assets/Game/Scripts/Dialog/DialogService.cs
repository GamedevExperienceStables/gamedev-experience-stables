using System;
using System.Collections.Generic;

namespace Game.Dialog
{
    public class DialogService
    {
        private readonly Queue<DialogData> _stack = new();

        public event Action Showing;
        public event Action Closing;

        public void ShowRequest(DialogData data)
        {
            _stack.Enqueue(data);

            if (_stack.Count == 1)
                Showing?.Invoke();
        }

        public bool TryDequeueDialog(out DialogData dialog)
            => _stack.TryDequeue(out dialog);

        public void CloseRequest() 
            => Closing?.Invoke();
    }
}