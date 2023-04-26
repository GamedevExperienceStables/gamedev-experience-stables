using System;

namespace Game.Stats
{
    public interface IStats : IDisposable
    {
        delegate void StatChangedEvent(StatValueChange change);
        
        float this[CharacterStats key] { get; set; }
        
        void ApplyModifier(CharacterStats key, StatModifierApplyData modifier);
        void AddModifier(CharacterStats key, StatModifier modifier);
        void RemoveModifier(CharacterStats key, StatModifier modifier);

        bool HasStat(CharacterStats key);
        void Subscribe(CharacterStats key, StatChangedEvent callback);
        void UnSubscribe(CharacterStats key, StatChangedEvent callback);
    }
}