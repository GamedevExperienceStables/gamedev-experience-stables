using System;
using System.Collections.Generic;
using Game.Level;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Game.Weapons
{
    public sealed class ProjectilePool : IDisposable
    {
        private readonly Dictionary<int, ObjectPool<Projectile>> _pools = new();
        private readonly ProjectileFactory _factory;
        private readonly PoolSettings _settings;

        private readonly List<Projectile> _buffer;

        public ProjectilePool(ProjectileFactory factory, PoolSettings settings)
        {
            _factory = factory;
            _settings = settings;
            
            _buffer = new List<Projectile>(_settings.prewarmCount);
        }


        public void Prewarm(ProjectileDefinition definition)
        {
            if (_settings.prewarmCount == 0)
                return;
            
            var pool = GetPool(definition); 

            while (pool.CountAll < _settings.prewarmCount) 
                _buffer.Add(pool.Get());

            foreach (Projectile projectile in _buffer) 
                pool.Release(projectile);
            
            _buffer.Clear();
        }

        public Projectile Get(ProjectileDefinition definition)
        {
            var pool = GetPool(definition);
            return pool.Get();
        }

        private ObjectPool<Projectile> GetPool(ProjectileDefinition definition)
        {
            int originId = definition.Prefab.GetHashCode();
            if (!_pools.TryGetValue(originId, out var pool))
            {
                pool = CreatePool(definition);
                _pools[originId] = pool;
            }

            return pool;
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
                maxSize: _settings.maxPoolSize);

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