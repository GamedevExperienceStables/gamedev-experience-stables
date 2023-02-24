using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Stats;
using UnityEngine;

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
        private CancellationTokenSource _cancellationTokenSource;
        public override bool CanActivateAbility()
            => true;
        

        protected override void OnActivateAbility()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            UniTask.Run(Regeneration).Forget();
        }
        
        protected override void OnGiveAbility()
        {
            base.OnGiveAbility();
            ActivateAbility();
        }
        
        private async UniTask Regeneration()
        {
            while (IsActive)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(Definition.RecoveryTime), cancellationToken:_cancellationTokenSource.Token);
                Owner.ApplyModifier(CharacterStats.Stamina, Definition.StaminaRegeneration);
                Owner.ApplyModifier(CharacterStats.Mana, Definition.ManaRegeneration);
                Owner.ApplyModifier(CharacterStats.Health, Definition.HealthRegeneration);
            }
        }
        
        protected override void OnEndAbility(bool wasCancelled)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }
}