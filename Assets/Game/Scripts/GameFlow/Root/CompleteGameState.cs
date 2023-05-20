using Cysharp.Threading.Tasks;
using Game.Achievements;
using Game.SceneManagement;
using VContainer;

namespace Game.GameFlow
{
    public class CompleteGameState : GameState
    {
        private readonly SceneLoader _loader;
        private readonly GameAchievements _achievements;

        [Inject]
        public CompleteGameState(SceneLoader loader, GameAchievements achievements)
        {
            _loader = loader;
            _achievements = achievements;
        }

        protected override UniTask OnEnter() 
            => _loader.LoadSceneAsync(SceneNames.ENDING);

        protected override UniTask OnExit()
        {
            _achievements.GameCompleted();
            
            return base.OnExit();
        }
    }
}