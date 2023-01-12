using Game.SceneManagement;
using VContainer;

namespace Game.GameFlow
{
    public class PlanetState : GameState
    {
        private readonly SceneLoader _sceneLoader;

        [Inject]
        public PlanetState(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        protected override async void OnEnter()
        {
            await _sceneLoader.LoadSceneAsync(SceneNames.GAMEPLAY_PLANET);
        }

        protected override void OnExit()
        {
            Child = null;
        }
    }
}