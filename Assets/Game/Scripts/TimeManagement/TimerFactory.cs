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

        public TimerUpdatable CreateTimer()
            => new(_timeProvider);

        public TimerUpdatable CreateTimer(TimeSpan duration, Action onComplete = null, bool isLooped = false,
            bool ignoreTimeScale = false)
        {
            TimerUpdatable timer = CreateTimer();
            timer.Init(duration, onComplete, isLooped, ignoreTimeScale);
            return timer;
        }
    }
}