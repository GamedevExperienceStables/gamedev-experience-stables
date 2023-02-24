using Cysharp.Threading.Tasks;
using Game.Input;
using Game.UI;
using VContainer;

namespace Game.GameFlow
{
    public class PlanetPlayState : GameState
    {
        private readonly GameplayView _view;
        private readonly IInputService _inputService;
        private readonly GameplayMenuInput _menuInput;

        [Inject]
        public PlanetPlayState(GameplayView view, IInputService inputService, GameplayMenuInput menuInput)
        {
            _view = view;
            _inputService = inputService;
            _menuInput = menuInput;
        }

        protected override UniTask OnEnter()
        {
            _inputService.ReplaceState(InputSchemeGame.Gameplay);
            _menuInput.ReplaceState(InputSchemeMenu.Gameplay);
            _view.ShowHud();

            return UniTask.CompletedTask;
        }
    }
}