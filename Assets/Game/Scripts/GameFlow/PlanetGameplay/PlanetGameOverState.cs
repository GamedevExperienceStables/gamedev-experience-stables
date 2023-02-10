using Cysharp.Threading.Tasks;
using Game.Input;
using Game.UI;
using VContainer;

namespace Game.GameFlow
{
    public class PlanetGameOverState : GameState
    {
        private readonly IInputService _input;
        private readonly GameplayView _view;

        [Inject]
        public PlanetGameOverState(IInputService input, GameplayView view)
        {
            _input = input;
            _view = view;
        }

        protected override async UniTask OnEnter()
        {
            _input.DisableAll();
            
            await _view.ShowGameOverAsync();
            
            _input.EnableMenus();
        }

        protected override async UniTask OnExit()
        {
            _input.DisableAll();
            
            await _view.HideGameOverAsync();
        }
    }
}