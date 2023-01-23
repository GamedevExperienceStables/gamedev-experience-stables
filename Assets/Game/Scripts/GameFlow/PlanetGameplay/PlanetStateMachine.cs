﻿using VContainer;

namespace Game.GameFlow
{
    public class PlanetStateMachine : GameStateMachine
    {
        [Inject]
        public PlanetStateMachine(
            PlanetState parentState,
            PlanetLocationLoadingState planetLoadingState,
            PlanetPlayState playState,
            PlanetPauseState pauseState
        )
        {
            parentState.Child = stateMachine;

            stateMachine.AddState(planetLoadingState);
            stateMachine.AddState(playState);
            stateMachine.AddState(pauseState);
        }
    }
}