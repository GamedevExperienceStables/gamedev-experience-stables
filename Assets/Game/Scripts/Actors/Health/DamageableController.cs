using System;
using Game.Stats;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Actors.Health
{
    public class DamageableController : MonoBehaviour
    {
        [SerializeField]
        private MMF_Player damageFeedback;

        public bool IsInvulnerable { get; private set; }

        private IDamageableStats _damageableStats;
        private ActorController _owner;

        private void Awake()
            => _owner = GetComponent<ActorController>();

        private void Start()
            => _damageableStats = _owner.GetStats<IDamageableStats>();

        public void Damage(int damageValue)
        {
            if (IsInvulnerable)
                return;

            if (_damageableStats.Health.Current.Value <= 0)
                return;

            MakeDamage(damageValue);
            PlayDamageFeedback();
        }

        private void PlayDamageFeedback()
        {
            if (damageFeedback)
            {
                Instantiate(damageFeedback, transform.position, transform.rotation);
            }
        }

        private void MakeDamage(int damageValue)
        {
            _damageableStats.Health.SubtractValue(damageValue);
        }

        public void MakeInvulnerable()
            => IsInvulnerable = true;
    }
}