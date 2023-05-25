using Cysharp.Threading.Tasks;
using Game.Achievements;
using Game.SceneManagement;
using VContainer;

namespace Game.GameFlow
{
    public class IntroState : GameState
    {
        private readonly SceneLoader _loader;
        private readonly GameAchievements _achievements;

        [Inject]
        public IntroState(SceneLoader loader, GameAchievements achievements)
        {
            _loader = loader;
            _achievements = achievements;
        }

        protected override UniTask OnEnter() 
            => _loader.LoadSceneAsync(SceneNames.INTRO);

        protected override UniTask OnExit()
        {
            _achievements.GameStarted();
            
            return base.OnExit();
        }
    }
}