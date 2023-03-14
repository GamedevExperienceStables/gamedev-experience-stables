using System;
using Game.TimeManagement;
using UnityEngine;
using VContainer;

namespace Game.Level
{
    public class TrapView : MonoBehaviour
    {
        [Header("FX")]
        [SerializeField]
        private GameObject spawnFeedbacks;

        [SerializeField]
        private GameObject destroyFeedbacks;

        private TimerPool _timers;
        private TimerUpdatable _timer;

        [Inject]
        public void Construct(TimerPool timers) 
            => _timers = timers;

        public void Init(float lifetime)
        {
            _timer = _timers.GetTimerStarted(TimeSpan.FromSeconds(lifetime), DestroySelf);

            if (spawnFeedbacks)
                spawnFeedbacks.SetActive(true);
        }

        private void OnDestroy()
            => _timers.ReleaseTimer(_timer);

        private void DestroySelf()
        {
            if (destroyFeedbacks)
                destroyFeedbacks.SetActive(true);

            Destroy(gameObject);
        }
    }
}