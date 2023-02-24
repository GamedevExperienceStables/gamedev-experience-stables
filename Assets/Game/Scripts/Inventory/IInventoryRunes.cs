using System;
using System.Collections.Generic;

namespace Game.Inventory
{
    public interface IInventoryRunes
    {
        void Subscribe(Action<RuneDefinition> callback);
        void UnSubscribe(Action<RuneDefinition> callback);
        IReadOnlyList<RuneDefinition> Items { get; }
    }
}