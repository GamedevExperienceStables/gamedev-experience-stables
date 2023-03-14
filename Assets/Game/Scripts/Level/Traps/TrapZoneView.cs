using Game.Actors;
using UnityEngine;

namespace Game.Level
{
    [RequireComponent(typeof(TrapView))]
    public class TrapZoneView : MonoBehaviour
    {
        private TrapZone _trap;

        private readonly ActorCollector _collector = new();

        public void Init(TrapZone trap)
        {
            _trap = trap;
            _collector.SetLayerMask(trap.LayerMask);
        }

        private void OnDestroy()
        {
            foreach (IActorController actor in _collector.Items)
                _trap.Exit(actor);

            _collector.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_collector.TryAdd(other, out IActorController target))
                _trap.Enter(target);
        }

        private void OnTriggerExit(Collider other)
        {
            if (_collector.TryRemove(other, out IActorController target))
                _trap.Exit(target);
        }
    }
}