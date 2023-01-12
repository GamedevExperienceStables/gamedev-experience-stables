using System;
using Game.Input;
using Game.UI;
using VContainer;
using VContainer.Unity;

namespace Game.GameFlow
{
    public sealed class InGameInputListener : IStartable, IDisposable
    {
        private readonly IInputControlGameplay _inputGameplay;
        private readonly IInputControlMenu _inputMenu;

        private readonly GameplayViewModel _viewModel;

        [Inject]
        public InGameInputListener(
            IInputControlGameplay inputGameplay,
            IInputControlMenu inputMenu,
            GameplayViewModel viewModel
        )
        {
            _inputGameplay = inputGameplay;
            _inputMenu = inputMenu;
            _viewModel = viewModel;
        }

        public void Start()
        {
            _inputGameplay.MenuButton.Performed += Pause;

            _inputMenu.CancelButton.Performed += Resume;
            _inputMenu.ExitButton.Performed += Resume;
        }

        public void Dispose()
        {
            _inputGameplay.MenuButton.Performed -= Pause;

            _inputMenu.CancelButton.Performed -= Resume;
            _inputMenu.ExitButton.Performed -= Resume;
        }

        private void Pause() => _viewModel.PauseGame();
        private void Resume() => _viewModel.ResumeGame();
    }
}