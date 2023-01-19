using System;
using System.Collections.Generic;

namespace Game.Stats
{
    public class CharacterStat
    {
        public float baseValue;
        private bool _isDirty = true;

        public float Value
        {
            get
            {
                if (!_isDirty) return _value;
                _value = CalculateFinalStatValue();
                _isDirty = false;
                return _value;
            }
        }

        private float _value;
        private readonly List<StatModifier> _statModifiers;

        public CharacterStat(float baseValue)
        {
            this.baseValue = baseValue;
            _statModifiers = new List<StatModifier>();
        }
    
        public void AddModifier(StatModifier statModifier)
        {
            _isDirty = true;
            _statModifiers.Add(statModifier);    
        }
        
        public void RemoveModifier(StatModifier statModifier)
        {
            _isDirty = true;
            _statModifiers.Remove(statModifier);    
        }

        
        private float CalculateFinalStatValue()
        {
            var finalValue = baseValue;
            foreach (var statModifier in _statModifiers)
            {
                var modifier = statModifier;
                if (modifier.type == StatsModifierType.Flat)
                {
                    finalValue += statModifier.value;
                }
                else if (modifier.type == StatsModifierType.Percent)
                {
                    finalValue *= 1 + modifier.value;
                }
            }
            finalValue = (float)Math.Round(finalValue, 2);
            return finalValue;
        }
    }
}
