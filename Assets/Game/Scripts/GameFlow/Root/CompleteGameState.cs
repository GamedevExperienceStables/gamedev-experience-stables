using Cysharp.Threading.Tasks;
using Game.SceneManagement;
using VContainer;

namespace Game.GameFlow
{
    public class CompleteGameState : GameState
    {
        private readonly SceneLoader _loader;

        [Inject]
        public CompleteGameState(SceneLoader loader) 
            => _loader = loader;

        protected override UniTask OnEnter() 
            => _loader.LoadSceneAsync(SceneNames.ENDING);
    }
}