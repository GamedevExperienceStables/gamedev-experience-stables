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
            [SerializeField, Min(0)]
            private float dashRange;

            [SerializeField, Min(0)]
            private float staminaCost;
            
            public float DashRange => dashRange;
            public float StaminaCost => staminaCost;

        }

        public class DashAbility : ActorAbility<DashAbilityDefinition>
        {
            private MovementController _movementController;
            private IActorInputController _inputController;
            private ActorAnimator _animator;
            private bool _isAnimationEnded;
            
            private bool _hasModifier;
            private bool _hasStaminaModifier;

            public override bool CanActivateAbility()
            {
                if (IsActive) 
                    return false;

                if (!_movementController.IsGrounded)
                    return false;

                if (!_isAnimationEnded)
                    return false;
                
                return Owner.GetCurrentValue(CharacterStats.Stamina) >= Mathf.Abs(GetCost());
            }


            protected override void OnInitAbility()
            {
                _inputController = Owner.GetComponent<IActorInputController>();
                _movementController = Owner.GetComponent<MovementController>();
                _animator = Owner.GetComponent<ActorAnimator>();
                _isAnimationEnded = true;
                
                _hasModifier = Owner.HasStat(CharacterStats.DashMultiplier);
                _hasStaminaModifier = Owner.HasStat(CharacterStats.DashStaminaMultiplier);
            }

            protected override async void OnActivateAbility()
            {
                _animator.ResetAnimation(AnimationNames.Damage);
                _animator.SetAnimation(AnimationNames.Dash, true);
                _isAnimationEnded = false;
                bool isEnded = await WaitAnimationEnd();
                if (isEnded)
                {
                    EndAbility();
                    return;
                }
                _animator.SetAnimation(AnimationNames.Dash, false);
                _inputController.BlockInput(IsActive);
                Owner.ApplyModifier(CharacterStats.Stamina, -GetCost());

                Vector3 dashDirection = GetDashDirection();
                Vector3 dashVelocity = dashDirection * GetRange();
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

            private async UniTask<bool> WaitAnimationEnd()
            {
                float dashAnimationTime = Definition.DashRange * 10;
                bool isCancelled = await UniTask.Delay(TimeSpan.FromMilliseconds(dashAnimationTime), 
                    ignoreTimeScale: false, 
                    cancellationToken: Owner.CancellationToken()).SuppressCancellationThrow();
                _isAnimationEnded = true;
                return isCancelled;
            }

            private float GetCost()
            {
                float baseCost = Definition.StaminaCost;

                if (!_hasStaminaModifier)
                    return baseCost;
                
                float modifier = baseCost * Owner.GetCurrentValue(CharacterStats.DashStaminaMultiplier);
                float cost = baseCost + modifier;
                return cost;
            }

            private float GetRange()
            {
                float baseRange = Definition.DashRange;
                if (!_hasModifier)
                    return baseRange;

                float modifier = baseRange * Owner.GetCurrentValue(CharacterStats.DashMultiplier);
                float cost = baseRange + modifier;
                return cost;
            }
        }
}