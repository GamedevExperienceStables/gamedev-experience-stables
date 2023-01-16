using VContainer;

namespace Game.GameFlow
{
    public class QuitGameState : GameState
    {
        private readonly QuitGameService _quitService;

        [Inject]
        public QuitGameState(QuitGameService quitService)
        {
            _quitService = quitService;
        }

        protected override void OnEnter()
        {
            // show confirmation popup
            // ...
            
            // On confirm Quit
            Quit();
        }

        private void Quit() 
            => _quitService.Quit();

        protected override void OnExit()
        {
            
        }
    }
}