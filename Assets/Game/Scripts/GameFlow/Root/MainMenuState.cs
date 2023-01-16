using Cysharp.Threading.Tasks;
using Game.SceneManagement;
using VContainer;

namespace Game.GameFlow
{
    public class MainMenuState : GameState
    {
        private readonly SceneLoader _sceneLoader;

        [Inject]
        public MainMenuState(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        protected override void OnEnter()
        {
            _sceneLoader.LoadSceneAsync(SceneNames.MAIN_MENU).Forget();
        }

        protected override void OnExit()
        {
        }
    }
}