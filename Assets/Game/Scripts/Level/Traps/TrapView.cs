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

        [SerializeField]
        private bool destroyOnTimeout = true;

        [Header("Feedback")]
        [SerializeField]
        private bool isLocalFeedback;

        [SerializeField]
        private GameObject destroyFeedback;

        private SpawnPool _spawnPool;

        public event Action Destroyed;

        [Inject]
        public void Construct(TimerPool timers, SpawnPool spawnPool)
        {
            _timers = timers;
            _spawnPool = spawnPool;
        }

        public void Init(float lifetime)
        {
            if (lifetime > 0)
                _timer = _timers.GetTimerStarted(TimeSpan.FromSeconds(lifetime), DestroySelf);
        }

        private void OnDestroy()
        {
            if (_timer != null)
                _timers.ReleaseTimer(_timer);

            Destroyed?.Invoke();
        }

        private void DestroySelf()
        {
            DestroyFeedback();

            if (destroyOnTimeout)
                Destroy(gameObject);
        }

        private void DestroyFeedback()
        {
            if (!destroyFeedback) 
                return;
            
            if (isLocalFeedback)
                destroyFeedback.SetActive(true);
            else
                _spawnPool.Spawn(destroyFeedback, transform.position, Quaternion.identity);
        }
    }
}