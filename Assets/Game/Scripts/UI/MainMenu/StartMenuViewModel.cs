using Game.GameFlow;
using Game.Persistence;
using VContainer;

namespace Game.UI
{
    public class StartMenuViewModel
    {
        private readonly MainMenuViewRouter _router;
        private readonly RootStateMachine _rootStateMachine;
        private readonly PersistenceService _persistence;

        [Inject]
        public StartMenuViewModel(
            MainMenuViewRouter router,
            RootStateMachine rootStateMachine,
            PersistenceService persistence
        )
        {
            _router = router;
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

        public void OpenArt()
            => _router.OpenArt();

        public void OpenAbout()
            => _router.OpenAbout();

        public void OpenSettings()
            => _router.OpenSettings();
    }
}