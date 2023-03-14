using System;
using System.Collections.Generic;
using Game.Actors;
using Game.Actors.Health;
using Game.TimeManagement;
using UnityEngine;
using VContainer;

namespace Game.Level
{
    [RequireComponent(typeof(ZoneEffect))]
    public class ZoneDamageablePeriodic : MonoBehaviour
    {
        [SerializeField, Min(1)]
        private float damage;

        [SerializeField, Min(0.1f)]
        private float interval;

        private readonly List<DamageableController> _damageables = new();

        private ZoneEffect _zone;

        private TimerPool _timers;
        private TimerUpdatable _timer;

        [Inject]
        public void Construct(TimerPool timers)
        {
            _timers = timers;
            _timer = _timers.GetTimerStarted(TimeSpan.FromSeconds(interval), OnInterval, isLooped: true);

            _zone = GetComponent<ZoneEffect>();
            _zone.ActorAdded += OnActorAdded;
            _zone.ActorRemoved += OnActorRemoved;
        }

        private void OnDestroy()
        {
            if (!_zone)
                return;

            _zone.ActorAdded -= OnActorAdded;
            _zone.ActorRemoved -= OnActorRemoved;

            _timers.ReleaseTimer(_timer);
        }

        private void OnActorAdded(IActorController actor)
        {
            if (actor.TryGetComponent(out DamageableController damageable)) 
                _damageables.Add(damageable);
        }

        private void OnActorRemoved(IActorController actor)
        {
            if (actor.TryGetComponent(out DamageableController damageable)) 
                _damageables.Remove(damageable);
        }

        private void OnInterval()
        {
            foreach (DamageableController damageable in _damageables)
                damageable.Damage(damage);
        }
    }
}