using System;
using Cysharp.Threading.Tasks;
using FMODUnity;
using Game.Animations.Hero;
using Game.Audio;
using Game.Stats;
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
        private ProjectileDefinition projectile;

        [SerializeField, Min(0)]
        private int manaCost;

        [SerializeField, Min(0)]
        private float castTime = 0.5f;

        public ProjectileDefinition Projectile => projectile;

        public int ManaCost => manaCost;

        public float CastTime => castTime;

        public EventReference FireSfx => fireSfx;
    }

    public class ProjectileAbility : ActorAbility<ProjectileAbilityDefinition>
    {
        private readonly FmodService _audio;
        private const int POOL_MAX_SIZE = 20;
        private readonly ObjectPool<Projectile> _projectilePool;

        private Transform _spawnPoint;
        private bool _hasMana;
        private ActorAnimator _animator;
        private bool _isAnimationEnded;

        private AimAbility _aim;
        private bool _hasAim;

        public ProjectileAbility(ProjectileFactory projectileFactory, FmodService audio)
        {
            _audio = audio;
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

        protected override void OnDestroyAbility()
        {
            _projectilePool.Clear();
            _projectilePool.Dispose();
        }

        private void OnCompleteProjectile(Projectile projectile)
            => _projectilePool.Release(projectile);

        protected override void OnInitAbility()
        {
            _hasMana = Owner.HasStat(CharacterStats.Mana);
            var view = Owner.GetComponent<ProjectileAbilityView>();
            _spawnPoint = view.SpawnPoint;
            _animator = Owner.GetComponent<ActorAnimator>();
            _isAnimationEnded = true;

            _hasAim = Owner.TryGetAbility(out _aim);
        }

        public override bool CanActivateAbility()
        {
            if (IsActive)
                return false;

            if (!_hasMana)
                return true;

            return Owner.GetCurrentValue(CharacterStats.Mana) >= Definition.ManaCost;
        }

        protected override async void OnActivateAbility()
        {
            if (_hasMana)
                Owner.ApplyModifier(CharacterStats.Mana, -Definition.ManaCost);

            if (_animator != null)
            {
                _animator.SetAnimation(AnimationNames.RangeAttack, true);
                _isAnimationEnded = false;

                try
                {
                    await WaitAnimationEnd();
                }
                catch (OperationCanceledException)
                {
                    EndAbility();
                    return;
                }

                _animator.SetAnimation(AnimationNames.RangeAttack, false);
            }

            FireProjectile();
            EndAbility();
        }

        private void FireProjectile()
        {
            _projectilePool.Get(out Projectile projectile);

            Vector3 targetPosition = _hasAim ? _aim.GetRealPosition() : Vector3.zero;
            projectile.Fire(_spawnPoint, targetPosition);

            if (!Definition.FireSfx.IsNull)
                _audio.PlayOneShot(Definition.FireSfx, Owner.Transform);
        }

        private async UniTask WaitAnimationEnd()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(Definition.CastTime), ignoreTimeScale: false, 
                cancellationToken: Owner.CancellationToken());
            _isAnimationEnded = true;
        }
    }
}