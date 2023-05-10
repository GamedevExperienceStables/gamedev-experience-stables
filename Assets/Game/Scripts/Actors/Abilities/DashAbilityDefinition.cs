using System;
using Game.Animations.Hero;
using Game.Level;
using Game.Stats;
using Game.TimeManagement;
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

        [SerializeField]
        private float animationTime = 0.15f;
        
        [SerializeField]
        private GameObject dashFeedback;

        public float DashRange => dashRange;
        public float StaminaCost => staminaCost;

        public float AnimationTime => animationTime;

        public GameObject DashFeedback => dashFeedback;
    }

    public class DashAbility : ActorAbility<DashAbilityDefinition>
    {
        private readonly TimerPool _timers;
        private readonly PrefabFactory _prefabFactory;

        private MovementController _movementController;
        private IActorInputController _inputController;
        private ActorAnimator _animator;

        private bool _hasModifier;
        private bool _hasStaminaModifier;

        private TimerUpdatable _animationTimer;
        private GameObject _dashFeedback;

        public DashAbility(TimerPool timers, PrefabFactory prefabFactory)
        {
            _timers = timers;
            _prefabFactory = prefabFactory;
        }

        public override bool CanActivateAbility()
        {
            if (IsActive)
                return false;

            if (!_movementController.IsGrounded)
                return false;
            
            if (_inputController.HasAnyBlock(InputBlock.Action))
                return false;

            return Owner.GetCurrentValue(CharacterStats.Stamina) >= Mathf.Abs(GetCost());
        }

        protected override void OnInitAbility()
        {
            _inputController = Owner.GetComponent<IActorInputController>();
            _movementController = Owner.GetComponent<MovementController>();
            _animator = Owner.GetComponent<ActorAnimator>();

            _hasModifier = Owner.HasStat(CharacterStats.DashMultiplier);
            _hasStaminaModifier = Owner.HasStat(CharacterStats.DashStaminaMultiplier);

            _animationTimer = _timers.GetTimer(TimeSpan.FromSeconds(Definition.AnimationTime), OnComplete);
            
            _dashFeedback = _prefabFactory.Create(Definition.DashFeedback, Owner.Transform);
            _dashFeedback.SetActive(false);
        }

        protected override void OnDestroyAbility()
            => _timers.ReleaseTimer(_animationTimer);

        protected override void OnActivateAbility()
        {
            SetAnimation(true);

            float dynamicCost = GetCost();
            Owner.ApplyModifier(CharacterStats.Stamina, -dynamicCost);

            _inputController.SetBlock(InputBlock.Rotation);
            _animationTimer.Start();
            
            _dashFeedback.SetActive(false);
            _dashFeedback.SetActive(true);
        }

        private void OnComplete()
            => EndAbility();

        protected override void OnEndAbility(bool wasCancelled)
        {
            SetAnimation(false);
            _animationTimer.Stop();
            _inputController.RemoveBlock(InputBlock.Rotation);

            if (!wasCancelled)
                AddVelocity();
        }

        private void AddVelocity()
        {
            Vector3 dashDirection = GetDashDirection();
            Vector3 dashVelocity = dashDirection * GetRange();
            _movementController.AddVelocity(dashVelocity);
        }

        private void SetAnimation(bool isActive)
            => _animator.SetAnimation(AnimationNames.Dash, isActive);

        private Vector3 GetDashDirection()
        {
            Vector3 direction = _inputController.DesiredDirection;

            if (direction.sqrMagnitude.AlmostZero())
                direction = Owner.Transform.forward;

            return direction;
        }

        private float GetCost()
        {
            float baseCost = Definition.StaminaCost;
            return _hasStaminaModifier
                ? baseCost.AddPercent(Owner.GetCurrentValue(CharacterStats.DashStaminaMultiplier))
                : baseCost;
        }

        private float GetRange()
        {
            float baseRange = Definition.DashRange;
            return _hasModifier
                ? baseRange.AddPercent(Owner.GetCurrentValue(CharacterStats.DashMultiplier))
                : baseRange;
        }
    }
}