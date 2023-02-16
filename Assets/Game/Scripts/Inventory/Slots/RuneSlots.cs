using System.Collections.Generic;
using UnityEngine;

namespace Game.Inventory
{
    public class RuneSlots : IReadOnlyRuneSlots
    {
        private readonly Dictionary<RuneSlotId, RuneSlot> _slots = new();
        private readonly RuneActiveSlot _activeSlot = new();

        public RuneSlots(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int slotId = i + 1;
                _slots[slotId] = new RuneSlot(slotId);
            }
        }

        public RuneSlotId Active => _activeSlot.Value;

        public IReadOnlyDictionary<RuneSlotId, RuneSlot> Items => _slots;

        public bool IsActive(RuneSlotId id)
            => _activeSlot.Value == id;

        public void SetActive(RuneSlotId id)
        {
            if (!_slots[id].IsEmpty)
                _activeSlot.Set(id);
        }

        public void ClearActive()
            => _activeSlot.Clear();

        public void Set(RuneSlotId id, RuneDefinition rune)
            => _slots[id].Set(rune);

        public void Clear(RuneSlotId id)
        {
            if (_activeSlot.Value == id)
                _activeSlot.Clear();

            _slots[id].Clear();
        }

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