using Cysharp.Threading.Tasks;
using Game.Input;
using Game.UI;
using VContainer;

namespace Game.GameFlow
{
    public class PlanetInventoryState : GameState
    {
        private readonly GameplayView _view;
        private readonly IInputService _input;
        private readonly GameplayMenuInput _menuInput;

        [Inject]
        public PlanetInventoryState(GameplayView view, IInputService input, GameplayMenuInput menuInput)
        {
            _view = view;
            _input = input;
            _menuInput = menuInput;
        }

        protected override async UniTask OnEnter()
        {
            _input.PushState(InputSchemeGame.None);
            _menuInput.PushState(InputSchemeMenu.None);
            
            await _view.ShowBookAsync();
            
            _input.ReplaceState(InputSchemeGame.Menu);
            _menuInput.ReplaceState(InputSchemeMenu.Inventory);
        }

        protected override async UniTask OnExit()
        {
            _input.ReplaceState(InputSchemeGame.None);
            _menuInput.ReplaceState(InputSchemeMenu.None);
            
            await _view.HideBookAsync();

            _input.Back();
            _menuInput.Back();
        }
    }
}