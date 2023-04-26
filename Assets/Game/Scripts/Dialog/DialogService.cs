using System;
using System.Collections.Generic;

namespace Game.Dialog
{
    public class DialogService
    {
        private readonly List<DialogData> _stack = new();

        public event Action<DialogData> Showing;
        public event Action Closing;

        public void ShowRequest(DialogData dialogToShow)
        {
            _stack.Add(dialogToShow);
            Showing?.Invoke(dialogToShow);
        }

        public void CloseRequest(DialogData dialogToClose)
        {
            bool activeClosed = _stack.IndexOf(dialogToClose) == _stack.Count - 1;

            if (!_stack.Remove(dialogToClose))
                return;

            bool hasOther = _stack.Count > 0;
            if (hasOther)
            {
                if (!activeClosed)
                    return;

                DialogData next = _stack[^1];
                Showing?.Invoke(next);
            }
            else
            {
                Closing?.Invoke();
            }
        }

        public void ClearDialog()
            => _stack.Clear();
    }
}