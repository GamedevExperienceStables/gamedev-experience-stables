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

        protected override UniTask OnEnter()
        {
            return _sceneLoader.LoadSceneAsync(SceneNames.GAMEPLAY_PLANET);
        }

        protected override UniTask OnExit()
        {
            Child = null;
            
            return base.OnExit();
        }
    }
}