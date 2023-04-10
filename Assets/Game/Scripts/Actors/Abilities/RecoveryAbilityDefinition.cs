using System;
using Game.Stats;
using Game.TimeManagement;
using UnityEngine;
using VContainer;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = MENU_PATH + "RecoveryAbility")]
    public class RecoveryAbilityDefinition : AbilityDefinition<RecoveryAbility>
    {
        [SerializeField]
        private StatModifier staminaRegeneration;

        [SerializeField]
        private StatModifier manaRegeneration;

        [SerializeField]
        private StatModifier healthRegeneration;

        [SerializeField]
        private float recoveryTime;

        public StatModifier StaminaRegeneration => staminaRegeneration;
        public StatModifier ManaRegeneration => manaRegeneration;
        public StatModifier HealthRegeneration => healthRegeneration;
        public float RecoveryTime => recoveryTime;
    }

    public class RecoveryAbility : ActorAbility<RecoveryAbilityDefinition>
    {
        private readonly TimerPool _timers;
        private TimerUpdatable _recoveryTimer;

        public override bool CanActivateAbility()
            => true;

        [Inject]
        public RecoveryAbility(TimerPool timers)
            => _timers = timers;

        protected override void OnInitAbility()
        {
            TimeSpan interval = TimeSpan.FromSeconds(Definition.RecoveryTime);
            _recoveryTimer = _timers.GetTimer(interval, Regeneration, isLooped: true);
        }

        protected override void OnDestroyAbility()
            => _timers.ReleaseTimer(_recoveryTimer);

        protected override void OnActivateAbility()
            => _recoveryTimer.Start();

        protected override void OnGiveAbility()
        {
            base.OnGiveAbility();
            ActivateAbility();
        }

        private void Regeneration()
        {
            Owner.ApplyModifier(CharacterStats.Stamina, Definition.StaminaRegeneration);
            Owner.ApplyModifier(CharacterStats.Mana, Definition.ManaRegeneration);
            Owner.ApplyModifier(CharacterStats.Health, Definition.HealthRegeneration);
        }

        protected override void OnEndAbility(bool wasCancelled)
            => _recoveryTimer.Stop();
    }
}