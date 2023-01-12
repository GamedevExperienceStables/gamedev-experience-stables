using Game.GameFlow;
using VContainer;

namespace Game.UI
{
    public class GameplayViewModel 
    {
        private RootStateMachine _rootStateMachine;
        private PlanetStateMachine _planetStateMachine;

        [Inject]
        public void Construct(
            RootStateMachine rootStateMachine,
            PlanetStateMachine planetStateMachine
        )
        {
            _planetStateMachine = planetStateMachine;
            _rootStateMachine = rootStateMachine;
        }

        public void PauseGame() => _planetStateMachine.EnterState<PlanetPauseState>();
        public void ResumeGame() => _planetStateMachine.EnterState<PlanetPlayState>();

        public void GoToMainMenu() => _rootStateMachine.EnterState<MainMenuState>();
    }
}