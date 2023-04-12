namespace Game.UI
{
    public class PauseMenuViewModel
    {
        private readonly GameplayViewModel _gameplay;
        private readonly ModalService _modal;

        public PauseMenuViewModel(GameplayViewModel gameplay, ModalService modal)
        {
            _gameplay = gameplay;
            _modal = modal;
        }

        public void ResumeGame()
            => _gameplay.ResumeGame();

        public void GoToMainMenu()
            => _gameplay.GoToMainMenu();

        public void ShowModal(ModalContext context)
            => _modal.Request(context);
    }
}