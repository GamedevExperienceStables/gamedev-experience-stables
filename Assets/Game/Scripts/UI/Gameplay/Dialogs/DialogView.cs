using Game.Dialog;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class DialogView
    {
        private readonly DialogViewModel _viewModel;

        private VisualElement _dialog;

        private Label _title;
        private Label _text;

        [Inject]
        public DialogView(DialogViewModel viewModel)
            => _viewModel = viewModel;

        public void Create(VisualElement root)
        {
            _dialog = root.Q<VisualElement>(LayoutNames.Hud.DIALOG);
            _title = _dialog.Q<Label>(LayoutNames.Hud.DIALOG_TITLE);
            _text = _dialog.Q<Label>(LayoutNames.Hud.DIALOG_TEXT);

            _viewModel.SubscribeDialogRequested(OnDialogRequested);
            _viewModel.SubscribeDialogClosing(OnDialogClosing);

            HideDialog();
        }

        public void Destroy()
        {
            _viewModel.UnSubscribeDialogRequested(OnDialogRequested);
            _viewModel.UnSubscribeDialogClosing(OnDialogClosing);
        }

        private void OnDialogRequested()
        {
            if (!_viewModel.TryGetDialog(out DialogData data))
                return;

            ShowDialog(data);
        }

        private void OnDialogClosing()
            => OnDialogComplete();

        private void OnDialogComplete()
        {
            if (!_viewModel.TryGetDialog(out DialogData data))
            {
                HideDialog();
                return;
            }

            ShowDialog(data);
        }

        private void ShowDialog(DialogData data)
        {
            if (data.Definition.Single.Text.IsEmpty)
            {
                Debug.LogWarning($"Dialog {data.Definition.name} has no text");
                return;
            }

            _text.text = data.Definition.Single.Text.GetLocalizedString();

            if (data.Definition.Single.Title.IsEmpty)
                _title.SetDisplay(false);
            else
            {
                _title.SetDisplay(true);
                _title.text = data.Definition.Single.Title.GetLocalizedString();
            }

            _dialog.SetVisibility(true);
        }

        private void HideDialog()
            => _dialog.SetVisibility(false);
    }
}