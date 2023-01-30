using System;
using System.Collections.Generic;

namespace Game.Inventory
{
    public class Runes : IReadOnlyRunes
    {
        private readonly List<RuneDefinition> _items = new();

        private event Action<RuneDefinition> RuneAdded;

        public void Subscribe(Action<RuneDefinition> callback)
            => RuneAdded += callback;

        public void UnSubscribe(Action<RuneDefinition> callback)
            => RuneAdded -= callback;

        public void Init()
            => _items.Clear();

        public bool Contains(RuneDefinition rune)
            => _items.Contains(rune);

        public void Add(RuneDefinition rune)
        {
            _items.Add(rune);

            RuneAdded?.Invoke(rune);
        }
    }
}