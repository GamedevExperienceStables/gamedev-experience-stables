using System;

namespace Game.TimeManagement
{
    public class TimerUpdatable
    {
        private readonly ITimeProvider _timeProvider;

        private readonly float _duration;
        private readonly bool _ignoreTimeScale;
        private readonly bool _isLooped;
        private readonly Action _onComplete;

        private float _timeElapsedBeforeStop = -1;
        private float _timeElapsedBeforePause = -1;

        private float _startTime;
        private float _lastUpdateTime;

        public bool IsCompleted { get; private set; }

        public bool IsStopped
            => _timeElapsedBeforeStop > 0;

        public bool IsPaused
            => _timeElapsedBeforePause > 0;

        public bool IsDone
            => IsCompleted || IsStopped;

        public TimeSpan RemainingTime
            => TimeSpan.FromSeconds(GetRemainingTime());

        public TimeSpan ElapsedTime
            => TimeSpan.FromSeconds(GetElapsedTime());

        public float RatioComplete
            => GetElapsedTime() / _duration;

        public float RatioRemaining
            => GetRemainingTime() / _duration;

        private float WorldTime
            => _ignoreTimeScale ? _timeProvider.RealtimeSinceStartup : _timeProvider.WorldTime;

        private float DeltaTime
            => WorldTime - _lastUpdateTime;

        private float CompletionTime
            => _startTime + _duration;

        public TimerUpdatable(TimeSpan duration, Action onComplete, bool isLooped, bool ignoreTimeScale,
            ITimeProvider timeProvider)
        {
            _duration = (float)duration.TotalSeconds;
            _onComplete = onComplete;
            _isLooped = isLooped;
            _ignoreTimeScale = ignoreTimeScale;
            _timeProvider = timeProvider;
        }

        public void Start()
        {
            _startTime = WorldTime;
            _lastUpdateTime = _startTime;

            _timeElapsedBeforeStop = -1;
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

            // ReSharper disable once InvertIf
            if (WorldTime >= CompletionTime)
            {
                _onComplete?.Invoke();

                if (_isLooped)
                    _startTime = WorldTime;
                else
                    IsCompleted = true;
            }
        }

        public void Stop()
        {
            if (IsDone)
                return;

            _timeElapsedBeforeStop = GetElapsedTime();
            _timeElapsedBeforePause = -1;
        }

        public void Pause()
        {
            if (IsPaused || IsDone)
                return;

            _timeElapsedBeforePause = GetElapsedTime();
        }

        public void Resume()
        {
            if (!IsPaused || IsDone)
                return;

            _timeElapsedBeforePause = -1;
        }

        private float GetRemainingTime()
            => _duration - GetElapsedTime();

        private float GetElapsedTime()
        {
            if (IsCompleted || WorldTime >= CompletionTime)
                return _duration;

            if (IsStopped)
                return _timeElapsedBeforeStop;

            if (IsPaused)
                return _timeElapsedBeforePause;

            return WorldTime - _startTime;
        }
    }
}