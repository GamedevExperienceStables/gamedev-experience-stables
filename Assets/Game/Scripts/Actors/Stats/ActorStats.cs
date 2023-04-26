using System;
using System.Collections.Generic;
using Game.Stats;
using Game.Utils;
using Game.Utils.Structures;
using UnityEngine;

namespace Game.Actors
{
    public class ActorStats : IStats
    {
        private readonly List<StatHandler> _handlers = new();
        private readonly Dictionary<CharacterStats, CharacterStat> _stats = new();

        private readonly DictionaryDelegate<CharacterStats, IStats.StatChangedEvent> _statChanged = new();
        private readonly List<StatValueChange> _bufferStatChanges = new();

        public float this[CharacterStats key]
        {
            get => GetCurrentValue(_stats[key]);
            set => SetBaseValue(_stats[key], value);
        }

        private static float GetCurrentValue(CharacterStat stat)
            => stat.Value;

        public bool HasStat(CharacterStats key)
            => _stats.ContainsKey(key);

        public void Subscribe(CharacterStats key, IStats.StatChangedEvent callback)
        {
            _statChanged.AddListener(key, callback);

            float value = GetCurrentValue(_stats[key]);
            callback.Invoke(new StatValueChange(key, value, value));
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

            CheckChanges();
        }

        public void AddStatHandler(StatHandler handler)
            => _handlers.Add(handler);

        public void InitStat(CharacterStats key, float baseValue)
        {
            _stats[key].Init(baseValue);

            CheckChanges();
        }

        public void CreateStat(CharacterStats key)
        {
            if (_stats.ContainsKey(key))
            {
                Debug.LogWarning($"Trying add stat '{key}' that already exists");
                return;
            }

            var stat = new CharacterStat(key);
            stat.Subscribe(OnStatChanged);

            _stats.Add(key, stat);
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
            RegisterStatChanges(stat, onValueChangeData);

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
            if (!HasStat(data.stat))
                return false;

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

        private void RegisterStatChanges(CharacterStat stat, StatValueChange change)
        {
            // squashing changes from very first to last change
            for (int i = 0; i < _bufferStatChanges.Count; i++)
            {
                StatValueChange oldChange = _bufferStatChanges[i];
                if (oldChange.stat != stat.Key)
                    continue;

                _bufferStatChanges[i] = new StatValueChange(stat.Key, oldChange.oldValue, change.newValue);
                return;
            }

            _bufferStatChanges.Add(change);
        }

        private void CheckChanges()
        {
            for (int i = _bufferStatChanges.Count - 1; i >= 0; i--)
            {
                StatValueChange change = _bufferStatChanges[i];
                if (!change.oldValue.AlmostEquals(change.newValue))
                {
                    if (_statChanged.TryGetValue(change.stat, out IStats.StatChangedEvent callback))
                        callback?.Invoke(change);
                }

                _bufferStatChanges.RemoveAt(i);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            foreach (CharacterStat stat in _stats.Values)
                stat.UnSubscribe(OnStatChanged);

            _bufferStatChanges.Clear();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}