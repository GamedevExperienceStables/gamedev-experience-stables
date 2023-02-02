using Game.GameFlow;
using Game.Persistence;
using VContainer;

namespace Game.UI
{
    public class StartMenuViewModel
    {
        private readonly RootStateMachine _rootStateMachine;
        private readonly PersistenceService _persistence;

        [Inject]
        public StartMenuViewModel(
            RootStateMachine rootStateMachine,
            PersistenceService persistence
        )
        {
            _rootStateMachine = rootStateMachine;
            _persistence = persistence;
        }

        public bool IsSaveGameExists()
            => _persistence.IsSaveGameExists();

        public void NewGame()
            => _rootStateMachine.EnterState<NewGameState>();

        public void QuitGame()
            => _rootStateMachine.EnterState<QuitGameState>();

        public void ContinueGame()
            => _rootStateMachine.EnterState<LoadGameState>();
    }
}