using System;
using Cysharp.Threading.Tasks;
using Game.Animations.Hero;
using Game.Stats;
using Game.Utils;
using UnityEngine;

namespace Game.Actors
{
        [CreateAssetMenu(menuName = MENU_PATH + "DashAbility")]
        public class DashAbilityDefinition : AbilityDefinition<DashAbility>
        {
            [SerializeField]
            private float dashRange;
            
            [SerializeField]
            private StatModifier staminaCost;
            
            public float DashRange => dashRange;
            public StatModifier StaminaCost => staminaCost;

        }

        public class DashAbility : ActorAbility<DashAbilityDefinition>
        {
            private MovementController _movementController;
            private IActorInputController _inputController;
            private ActorAnimator _animator;
            private bool _isAnimationEnded;

            public override bool CanActivateAbility()
            {
                if (IsActive) 
                    return false;

                if (!_movementController.IsGrounded)
                    return false;
                
                return Owner.GetCurrentValue(CharacterStats.Stamina) >= Mathf.Abs(Definition.StaminaCost.Value);
            }
                 

            protected override void OnInitAbility()
            {
                _inputController = Owner.GetComponent<IActorInputController>();
                _movementController = Owner.GetComponent<MovementController>();
                _animator = Owner.GetComponent<ActorAnimator>();
                _isAnimationEnded = true;
            }

            protected override async void OnActivateAbility()
            {
                if (_animator != null)
                {
                    _animator.ResetAnimation(AnimationNames.Damage);
                    _animator.SetAnimation(AnimationNames.Dash, true);
                    _isAnimationEnded = false;
                    await WaitAnimationEnd();
                    _animator.SetAnimation(AnimationNames.Dash, false);
                }
                
                _inputController.BlockInput(IsActive);
                Owner.ApplyModifier(CharacterStats.Stamina, Definition.StaminaCost);

                Vector3 dashDirection = GetDashDirection();
                Vector3 dashVelocity = dashDirection * Definition.DashRange;
                // to do: change to timer from assets and add ability deactivate after enviroment collision
                _movementController.AddVelocity(dashVelocity);
                EndAbility();
            }
            
            protected override void OnEndAbility(bool wasCancelled)
            {
                _inputController.BlockInput(false);
            }

            private Vector3 GetDashDirection()
            {
                Vector3 direction = _inputController.DesiredDirection;
                
                if (direction.sqrMagnitude.AlmostZero())
                    direction = Owner.Transform.forward;
                
                return direction;
            }
            
            private async UniTask WaitAnimationEnd()
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f), ignoreTimeScale: false, 
                    cancellationToken: Owner.CancellationToken()).SuppressCancellationThrow();
                _isAnimationEnded = true;
            }
        }
}