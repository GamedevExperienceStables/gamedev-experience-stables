using Cysharp.Threading.Tasks;
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

        protected override void OnEnter()
        {
            _sceneLoader.LoadSceneAsync(SceneNames.GAMEPLAY_PLANET).Forget();
        }

        protected override void OnExit()
        {
            Child = null;
        }
    }
}