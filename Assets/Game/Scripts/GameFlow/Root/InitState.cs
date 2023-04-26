using Cysharp.Threading.Tasks;
using VContainer;

namespace Game.GameFlow
{
    public class InitState : GameState
    {
        [Inject]
        public InitState()
        {
        }

        protected override UniTask OnEnter()
        {
            return Parent.EnterState<MainMenuState>();
        }

        protected override UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }
    }
}