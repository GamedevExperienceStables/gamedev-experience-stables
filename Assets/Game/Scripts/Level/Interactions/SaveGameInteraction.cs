using Game.GameFlow;
using Game.Persistence;
using VContainer;

namespace Game.Level
{
    public class SaveGameInteraction : Interaction
    {
        private readonly PlanetStateMachine _gameStateMachine;
        private readonly PersistenceService _persistence;
        
        private SavePoint _savePoint;

        [Inject]
        public SaveGameInteraction(PersistenceService persistence, PlanetStateMachine gameStateMachine)
        {
            _persistence = persistence;
            _gameStateMachine = gameStateMachine;
        }

        public override void OnCreate() 
            => _savePoint = Source.GetComponent<SavePoint>();

        public override bool CanExecute()
            => !_persistence.IsRunning;

        public override void Execute()
        {
            _savePoint.Executed();
            
            _gameStateMachine.PushState<PlanetSaveGameState>();
        }
    }
}