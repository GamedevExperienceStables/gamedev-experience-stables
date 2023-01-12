using Game.GameFlow;
using VContainer;

namespace Game.UI
{
    public class StartMenuViewModel
    {
        private readonly RootStateMachine _rootStateMachine;

        [Inject]
        public StartMenuViewModel(RootStateMachine rootStateMachine)
        {
            _rootStateMachine = rootStateMachine;
        }

        public void NewGame()
        {
            _rootStateMachine.EnterState<NewGameState>();
        }

        public void QuitGame()
        {
            _rootStateMachine.EnterState<QuitGameState>();
        }
    }
}