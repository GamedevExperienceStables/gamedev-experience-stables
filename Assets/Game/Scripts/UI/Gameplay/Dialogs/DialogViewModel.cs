using System;
using Game.Dialog;
using VContainer;

namespace Game.UI
{
    public class DialogViewModel
    {
        private readonly DialogService _dialogService;

        [Inject]
        public DialogViewModel(DialogService dialogService)
            => _dialogService = dialogService;

        public void SubscribeDialogRequested(Action<DialogData> callback)
            => _dialogService.Showing += callback;

        public void UnSubscribeDialogRequested(Action<DialogData> callback)
            => _dialogService.Showing -= callback;

        public void SubscribeDialogClosing(Action callback)
            => _dialogService.Closing += callback;

        public void UnSubscribeDialogClosing(Action callback)
            => _dialogService.Closing -= callback;

        public void DialogClosed() 
            => _dialogService.ClearDialog();
    }
}