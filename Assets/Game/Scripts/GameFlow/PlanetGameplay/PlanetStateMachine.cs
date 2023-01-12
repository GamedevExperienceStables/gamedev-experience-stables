using VContainer;

namespace Game.GameFlow
{
    public class PlanetStateMachine : GameStateMachine
    {
        [Inject]
        public PlanetStateMachine(
            PlanetState parentState,
            PlanetLocationLoadingState planetLoadingState,
            PlanetPlayState planetState,
            PlanetPauseState pauseState
        )
        {
            parentState.Child = stateMachine;

            stateMachine.AddState(planetLoadingState);
            stateMachine.AddState(planetState);
            stateMachine.AddState(pauseState);
        }
    }
}