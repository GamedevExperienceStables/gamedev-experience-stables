using System;
using System.Collections.Generic;

namespace Game.Inventory
{
    public interface IInventoryRunes
    {
        void SubscribeOnAdded(Action<RuneDefinition> callback);
        void UnSubscribeOnAdded(Action<RuneDefinition> callback);
        IReadOnlyList<RuneDefinition> Items { get; }
    }
}