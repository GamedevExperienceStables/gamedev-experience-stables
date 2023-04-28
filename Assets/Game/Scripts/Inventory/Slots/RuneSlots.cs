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

        public bool HasActive => !_activeSlot.IsEmpty;

        public IReadOnlyDictionary<RuneSlotId, RuneSlot> Items => _slots;

        public RuneSlot GetActive()
            => _slots[_activeSlot.Value];

        public bool IsEmpty(RuneSlotId id)
            => _slots[id].IsEmpty;

        public bool IsActive(RuneSlotId id)
            => _activeSlot.Value == id;

        public void SetActive(RuneSlotId id) 
            => _activeSlot.Set(id);

        public void ClearActive()
            => _activeSlot.Clear();
        
        public RuneDefinition Get(RuneSlotId id) 
            => _slots[id].Rune;

        public void Set(RuneSlotId id, RuneDefinition rune)
            => _slots[id].Set(rune);

        public void Clear(RuneSlotId id) 
            => _slots[id].Clear();

        public void Init(IDictionary<RuneSlotId, RuneDefinition> slots)
        {
            Reset();

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

        public bool Find(RuneDefinition targetRune, out RuneSlotId runeSlotId)
        {
            foreach (RuneSlot runeSlot in _slots.Values)
            {
                if (runeSlot.Rune != targetRune)
                    continue;

                runeSlotId = runeSlot.Id;
                return true;
            }

            runeSlotId = default;
            return false;
        }
    }
}