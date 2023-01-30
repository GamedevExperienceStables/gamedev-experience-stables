using System;
using System.Collections.Generic;
using Game.Stats;
using Game.Utils.Structures;
using UnityEngine;

namespace Game.Actors
{
    public class ActorStats : IStats
    {
        private readonly List<StatHandler> _handlers = new();
        private readonly Dictionary<CharacterStats, CharacterStat> _stats = new();

        private readonly DictionaryDelegate<CharacterStats, IStats.StatChangedEvent> _statChanged = new();

        public float this[CharacterStats key]
        {
            get => GetCurrentValue(_stats[key]);
            set => SetBaseValue(_stats[key], value);
        }
        
        public static float GetCurrentValue(CharacterStat stat)
            => stat.Value;

        public bool HasStat(CharacterStats key)
            => _stats.ContainsKey(key);

        public void Subscribe(CharacterStats key, IStats.StatChangedEvent callback)
        {
            _statChanged.AddListener(key, callback);

            float value = GetCurrentValue(_stats[key]);
            callback.Invoke(new StatValueChange(key,value,value));
        }

        public void UnSubscribe(CharacterStats key, IStats.StatChangedEvent callback)
            => _statChanged.RemoveListener(key, callback);

        public void AddModifier(CharacterStats key, StatModifier modifier) 
            => _stats[key].AddModifier(modifier);

        public void RemoveModifier(CharacterStats key, StatModifier modifier) 
            => _stats[key].RemoveModifier(modifier);

        public void ApplyModifier(CharacterStats key, StatModifierApplyData modifier)
        {
            if (!CanApplyModifier(modifier)) 
                return;
         
            CharacterStat stat = _stats[key];
            float newBase = ExecuteModifier(stat.BaseValue, modifier.type, modifier.value);
            SetBaseValue(stat, newBase);

            foreach (StatHandler handler in _handlers)
                handler.OnModifierApplied(this, modifier);
        }

        public void AddStatHandler(StatHandler handler)
            => _handlers.Add(handler);
        
        public void InitStat(CharacterStats key, float baseValue)
            => _stats[key].Init(baseValue);

        public void CreateStat(CharacterStats key)
        {
            if (_stats.ContainsKey(key))
            {
                Debug.LogWarning($"Trying add stat '{key}' that already exists");
                return;
            }

            var stat = new CharacterStat(key);
            stat.Subscribe(OnStatChanged);

            _stats.Add(key,stat);
        }

        public void RemoveStat(CharacterStats key)
        {
            if (!_stats.ContainsKey(key))
            {
                Debug.LogWarning($"Trying remove stat '{key}' that not exists");
                return;
            }

            CharacterStat stat = _stats[key];
            stat.UnSubscribe(OnStatChanged);
            
            _stats.Remove(key);
        }

        private void SetBaseValue(CharacterStat stat, float newValue)
        {
            foreach (StatHandler handler in _handlers)
                newValue = handler.OnStatBaseValueChanging(this, stat.Key, newValue);

            stat.BaseValue = newValue;
        }

        private void SetCurrentValue(CharacterStat stat, float newValue)
        {
            float oldValue = stat.Value;

            foreach (StatHandler handler in _handlers)
                newValue = handler.OnStatChanging(this, stat.Key, newValue);

            stat.Value = newValue;

            var onValueChangeData = new StatValueChange(stat.Key, oldValue, newValue);
            _statChanged[stat.Key]?.Invoke(onValueChangeData);

            foreach (StatHandler handler in _handlers)
                handler.OnStatValueChanged(this, onValueChangeData);
        }

        private static float ExecuteModifier(float value, StatsModifierType type, float magnitude)
        {
            switch (type)
            {
                case StatsModifierType.Flat:
                    value += magnitude;
                    break;

                case StatsModifierType.Percent:
                    value *= magnitude;
                    break;

                case StatsModifierType.Override:
                    value = magnitude;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return value;
        }

        private bool CanApplyModifier(StatModifierApplyData data)
        {
            foreach (StatHandler handler in _handlers)
            {
                if (!handler.OnModifierApplying(this, data))
                    return false;
            }

            return true;
        }

        private void OnStatChanged(CharacterStat stat)
        {
            float newValue = stat.CalculateFinalValue();
            SetCurrentValue(stat, newValue);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) 
                return;
            
            foreach (CharacterStat stat in _stats.Values)
                stat.UnSubscribe(OnStatChanged);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}