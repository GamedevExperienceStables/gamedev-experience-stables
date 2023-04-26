using Cysharp.Threading.Tasks;
using Game.SceneManagement;
using VContainer;

namespace Game.GameFlow
{
    public class IntroState : GameState
    {
        private readonly SceneLoader _loader;

        [Inject]
        public IntroState(SceneLoader loader)
            => _loader = loader;

        protected override UniTask OnEnter() 
            => _loader.LoadSceneAsync(SceneNames.INTRO);
    }
}