using Game.GameFlow;
using UnityEngine;
using VContainer;

namespace Game.Level
{
    public class TransitionToLocationInteraction : Interaction
    {
        private readonly PlanetStateMachine _planetStateMachine;
        private readonly LocationDataHandler _locationController;

        private LocationPointDefinition _targetLocation;

        [Inject]
        public TransitionToLocationInteraction(
            PlanetStateMachine planetStateMachine,
            LocationDataHandler locationController
        )
        {
            _planetStateMachine = planetStateMachine;
            _locationController = locationController;
        }

        public void Init(LocationPointDefinition targetLocation, GameObject source)
        {
            Source = source;

            _targetLocation = targetLocation;
        }

        public override bool CanExecute()
            => _locationController.CanTransferTo(_targetLocation);

        public override void Execute()
        {
            _locationController.SetLocation(_targetLocation);

            _planetStateMachine.EnterState<PlanetLocationLoadingState>();
        }
    }
}