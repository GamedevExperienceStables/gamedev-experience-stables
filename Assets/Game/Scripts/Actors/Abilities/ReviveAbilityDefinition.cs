using System;
using Cysharp.Threading.Tasks;
using Game.Actors.Health;
using Game.Animations.Hero;
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
        private ActorAnimator _animator;

        protected override void OnInitAbility()
        {
            _deathController = Owner.GetComponent<DeathController>();
            _inputController = Owner.GetComponent<IActorInputController>();
            _animator = Owner.GetComponent<ActorAnimator>();
        }

        public override bool CanActivateAbility()
            => !IsActive;

        protected override void OnActivateAbility()
            => Revive().Forget();

        private async UniTask Revive()
        {
            _inputController.SetBlock(true);
            
            await PlayReviveAnimation();
            ResetStats();
            ResetDeath();

            EndAbility();
        }

        protected override void OnEndAbility(bool wasCancelled)
        {
            _inputController.SetBlock(false);
            
            RemoveAbility();
        }

        private void ResetDeath()
            => _deathController.Revive();

        private async UniTask PlayReviveAnimation()
        {
            if (Definition.ReviveFeedback)
                Object.Instantiate(Definition.ReviveFeedback, Owner.Transform.position, Quaternion.identity);
            
            _animator.SetAnimation(AnimationNames.Death, false);
            _animator.SetAnimation(AnimationNames.Revive, true);
            await UniTask.Delay(TimeSpan.FromSeconds(Definition.AnimationDuration));
            _animator.SetAnimation(AnimationNames.Revive, false);
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