using System.Collections.Generic;
using Game.Stats;
using UnityEngine;

namespace Game.Actors
{
    public interface IActorController
    {
        Transform Transform { get; }

        T GetComponent<T>();
        bool TryGetComponent<T>(out T component);

        bool HasStat(CharacterStats key);
        float GetCurrentValue(CharacterStats key);
        void ApplyModifier(CharacterStats key, float value);
        void ApplyModifier(CharacterStats key, StatModifier modifier);
        void AddModifier(CharacterStats key, StatModifier modifier);
        void RemoveModifier(CharacterStats key, StatModifier modifier);

        void Subscribe(CharacterStats key, IStats.StatChangedEvent callback);
        void UnSubscribe(CharacterStats key, IStats.StatChangedEvent callback);

        void GiveAbility(AbilityDefinition definition);
        T GetAbility<T>() where T : ActorAbility;
        bool TryGetAbility(AbilityDefinition definition, out ActorAbility foundAbility);
        bool TryGetAbility<T>(out T foundAbility) where T : ActorAbility;

        void ApplyEffect(Effect effect);
        void AddEffect(Effect effect);
        void CancelEffectsByInstigator(object instigator, IEnumerable<EffectDefinition> toCancel);
    }
}