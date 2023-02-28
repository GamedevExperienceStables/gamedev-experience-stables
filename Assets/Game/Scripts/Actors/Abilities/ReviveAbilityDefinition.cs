using System;
using Cysharp.Threading.Tasks;
using Game.Actors.Health;
using Game.Stats;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = MENU_PATH + "Revive")]
    public class ReviveAbilityDefinition : AbilityDefinition<ReviveAbility>
    {
        [SerializeField]
        private GameObject reviveFeedback;

        [SerializeField, Min(0)]
        private float animationDuration = 1f;

        public GameObject ReviveFeedback => reviveFeedback;
        
        public float AnimationDuration => animationDuration;
    }

    public class ReviveAbility : ActorAbility<ReviveAbilityDefinition>
    {
        private DeathController _deathController;
        private IActorInputController _inputController;

        protected override void OnInitAbility()
        {
            _deathController = Owner.GetComponent<DeathController>();
            _inputController = Owner.GetComponent<IActorInputController>();
        }

        public override bool CanActivateAbility()
            => !IsActive;

        protected override void OnActivateAbility()
            => Revive().Forget();

        private async UniTask Revive()
        {
            _inputController.BlockInput(true);
            
            await PlayReviveAnimation();
            ResetStats();
            ResetDeath();

            EndAbility();
        }

        protected override void OnEndAbility(bool wasCancelled)
        {
            _inputController.BlockInput(false);
            RemoveAbility();
        }

        private void ResetDeath()
            => _deathController.Revive();

        private async UniTask PlayReviveAnimation()
        {
            if (Definition.ReviveFeedback)
                Object.Instantiate(Definition.ReviveFeedback, Owner.Transform.position, Quaternion.identity);
            
            await UniTask.Delay(TimeSpan.FromSeconds(Definition.AnimationDuration));
        }

        private void ResetStats()
        {
            ResetStat(CharacterStats.Health, CharacterStats.HealthMax);
            ResetStat(CharacterStats.Mana, CharacterStats.ManaMax);
            ResetStat(CharacterStats.Stamina, CharacterStats.StaminaMax);
        }

        private void ResetStat(CharacterStats current, CharacterStats max)
        {
            var resetModifier = new StatModifier(Owner.GetCurrentValue(max), StatsModifierType.Override);
            Owner.ApplyModifier(current, resetModifier);
        }
    }
}