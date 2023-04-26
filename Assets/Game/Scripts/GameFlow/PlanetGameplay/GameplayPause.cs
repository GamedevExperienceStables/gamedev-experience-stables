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
            _menuInput.SubscribeBack(OnBackRequested);
        }

        public void Dispose()
        {
            _menuInput.UnSubscribeMenu(OnMenuRequested);
            _menuInput.UnSubscribeBack(OnBackRequested);
        }

        public void Enable()
            => _planetStateMachine.PushState<PlanetPauseState>();

        public void Disable()
            => _planetStateMachine.PopState();

        private void OnMenuRequested()
            => Enable();

        private void OnBackRequested()
        {
            if (_planetStateMachine.IsState<PlanetPauseState>())
                Disable();
        }
    }
}