using Cysharp.Threading.Tasks;
using Game.SceneManagement;
using VContainer;

namespace Game.GameFlow
{
    public class CreditsGameState : GameState
    {
        private readonly SceneLoader _loader;

        [Inject]
        public CreditsGameState(SceneLoader loader) 
            => _loader = loader;

        protected override UniTask OnEnter() 
            => _loader.LoadSceneAsync(SceneNames.CREDITS);
    }
}