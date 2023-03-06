using Cysharp.Threading.Tasks;
using Game.Audio;
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
        private readonly IAudioService _audio;

        [Inject]
        public PlanetPlayState(GameplayView view, IInputService inputService, GameplayMenuInput menuInput,
            IAudioService audio)
        {
            _view = view;
            _inputService = inputService;
            _menuInput = menuInput;
            _audio = audio;
        }

        protected override UniTask OnEnter()
        {
            _audio.StartLocationSounds();

            _inputService.ReplaceState(InputSchemeGame.Gameplay);
            _menuInput.ReplaceState(InputSchemeMenu.Gameplay);
            _view.ShowHud();

            return UniTask.CompletedTask;
        }
    }
}