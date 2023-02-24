using Cysharp.Threading.Tasks;
using Game.Input;
using Game.UI;
using VContainer;

namespace Game.GameFlow
{
    public class PlanetGameOverState : GameState
    {
        private readonly IInputService _input;
        private readonly GameplayMenuInput _menuInput;
        private readonly GameplayView _view;

        [Inject]
        public PlanetGameOverState(IInputService input, GameplayMenuInput menuInput, GameplayView view)
        {
            _input = input;
            _menuInput = menuInput;
            
            _view = view;
        }

        protected override async UniTask OnEnter()
        {
            _input.PushState(InputSchemeGame.None);
            _menuInput.ReplaceState(InputSchemeMenu.None);
            
            await _view.ShowGameOverAsync();
            
            _input.ReplaceState(InputSchemeGame.Menu);
        }

        protected override async UniTask OnExit()
        {
            _input.ReplaceState(InputSchemeGame.None);
            
            await _view.HideGameOverAsync();
        }
    }
}