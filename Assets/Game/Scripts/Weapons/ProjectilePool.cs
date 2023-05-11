using System;
using System.Collections.Generic;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Game.Weapons
{
    public sealed class ProjectilePool : IDisposable
    {
        private const int POOL_MAX_SIZE = 20;

        private readonly Dictionary<int, ObjectPool<Projectile>> _pools = new();
        private readonly ProjectileFactory _factory;

        public ProjectilePool(ProjectileFactory factory)
            => _factory = factory;

        public Projectile Get(ProjectileDefinition definition)
        {
            int originId = definition.Prefab.GetHashCode();
            if (!_pools.TryGetValue(originId, out var pool))
            {
                pool = CreatePool(definition);
                _pools[originId] = pool;
            }

            return pool.Get();
        }

        private ObjectPool<Projectile> CreatePool(ProjectileDefinition definition)
        {
            var projectilePool = new ObjectPool<Projectile>(
                createFunc: () =>
                {
                    Projectile projectile = _factory.Create(definition.Prefab, definition);
                    projectile.Completed += OnCompleteProjectile;
                    return projectile;
                },
                actionOnRelease: target => target.gameObject.SetActive(false),
                actionOnDestroy: target => Object.Destroy(target.gameObject),
                maxSize: POOL_MAX_SIZE);

            return projectilePool;
        }


        public void OnCompleteProjectile(Projectile projectile)
        {
            if (!_pools.TryGetValue(projectile.OriginId, out var pool))
            {
                Object.Destroy(projectile.gameObject);
                return;
            }

            pool.Release(projectile);
        }

        public void Clear()
        {
            foreach (var pool in _pools.Values)
                pool.Clear();

            _pools.Clear();
        }

        public void Dispose()
            => _pools.Clear();
    }
}