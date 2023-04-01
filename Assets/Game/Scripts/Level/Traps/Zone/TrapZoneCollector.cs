using Game.Actors;
using UnityEngine;

namespace Game.Level
{
    public class TrapZoneCollector
    {
        private readonly ActorCollector _collector = new();
        private readonly TrapZone _trap;

        public TrapZoneCollector(TrapZone trap)
        {
            _trap = trap;
            _collector.SetLayerMask(trap.LayerMask);
        }

        public void Clear()
        {
            foreach (IActorController actor in _collector.Items)
                _trap.Exit(actor);

            _collector.Clear();
        }

        public bool OnEnter(Collider other)
        {
            if (!_collector.TryAdd(other, out IActorController target)) 
                return false;
            
            _trap.Enter(target);
            return true;
        }

        public void OnExit(Collider other)
        {
            if (_collector.TryRemove(other, out IActorController target))
                _trap.Exit(target);
        }
    }
}