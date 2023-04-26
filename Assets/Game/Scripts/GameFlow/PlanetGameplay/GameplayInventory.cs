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
            _menuInput.SubscribeActiveSlotShifted(OnActiveSlotShifted);
        }

        public void Dispose()
        {
            _menuInput.UnSubscribeInventory(OnInventoryRequested);
            _menuInput.UnSubscribeBack(OnBackRequested);
            _menuInput.UnSubscribeActiveSlot(OnActiveSlotChanging);
            _menuInput.UnSubscribeActiveSlotShifted(OnActiveSlotShifted);
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

        private void OnActiveSlotShifted(int direction)
        {
            int currentSlotId = _inventorySlots.HasActive ? (int)_inventorySlots.ActiveSlot.Id : 0;

            int slotId = FindNonEmptySlotId(currentSlotId, direction);
            if (slotId > 0 && currentSlotId != slotId)
                _inventorySlots.SetActiveSlot(slotId);
        }

        private int FindNonEmptySlotId(int slotId, int direction)
        {
            int iteration = _inventorySlots.Items.Count;
            while (iteration > 0)
            {
                iteration--;

                RuneSlot slot = GetNextSlotInDirection(ref slotId, direction);
                if (!slot.IsEmpty)
                    return slotId;
            }

            return -1;
        }

        private RuneSlot GetNextSlotInDirection(ref int slotId, int direction)
        {
            slotId += direction;

            int lastId = _inventorySlots.Items.Count;
            if (slotId <= 0)
                slotId = lastId;
            else if (slotId > lastId)
                slotId = 1;

            RuneSlot slot = _inventorySlots.Items[slotId];
            return slot;
        }
    }
}