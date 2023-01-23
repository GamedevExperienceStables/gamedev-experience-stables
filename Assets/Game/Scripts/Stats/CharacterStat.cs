using System;
using System.Collections.Generic;
using Game.Utils;

namespace Game.Stats
{
    public class CharacterStat : IReadOnlyCharacterStat
    {
        private readonly List<StatModifier> _statModifiers = new();

        private float _baseValue;
        
        private event Action<float> ValueChanged;

        public float BaseValue
        {
            get => _baseValue;
            set
            {
                if (MathExtensions.AlmostEquals(_baseValue, value))
                    return;
                
                _baseValue = value;
                
                CalculateFinalStatValue();
            }
        }

        public float Value { get; private set; }

        public void Subscribe(Action<float> action)
        {
            ValueChanged += action;

            action.Invoke(Value);
        }

        public void UnSubscribe(Action<float> action) 
            => ValueChanged -= action;

        public void Init(float baseValue)
        {
            _baseValue = baseValue;

            Reset();
        }

        private void Reset()
        {
            _statModifiers.Clear();
            
            CalculateFinalStatValue();
        }

        public void AddModifier(StatModifier statModifier)
        {
            _statModifiers.Add(statModifier);
            
            CalculateFinalStatValue();
        }

        public void RemoveModifier(StatModifier statModifier)
        {
            _statModifiers.Remove(statModifier);
            
            CalculateFinalStatValue();
        }


        private void CalculateFinalStatValue()
        {
            float finalValue = BaseValue;
            foreach (StatModifier statModifier in _statModifiers)
            {
                switch (statModifier.Type)
                {
                    case StatsModifierType.Flat:
                        finalValue += statModifier.Value;
                        break;
                    case StatsModifierType.Percent:
                        finalValue *= 1 + statModifier.Value;
                        break;
                }
            }

            finalValue = (float)Math.Round(finalValue, 2);

            Value = finalValue;
            
            ValueChanged?.Invoke(Value);
        }
    }
}