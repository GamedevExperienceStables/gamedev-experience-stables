using System;
using Game.Stats;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Actors.Health
{
    [RequireComponent(typeof(DamageableController))]
    public class DeathController : MonoBehaviour
    {
        [SerializeField]
        private MMF_Player deathFeedback;

        [SerializeField]
        private bool destroyOnDeath = true;

        public event Action Died;

        private DamageableController _damageableController;
        private bool _isDead;

        private IDamageableStats _damageableStats;

        private void Start()
        {
            _damageableController = GetComponent<DamageableController>();

            var owner = GetComponent<ActorController>();
            _damageableStats = owner.GetStats<IDamageableStats>();
            
            _damageableStats.Health.Current.Subscribe(OnHealthChanged);
        }

        private void OnDestroy() 
            => _damageableStats.Health.Current.UnSubscribe(OnHealthChanged);

        private void OnHealthChanged(float newValue)
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
            _damageableController.MakeInvulnerable();

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