using System;
using VContainer;

namespace Game.GameFlow
{
    public sealed class GameplayInventory : IDisposable
    {
        private readonly PlanetStateMachine _planetStateMachine;
        private readonly GameplayMenuInput _menuInput;

        [Inject]
        public GameplayInventory(PlanetStateMachine planetStateMachine, GameplayMenuInput menuInput)
        {
            _planetStateMachine = planetStateMachine;
            _menuInput = menuInput;

            _menuInput.SubscribeInventory(OnInventoryRequested);
            _menuInput.SubscribeBack(OnBackRequested);
        }

        public void Dispose()
        {
            _menuInput.UnSubscribeInventory(OnInventoryRequested);
            _menuInput.UnSubscribeBack(OnBackRequested);
        }

        public void Enable()
            => _planetStateMachine.PushState<PlanetInventoryState>();

        public void Disable()
            => _planetStateMachine.PopState();

        private void OnInventoryRequested()
        {
            if (_planetStateMachine.IsState<PlanetInventoryState>())
                Disable();
            else
                Enable();
        }

        private void OnBackRequested()
        {
            if (_planetStateMachine.IsState<PlanetInventoryState>()) 
                Disable();
        }
    }
}