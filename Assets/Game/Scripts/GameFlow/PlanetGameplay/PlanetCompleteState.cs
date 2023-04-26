using Cysharp.Threading.Tasks;
using Game.Input;
using VContainer;

namespace Game.GameFlow
{
    public class PlanetCompleteState : GameState
    {
        private readonly IInputService _input;
        private readonly RootStateMachine _rootStateMachine;

        [Inject]
        public PlanetCompleteState(IInputService input, RootStateMachine rootStateMachine)
        {
            _input = input;
            _rootStateMachine = rootStateMachine;
        }

        protected override UniTask OnEnter()
        {
            _input.ReplaceState(InputSchemeGame.None);
            _rootStateMachine.EnterState<CompleteLevelState>();

            return UniTask.CompletedTask;
        }
    }
}