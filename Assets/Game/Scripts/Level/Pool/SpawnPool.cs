using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Game.Level
{
    public sealed class SpawnPool : IDisposable
    {
        private readonly Dictionary<int, ObjectPool<SpawnPoolTarget>> _pools = new();
        private readonly PrefabFactory _factory;
        private readonly PoolSettings _settings;

        private readonly List<SpawnPoolTarget> _buffer;

        public SpawnPool(PrefabFactory factory, PoolSettings settings)
        {
            _factory = factory;
            _settings = settings;

            _buffer = new List<SpawnPoolTarget>(_settings.prewarmCount);
        }

        public void Prewarm(GameObject definition)
        {
            if (_settings.prewarmCount == 0)
                return;
            
            var pool = GetPool(definition); 

            while (pool.CountAll < _settings.prewarmCount) 
                _buffer.Add(pool.Get());

            foreach (SpawnPoolTarget instance in _buffer) 
                pool.Release(instance);
            
            _buffer.Clear();
        }

        public void Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            SpawnPoolTarget instance = GetInstance(prefab);
            instance.Activate(position, rotation);
        }

        private SpawnPoolTarget GetInstance(GameObject prefab)
        {
            var pool = GetPool(prefab);
            SpawnPoolTarget instance = pool.Get();
            return instance;
        }

        private ObjectPool<SpawnPoolTarget> GetPool(GameObject prefab)
        {
            int originId = prefab.GetHashCode();
            if (!_pools.TryGetValue(originId, out var pool))
            {
                pool = CreatePool(prefab, originId);
                _pools[originId] = pool;
            }

            return pool;
        }


        private ObjectPool<SpawnPoolTarget> CreatePool(GameObject prefab, int originId)
        {
            var projectilePool = new ObjectPool<SpawnPoolTarget>(
                createFunc: () =>
                {
                    bool originState = prefab.activeSelf;
                    prefab.SetActive(false);

                    GameObject instance = _factory.Create(prefab);

                    prefab.SetActive(originState);

                    var target = instance.AddComponent<SpawnPoolTarget>();
                    target.Init(originId, OnInstanceDisable);

                    return target;
                },
                actionOnRelease: target => target.gameObject.SetActive(false),
                actionOnDestroy: target => Object.Destroy(target.gameObject),
                maxSize: _settings.maxPoolSize);

            return projectilePool;
        }

        private void OnInstanceDisable(SpawnPoolTarget target)
        {
            if (!_pools.TryGetValue(target.OriginId, out var pool))
            {
                Object.Destroy(target.gameObject);
                return;
            }

            pool.Release(target);
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