using System;

namespace Game.TimeManagement
{
    public class TimerUpdatable
    {
        private readonly bool _ignoreTimeScale;
        private readonly ITimeProvider _timeProvider;

        private readonly float _duration;

        private float _timeElapsedBeforeCancel;
        private float _timeElapsedBeforePause;

        private float _startTime;
        private float _lastUpdateTime;

        public bool IsCompleted { get; private set; }
        public bool IsCancelled => _timeElapsedBeforeCancel >= 0;
        public bool IsPaused => _timeElapsedBeforePause >= 0;
        public bool IsDone => IsCompleted || IsCancelled;

        public TimeSpan RemainingTime => TimeSpan.FromSeconds(GetRemainingTime());

        public float RatioComplete => ElapsedTime / _duration;

        public float RatioRemaining => GetRemainingTime() / _duration;

        private float WorldTime => _ignoreTimeScale ? _timeProvider.UnscaledTime : _timeProvider.WorldTime;
        private float DeltaTime => WorldTime + _lastUpdateTime;
        private float CompletionTime => _startTime + _duration;
        private float ElapsedTime
        {
            get
            {
                if (IsCompleted || WorldTime >= CompletionTime)
                    return _duration;

                if (IsCancelled)
                    return _timeElapsedBeforeCancel;

                if (IsPaused)
                    return _timeElapsedBeforePause;

                return WorldTime - _startTime;
            }
        }

        public TimerUpdatable(TimeSpan duration, bool ignoreTimeScale, ITimeProvider timeProvider)
        {
            _duration = (float)duration.TotalSeconds;
            _ignoreTimeScale = ignoreTimeScale;
            _timeProvider = timeProvider;
        }

        public void Start()
        {
            _startTime = WorldTime;
            _lastUpdateTime = _startTime;

            _timeElapsedBeforeCancel = -1;
            _timeElapsedBeforePause = -1;

            IsCompleted = false;
        }

        public void Tick()
        {
            if (IsDone)
                return;

            if (IsPaused)
            {
                _startTime += DeltaTime;
                _lastUpdateTime = WorldTime;
                return;
            }

            _lastUpdateTime = WorldTime;

            if (WorldTime >= CompletionTime)
                IsCompleted = true;
        }

        public void Cancel()
        {
            if (IsDone)
                return;

            _timeElapsedBeforeCancel = ElapsedTime;
            _timeElapsedBeforePause = -1;
        }

        public void Pause()
        {
            if (IsPaused || IsDone)
                return;

            _timeElapsedBeforePause = ElapsedTime;
        }

        public void Resume()
        {
            if (!IsPaused || IsDone)
                return;

            _timeElapsedBeforePause = -1;
        }

        private float GetRemainingTime()
            => _duration - ElapsedTime;
    }
}