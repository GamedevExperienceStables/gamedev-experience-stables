using System;
using System.Collections.Generic;

namespace Game.Inventory
{
    public interface IReadOnlyRunes
    {
        void SubscribeOnAdded(Action<RuneDefinition> callback);
        void UnSubscribeOnAdded(Action<RuneDefinition> callback);
        
        void SubscribeOnRemoved(Action<RuneDefinition> callback);
        void UnSubscribeOnRemoved(Action<RuneDefinition> callback);
        
        bool Contains(RuneDefinition rune);
        
        IReadOnlyList<RuneDefinition> Items { get; }
    }
}