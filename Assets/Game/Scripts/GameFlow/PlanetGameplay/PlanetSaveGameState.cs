using System;
using Cysharp.Threading.Tasks;
using Game.Input;
using Game.Persistence;
using Game.UI;
using VContainer;

namespace Game.GameFlow
{
    public class PlanetSaveGameState : GameState
    {
        private const float WAIT_TIME = 0.3f;

        private readonly PersistenceService _persistence;
        private readonly ILoadingScreen _faderScreen;
        private readonly IInputService _input;

        [Inject]
        public PlanetSaveGameState(PersistenceService persistence, IFaderScreen faderScreen, IInputService input)
        {
            _persistence = persistence;
            _faderScreen = faderScreen;
            _input = input;
        }

        protected override async UniTask OnEnter()
        {
            _input.PushState(InputSchemeGame.None);

            await _faderScreen.ShowAsync();

            await UniTask.Delay(TimeSpan.FromSeconds(WAIT_TIME));
            await SaveGame();

            await Parent.PopState();
        }

        protected override UniTask OnExit()
        {
            _input.Back();

            _faderScreen.Hide();

            return UniTask.CompletedTask;
        }

        private async UniTask SaveGame()
            => await _persistence.SaveDataAsync();
    }
}