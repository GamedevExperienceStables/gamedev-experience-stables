using Game.GameFlow;
using UnityEngine;
using VContainer;

namespace Game.Level
{
    public class TransitionToLocationInteraction : Interaction
    {
        private readonly PlanetStateMachine _planetStateMachine;
        private readonly GameStateModel _gameStateModel;

        private LocationPointDefinition _targetLocation;

        [Inject]
        public TransitionToLocationInteraction(PlanetStateMachine planetStateMachine, GameStateModel gameStateModel)
        {
            _planetStateMachine = planetStateMachine;
            _gameStateModel = gameStateModel;
        }

        public void Init(GameObject source, LocationPointDefinition targetLocation)
        {
            Source = source;

            _targetLocation = targetLocation;
        }

        public override void Execute()
        {
            _gameStateModel.LastLocation = _gameStateModel.CurrentLocation;
            _gameStateModel.CurrentLocation = _targetLocation;

            _planetStateMachine.EnterState<PlanetLocationLoadingState>();
        }
    }
}