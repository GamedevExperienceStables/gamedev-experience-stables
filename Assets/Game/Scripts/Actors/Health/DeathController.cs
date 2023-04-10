using System;
using Game.Stats;
using UnityEngine;

namespace Game.Actors.Health
{
    [RequireComponent(typeof(DamageableController))]
    public class DeathController : MonoBehaviour
    {
        [SerializeField]
        private GameObject deathFeedback;

        [SerializeField]
        private bool destroyOnDeath = true;

        public event Action Died;
        public event Action<DeathCause> DiedWithCause;
        
        public event Action Revived;

        private DamageableController _damageableController;
        private bool _isDead;

        private IActorController _owner;

        private void Start()
        {
            _damageableController = GetComponent<DamageableController>();

            _owner = GetComponent<IActorController>();
            _owner.Subscribe(CharacterStats.Health, OnHealthChanged);
        }

        private void OnDestroy()
        {
            _owner.UnSubscribe(CharacterStats.Health, OnHealthChanged);
        }

        public void Revive()
        {
            _isDead = false;
            _damageableController.MakeVulnerable();
            
            Revived?.Invoke();
        }

        private void OnHealthChanged(StatValueChange change)
        {
            if (change.newValue > 0)
                return;

            if (_isDead)
                return;

            Kill(DeathCause.Damage);
        }

        public void Kill(DeathCause cause)
        {
            _isDead = true;
            _damageableController.MakeInvulnerable();

            Died?.Invoke();
            DiedWithCause?.Invoke(cause);

            PlayDeathFeedback();

            if (destroyOnDeath)
                Destroy(gameObject);
        }

        private void PlayDeathFeedback()
        {
            if (deathFeedback)
                Instantiate(deathFeedback, transform.position, transform.rotation);
        }
    }
}