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
#if UNITY_EDITOR
            Debug.Log($"Slot activated: {Value}");
#endif
        }

        public void Clear()
        {
#if UNITY_EDITOR
            Debug.Log($"Slot deactivated: {Value}");
#endif  
            Value = RuneSlotId.INVALID_ID;
        }
    }
}