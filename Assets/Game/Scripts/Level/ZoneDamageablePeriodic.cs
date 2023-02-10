using System;
using System.Collections.Generic;
using Game.Actors;
using Game.Actors.Health;
using Game.Stats;
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
        private TimerUpdatable _timer;
        private StatModifier _modifier;
        private bool _initialized;

        [Inject]
        public void Construct(TimerFactory timers)
        {
            _modifier = new StatModifier(-damage, StatsModifierType.Flat);
            _timer = timers.CreateTimer(TimeSpan.FromSeconds(interval), OnInterval, isLooped: true);

            _initialized = true;
        }

        private void Awake()
            => _zone = GetComponent<ZoneEffect>();

        private void OnEnable()
        {
            _zone.ActorAdded += OnActorAdded;
            _zone.ActorRemoved += OnActorRemoved;
        }

        private void OnDisable()
        {
            _zone.ActorAdded -= OnActorAdded;
            _zone.ActorRemoved -= OnActorRemoved;
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

        private void Update()
        {
            if (_initialized)
                _timer.Tick();
        }

        private void OnInterval()
        {
            foreach (DamageableController damageable in _damageables)
                damageable.Damage(_modifier);
        }
    }
}