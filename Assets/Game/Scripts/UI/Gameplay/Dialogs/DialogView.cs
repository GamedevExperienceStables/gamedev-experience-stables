using System;
using Game.Dialog;
using Game.Level;
using Game.TimeManagement;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class DialogView
    {
        private static readonly TimeSpan HideDelay = TimeSpan.FromSeconds(2);
        private static readonly TimeSpan ShowDelay = TimeSpan.FromSeconds(0.3);

        private readonly DialogViewModel _viewModel;
        private readonly TimerPool _timers;

        private TimerUpdatable _hideTimer;

        private VisualElement _dialog;

        private Label _title;
        private Label _text;
        private TimerUpdatable _delayTimer;
        private DialogData? _data;

        [Inject]
        public DialogView(DialogViewModel viewModel, TimerPool timers)
        {
            _viewModel = viewModel;
            _timers = timers;
        }

        public void Create(VisualElement root)
        {
            _dialog = root.Q<VisualElement>(LayoutNames.Hud.DIALOG_WINDOW);
            _title = _dialog.Q<Label>(LayoutNames.Hud.DIALOG_TITLE);
            _text = _dialog.Q<Label>(LayoutNames.Hud.DIALOG_TEXT);

            _viewModel.SubscribeDialogRequested(OnDialogRequested);
            _viewModel.SubscribeDialogClosing(OnDialogClosing);

            _hideTimer = _timers.GetTimer(HideDelay, OnDialogComplete);
            _delayTimer = _timers.GetTimer(ShowDelay, ShowDialog);

            HideDialog();
        }

        public void Destroy()
        {
            _timers.ReleaseTimer(_hideTimer);
            _timers.ReleaseTimer(_delayTimer);

            _viewModel.UnSubscribeDialogRequested(OnDialogRequested);
            _viewModel.UnSubscribeDialogClosing(OnDialogClosing);
        }

        private void OnDialogRequested(DialogData nextDialog)
        {
            if (_data == nextDialog)
            {
                _hideTimer.Stop();
                return;
            }

            bool inTransition = !_hideTimer.IsDone || !_delayTimer.IsDone;
            if (!_data.HasValue && !inTransition)
            {
                _hideTimer.Stop();
                ShowDialog(nextDialog);
                return;
            }

            _hideTimer.Stop();

            HideDialog();
            ShowDialogWithDelay(nextDialog);
        }

        private void ShowDialog()
        {
            if (!_data.HasValue)
            {
                Debug.LogWarning("Trying to show dialog without data");
                return;
            }

            DialogData data = _data.Value;
            if (string.IsNullOrEmpty(data.Definition.Single.Text))
            {
                Debug.LogWarning($"Dialog {data.Definition} has no text");
                return;
            }

            SetupDialog(data.Definition);

            _dialog.RemoveFromClassList(LayoutNames.Hud.DIALOG_HIDDEN_CLASS_NAME);

            if (data.OneShot)
                _hideTimer.Start();
        }

        private void ShowDialog(DialogData newData)
        {
            _data = newData;

            ShowDialog();
        }

        private void ShowDialogWithDelay(DialogData data)
        {
            _data = data;

            _delayTimer.Start();
        }

        private void OnDialogClosing(bool immediate)
        {
            if (immediate)
                CloseDialog();
            else
                _hideTimer.Start();
        }

        private void OnDialogComplete()
            => CloseDialog();

        private void CloseDialog()
        {
            _data = null;
            _viewModel.DialogClosed();

            HideDialog();
        }

        private void SetupDialog(IDialogDefinition dialog)
        {
            _text.text = dialog.Single.Text;

            if (string.IsNullOrEmpty(dialog.Single.Title))
                _title.SetDisplay(false);
            else
            {
                _title.SetDisplay(true);
                _title.text = dialog.Single.Title;
            }
        }

        private void HideDialog()
            => _dialog.AddToClassList(LayoutNames.Hud.DIALOG_HIDDEN_CLASS_NAME);
    }
}