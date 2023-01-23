using UnityEngine;

namespace Game.Stats
{
    public class CharacterStatWithMax : IReadOnlyCharacterStatWithMax
    {
        private readonly CharacterStat _current = new();
        private readonly CharacterStat _max = new();

        public IReadOnlyCharacterStat Current => _current;
        public IReadOnlyCharacterStat Max => _max;

        public void Init(float baseValue)
        {
            _current.Init(baseValue);
            _max.Init(baseValue);
        }

        public void SetValue(float newValue)
        {
            newValue = Mathf.Clamp(newValue, 0.0f, _max.Value);
            _current.BaseValue = newValue;
        }

        public void SetMaxValue(float newValue)
        {
            newValue = Mathf.Max(newValue, 0.0f);
            _max.BaseValue = newValue;
        }

        public void AddValue(float value)
            => SetValue(_current.BaseValue + value);

        public void SubtractValue(float value)
            => SetValue(_current.BaseValue - value);
    }
}