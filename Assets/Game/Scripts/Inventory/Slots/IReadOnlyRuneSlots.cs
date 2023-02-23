using System.Collections.Generic;

namespace Game.Inventory
{
    public interface IReadOnlyRuneSlots
    {
        IReadOnlyDictionary<RuneSlotId, RuneSlot> Items { get; }
    }
}