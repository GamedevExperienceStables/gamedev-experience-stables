using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Game.TimeManagement
{
    [UsedImplicitly]
    public sealed class TimerUpdater : ITimerUpdater, ITickable, IDisposable
    {
        private readonly List<TimerUpdatable> _timers = new();
        private readonly List<TimerUpdatable> _timersToAdd = new();
        private readonly List<TimerUpdatable> _timersToRemove = new();

        public void Add(TimerUpdatable timer)
            => _timersToAdd.Add(timer);

        public void Remove(TimerUpdatable timer)
            => _timersToRemove.Add(timer);

        public void Tick()
        {
            AddTimers();
            UpdateTimers();
            RemoveTimers();
        }

        private void UpdateTimers()
        {
            foreach (TimerUpdatable timer in _timers)
                timer.Tick();
        }

        private void AddTimers()
        {
            if (_timersToAdd.Count <= 0)
                return;

            _timers.AddRange(_timersToAdd);
            _timersToAdd.Clear();
        }

        private void RemoveTimers()
        {
            if (_timersToRemove.Count <= 0)
                return;

            foreach (TimerUpdatable timer in _timersToRemove)
                _timers.Remove(timer);

            _timersToRemove.Clear();
        }

        public void Dispose()
        {
            _timers.Clear();
            _timersToAdd.Clear();
            _timersToRemove.Clear();
        }
    }
}