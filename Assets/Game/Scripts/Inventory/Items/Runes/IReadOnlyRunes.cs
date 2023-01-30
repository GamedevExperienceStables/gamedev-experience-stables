using System;

namespace Game.Inventory
{
    public interface IReadOnlyRunes
    {
        void Subscribe(Action<RuneDefinition> callback);
        void UnSubscribe(Action<RuneDefinition> callback);
        bool Contains(RuneDefinition rune);
    }
}