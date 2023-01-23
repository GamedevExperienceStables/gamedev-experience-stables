using Game.Persistence;
using VContainer;

namespace Game.GameFlow
{
    public class NewGameState : GameState
    {
        private readonly PersistenceService _persistenceService;

        [Inject]
        public NewGameState(PersistenceService persistenceService) 
            => _persistenceService = persistenceService;

        protected override void OnEnter()
        {
            _persistenceService.InitData();

            Parent.EnterState<IntroState>();
        }

        protected override void OnExit()
        {
        }
    }
}