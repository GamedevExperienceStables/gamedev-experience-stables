using Cysharp.Threading.Tasks;
using Game.Input;
using Game.Level;
using Game.Persistence;
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
        private readonly LevelDataHandler _level;
        private readonly SceneLoader _sceneLoader;
        private readonly LocationController _locationController;
        private readonly IInputService _inputService;

        [Inject]
        public PlanetLocationLoadingState(
            IFaderScreen loadingScreen,
            LevelDataHandler level,
            SceneLoader sceneLoader,
            LocationController locationController,
            IInputService inputService
        )
        {
            _loadingScreen = loadingScreen;
            _level = level;
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

            LocationPointData spawnLocation = _level.GetCurrentLocation();
            Scene location = await LoadLocationAsync(spawnLocation.location);
            InitLocation(location, spawnLocation.pointKey);

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
            if (!_level.TryGetLastLocation(out LocationPointData lastLocation))
                return;

            await _sceneLoader.UnloadSceneIfLoadedAsync(lastLocation.location.SceneName);
        }

        private async UniTask<Scene> LoadLocationAsync(LocationDefinition spawnLocation)
            => await _sceneLoader.LoadSingleSceneAdditiveAsync(spawnLocation.SceneName);

        private static LocationContext GetContext(Scene location)
        {
            LifetimeScope scope = LifetimeScope.Find<LifetimeScope>(location);
            return scope.GetComponent<LocationContext>();
        }
    }
}