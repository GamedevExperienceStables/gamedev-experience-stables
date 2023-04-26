using System;
using System.Collections.Generic;

namespace Game.Inventory
{
    public interface IInventorySlots
    {
        bool HasActive { get; }
        RuneSlot ActiveSlot { get; }
        IReadOnlyDictionary<RuneSlotId, RuneSlot> Items { get; }

        event Action<RuneActiveSlotChangedEvent> ActiveSlotChanged;
        event Action<RuneSlotChangedEvent> SlotChanged;

        void SetRuneToSlot(RuneSlotId slotId, RuneDefinition targetRune);
        void SwapSlots(RuneSlotId slotId1, RuneSlotId slotId2);
        void ClearSlot(RuneSlotId slotId);
        void SetActiveSlot(RuneSlotId slotId);
    }
}