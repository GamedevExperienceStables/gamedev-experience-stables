using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Game.Level
{
    public sealed class SpawnPool : IDisposable
    {
        private const int POOL_MAX_SIZE = 20;

        private readonly Dictionary<int, ObjectPool<SpawnPoolTarget>> _pools = new();
        private readonly PrefabFactory _factory;

        public SpawnPool(PrefabFactory factory) 
            => _factory = factory;

        public void Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            SpawnPoolTarget instance = GetInstance(prefab);
            instance.Activate(position, rotation);
        }

        private SpawnPoolTarget GetInstance(GameObject prefab)
        {
            int originId = prefab.GetHashCode();
            if (!_pools.TryGetValue(originId, out var pool))
            {
                pool = CreatePool(prefab, originId);
                _pools[originId] = pool;
            }

            SpawnPoolTarget instance = pool.Get();
            return instance;
        }


        private ObjectPool<SpawnPoolTarget> CreatePool(GameObject prefab, int originId)
        {
            var projectilePool = new ObjectPool<SpawnPoolTarget>(
                createFunc: () =>
                {
                    GameObject instance = _factory.Create(prefab);
                    instance.SetActive(false);

                    var target = instance.AddComponent<SpawnPoolTarget>();
                    target.Init(originId, OnInstanceDisable);

                    return target;
                },
                actionOnRelease: target => target.gameObject.SetActive(false),
                actionOnDestroy: target => Object.Destroy(target.gameObject),
                maxSize: POOL_MAX_SIZE);

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