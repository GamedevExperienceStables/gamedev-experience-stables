using System;
using System.Collections.Generic;

namespace Game.Inventory
{
    public class Runes : IReadOnlyRunes
    {
        private readonly List<RuneDefinition> _items = new();

        public IReadOnlyList<RuneDefinition> Items => _items.AsReadOnly();

        private event Action<RuneDefinition> RuneAdded;

        public void Subscribe(Action<RuneDefinition> callback)
            => RuneAdded += callback;

        public void UnSubscribe(Action<RuneDefinition> callback)
            => RuneAdded -= callback;

        public void Init()
            => _items.Clear();

        public void Init(IEnumerable<RuneDefinition> runes)
        {
            _items.Clear();
            foreach (RuneDefinition rune in runes) 
                _items.Add(rune);
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