using Cysharp.Threading.Tasks;
using VContainer;

namespace Game.GameFlow
{
    public class StartMenuState : GameState
    {
        [Inject]
        public StartMenuState()
        {
        }

        protected override UniTask OnEnter()
        {
            return UniTask.CompletedTask;
        }
    }
}