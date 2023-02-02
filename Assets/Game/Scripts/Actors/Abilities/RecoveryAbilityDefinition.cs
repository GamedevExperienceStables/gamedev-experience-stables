using System;
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
        public override bool CanActivateAbility()
            => true;
        

        protected override void OnActivateAbility()
        {
            UniTask.Run(Regeneration);
        }

        private async UniTask Regeneration()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(Definition.RecoveryTime), ignoreTimeScale: false);
                Owner.AddModifier(CharacterStats.Stamina, Definition.StaminaRegeneration);
                Owner.AddModifier(CharacterStats.Stamina, Definition.ManaRegeneration);
                Owner.AddModifier(CharacterStats.Stamina, Definition.HealthRegeneration);
            }
        }
    }
}