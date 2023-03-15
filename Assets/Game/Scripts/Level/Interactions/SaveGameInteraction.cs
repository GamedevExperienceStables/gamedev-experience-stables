using Game.GameFlow;
using Game.Persistence;
using VContainer;

namespace Game.Level
{
    public class SaveGameInteraction : Interaction
    {
        private readonly PlanetStateMachine _gameStateMachine;
        private readonly PersistenceService _persistence;

        [Inject]
        public SaveGameInteraction(PersistenceService persistence, PlanetStateMachine gameStateMachine)
        {
            _persistence = persistence;
            _gameStateMachine = gameStateMachine;
        }

        public override bool CanExecute()
            => !_persistence.IsRunning;

        public override void Execute()
            => _gameStateMachine.PushState<PlanetSaveGameState>();
    }
}