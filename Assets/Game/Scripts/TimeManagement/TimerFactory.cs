using System;
using VContainer;

namespace Game.TimeManagement
{
    public class TimerFactory
    {
        private readonly ITimeProvider _timeProvider;

        [Inject]
        public TimerFactory(ITimeProvider timeProvider)
            => _timeProvider = timeProvider;

        public TimerUpdatable CreateTimer(TimeSpan duration)
            => new(duration, false, _timeProvider);
    }
}