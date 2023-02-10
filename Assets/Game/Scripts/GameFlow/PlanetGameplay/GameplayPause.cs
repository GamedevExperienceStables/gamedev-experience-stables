using System;
using Game.Input;
using VContainer;

namespace Game.GameFlow
{
    public sealed class GameplayPause : IDisposable
    {
        private readonly PlanetStateMachine _planetStateMachine;
        private readonly GameplayBackButton _backButton;

        [Inject]
        public GameplayPause(
            PlanetStateMachine planetStateMachine,
            GameplayBackButton backButton
        )
        {
            _planetStateMachine = planetStateMachine;
            _backButton = backButton;

            _backButton.Subscribe(OnBackRequested);
        }

        public void Dispose()
            => _backButton.UnSubscribe(OnBackRequested);

        public void Enable()
            => _planetStateMachine.PushState<PlanetPauseState>();

        public void Disable()
            => _planetStateMachine.PopState();

        private void OnBackRequested(InputSchema schema)
        {
            switch (schema)
            {
                case InputSchema.Gameplay:
                    Enable();
                    break;

                case InputSchema.Menus:
                    Disable();
                    break;

                case InputSchema.Undefined:
                default:
                    throw new ArgumentOutOfRangeException(nameof(schema), schema, null);
            }
        }
    }
}