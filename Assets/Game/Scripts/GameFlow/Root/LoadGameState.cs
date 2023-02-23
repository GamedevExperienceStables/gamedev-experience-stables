using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Persistence;
using Game.UI;
using UnityEngine;
using VContainer;

namespace Game.GameFlow
{
    public class LoadGameState : GameState
    {
        private readonly PersistenceService _persistence;
        private readonly ILoadingScreen _loadingScreen;

        [Inject]
        public LoadGameState(PersistenceService persistence, ILoadingScreen loadingScreen)
        {
            _persistence = persistence;
            _loadingScreen = loadingScreen;
        }

        protected override async UniTask OnEnter()
        {
            await _loadingScreen.ShowAsync();

            await LoadGame();

            await Parent.EnterState<PlanetState>();
        }

        private UniTask LoadGame()
        {
            if (_persistence.IsSaveGameExists())
                return _persistence.LoadDataAsync();

            _persistence.InitData();
            return UniTask.CompletedTask;
        }
    }
}