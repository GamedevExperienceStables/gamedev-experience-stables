﻿using System;
using FMODUnity;
using Game.Animations.Hero;
using Game.Audio;
using Game.Stats;
using Game.TimeManagement;
using Game.Weapons;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = MENU_PATH + "Projectile")]
    public class ProjectileAbilityDefinition : AbilityDefinition<ProjectileAbility>
    {
        [SerializeField]
        private EventReference fireSfx;

        [SerializeField, Expandable]
        private TargetingDefinition targeting;

        [SerializeField, Expandable]
        private ProjectileDefinition projectile;

        [SerializeField, Min(0)]
        private int manaCost;

        [SerializeField, Min(0)]
        private float fireTime = 0.1f;

        [SerializeField, Min(0)]
        private float castTime = 0.4f;

        public ProjectileDefinition Projectile => projectile;

        public int ManaCost => manaCost;

        public float CastTime => castTime;
        public float FireTime => fireTime;

        public EventReference FireSfx => fireSfx;

        public TargetingDefinition Targeting => targeting;
    }

    public class ProjectileAbility : ActorAbility<ProjectileAbilityDefinition>
    {
        private readonly FmodService _audio;
        private readonly TimerPool _timers;

        private const int POOL_MAX_SIZE = 20;
        private readonly ObjectPool<Projectile> _projectilePool;

        private Transform _spawnPoint;
        private bool _hasMana;
        private ActorAnimator _animator;

        private IActorInputController _input;

        private TimerUpdatable _fireTimer;
        private TimerUpdatable _castTimer;
        private Vector3 _targetPosition;
        
        private bool _forceCollectOnFire;

        public ProjectileAbility(ProjectileFactory projectileFactory, FmodService audio, TimerPool timers)
        {
            _audio = audio;
            _timers = timers;

            _projectilePool = new ObjectPool<Projectile>(
                createFunc: () =>
                {
                    Projectile projectile = projectileFactory.Create(Definition.Projectile);
                    projectile.Completed += OnCompleteProjectile;
                    return projectile;
                },
                actionOnDestroy: projectile => projectile.Completed -= OnCompleteProjectile,
                maxSize: POOL_MAX_SIZE);
        }

        private void OnCompleteProjectile(Projectile projectile)
            => _projectilePool.Release(projectile);

        protected override void OnInitAbility()
        {
            _hasMana = Owner.HasStat(CharacterStats.Mana);
            var view = Owner.GetComponent<ProjectileAbilityView>();
            _spawnPoint = view.SpawnPoint;
            _input = Owner.GetComponent<IActorInputController>();
            _animator = Owner.GetComponent<ActorAnimator>();

            _fireTimer = _timers.GetTimer(TimeSpan.FromSeconds(Definition.FireTime), OnFire);
            _castTimer = _timers.GetTimer(TimeSpan.FromSeconds(Definition.CastTime), OnComplete);
        }

        protected override void OnDestroyAbility()
        {
            _projectilePool.Clear();
            _projectilePool.Dispose();

            _timers.ReleaseTimer(_fireTimer);
            _timers.ReleaseTimer(_castTimer);
        }

        public override bool CanActivateAbility()
        {
            if (IsActive)
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
            {
                _targetPosition = GetTargetPosition();

                Vector3 toTarget = _targetPosition - Owner.Transform.position;
                float sqrMinDistance = Definition.Targeting.MinDistanceToTarget * Definition.Targeting.MinDistanceToTarget;
                _forceCollectOnFire = toTarget.sqrMagnitude < sqrMinDistance;
            }

            _fireTimer.Start();
            _castTimer.Start();

            _input.SetBlock(InputBlock.Rotation);

            SetAnimation(true);
        }

        private void OnFire()
        {
            if (Definition.Targeting.CollectTargetPosition is TargetCollecting.OnFire)
                _targetPosition = GetTargetPosition();
            else if (_forceCollectOnFire)
                _targetPosition = _spawnPoint.position + _spawnPoint.forward;

            FireProjectile();

            SetAnimation(false);
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
            => _animator.SetAnimation(AnimationNames.RangeAttack, isActive);

        private void FireProjectile()
        {
            _projectilePool.Get(out Projectile projectile);

            projectile.Fire(_spawnPoint, _targetPosition);

            if (!Definition.FireSfx.IsNull)
                _audio.PlayOneShot(Definition.FireSfx, Owner.Transform);
        }

        private Vector3 GetTargetPosition()
        {
            TargetingDefinition targeting = Definition.Targeting;

            Vector3 position = Owner.Transform.position;
            bool groundedPosition = targeting.RelativeToGround;
            Vector3 targetPosition = _input.GetTargetPosition(groundedPosition);

            Vector3 origin = _spawnPoint.position;
            if (groundedPosition)
                origin.y = position.y;

            if (!targeting.AllowTargetAbove)
                targetPosition.y = Mathf.Min(targetPosition.y, origin.y);

            float sqrDistance = (targetPosition - position).sqrMagnitude;
            bool allowTargetBelow = targeting.AllowTargetBelow;
            float minDistanceBelow = targeting.MinDistanceToTargetBelow;
            if (allowTargetBelow && minDistanceBelow > 0)
            {
                float sqrMinDistance = minDistanceBelow * minDistanceBelow;
                allowTargetBelow = sqrDistance >= sqrMinDistance;
            }

            if (!allowTargetBelow)
                targetPosition.y = Mathf.Max(targetPosition.y, origin.y);

            targetPosition = HandleMinDistanceToTarget(targetPosition, origin, targeting.MinDistanceToTarget);

            targetPosition += targeting.TargetPositionOffset;

            return targetPosition;
        }

        private Vector3 HandleMinDistanceToTarget(Vector3 targetPosition, Vector3 origin, float minDistanceToTarget)
        {
            if (minDistanceToTarget <= 0)
                return targetPosition;

            float sqrDistance = (targetPosition - Owner.Transform.position).sqrMagnitude;
            float sqrMinDistance = minDistanceToTarget * minDistanceToTarget;
            if (sqrMinDistance >= sqrDistance)
                targetPosition = origin + _spawnPoint.forward;

            return targetPosition;
        }
    }
}