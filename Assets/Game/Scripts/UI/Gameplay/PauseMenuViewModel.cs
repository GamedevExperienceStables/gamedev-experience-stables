namespace Game.UI
{
    public class PauseMenuViewModel
    {
        private readonly GameplayViewModel _gameplay;

        public PauseMenuViewModel(GameplayViewModel gameplay) 
            => _gameplay = gameplay;

        public void ResumeGame()
            => _gameplay.ResumeGame();

        public void GoToMainMenu()
            => _gameplay.GoToMainMenu();
    }
}