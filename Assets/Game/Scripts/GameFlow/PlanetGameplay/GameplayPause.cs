using System;
using VContainer;

namespace Game.GameFlow
{
    public sealed class GameplayPause : IDisposable
    {
        private readonly PlanetStateMachine _planetStateMachine;
        private readonly GameplayMenuInput _menuInput;

        [Inject]
        public GameplayPause(PlanetStateMachine planetStateMachine, GameplayMenuInput menuInput)
        {
            _planetStateMachine = planetStateMachine;
            _menuInput = menuInput;

            _menuInput.SubscribeMenu(OnMenuRequested);
        }

        public void Dispose()
        {
            _menuInput.UnSubscribeMenu(OnMenuRequested);
        }

        public void Enable()
            => _planetStateMachine.PushState<PlanetPauseState>();

        public void Disable()
            => _planetStateMachine.PopState();

        private void OnMenuRequested()
            => Enable();
    }
}