using System;
using Cysharp.Threading.Tasks;
using Game.Hero;
using Game.Stats;
using KinematicCharacterController;
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
            private IActorInputController _inputController;
            private KinematicCharacterMotor _kinematicCharacterMotor;


            public override bool CanActivateAbility()
            {
                if (_isDashActive) 
                    return false;
                return Owner.GetCurrentValue(CharacterStats.Stamina) >= Mathf.Abs(Definition.StaminaCost.Value);
            }
                 

            protected override void OnInitAbility()
            {
                _aim = Owner.GetAbility<AimAbility>();
                _inputController = Owner.GetComponent<IActorInputController>();
                _movementController = Owner.GetComponent<HeroInputController>();
                _kinematicCharacterMotor = Owner.GetComponent<KinematicCharacterMotor>();
            }

            protected override void OnActivateAbility()
            {
                _aim.EndAbility();
                _movementController.BlockInput(_isDashActive = true);
                Owner.ApplyModifier(CharacterStats.Stamina, Definition.StaminaCost);
                _inputController.BlockInput(true);
                
                Vector3 dashVelocity = _kinematicCharacterMotor.CharacterForward * Definition.DashRange;
                // to do: change to timer from assets and add ability deactivate after enviroment collision
                Owner.GetComponent<MovementController>().AddVelocity(dashVelocity);
                EndAbility();
            }
            
            protected override void OnEndAbility(bool wasCancelled)
            {
                _isDashActive = false;
                _movementController.BlockInput(false);
                Owner.RemoveModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
            }
            
        }
}