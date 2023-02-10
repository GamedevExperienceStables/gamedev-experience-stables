using Cysharp.Threading.Tasks;
using Game.Input;
using Game.TimeManagement;
using Game.UI;
using VContainer;

namespace Game.GameFlow
{
    public class PlanetPauseState : GameState
    {
        private readonly ITimeService _timeService;
        private readonly IInputService _inputService;
        private readonly GameplayView _view;
        private readonly GameplayBackButton _backButton;

        [Inject]
        public PlanetPauseState(
            ITimeService timeService,
            IInputService inputService,
            GameplayView view,
            GameplayBackButton backButton
        )
        {
            _timeService = timeService;
            _inputService = inputService;
            _view = view;
            _backButton = backButton;
        }

        protected override async UniTask OnEnter()
        {
            _timeService.Pause();
            _inputService.DisableAll();

            await _view.ShowPauseAsync();

            _inputService.EnableMenus();
            _backButton.SetActive(InputSchema.Menus, true);
        }

        protected override async UniTask OnExit()
        {
            _backButton.SetActive(InputSchema.Menus, false);
            _inputService.DisableAll();

            await _view.HidePauseAsync();

            _timeService.Play();
            _inputService.EnableGameplay();
        }
    }
}