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
            private bool _isDashActive;
            private AimAbility _aim;


            public override bool CanActivateAbility()
            {
                if (_aim.IsActive) return false;
                if (_isDashActive) return false;
                return Owner.GetCurrentValue(CharacterStats.Stamina) >= Mathf.Abs(Definition.StaminaCost.Value);
            }
                 

            protected override void OnInitAbility()
            {
                _aim = Owner.GetAbility<AimAbility>();
                _movementController = Owner.GetComponent<HeroInputController>();
            }

            protected override void OnActivateAbility()
            {
                _movementController.BlockInput(_isDashActive = true);
                Owner.ApplyModifier(CharacterStats.Stamina, Definition.StaminaCost);
                Owner.AddModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
                float time = Definition.DashRange / Owner.GetCurrentValue(CharacterStats.MovementSpeed);
                UniTask.Run(() => StartDash(time));
            }

            private async UniTask StartDash(float time)
            {
                // to do: change to timer from assets and add ability deactivate after enviroment collision
                await UniTask.Delay(TimeSpan.FromSeconds(time), ignoreTimeScale: false);
                OnEndAbility(false);
            }
            
            protected override void OnEndAbility(bool wasCancelled)
            {
                _movementController.BlockInput(_isDashActive = false);
                Owner.RemoveModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
            }
            
        }
}