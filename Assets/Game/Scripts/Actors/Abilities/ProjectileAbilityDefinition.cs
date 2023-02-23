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
        [SerializeField, Expandable]
        private ProjectileDefinition projectile;

        [SerializeField, Min(0)]
        private int manaCost;

        [SerializeField, Min(0)]
        private float castTime;

        public ProjectileDefinition Projectile => projectile;

        public int ManaCost => manaCost;
    }

    public class ProjectileAbility : ActorAbility<ProjectileAbilityDefinition>
    {
        private const int POOL_MAX_SIZE = 20;
        private readonly ObjectPool<Projectile> _projectilePool;

        private Transform _spawnPoint;
        private bool _hasMana;

        public ProjectileAbility(ProjectileFactory projectileFactory)
        {
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
        }

        public override bool CanActivateAbility()
        {
            if (IsActive)
                return false;

            if (!_hasMana)
                return true;

            return Owner.GetCurrentValue(CharacterStats.Mana) >= Definition.ManaCost;
        }

        protected override void OnActivateAbility()
        {
            if (_hasMana)
                Owner.ApplyModifier(CharacterStats.Mana, -Definition.ManaCost);
            
            _projectilePool.Get(out Projectile projectile);
            projectile.Fire(_spawnPoint);

            EndAbility();
        }
    }
}