using System.Collections.Generic;
using System.Data;
using Cysharp.Threading.Tasks;
using Game.Input;
using Game.Level;
using Game.SceneManagement;
using Game.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Game.GameFlow
{
    public class PlanetLocationLoadingState : GameState
    {
        private readonly IFaderScreen _loadingScreen;
        private readonly GameStateModel _gameStateModel;
        private readonly SceneLoader _sceneLoader;
        private readonly LocationController _locationController;
        private readonly IInputService _inputService;

        private readonly List<GameObject> _loadingSceneRootGameObjectsBuffer = new(5);

        [Inject]
        public PlanetLocationLoadingState(
            IFaderScreen loadingScreen,
            GameStateModel gameStateModel,
            SceneLoader sceneLoader,
            LocationController locationController,
            IInputService inputService
        )
        {
            _loadingScreen = loadingScreen;
            _gameStateModel = gameStateModel;
            _sceneLoader = sceneLoader;
            _locationController = locationController;
            _inputService = inputService;
        }

        protected override async void OnEnter()
        {
            _inputService.DisableAll();
            
            await _loadingScreen.ShowAsync();

            _locationController.Clear();

            await UnloadLastLocationIfExists();
            
            LocationPointDefinition spawnLocation = _gameStateModel.CurrentLocation;
            Scene location = await LoadLocationAsync(spawnLocation.Location);
            InitLocation(location, spawnLocation.PointKey);

            Parent.EnterState<PlanetPlayState>();
        }

        protected override void OnExit()
        {
            _loadingScreen.Hide();
        }

        private void InitLocation(Scene location, LocationPointKey locationPoint)
        {
            LocationContext context = GetContext(location);
            LocationPoint spawnPoint = context.FindLocationPoint(locationPoint);

            _locationController.Init(context, spawnPoint.transform);
        }

        private async UniTask UnloadLastLocationIfExists()
        {
            LocationPointDefinition lastLocation = _gameStateModel.LastLocation;
            if (!lastLocation)
                return;

            await _sceneLoader.UnloadSceneIfLoadedAsync(lastLocation.Location.SceneName);
        }

        private async UniTask<Scene> LoadLocationAsync(LocationDefinition spawnLocation) 
            => await _sceneLoader.LoadSingleSceneAdditiveAsync(spawnLocation.SceneName);

        private LocationContext GetContext(Scene location)
        {
            LocationContext context = FindContext(location);
            if (context)
                return context;

            throw new NoNullAllowedException($"'{nameof(LocationContext)}' not found in location scene");
        }

        private LocationContext FindContext(Scene scene)
        {
            LocationContext context = null;

            scene.GetRootGameObjects(_loadingSceneRootGameObjectsBuffer);
            foreach (GameObject go in _loadingSceneRootGameObjectsBuffer)
            {
                if (go.TryGetComponent(out context))
                    break;
            }

            _loadingSceneRootGameObjectsBuffer.Clear();

            return context;
        }
    }
}