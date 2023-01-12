using Game.Input;
using Game.TimeManagment;
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
        {
            _timeService.Pause();
            
            _inputService.EnableMenus();
            _view.ShowPause();
        }

        protected override void OnExit()
        {
            _timeService.Play();
        }
    }
}