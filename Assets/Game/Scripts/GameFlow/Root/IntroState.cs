using System;
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

        protected override async UniTask OnEnter()
        {
            await _loader.LoadSceneAsync(SceneNames.INTRO);
            await Delay_ShouldBeRemovedBeforeProduction();

            await Parent.EnterState<PlanetState>();
        }

        private static async UniTask Delay_ShouldBeRemovedBeforeProduction()
            => await UniTask.Delay(TimeSpan.FromSeconds(1f));
    }
}