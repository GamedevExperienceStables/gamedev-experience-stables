using System;
using UnityEngine.Pool;
using VContainer;

namespace Game.TimeManagement
{
    public class TimerPool
    {
        private readonly ITimerUpdater _updater;
        private readonly ObjectPool<TimerUpdatable> _pool;

        [Inject]
        public TimerPool(ITimerUpdater updater, TimerFactory factory)
        {
            _updater = updater;

            _pool = new ObjectPool<TimerUpdatable>(
                createFunc: factory.CreateTimer,
                actionOnRelease: timer =>
                {
                    timer.Stop();
                    timer.Reset();
                });
        }

        public TimerUpdatable GetTimer()
        {
            _pool.Get(out TimerUpdatable timer);
            _updater.Add(timer);

            return timer;
        }

        public TimerUpdatable GetTimer(TimeSpan duration, Action onComplete = null, bool isLooped = false,
            bool ignoreTimeScale = false)
        {
            TimerUpdatable timer = GetTimer();
            timer.Init(duration, onComplete, isLooped, ignoreTimeScale);

            return timer;
        }

        public TimerUpdatable GetTimerStarted(TimeSpan duration, Action onComplete = null, bool isLooped = false,
            bool ignoreTimeScale = false)
        {
            TimerUpdatable timer = GetTimer(duration, onComplete, isLooped, ignoreTimeScale);
            timer.Start();
            return timer;
        }

        public void ReleaseTimer(TimerUpdatable timer)
        {
            _pool.Release(timer);
            _updater.Remove(timer);
        }
    }
}