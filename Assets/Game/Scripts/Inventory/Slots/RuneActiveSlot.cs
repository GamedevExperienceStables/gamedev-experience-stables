using UnityEngine;

namespace Game.Inventory
{
    public class RuneActiveSlot
    {
        private const int NOT_SET_ACTIVE_SLOT_ID = -1;
        
        public RuneSlotId Value { get; private set; } = NOT_SET_ACTIVE_SLOT_ID;

        public void Set(RuneSlotId id)
        {
            Value = id;
            Debug.Log($"new slot active {Value}");
        }

        public void Clear()
        {
            Debug.Log($"active slot cleared {Value}");
            Value = NOT_SET_ACTIVE_SLOT_ID;
        }
    }
}