using Cysharp.Threading.Tasks;
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

        protected override UniTask OnEnter()
        {
            // show confirmation popup
            // ...

            // On confirm Quit
            Quit();
            
            return UniTask.CompletedTask;
        }

        private void Quit()
            => _quitService.Quit();
    }
}