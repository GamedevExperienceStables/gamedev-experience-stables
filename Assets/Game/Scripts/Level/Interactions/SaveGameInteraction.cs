using Game.GameFlow;
using Game.Persistence;
using VContainer;

namespace Game.Level
{
    public class SaveGameInteraction : Interaction
    {
        private readonly PlanetStateMachine _gameStateMachine;
        private readonly PersistenceService _persistence;
        private readonly LevelController _level;

        private SavePoint _savePoint;

        [Inject]
        public SaveGameInteraction(PersistenceService persistence, LevelController level,
            PlanetStateMachine gameStateMachine)
        {
            _persistence = persistence;
            _level = level;
            _gameStateMachine = gameStateMachine;
        }

        public override void OnCreate()
            => _savePoint = Source.GetComponent<SavePoint>();

        public override bool CanExecute()
            => !_persistence.IsRunning;

        public override void Execute()
        {
            UpdateLevelLocationPoint();

            _savePoint.Executed();
            
            _gameStateMachine.PushState<PlanetSaveGameState>();
        }

        private void UpdateLevelLocationPoint()
        {
            ILocationPoint locationPoint = _level.GetCurrentLocationPoint();
            var savePoint = new LocationPointData(locationPoint.Location, _savePoint.PointKey);
            _level.SetLocationPoint(savePoint);
        }
    }
}