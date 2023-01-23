using Cysharp.Threading.Tasks;
using Game.Input;
using Game.TimeManagement;
using Game.UI;
using VContainer;

namespace Game.GameFlow
{
    public class PlanetPauseState : GameState
    {
        private readonly TimeService _timeService;
        private readonly IInputService _inputService;
        private readonly GameplayView _view;

        [Inject]
        public PlanetPauseState(
            TimeService timeService,
            IInputService inputService,
            GameplayView view
        )
        {
            _timeService = timeService;
            _inputService = inputService;
            _view = view;
        }

        protected override void OnEnter() 
            => EnterAsync().Forget();

        protected override void OnExit() 
            => ExitAsync().Forget();

        private async UniTask EnterAsync()
        {
            _timeService.Pause();
            _inputService.DisableAll();
            
            await _view.ShowPauseAsync();

            _inputService.EnableMenus();
        }

        private async UniTask ExitAsync()
        {
            _inputService.DisableAll();

            await _view.HidePauseAsync();
            
            _timeService.Play();
            _inputService.EnableGameplay();
        }
    }
}