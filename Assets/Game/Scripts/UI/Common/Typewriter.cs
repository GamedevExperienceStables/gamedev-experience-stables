using System;
using Game.TimeManagement;
using VContainer;

namespace Game.UI
{
    public sealed class Typewriter : IDisposable
    {
        private TypewriterLabel _label;

        private readonly TimerUpdatable _timer;
        private readonly TimerPool _timers;
        public Action onComplete;

        [Inject]
        public Typewriter(TimerPool timers)
        {
            _timers = timers;
            _timer = timers.GetTimer(TimeSpan.Zero, OnNext, isLooped: true);
        }

        public bool IsComplete => _label.IsTextFullyVisible;

        public void Dispose()
            => _timers.ReleaseTimer(_timer);


        public void Start(TypewriterLabel label, float speed)
        {
            _label = label;

            _label.MaxVisibleCharacters = 0;

            TimeSpan interval = TimeSpan.FromSeconds(1 / speed);
            _timer.Start(interval);
        }

        private void OnNext()
        {
            if (_label.IsTextFullyVisible)
            {
                Complete();
                onComplete?.Invoke();
            }
            else
            {
                _label.MaxVisibleCharacters++;
            }
        }

        public void Complete()
        {
            _timer.Stop();
            _label.MaxVisibleCharacters = _label.Text.Length + 1;
        }
    }
}