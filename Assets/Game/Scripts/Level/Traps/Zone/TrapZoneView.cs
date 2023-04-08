using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    [RequireComponent(typeof(TrapView))]
    public class TrapZoneView : MonoBehaviour
    {
        private TrapZone _trap;
        private int _durability = -1;

        private readonly List<TrapZoneCollector> _collectors = new();
        private GameObject _destroyVfx;

        public void SetDurability(int durability, GameObject destroyVfx)
        {
            _durability = durability;
            _destroyVfx = destroyVfx;
        }

        public void Add(TrapZone trap)
            => _collectors.Add(new TrapZoneCollector(trap));

        private void OnDestroy()
        {
            foreach (TrapZoneCollector collector in _collectors)
                collector.Clear();

            _collectors.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            bool triggered = false;
            foreach (TrapZoneCollector collector in _collectors)
                triggered |= collector.OnEnter(other);

            if (triggered && HasDurability())
                ReduceDurability();

            if (DurabilityEnded())
                DestroySelf();
        }

        private void OnTriggerExit(Collider other)
        {
            foreach (TrapZoneCollector collector in _collectors)
                collector.OnExit(other);
        }

        private bool HasDurability()
            => _durability > 0;

        private bool DurabilityEnded()
            => _durability == 0;

        private void ReduceDurability()
            => _durability--;

        private void DestroySelf()
        {
            Destroy(gameObject);

            if (_destroyVfx)
                SpawnVfx();
        }

        private void SpawnVfx()
            => Instantiate(_destroyVfx, transform.position, Quaternion.identity);
    }
}