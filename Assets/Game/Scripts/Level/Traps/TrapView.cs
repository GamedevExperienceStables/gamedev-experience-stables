using System;
using Game.TimeManagement;
using UnityEngine;
using VContainer;

namespace Game.Level
{
    public class TrapView : MonoBehaviour
    {
        private TimerPool _timers;
        private TimerUpdatable _timer;

        [Inject]
        public void Construct(TimerPool timers)
            => _timers = timers;

        public void Init(float lifetime)
        {
            if (lifetime > 0)
                _timer = _timers.GetTimerStarted(TimeSpan.FromSeconds(lifetime), DestroySelf);
        }

        private void OnDestroy()
        {
            if (_timer != null)
                _timers.ReleaseTimer(_timer);
        }

        private void DestroySelf()
            => Destroy(gameObject);
    }
}