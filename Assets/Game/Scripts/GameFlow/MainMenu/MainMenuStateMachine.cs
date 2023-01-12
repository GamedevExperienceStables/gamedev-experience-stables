using VContainer;

namespace Game.GameFlow
{
    public class MainMenuStateMachine : GameStateMachine
    {
        [Inject]
        public MainMenuStateMachine(
            StartMenuState startMenu
        )
        {
            stateMachine.AddState(startMenu);
        }
    }
}