using System;
using System.Collections.Generic;
using Game.Inventory;
using VContainer;

namespace Game.UI
{
    public class HudRuneSlotsViewModel
    {
        private readonly IInventorySlots _slots;

        [Inject]
        public HudRuneSlotsViewModel(IInventorySlots slots)
            => _slots = slots;

        public IReadOnlyDictionary<RuneSlotId, RuneSlot> Slots => _slots.Items;

        public void SubscribeRuneSlotChanged(Action<RuneSlotChangedEvent> callback)
            => _slots.SlotChanged += callback;

        public void UnSubscribeRuneSlotsChanges(Action<RuneSlotChangedEvent> callback)
            => _slots.SlotChanged -= callback;

        public void SubscribeActiveRuneSlotChanged(Action<RuneActiveSlotChangedEvent> callback)
            => _slots.ActiveSlotChanged += callback;

        public void UnSubscribeActiveRuneSlotChanged(Action<RuneActiveSlotChangedEvent> callback)
            => _slots.ActiveSlotChanged -= callback;
    }
}