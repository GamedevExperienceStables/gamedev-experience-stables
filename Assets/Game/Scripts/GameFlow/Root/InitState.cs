using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Game.GameFlow
{
    public class InitState : GameState
    {
        [Inject]
        public InitState()
        {
        }

        protected override UniTask OnEnter()
        {
            Application.targetFrameRate = 120;

            return Parent.EnterState<MainMenuState>();
        }

        protected override UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }
    }
}