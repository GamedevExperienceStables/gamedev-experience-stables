using Game.GameFlow;
using VContainer;

namespace Game.Level
{
    public class TransitionToLocationInteraction : Interaction
    {
        private readonly PlanetStateMachine _planetStateMachine;
        private readonly LevelController _level;
        private readonly LocationDataHandler _location;

        private LocationPointDefinition _targetLocation;

        [Inject]
        public TransitionToLocationInteraction(
            PlanetStateMachine planetStateMachine,
            LevelController level,
            LocationDataHandler location
        )
        {
            _planetStateMachine = planetStateMachine;
            _level = level;
            _location = location;
        }

        public override void OnCreate()
        {
            var door = Source.GetComponent<LocationDoor>();
            _targetLocation = door.TargetLocation;
        }

        public override bool CanExecute()
            => _location.CanTransferTo(_targetLocation);

        public override void Execute()
        {
            _level.SetLocation(_targetLocation);

            _planetStateMachine.EnterState<PlanetLocationLoadingState>();
        }
    }
}