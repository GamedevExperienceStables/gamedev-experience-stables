using System;
using Game.Stats;
using Game.TimeManagement;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = MENU_PATH + "Stat Change")]
    public class EffectStatChangeDefinition : EffectDefinition<EffectStatChange>
    {
        [HideIf(nameof(EffectType), EffectDuration.Instant)]
        [SerializeField, Min(0)]
        private float interval;

        [Space]
        [SerializeField]
        private CharacterStats stat;

        [SerializeField]
        private StatModifier modifier;

        public StatModifier Modifier => modifier;

        public float Interval => interval;

        public CharacterStats Stat => stat;

        public override bool CanExecute(IActorController target)
            => target.HasStat(Stat);
    }

    public class EffectStatChange : Effect<EffectStatChangeDefinition>
    {
        private readonly TimerPool _timers;

        private IActorController _target;

        private TimerUpdatable _timerInterval;
        private TimerUpdatable _timerTotal;

        [Inject]
        public EffectStatChange(TimerPool timers)
            => _timers = timers;

        protected override void OnExecute(IActorController target)
        {
            _target = target;

            switch (Definition.EffectType)
            {
                case EffectDuration.Instant:
                {
                    ApplyModifier();
                    break;
                }

                case EffectDuration.Infinite:
                case EffectDuration.Limited:
                {
                    AddEffect();
                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(Definition.EffectType), Definition.EffectType, null);
            }
        }

        protected override void OnCancel()
        {
            if (Definition.Interval > 0)
                _timers.ReleaseTimer(_timerInterval);

            if (Definition.Duration > 0)
                _timers.ReleaseTimer(_timerTotal);

            _target.RemoveModifier(Definition.Stat, Definition.Modifier);
        }

        private void AddEffect()
        {
            float interval = Definition.Interval;
            float duration = Definition.Duration;

            if (duration > 0)
                _timerTotal = _timers.GetTimerStarted(TimeSpan.FromSeconds(duration), Cancel);

            if (interval > 0)
            {
                _timerInterval = _timers.GetTimerStarted(TimeSpan.FromSeconds(interval), ApplyModifier, isLooped: true);
                ApplyModifier();
            }
            else
                AddModifier();
        }

        private void AddModifier()
            => _target.AddModifier(Definition.Stat, Definition.Modifier);

        private void ApplyModifier()
        {
            if (!IsSuppressed)
                _target.ApplyModifier(Definition.Stat, Definition.Modifier);
        }
    }
}