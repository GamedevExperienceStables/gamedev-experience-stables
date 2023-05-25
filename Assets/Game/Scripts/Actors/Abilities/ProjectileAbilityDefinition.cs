using System;
using Game.Animations.Hero;
using Game.Stats;
using Game.TimeManagement;
using Game.Weapons;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = MENU_PATH + "Projectile")]
    public class ProjectileAbilityDefinition : AbilityDefinition<ProjectileAbility>
    {
        [SerializeField, Expandable]
        private TargetingDefinition targeting;

        [SerializeField, Expandable]
        private ProjectileDefinition projectile;

        [SerializeField, Min(0)]
        private int manaCost;

        [Space]
        [SerializeField, Min(0)]
        private float fireTime = 0.1f;

        [SerializeField, Min(0)]
        private float castTime = 0.4f;

        [Space]
        [SerializeField]
        private float animationSpeedMultiplier = 1f;

        public ProjectileDefinition Projectile => projectile;

        public int ManaCost => manaCost;

        public float CastTime => castTime;
        public float FireTime => fireTime;

        public TargetingDefinition Targeting => targeting;

        public float AnimationSpeedMultiplier => animationSpeedMultiplier;
    }

    public class ProjectileAbility : ActorAbility<ProjectileAbilityDefinition>
    {
        private const float BACKWARD_THRESHOLD = 0.2f;

        private readonly TargetingHandler _targeting;
        private readonly TimerPool _timers;

        private readonly ProjectilePool _projectilePool;

        private Transform _spawnPoint;
        private bool _hasMana;
        private ActorAnimator _animator;

        private IActorInputController _input;

        private TimerUpdatable _fireTimer;
        private TimerUpdatable _castTimer;
        private Vector3 _targetPosition;

        public ProjectileAbility(TargetingHandler targeting, ProjectilePool projectilePool, TimerPool timers)
        {
            _targeting = targeting;
            _timers = timers;
            _projectilePool = projectilePool;
        }

        protected override void OnInitAbility()
        {
            _hasMana = Owner.HasStat(CharacterStats.Mana);
            var view = Owner.GetComponent<ProjectileAbilityView>();
            _spawnPoint = view.SpawnPoint;
            _input = Owner.GetComponent<IActorInputController>();
            _animator = Owner.GetComponent<ActorAnimator>();

            _fireTimer = _timers.GetTimer(TimeSpan.FromSeconds(Definition.FireTime), OnFire);
            _castTimer = _timers.GetTimer(TimeSpan.FromSeconds(Definition.CastTime), OnComplete);
            
            _projectilePool.Prewarm(Definition.Projectile);
        }

        protected override void OnDestroyAbility()
        {
            _timers.ReleaseTimer(_fireTimer);
            _timers.ReleaseTimer(_castTimer);
        }

        public override bool CanActivateAbility()
        {
            if (IsActive)
                return false;

            if (_input.HasAnyBlock(InputBlock.Action))
                return false;

            if (_hasMana)
                return Owner.GetCurrentValue(CharacterStats.Mana) >= Definition.ManaCost;

            return true;
        }

        protected override void OnActivateAbility()
        {
            if (_hasMana)
                Owner.ApplyModifier(CharacterStats.Mana, -Definition.ManaCost);

            if (Definition.Targeting.CollectTargetPosition is TargetCollecting.OnActivate)
                _targetPosition = GetTargetPosition();
            
            _fireTimer.Start();
            _castTimer.Start();
            
            _input.SetBlock(InputBlock.Rotation);

            SetAnimation(true);
        }

        private void OnFire()
        {
            if (Definition.Targeting.CollectTargetPosition is TargetCollecting.OnFire)
                _targetPosition = GetTargetPosition();

            PreventBackwardFiring();

            FireProjectile();
            
            SetAnimation(false);
        }

        private void PreventBackwardFiring()
        {
            Vector3 spawnPoint = _spawnPoint.position;
            Vector3 spawnPointForward = _spawnPoint.forward;
            Vector3 targetDirection = (_targetPosition - spawnPoint).normalized;

            float dot = Vector3.Dot(targetDirection, spawnPointForward);
            if (dot < BACKWARD_THRESHOLD)
                _targetPosition = spawnPoint + spawnPointForward;
        }

        private void OnComplete()
            => EndAbility();

        protected override void OnEndAbility(bool wasCancelled)
        {
            _fireTimer.Stop();
            _castTimer.Stop();
            
            SetAnimation(false);
            
            _input.RemoveBlock(InputBlock.Rotation);
        }

        private void SetAnimation(bool isActive)
        {
            float animationSpeed = isActive ? Definition.AnimationSpeedMultiplier : 1;
            _animator.SetAnimation(AnimationNames.AttackSpeedMultiplier, animationSpeed);
            _animator.SetAnimation(AnimationNames.RangeAttack, isActive);
        }

        private void FireProjectile()
        {
            Projectile projectile = _projectilePool.Get(Definition.Projectile);
            projectile.Fire(_spawnPoint, _targetPosition);
        }

        private Vector3 GetTargetPosition()
            => _targeting.GetTargetPosition(Definition.Targeting, Owner.Transform, _input, _spawnPoint);
    }
}