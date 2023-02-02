using System;
using Cysharp.Threading.Tasks;
using Game.Hero;
using Game.Stats;
using UnityEngine;

namespace Game.Actors
{
        [CreateAssetMenu(menuName = MENU_PATH + "DashAbility")]
        public class DashAbilityDefinition : AbilityDefinition<DashAbility>
        {
            [SerializeField]
            private StatModifier speedModifier;
            
            [SerializeField]
            private float dashRange;
            
            [SerializeField]
            private StatModifier staminaCost;
            
            
            public StatModifier SpeedModifier => speedModifier;
            public float DashRange => dashRange;
            public StatModifier StaminaCost => staminaCost;

        }

        public class DashAbility : ActorAbility<DashAbilityDefinition>
        {
            private HeroInputController _movementController;           
            public override bool CanActivateAbility()
                => Owner.GetCurrentValue(CharacterStats.Stamina) > Definition.StaminaCost.Value;

            protected override void OnInitAbility()
            {
                _movementController = Owner.GetComponent<HeroInputController>();
            }

            protected override void OnActivateAbility()
            {
                Owner.AddModifier(CharacterStats.Stamina, Definition.StaminaCost);
                Owner.AddModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
                float time = Definition.DashRange / Owner.GetCurrentValue(CharacterStats.MovementSpeed);
                UniTask.Run(() => StartDash(time));
            }

            private async UniTask StartDash(float time)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(time), ignoreTimeScale: false);
                Debug.Log("Dash ended");
                OnEndAbility(false);
            }
            
            protected override void OnEndAbility(bool wasCancelled)
            {
                Owner.RemoveModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
            }
            
        }
}