using Game.GameFlow;
using Game.Persistence;
using UnityEngine;
using VContainer;

namespace Game.Level
{
    public class TransitionToLocationInteraction : Interaction
    {
        private readonly PlanetStateMachine _planetStateMachine;
        private readonly LevelDataHandler _level;
        private readonly LocationDataHandler _location;

        private LocationPointDefinition _targetLocation;

        [Inject]
        public TransitionToLocationInteraction(
            PlanetStateMachine planetStateMachine,
            LevelDataHandler level,
            LocationDataHandler location
        )
        {
            _planetStateMachine = planetStateMachine;
            _level = level;
            _location = location;
        }

        public void Init(LocationPointDefinition targetLocation, GameObject source)
        {
            Source = source;

            _targetLocation = targetLocation;
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