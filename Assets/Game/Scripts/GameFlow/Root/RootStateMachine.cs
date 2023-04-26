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
            LoadGameState loadGameState,
            QuitGameState quitGameState,
            PlanetState planetGameplayState,
            CompleteLevelState completeLevelState,
            CompleteGameState completeGameState
        )
        {
            stateMachine.AddState(initState);
            stateMachine.AddState(mainMenuState);
            stateMachine.AddState(introState);

            stateMachine.AddState(newGameState);
            stateMachine.AddState(loadGameState);
            stateMachine.AddState(completeGameState);
            stateMachine.AddState(quitGameState);

            stateMachine.AddState(planetGameplayState);
            stateMachine.AddState(completeLevelState);
        }
    }
}