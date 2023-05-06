using System;
using Cysharp.Threading.Tasks;
using Game.Input;
using Game.Level;
using Game.Persistence;
using Game.TimeManagement;
using Game.UI;
using VContainer;

namespace Game.GameFlow
{
    public class PlanetSaveGameState : GameState
    {
        private const float WAIT_TIME = 1f;

        private readonly PersistenceService _persistence;
        private readonly LocationStateStore _locationState;
        private readonly ILoadingScreen _faderScreen;
        private readonly IInputService _input;
        private readonly ITimeService _timeService;

        [Inject]
        public PlanetSaveGameState(PersistenceService persistence, LocationStateStore locationState,
            IFaderScreen faderScreen, IInputService input, ITimeService timeService)
        {
            _persistence = persistence;
            _locationState = locationState;
            _faderScreen = faderScreen;
            _input = input;
            _timeService = timeService;
        }

        protected override async UniTask OnEnter()
        {
            _input.PushState(InputSchemeGame.None);

            await _faderScreen.ShowAsync();

            _timeService.Pause();
            
            _locationState.Store();

            await UniTask.Delay(TimeSpan.FromSeconds(WAIT_TIME), DelayType.UnscaledDeltaTime);
            await SaveGame();

            await Parent.PopState();
        }

        protected override UniTask OnExit()
        {
            _input.Back();
            _timeService.Play();

            _faderScreen.Hide();

            return UniTask.CompletedTask;
        }

        private async UniTask SaveGame()
            => await _persistence.SaveDataAsync();
    }
}