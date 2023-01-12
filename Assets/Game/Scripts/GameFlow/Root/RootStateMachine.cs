using VContainer;

namespace Game.GameFlow
{
    public class RootStateMachine : GameStateMachine
    {
        [Inject]
        public RootStateMachine(
            InitState initState,
            IntroState introState,
            MainMenuState mainMenuState,
            NewGameState newGameState,
            QuitGameState quitGameState,
            PlanetState planetGameplayState
        )
        {
            stateMachine.AddState(initState);
            stateMachine.AddState(mainMenuState);
            stateMachine.AddState(introState);
            
            stateMachine.AddState(newGameState);
            stateMachine.AddState(quitGameState);
            
            stateMachine.AddState(planetGameplayState);
        }
    }
}