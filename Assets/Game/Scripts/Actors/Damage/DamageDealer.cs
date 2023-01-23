using Game.Actors.Health;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Actors.Damage
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField]
        private int initialValue = 1;

        [SerializeField]
        private MMF_Player hitFeedback;

        public void Init(int value) 
            => initialValue = value;

        public bool TryDealDamage(Transform target)
        {
            if (!target.TryGetComponent(out DamageableController damageable))
            {
                return false;
            }

            if (damageable.IsInvulnerable)
            {
                return false;
            }

            PlayHitFeedback();
            ApplyDamage(damageable);

            return true;
        }

        private void PlayHitFeedback()
        {
            if (hitFeedback)
            {
                Instantiate(hitFeedback, transform.position, transform.rotation);
            }
        }

        private void ApplyDamage(DamageableController damageable)
        {
            damageable.Damage(initialValue);
        }
    }
}