using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stats
{
    public class CharacterStat
    {
        private readonly List<StatModifier> _modifiers = new();

        private float _baseValue;
        private event Action<CharacterStat> ValueChanged;

        public CharacterStat(CharacterStats key, float baseValue = 0)
        {
            Key = key;

            _baseValue = baseValue;
            Value = baseValue;
        }

        public CharacterStats Key { get; }

        public float BaseValue
        {
            get => _baseValue;
            set
            {
                _baseValue = value;

                SetDirty();
            }
        }

        public float Value { get; set; }

        public void Init(float baseValue)
        {
            _baseValue = baseValue;
            _modifiers.Clear();

            SetDirty();
        }

        public void Subscribe(Action<CharacterStat> action)
            => ValueChanged += action;

        public void UnSubscribe(Action<CharacterStat> action)
            => ValueChanged -= action;

        public void AddModifier(StatModifier statModifier)
        {
            _modifiers.Add(statModifier);

            SetDirty();
        }

        public void RemoveModifier(StatModifier statModifier)
        {
            _modifiers.Remove(statModifier);

            SetDirty();
        }

        public float CalculateFinalValue()
        {
            float flat = 0;
            float percent = 1;

            foreach (StatModifier modifier in _modifiers)
            {
                switch (modifier.Type)
                {
                    case StatsModifierType.Override:
                        return modifier.Value;

                    case StatsModifierType.Flat:
                        flat += modifier.Value;
                        break;

                    case StatsModifierType.Percent:
                        percent += modifier.Value;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return (BaseValue + flat) * percent;
        }

        private void SetDirty()
            => ValueChanged?.Invoke(this);
    }
}