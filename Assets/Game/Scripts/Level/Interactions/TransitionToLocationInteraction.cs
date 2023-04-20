using Game.GameFlow;
using VContainer;

namespace Game.Level
{
    public class TransitionToLocationInteraction : Interaction
    {
        private readonly PlanetStateMachine _planetStateMachine;
        private readonly LevelController _level;

        private LocationPointDefinition _targetLocation;

        [Inject]
        public TransitionToLocationInteraction(
            PlanetStateMachine planetStateMachine,
            LevelController level
        )
        {
            _planetStateMachine = planetStateMachine;
            _level = level;
        }

        public LocationPointDefinition TargetLocation => _targetLocation;

        public override void OnCreate()
        {
            var door = Source.GetComponent<LocationDoor>();
            _targetLocation = door.TargetLocationPoint;
        }

        public override bool CanExecute()
            => true;

        public override void Execute()
        {
            _level.SetLocationPoint(_targetLocation);

            _planetStateMachine.EnterState<PlanetLocationLoadingState>();
        }
    }
}