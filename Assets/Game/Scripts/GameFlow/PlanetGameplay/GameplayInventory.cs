using System;
using Game.Inventory;
using VContainer;

namespace Game.GameFlow
{
    public sealed class GameplayInventory : IDisposable
    {
        private readonly PlanetStateMachine _planetStateMachine;
        private readonly GameplayMenuInput _menuInput;
        private readonly IInventorySlots _inventorySlots;

        [Inject]
        public GameplayInventory(
            PlanetStateMachine planetStateMachine,
            GameplayMenuInput menuInput,
            IInventorySlots inventorySlots
        )
        {
            _planetStateMachine = planetStateMachine;
            _menuInput = menuInput;
            _inventorySlots = inventorySlots;

            _menuInput.SubscribeInventory(OnInventoryRequested);
            _menuInput.SubscribeBack(OnBackRequested);
            _menuInput.SubscribeActiveSlot(OnActiveSlotChanging);
        }

        public void Dispose()
        {
            _menuInput.UnSubscribeInventory(OnInventoryRequested);
            _menuInput.UnSubscribeBack(OnBackRequested);
            _menuInput.UnSubscribeActiveSlot(OnActiveSlotChanging);
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

        private void OnActiveSlotChanging(int newSlotId) 
            => _inventorySlots.SetActiveSlot(newSlotId);
    }
}