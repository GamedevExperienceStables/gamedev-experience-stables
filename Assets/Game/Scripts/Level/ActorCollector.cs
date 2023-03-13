using System.Collections.Generic;
using Game.Actors;
using MoreMountains.Tools;
using UnityEngine;

namespace Game.Level
{
    public sealed class ActorCollector
    {
        private readonly List<IActorController> _items = new();

        private LayerMask _layerMask = ~0;

        public IEnumerable<IActorController> Items => _items.AsReadOnly();

        public void SetLayerMask(LayerMask layerMask)
            => _layerMask = layerMask;

        public bool TryAdd(Collider other, out IActorController actor)
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

        public bool TryRemove(Collider other, out IActorController actor)
        {
            if (!_layerMask.MMContains(other.gameObject))
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

        public void Clear()
            => _items.Clear();
    }
}