using UnityEngine;

namespace Game.Inventory
{
    public class RuneActiveSlot
    {
        public RuneSlotId Value { get; private set; } = RuneSlotId.INVALID_ID;

        public bool IsEmpty => !Value.IsValid();

        public void Set(RuneSlotId id)
        {
            Value = id;
            Debug.Log($"Slot activated: {Value}");
        }

        public void Clear()
        {
            Debug.Log($"Slot deactivated: {Value}");
            Value = RuneSlotId.INVALID_ID;
        }
    }
}