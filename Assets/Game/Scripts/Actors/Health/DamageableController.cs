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

        private IActorController _owner;
        public event Action DamageFeedback;

        private void Awake()
            => _owner = GetComponent<IActorController>();

        private void Start()
            => _owner.Subscribe(CharacterStats.Health, OnHealthChanged);

        private void OnDestroy()
            => _owner.UnSubscribe(CharacterStats.Health, OnHealthChanged);

        private void OnHealthChanged(StatValueChange change)
        {
            if (change.newValue >= change.oldValue)
                return;

            DamageFeedback?.Invoke();
            PlayDamageFeedback();
        }

        public void Damage(float damage)
        {
            if (IsInvulnerable)
                return;

            ApplyDamage(damage);
        }

        private void PlayDamageFeedback()
        {
            if (damageFeedback)
            {
                Instantiate(damageFeedback, transform.position, transform.rotation);
            }
        }

        private void ApplyDamage(float damage)
            => _owner.ApplyModifier(CharacterStats.Health, -damage);

        public void MakeInvulnerable()
            => IsInvulnerable = true;

        public void MakeVulnerable()
            => IsInvulnerable = false;
    }
}