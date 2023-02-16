using System.Collections.Generic;
using UnityEngine;

namespace Game.Inventory
{
    public class RuneSlots : IReadOnlyRuneSlots
    {
        private const int NOT_SET_ACTIVE_SLOT_ID = -1;

        private readonly Dictionary<RuneSlotId, RuneSlot> _slots = new();

        public RuneSlots(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int slotId = i + 1;
                _slots[slotId] = new RuneSlot(slotId);
            }
        }

        public RuneSlotId Active { get; private set; } = NOT_SET_ACTIVE_SLOT_ID;

        public IReadOnlyDictionary<RuneSlotId, RuneSlot> Items => _slots;

        public void SetActive(RuneSlotId id)
            => Active = id;

        public void ClearActive()
            => Active = NOT_SET_ACTIVE_SLOT_ID;

        public void Set(RuneSlotId id, RuneDefinition rune)
            => _slots[id].Set(rune);

        public void Clear(RuneSlotId id)
            => _slots[id].Clear();

        public void Init(IDictionary<RuneSlotId, RuneDefinition> slots)
        {
            ClearActive();
            
            foreach ((RuneSlotId id, RuneDefinition value) in slots)
            {
                if (_slots.ContainsKey(id))
                    Set(id, value);
                else
                    Debug.LogWarning($"Trying to set slot '{id.ToString()}' that not exists");
            }
        }

        public void Reset()
        {
            ClearActive();
            foreach (RuneSlotId id in _slots.Keys)
                Clear(id);
        }
    }
}