using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Actors.Health
{
    [RequireComponent(typeof(HealthController))]
    public class DeathController : MonoBehaviour
    {
        [SerializeField]
        private MMF_Player deathFeedback;

        [SerializeField]
        private bool destroyOnDeath = true;

        public event Action Died;

        private HealthController _health;
        private bool _isDead;

        private void Awake()
            => _health = GetComponent<HealthController>();

        private void Start()
            => _health.HealthChanged += OnHealthChanged;

        private void OnDestroy()
            => _health.HealthChanged -= OnHealthChanged;

        private void OnHealthChanged(int newValue)
        {
            if (newValue > 0)
                return;

            if (_isDead)
                return;

            Kill();
        }

        private void Kill()
        {
            MakeDead();
            _health.MakeInvulnerable();
            
            Died?.Invoke();
            
            PlayDeathFeedback();

            if (destroyOnDeath)
                Destroy(gameObject);
        }

        private void MakeDead() 
            => _isDead = true;

        private void PlayDeathFeedback()
        {
            if (deathFeedback)
                Instantiate(deathFeedback, transform.position, transform.rotation);
        }
    }
}