using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace Game.Level
{
    public class ComponentCollector<T>
    {
        private readonly List<T> _items = new();

        private LayerMask _layerMask = ~0;

        public IEnumerable<T> Items => _items.AsReadOnly();

        public void SetLayerMask(LayerMask layerMask)
            => _layerMask = layerMask;

        public bool TryAdd(GameObject other, out T actor)
        {
            if (!_layerMask.MMContains(other.gameObject))
            {
                actor = default;
                return false;
            }

            if (!other.TryGetComponent(out actor))
                return false;

            if (_items.Contains(actor))
                return false;

            _items.Add(actor);
            return true;
        }

        public bool TryAdd(Collider other, out T actor)
            => TryAdd(other.gameObject, out actor);

        public bool TryRemove(GameObject other, out T actor)
        {
            if (!_layerMask.MMContains(other))
            {
                actor = default;
                return false;
            }

            if (!other.TryGetComponent(out actor))
                return false;

            if (!_items.Contains(actor))
                return false;

            _items.Remove(actor);
            return true;
        }

        public bool TryRemove(Collider other, out T actor)
            => TryRemove(other.gameObject, out actor);

        public void Clear()
            => _items.Clear();
    }
}