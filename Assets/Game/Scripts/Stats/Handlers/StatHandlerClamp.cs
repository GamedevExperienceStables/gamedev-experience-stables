using UnityEngine;

namespace Game.Stats
{
    public class StatHandlerClamp : StatHandler
    {
        private readonly CharacterStats _affectedStat;
        private readonly CharacterStats _maxStat;

        public StatHandlerClamp(CharacterStats affectedStat, CharacterStats maxStat)
        {
            _affectedStat = affectedStat;
            _maxStat = maxStat;
        }

        public override void OnModifierApplied(IStats stats, StatModifierApplyData data)
        {
            if (data.stat != _affectedStat)
                return;

            float currentValue = stats[_affectedStat];
            float newValue = ClampValue(currentValue, stats[_maxStat]);
            
            if (AreEquals(currentValue, newValue))
                return;

            stats[_affectedStat] = newValue;
        }

        private static bool AreEquals(float currentValue, float newValue) 
            => Mathf.Approximately(currentValue, newValue);

        private static float ClampValue(float current, float max) 
            => Mathf.Clamp(current, 0, max);
    }
}