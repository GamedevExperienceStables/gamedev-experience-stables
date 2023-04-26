using Cysharp.Threading.Tasks;
using Game.Audio;
using Game.Input;
using Game.TimeManagement;
using Game.UI;
using VContainer;

namespace Game.GameFlow
{
    public class PlanetPauseState : GameState
    {
        private readonly ITimeService _timeService;
        private readonly IInputService _input;
        private readonly GameplayView _view;
        private readonly GameplayMenuInput _menuInput;
        private readonly IAudioService _audio;

        [Inject]
        public PlanetPauseState(
            ITimeService timeService,
            IInputService input,
            GameplayView view,
            GameplayMenuInput menuInput,
            IAudioService audio
        )
        {
            _timeService = timeService;
            _input = input;
            _menuInput = menuInput;
            _audio = audio;
            _view = view;
        }

        protected override async UniTask OnEnter()
        {
            _audio.Pause();
            
            _timeService.Pause();
            _input.PushState(InputSchemeGame.None);
            _menuInput.PushState(InputSchemeMenu.None);

            await _view.ShowPauseAsync();
            
            _input.ReplaceState(InputSchemeGame.Menu);
            _menuInput.ReplaceState(InputSchemeMenu.Pause);
        }

        protected override async UniTask OnExit()
        {
            _audio.Resume();
            
            _input.ReplaceState(InputSchemeGame.None);
            _menuInput.ReplaceState(InputSchemeMenu.None);

            await _view.HidePauseAsync();

            _input.Back();
            _menuInput.Back();
            
            _timeService.Play();
        }
    }
}