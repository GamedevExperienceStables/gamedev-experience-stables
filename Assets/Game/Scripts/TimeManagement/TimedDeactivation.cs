using System;
using UnityEngine;
using VContainer;

namespace Game.TimeManagement
{
    public class TimedDeactivation : MonoBehaviour
    {
        [SerializeField]
        private float timeBeforeDisable = 2;

        private TimerPool _timers;
        private TimerUpdatable _timer;


        [Inject]
        public void Construct(TimerPool timers)
        {
            _timers = timers;
            _timer = _timers.GetTimer(TimeSpan.FromSeconds(timeBeforeDisable), OnComplete);
        }

        private void OnDestroy() 
            => _timers?.ReleaseTimer(_timer);

        private void OnComplete() 
            => gameObject.SetActive(false);

        private void OnEnable() 
            => _timer?.Start();

        private void OnDisable() 
            => _timer?.Stop();
    }
}