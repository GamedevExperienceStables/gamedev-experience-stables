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

        protected override void OnEnter()
        {
            Application.targetFrameRate = 120;

            Parent.EnterState<MainMenuState>();
        }

        protected override void OnExit()
        {
        }
    }
}