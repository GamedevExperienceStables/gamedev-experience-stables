using System.Data;
using Cysharp.Threading.Tasks;
using Game.Input;
using Game.Level;
using Game.SceneManagement;
using Game.UI;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Game.GameFlow
{
    public class PlanetLocationLoadingState : GameState
    {
        private readonly IFaderScreen _loadingScreen;
        private readonly LevelController _level;
        private readonly SceneLoader _sceneLoader;
        private readonly LocationController _locationController;
        private readonly LocationContextHandler _locationContextHandler;
        private readonly LocationStateHandler _locationState;
        private readonly IInputService _inputService;

        [Inject]
        public PlanetLocationLoadingState(
            IFaderScreen loadingScreen,
            LevelController level,
            SceneLoader sceneLoader,
            LocationController locationController,
            LocationContextHandler locationContextHandler,
            LocationStateHandler locationState,
            IInputService inputService
        )
        {
            _loadingScreen = loadingScreen;
            _level = level;
            _sceneLoader = sceneLoader;
            _locationController = locationController;
            _locationContextHandler = locationContextHandler;
            _locationState = locationState;
            _inputService = inputService;
        }

        protected override async UniTask OnEnter()
        {
            _inputService.ReplaceState(InputSchemeGame.None);

            await _loadingScreen.ShowAsync();
            
            DisposeLocationData();

            await UnloadLastLocationIfExists();

            ILocationPoint spawnLocationPoint = _level.GetCurrentLocationPoint();
            Scene location = await LoadLocationAsync(spawnLocationPoint.Location);
            InitLocation(location, spawnLocationPoint.Location, spawnLocationPoint.PointKey);

            await Parent.EnterState<PlanetPlayState>();
        }

        protected override UniTask OnExit()
        {
            _loadingScreen.Hide();
            return UniTask.CompletedTask;
        }

        private void InitLocation(Scene location, ILocationDefinition locationDefinition,
            ILocationPointKey locationPoint)
        {
            LocationContext context = GetContext(location);
            _locationContextHandler.Init(context);

            _locationController.Init(locationDefinition, _locationContextHandler, locationPoint);
            _locationState.Init(locationDefinition, _locationContextHandler);
        }

        private async UniTask UnloadLastLocationIfExists()
        {
            if (!_level.TryGetLastLocationPoint(out ILocationPoint lastLocation))
                return;

            await _sceneLoader.UnloadSceneIfLoadedAsync(lastLocation.Location.SceneName);
        }

        private void DisposeLocationData()
        {
            if (!_locationContextHandler.Initialized)
                return;
            
            _locationState.Store(_locationContextHandler);
            _locationController.Clear();
            _locationContextHandler.Clear();
        }

        private async UniTask<Scene> LoadLocationAsync(ILocationDefinition location)
            => await _sceneLoader.LoadSingleSceneAdditiveAsync(location.SceneName);

        private static LocationContext GetContext(Scene location)
        {
            LifetimeScope scope = LifetimeScope.Find<LocationLifetimeScope>(location);
            if (!scope)
                throw new NoNullAllowedException("Not found 'LocationContext'");

            var locationContext = scope.GetComponent<LocationContext>();
            return locationContext;
        }
    }
}