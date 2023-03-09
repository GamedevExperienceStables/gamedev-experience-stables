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

        public void Add(TimerUpdatable timer)
            => _timersToAdd.Add(timer);

        public void Remove(TimerUpdatable timer)
        {
            _timers.Remove(timer);
            _timersToAdd.Remove(timer);
        }

        public void Tick()
        {
            if (_timersToAdd.Count > 0)
            {
                _timers.AddRange(_timersToAdd);
                _timersToAdd.Clear();
            }

            for (int i = _timers.Count - 1; i >= 0; i--) 
                _timers[i].Tick();
        }

        public void Dispose()
        {
            _timers.Clear();
            _timersToAdd.Clear();
        }
    }
}