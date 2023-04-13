using System;
using Game.Dialog;
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
        private DialogData _data;

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

        private void OnDialogRequested()
        {
            if (!_viewModel.TryDequeueDialog(out DialogData data))
                return;

            if (_hideTimer.IsDone)
            {
                ShowDialog(data);
                return;
            }

            _hideTimer.Stop();

            if (IsSameDialog(data))
                return;

            HideDialog();
            ShowDialogWithDelay(data);
        }

        private bool IsSameDialog(DialogData data)
            => ReferenceEquals(_data.Definition, data.Definition);

        private void ShowDialog()
        {
            if (_data.Definition.Single.Text.IsEmpty)
            {
                Debug.LogWarning($"Dialog {_data.Definition.name} has no text");
                return;
            }

            _hideTimer.Stop();

            SetupDialog();

            _dialog.RemoveFromClassList(LayoutNames.Hud.DIALOG_HIDDEN_CLASS_NAME);
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

        private void OnDialogClosing()
            => _hideTimer.Start();

        private void OnDialogComplete()
            => HideDialog();

        private void SetupDialog()
        {
            _text.text = _data.Definition.Single.Text.GetLocalizedString();

            if (_data.Definition.Single.Title.IsEmpty)
                _title.SetDisplay(false);
            else
            {
                _title.SetDisplay(true);
                _title.text = _data.Definition.Single.Title.GetLocalizedString();
            }
        }

        private void HideDialog()
            => _dialog.AddToClassList(LayoutNames.Hud.DIALOG_HIDDEN_CLASS_NAME);
    }
}