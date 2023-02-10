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

        private void Awake()
            => _owner = GetComponent<IActorController>();

        public void Damage(StatModifier damage)
        {
            if (IsInvulnerable)
                return;

            if (_owner.GetCurrentValue(CharacterStats.Health) <= 0)
                return;

            MakeDamage(damage);
            PlayDamageFeedback();
        }

        private void PlayDamageFeedback()
        {
            if (damageFeedback)
            {
                Instantiate(damageFeedback, transform.position, transform.rotation);
            }
        }

        public void MakeDamage(StatModifier damageModifier) 
            => _owner.ApplyModifier(CharacterStats.Health, damageModifier);

        public void MakeInvulnerable()
            => IsInvulnerable = true;

        public void MakeVulnerable() 
            => IsInvulnerable = false;
    }
}