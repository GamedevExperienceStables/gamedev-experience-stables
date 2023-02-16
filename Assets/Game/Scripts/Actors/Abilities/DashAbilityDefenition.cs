﻿using System;
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
            private HeroInputController _heroInput;
            private MovementController _movementController;
            private AimAbility _aim;
            private IActorInputController _inputController;
            private KinematicCharacterMotor _kinematicCharacterMotor;

            public override bool CanActivateAbility()
            {
                if (IsActive) 
                    return false;
                return Owner.GetCurrentValue(CharacterStats.Stamina) >= Mathf.Abs(Definition.StaminaCost.Value);
            }
                 

            protected override void OnInitAbility()
            {
                _aim = Owner.GetAbility<AimAbility>();
                _inputController = Owner.GetComponent<IActorInputController>();
                _heroInput = Owner.GetComponent<HeroInputController>();
                _kinematicCharacterMotor = Owner.GetComponent<KinematicCharacterMotor>();
                _movementController = Owner.GetComponent<MovementController>();
            }

            protected override void OnActivateAbility()
            {
                _heroInput.BlockInput(IsActive);
                Owner.ApplyModifier(CharacterStats.Stamina, Definition.StaminaCost);
                _inputController.BlockInput(true);
                
                Vector3 dashVelocity = _kinematicCharacterMotor.CharacterForward * Definition.DashRange;
                // to do: change to timer from assets and add ability deactivate after enviroment collision
                _movementController.AddVelocity(dashVelocity);
                EndAbility();
            }
            
            protected override void OnEndAbility(bool wasCancelled)
            {
                _heroInput.BlockInput(false);
                Owner.RemoveModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
            }
            
        }
}