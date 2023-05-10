using System;
using System.Collections.Generic;

namespace Game.Inventory
{
    public class Runes : IReadOnlyRunes
    {
        private readonly List<RuneDefinition> _items = new();

        public IReadOnlyList<RuneDefinition> Items => _items.AsReadOnly();

        private event Action<RuneDefinition> RuneAdded;

        public void SubscribeOnAdded(Action<RuneDefinition> callback)
            => RuneAdded += callback;

        public void UnSubscribeOnAdded(Action<RuneDefinition> callback)
            => RuneAdded -= callback;

        public void Reset()
            => _items.Clear();

        public void Init(IEnumerable<RuneDefinition> runes)
        {
            Reset();
            foreach (RuneDefinition rune in runes)
                Add(rune);
        }

        public bool Contains(RuneDefinition rune)
            => _items.Contains(rune);

        public void Add(RuneDefinition rune)
        {
            _items.Add(rune);

            RuneAdded?.Invoke(rune);
        }
    }
}