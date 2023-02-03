using System;
using System.Collections.Generic;

namespace Game.Inventory
{
    public interface IReadOnlyRunes
    {
        void Subscribe(Action<RuneDefinition> callback);
        void UnSubscribe(Action<RuneDefinition> callback);
        bool Contains(RuneDefinition rune);
        
        IReadOnlyList<RuneDefinition> Items { get; }
    }
}