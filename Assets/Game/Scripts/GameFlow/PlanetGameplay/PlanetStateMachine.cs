using VContainer;

namespace Game.GameFlow
{
    public class PlanetStateMachine : GameStateMachine
    {
        [Inject]
        public PlanetStateMachine(
            PlanetState parentState,
            PlanetLocationLoadingState planetLoadingState,
            PlanetPlayState playState,
            PlanetPauseState pauseState,
            PlanetInventoryState inventoryState,
            PlanetGameOverState gameOverState,
            PlanetCompleteState completeState
        )
        {
            parentState.Child = stateMachine;

            stateMachine.AddState(planetLoadingState);
            stateMachine.AddState(playState);
            stateMachine.AddState(pauseState);
            stateMachine.AddState(inventoryState);
            stateMachine.AddState(gameOverState);
            stateMachine.AddState(completeState);
        }
    }
}