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
        private readonly GameplayBackButton _backButton;

        [Inject]
        public PlanetPlayState(GameplayView view, IInputService inputService, GameplayBackButton backButton)
        {
            _view = view;
            _inputService = inputService;
            _backButton = backButton;
        }

        protected override UniTask OnEnter()
        {
            _backButton.SetActive(InputSchema.Gameplay, true);
            _inputService.EnableGameplay();
            _view.ShowHud();

            return UniTask.CompletedTask;
        }

        protected override UniTask OnExit()
        {
            _backButton.SetActive(InputSchema.Gameplay, false);
            return UniTask.CompletedTask;
        }
    }
}