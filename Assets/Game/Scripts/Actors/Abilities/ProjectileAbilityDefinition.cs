using System;
using Cysharp.Threading.Tasks;
using Game.Animations.Hero;
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
                await WaitAnimationEnd();
                _animator.SetAnimation(AnimationNames.RangeAttack, false);
            }
            
            _projectilePool.Get(out Projectile projectile);
            projectile.Fire(_spawnPoint);
            EndAbility();
        }
        
        private async UniTask WaitAnimationEnd()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), ignoreTimeScale: false);
            _isAnimationEnded = true;
        }
    }
}